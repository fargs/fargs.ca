Param(
	$DBName = "Orvosi",
	$Server = "localhost"
)

function Create-SQLScript
{
    param(
        [string] $T4 = ""
    )

    Write-Output $T4
    $AuditColumns = $true;
    $StaticData = "StaticData";
    $ConfigData = "ConfigData";
    $TestData = "TestData";

    echo "`nGenerating SQL script`n---"
    & 'C:\Program Files (x86)\Common Files\microsoft shared\TextTemplating\14.0\TextTransform.exe' $t4 -a !!DataSet!$StaticData -a !!AuditColumns!$AuditColumns -a !!Environment!$Environment -out (Join-Path -path $Deploy_Dir.FullName -childpath "Script.Data.PostDeployment.Static.sql")

    echo "`nGenerating SQL script`n---"
    & 'C:\Program Files (x86)\Common Files\microsoft shared\TextTemplating\14.0\TextTransform.exe' $t4 -a !!DataSet!$ConfigData -a !!AuditColumns!$AuditColumns -a !!Environment!$Environment -out (Join-Path -path $Deploy_Dir.FullName -childpath "Script.Data.PostDeployment.Config.sql")

    echo "`nGenerating SQL script`n---"
    & 'C:\Program Files (x86)\Common Files\microsoft shared\TextTemplating\14.0\TextTransform.exe' $t4 -a !!DataSet!$TestData -a !!AuditColumns!$AuditColumns -a !!Environment!$Environment -out (Join-Path -path $Deploy_Dir.FullName -childpath "Script.Data.PostDeployment.Test.sql")
}

function Export-TableData
{
    param(
        [string] $TableName = "", 
        [string] $Destination_Dir = ""
    )

    $Table = $_
	$FullTableName =  $DBName + "." + (($Table.split(".") | %{ "[$_]"}) -join ".")
    $TableName =  $Table.Split(".")[1]
    $TableSchema =  $Table.Split(".")[0]
    $FilePath = join-path -path $Destination_Dir -childpath $Table
    $FilePathHeader = -join ($FilePath, "_header")
    $FilePathData = -join ($FilePath , "_data")
    
    write-output "Export table '$FullTableName' to data config file '$FilePath'..."

	#temporarily leaving this in here to get the column names
    bcp "DECLARE @colnames VARCHAR(max);
	
		SELECT @colnames = COALESCE(@colnames + '|', '') + c.name 
		FROM $DBName.sys.columns c
        INNER JOIN $DBName.sys.tables t ON c.object_id = t.object_id
        INNER JOIN $DBName.sys.schemas s ON t.schema_id = s.schema_id
        WHERE t.name='$TableName' 
        AND s.name ='$TableSchema'
        AND c.name <> 'CreatedDate'
        AND c.name <> 'CreatedUser'
        AND c.name <> 'ModifiedDate'
        AND c.name <> 'ModifiedUser'
        AND c.is_computed = 0;

		SELECT @colnames;" queryout $FilePathHeader -S $Server -C RAW -w -T -t "|"
    
	$Query = [IO.File]::ReadAllText("$FilePathHeader")

	$Query = $Query.Replace("|",", ")

	$PrimaryKey = $Query.Split(",")[0]

	$Query = "DECLARE @Text NVARCHAR(MAX)
	SET @Text = (SELECT $Query from $FullTableName ORDER BY $PrimaryKey FOR XML RAW, ROOT)
	SELECT @Text
	"

	$conn=new-object System.Data.SqlClient.SQLConnection
	$conn.ConnectionString = "Server=$Server;Database=$DBName;Integrated Security=SSPI;"
	$conn.Open()
	$cmd = new-object System.Data.SqlClient.SqlCommand($Query,$conn)
	$cmd.ExecuteScalar() | out-file $FilePathData

	#if the file is 12 bytes or smaller, then it is couldn't possibly contain the <root></root> required, and this must be empty (or contains only newlines) so make a default empty xml
	if ((new-object System.IO.FileInfo($FilePathData)).Length -le 12){
		Set-Content $FilePathData "<root></root>"
	}
	
	$reader = new-object System.Xml.XmlTextReader($FilePathData)
	$writer = [System.Xml.XmlWriter]::Create($FilePath, $settings)
	$writer.WriteNode($reader, $false) 
	$writer.Close()
	$reader.Close()

	#remove the header and data files
    Remove-Item $FilePathHeader
    Remove-Item $FilePathData
    
}

$Deploy_Dir = (get-item $MyInvocation.MyCommand.Path).Directory
write-output $Deploy_Dir.FullName
$StaticData_Dir = (Get-ChildItem -Directory -Filter StaticData -Path $Deploy_Dir.FullName)
write-output $StaticData_Dir.FullName
$ConfigData_Dir = (Get-ChildItem -Directory -Filter ConfigData -Path $Deploy_Dir.FullName)
write-output $ConfigData_Dir.FullName
$TestData_Dir = (Get-ChildItem -Directory -Filter TestData -Path $Deploy_Dir.FullName)
write-output $TestData_Dir.FullName
$SqlT4_File = (Get-ChildItem -File -Filter Script.Data.PostDeployment.tt -Path $Deploy_Dir.FullName)
write-output $SqlT4_File.FullName

[System.Reflection.Assembly]::LoadWithPartialName("System.Xml") > $null
[System.Reflection.Assembly]::LoadWithPartialName("System.Environment") > $null

#XML formatting settings
$settings = new-object System.Xml.XmlWriterSettings
$settings.NewLineHandling = [System.Xml.NewLineHandling]::Replace
$settings.NewLineOnAttributes = $true
$settings.Indent = $true
$settings.IndentChars = '  '
$settings.NewLineChars = [System.Environment]::NewLine
$settings.OmitXmlDeclaration = $true

#Export Static Data to StaticData folder
Get-Content ((Join-Path -path $Deploy_Dir.FullName -childpath "StaticData.txt")) | ForEach-Object {

    Export-TableData $_ $StaticData_Dir.FullName
}


#Export Config Data to ConfigData folder
Get-Content ((Join-Path -path $Deploy_Dir.FullName -childpath "ConfigData.txt")) | ForEach-Object {

    Export-TableData $_ $ConfigData_Dir.FullName

}

#Export Test Data to ConfigData folder
Get-Content ((Join-Path -path $Deploy_Dir.FullName -childpath "TestData.txt")) | ForEach-Object {

    Export-TableData $_ $TestData_Dir.FullName
}

Create-SQLScript $SqlT4_File.FullName


