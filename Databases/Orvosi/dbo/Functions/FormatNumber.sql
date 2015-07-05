CREATE FUNCTION [dbo].[FormatNumber]
(@value NVARCHAR (50), @formatstr NVARCHAR (50))
RETURNS NVARCHAR (100)
AS
BEGIN

Declare @formatlen as int 
Declare @formatdecimal as int
Declare @FormatedNumber as nvarchar (50)
Declare @numberOfDecimalPlaces as int



SET @formatlen = len(@formatstr) 
SET @formatdecimal=Charindex('.',@formatstr)  
SET @numberOfDecimalPlaces = CASE WHEN @formatdecimal = 0 THEN 0 ELSE @formatlen - @formatdecimal END
SET @FormatedNumber= replace(str(@value,@formatlen,@numberOfDecimalPlaces),' ', '0')



	RETURN(@FormatedNumber) 
END