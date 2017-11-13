using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace WebApp.Library
{

    /// <summary>
    /// Class used to send a CSV file to the response.
    /// </summary>
    /// <remarks>To ensure predictable output, all fields should be string-valued properties of the
    /// objects passed; where this is not the case, it may be best to construct an array of anonymous
    /// objects.</remarks>

    public class CsvResult : ActionResult
    {
        #region Fields
        private StringBuilder _CsvString;
        private Object[] _Data;
        private string[] _Fields;
        private bool _PreserveWhitespace;
        private bool _UseHeaderRecord;
        private string _NewLine;
        private Regex _MatchWhitespace = new Regex("(?:^(\\s)+|(\\s)+$)");
        private Regex _MatchEscapeCharacters = new Regex("[\"\",\n]");
        private Regex _MatchQuotes = new Regex("([\"\"])");
        private string _FileName = "DataFile";
        #endregion

        #region Properties
        /// <summary>
        /// Used to build the CSV file contents.
        /// </summary>
        protected StringBuilder CsvString { get; set; }

        /// <summary>
        /// Gets or sets the array of data objects.
        /// </summary>
        /// <value>An array of objects to use as the CSV data.</value>
        /// <returns>The objects being used to populate the CSV.</returns>
        public Object[] Data { get; set; }

        /// <summary>
        /// Gets or sets the fields being used in this CSV.
        /// </summary>
        /// <value>An array of field (property) names.</value>
        /// <returns>The fields to output to the CSV.</returns>
        public string[] Fields { get; set; }

        /// <summary>
        /// Determines if whitespace should be preserved or trimmed in the output CSV.
        /// </summary>
        /// <value>True if whitespace should be preserved, false if it should be trimmed.</value>
        /// <returns>A boolean value representing whether whitespace is preserved or trimmed.</returns>
        public bool PreserveWhitespace { get; set; }

        /// <summary>
        /// Gets or sets the filename of the csv file to be returned.
        /// </summary>
        /// <value>Defaults to DataFile_yyyyMMdd_HHmmss.</value>
        /// <returns>A string value representing the filename of the downloaded CSV file.</returns>
        public string FileName { get; set; }

        /// <summary>
        /// Determines if the first row of the CSV is a header row.
        /// </summary>
        /// <value>True if a header row should be output, false if it should be omitted.</value>
        /// <returns>A representation of whether the header row will be output or not.</returns>
        public bool UseHeaderRecord { get; set; }

        /// <summary>
        /// The newline characters to use in this CSV (defaults to native terminator if not set).
        /// </summary>
        /// <value>A string to use to terminate lines in this CSV.</value>
        /// <returns>The line terminator being used by this CSV.</returns>
        public string NewLine
        {
            get { return string.IsNullOrEmpty(_NewLine) ? Environment.NewLine : _NewLine; }
            set { _NewLine = value; }
        }
        #endregion


        #region Constructors

        public CsvResult()
        {
            PreserveWhitespace = true;
            UseHeaderRecord = true;
        }

        /// <summary>
        /// Create a new CSV result.
        /// </summary>
        /// <param name="isWhitespacePreserved">Preserve or ignore leading and trailing whitespace.</param>
        /// <param name="isHeaderRecord">Indicate if the first line should be a header record.</param>
        /// <param name="useNewLine">A string to use for new lines (defaults to native).</param>
        public CsvResult(Object[] withData, bool isWhitespacePreserved, bool isHeaderRecord)
        {
            Data = withData;
            PreserveWhitespace = isWhitespacePreserved;
            UseHeaderRecord = isHeaderRecord;
        }

        /// <summary>
        /// Create a new CSV result.
        /// </summary>
        /// <param name="withData">The objects to convert to CSV.</param>
        /// <param name="isWhitespacePreserved">Preserve or ignore leading and trailing whitespace.</param>
        /// <param name="isHeaderRecord">Indicate if the first line should be a header record.</param>
        /// <param name="useNewLine">A string to use for new lines (defaults to native).</param>
        public CsvResult(string[] withFields, bool isWhitespacePreserved, bool isHeaderRecord, string useNewLine)
        {
            Fields = withFields;
            PreserveWhitespace = isWhitespacePreserved;
            UseHeaderRecord = isHeaderRecord;
            NewLine = useNewLine;
        }

        /// <summary>
        /// Create a new CSV result.
        /// </summary>
        /// <param name="withData">The objects to convert to CSV.</param>
        /// <param name="withFields">The fields to use for the CSV.</param>
        /// <param name="isWhitespacePreserved">Preserve or ignore leading and trailing whitespace.</param>
        /// <param name="isHeaderRecord">Indicate if the first line should be a header record.</param>
        /// <param name="useNewLine">A string to use for new lines (defaults to native).</param>
        public CsvResult(Object[] withData, string[] withFields, bool isWhitespacePreserved, bool isHeaderRecord, string useNewLine)
        {
            Data = withData;
            Fields = withFields;
            PreserveWhitespace = isWhitespacePreserved;
            UseHeaderRecord = isHeaderRecord;
            NewLine = useNewLine;
        }
        #endregion


        #region "Methods"
        /// <summary>
        /// Override the ExecuteResult method to write the CSV data to the response stream.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            CsvString = new StringBuilder();
            List<string> row;

            if (Fields == null & Data != null && Data.FirstOrDefault() != null)
            {
                var fieldList = new List<string>();
                var properties = Data.FirstOrDefault().GetType().GetProperties();
                foreach (var item in properties)
                {
                    if (item.CanRead & item.GetGetMethod().IsPublic & item.GetGetMethod().GetParameters().Length == 0)
                    {
                        fieldList.Add(item.Name);
                    }
                }

                if (fieldList.Count() > 0) { Fields = fieldList.ToArray(); }
            }

            if (Fields != null && Fields.Length > 0)
            {
                if (UseHeaderRecord)
                {
                    row = new List<string>();
                    AppendRow(Fields);
                }

                if (Data != null && Data.FirstOrDefault() != null)
                {
                    foreach (var o in Data)
                    {
                        row = new List<string>();
                        foreach (var f in Fields)
                        {
                            row.Add(EscapeValue(GetPropertyValue(o, f)));
                        }
                        AppendRow(row.ToArray());
                    }
                }
            }

            context.HttpContext.Response.Clear();
            context.HttpContext.Response.AddHeader("Cache-Control", "must-revalidate");
            context.HttpContext.Response.AddHeader("Pragma", "must-revalidate");
            context.HttpContext.Response.ContentType = "text/csv";
            context.HttpContext.Response.AddHeader("content-disposition", string.Format("attachment: filename={0}_{1}.csv", this.FileName, DateTime.Now.ToString("yyyyMMdd_hhmmss")));
            context.HttpContext.Response.Write(CsvString.ToString().TrimEnd(NewLine.ToCharArray()));
            context.HttpContext.Response.Flush();
            context.HttpContext.Response.Close();
            context.HttpContext.Response.End();
        }

        /// <summary>
        /// Format the value to be suitable for CSV output.
        /// </summary>
        /// <param name="value">The value to be escaped.</param>
        /// <remarks>This method takes care of the basics: escape double quotes, trim leading and
        /// trailing whitespace (if <c>PreserveWhitespace</c> is false), and enclose fields in double
        /// quotes where required.</remarks>
        private string EscapeValue(string value)
        {
            if (!PreserveWhitespace)
            {
                value = value.Trim();
            }
            if (_MatchEscapeCharacters.IsMatch(value) | (PreserveWhitespace & _MatchWhitespace.IsMatch(value)))
            {
                value = string.Format("\"{0}\"", _MatchQuotes.Replace(value, "\"\""));
            }

            value = value.Replace(NewLine, " ");

            return value;
        }

        /// <summary>
        /// A quick way of getting string properties from an object.
        /// </summary>
        /// <param name="rowObject">The object whose property we want.</param>
        /// <param name="propertyName">The name of the property we want.</param>
        private string GetPropertyValue(Object rowObject, string propertyName)
        {
            var rowValue = rowObject.GetType().InvokeMember(propertyName, System.Reflection.BindingFlags.GetProperty, null, rowObject, null);
            return rowValue != null ? rowValue.ToString() : string.Empty;
        }

        /// <summary>
        /// Append a row to the <c>StringBuilder</c> containing the CSV data.
        /// </summary>
        /// <param name="row">The row to append to this instance.</param>
        private void AppendRow(string[] row)
        {
            CsvString.Append(string.Join(",", row));
            CsvString.Append(NewLine); //Allows newline character to be changed
        }
        #endregion

    }
}