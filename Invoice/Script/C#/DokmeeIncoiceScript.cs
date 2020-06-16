using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Globalization;

namespace DokmeeCaptureDynamic
{
    /// <summary>
    /// only one method should be kept GetIndexValue(param), OR GetIndexValue(p,p,p,p)
    /// </summary>
    public class CaptureScripting
    {

        /// <summary>
        /// This method is optional but can be used to export/write index information to external system/files per document.
        /// </summary>
        /// <param name="documentNumber">Document being indexed</param>
        /// <param name="pageNumber">Document's page having current zone on</param>
        /// <param name="indexField">Index field mapped to zone currently being indexed</param>
        /// <param name="zoneText">Text read from the zone created in barcode template module.</param>
        /// <returns></returns>
        public string GetIndexValue(string zoneText, int documentNumber, int pageNumber, string indexField)
        {
            string results = string.Empty;
            char[] delimiters = new char[] { '\r', '\n' };
            string[] lines = zoneText.Split(delimiters, System.StringSplitOptions.RemoveEmptyEntries);
            string input = zoneText;
            indexField = indexField.ToLower();

            if (indexField == "supplier")
            {
                string pattern = @"(?<=(Account Name|\bACCT NAME|\ACCOUNT NAME|Acct Name|ACCT Name)([:])?(\t+|\s+))(.*)";
                Match m = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
                if (m.Success){
                    results = m.Value;
                }else{
                    pattern = @"\bLLC.|\bLLC|\bCo.|\bInc.|\bINC.";
                    Match value;
                    for (int i =0 ; i< lines.Length;i++)
                    {
                        input = lines[i];
                        value = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
                        if(value.Success){
                            results = input;
                            break;
                        }else if(i == (lines.Length-1)){
                            results = "Unknown supplier";
                        }
                    }
                
                }
            }
            else if (indexField == "invoice #")
            {
                string pattern = @"(?<=(INVOICE#|Order Number|Invoice Number|Invoice No.|INVOICE NO:|Purchase Order|Receipt #|NO.|INVOICE #)([:])?(\t+|\s+))(.*)(\d*)?.?(\d*)?(\d+)";
                Match m = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
                if (m.Success){
                    results = m.Value;
                    results.Replace(":",".");
                }
            }
            else if (indexField == "total")
            {
                string pattern = @"(?<=(GRAND TOTAL|Balance Due|TOTAL DUE|AMOUNT DUE)([:])?(\t+|\s+))(.*)(\d*)?.?(\d*)?(\d+)";
                Match m = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
                if (m.Success){
                    results = m.Value;
                    
                }
                
            }
            else if (indexField == "date")
            {
                string pattern = @"\b[0-9]{1,2}(-)[0-9]{1,2}(-)[0-9]{2,4}|\b[0-9]{1,2}(\/)[0-9]{1,2}(\/)[0-9]{2,4}|\b[0-9]{1,2}(~)[0-9]{1,2}(-)[0-9]{2,4}|\b[0-9]{1,2}(\^)[0-9]{1,2}(-)[0-9]{2,4}";
                Match m = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
                if (m.Success){
                    results = m.Value;
                }else{
                    pattern = @"(?<=(Date|DATE|Issued on|ISSUED ON)([:|;|\s])?(\t+|\s+))(.*)(\d*)?.?(\d*)?(\d+)";
                    Match date = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
                    if (date.Success){
                    var cultureInfo = new CultureInfo("fr-Fr");
                    string dateString = date.Value;
                    var dateTime = DateTime.Parse(dateString, cultureInfo);
                    results = dateTime.ToString("d",cultureInfo);
                    }
                }
            }

            WriteToIndexFile(documentNumber, pageNumber, indexField, results);

            return results;
        }

        private void ConnecToDatabase()
        {
            //TODO: connect databases
        }

        private void CallExternalAPI()
        {
            //TODO: API calls
        }

        private void WriteToIndexFile(int documentNumber, int pageNumber, string indexField, string indexValue)
        {
            //string rootDir = Path.GetDirectoryName(System.Reflection.Assembly.GetCallingAssembly().Location);
            string rootDir = Path.GetTempPath();// Path.GetDirectoryName();

            string indexFile = Path.Combine(rootDir, "IndexFile.txt");
            using (TextWriter writer = new StreamWriter(indexFile, true))
            {
                //writer.WriteLine($"Document Number:{documentNumber} | Page Number:{pageNumber} | Index Field:{indexField} | Index Value:{indexValue}");
                writer.WriteLine(string.Format("Document Number:{0} | Page Number:{1} | Index Field:{2} | Index Value:{3}", documentNumber, pageNumber, indexField, indexValue));
            }
        }
    }

    public class DemoClassOne
    {
        public string Test()
        {
            return "Test from DemoClassOne";
        }
    }

    public class DemoClassTwo
    {
        public string Test()
        {
            return "Test from DemoClassTwo";
        }
    }
}