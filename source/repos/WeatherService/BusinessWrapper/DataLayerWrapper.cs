using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml;


namespace BusinessWrapper
{
    public class DataLayerWrapper : IDataLayer
    {
        public List<ContractCityList> ConvertDataSetToList(DataSet _excelResult)
        {
            var citylist = _excelResult.Tables[0].AsEnumerable().Select(s => new ContractCityList
            {
                cityId = Convert.ToInt32(s["CityID"]),
                cityName = Convert.ToString(s["CityName"])

            }).ToList();

            return citylist;
        }

        public void Createfolder()
        {
            String Todaysdate = DateTime.Now.ToString("dd-MMM-yyyy");
            string root = @"C:\\Temp";
            string subdir = @"C:\\Temp\\WeatherServiceData";
            string nestedsubdir = @"C:\\Temp\WeatherServiceData\\" + Todaysdate;
            // If directory does not exist, create it. 
            try
            {
                if (!Directory.Exists(root))
                {
                    // Try to create the directory.
                    DirectoryInfo di = Directory.CreateDirectory(root);
                    // Try to create the sub directory.
                    DirectoryInfo si = Directory.CreateDirectory(subdir);

                    DirectoryInfo mi = Directory.CreateDirectory(nestedsubdir);
                }
                else if (!Directory.Exists(subdir))
                {
                    // Try to create the sub directory.
                    DirectoryInfo si = Directory.CreateDirectory(subdir);
                    DirectoryInfo mi = Directory.CreateDirectory(nestedsubdir);
                }
                else if (!Directory.Exists(nestedsubdir))
                { // Try to create the nested sub directory.
                    DirectoryInfo di = Directory.CreateDirectory(nestedsubdir);
                }

            }
            catch (IOException ioex)
            {
                Console.WriteLine(ioex.Message);
            }
        }

        public string GetFormattedXml(string xml)
        {// Create a web client.
            using (WebClient client = new WebClient())
            {
                // Load the response into an XML document.
                XmlDocument xml_document = new XmlDocument();
                xml_document.LoadXml(xml);

                // Format the XML.
                using (StringWriter string_writer = new StringWriter())
                {
                    XmlTextWriter xml_text_writer =
                    new XmlTextWriter(string_writer);
                    xml_text_writer.Formatting = System.Xml.Formatting.Indented;
                    xml_document.WriteTo(xml_text_writer);

                    // Return the result.
                    return string_writer.ToString();
                }
            }
        }

        public void SaveDataIntoFolder(string citiName, string response)
        {
            String Todaysdate = DateTime.Now.ToString("dd-MMM-yyyy");
            string directoryName = @"C:\\Temp\\WeatherServiceData\\" + Todaysdate;

            string FileName = string.Format(citiName + " -{0:yyyy-MM-dd_hh-mm-ss-tt}.txt", DateTime.Now);

            // Use Combine again to add the file name to the path.
            var pathString = System.IO.Path.Combine(directoryName, FileName);

            var formattedXML = GetFormattedXml(response);
            using (StreamWriter swriter = new StreamWriter(pathString))
            {
                swriter.Write(formattedXML);
            }
        }

        public DataSet UploadExcelContent(HttpPostedFileBase upload)
        {
            // ExcelDataReader works with the binary Excel file, so it needs a FileStream
            // to get started. This is how we avoid dependencies on ACE or Interop:
            Stream stream = upload.InputStream;

            // We return the interface, so that
            IExcelDataReader reader = null;
            if (upload.FileName.EndsWith(".xls"))
            {
                reader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else if (upload.FileName.EndsWith(".xlsx"))
            {
                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            DataSet result = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });
            var Citylist = ConvertDataSetToList(result);
            WeatherAPICall(Citylist);
            reader.Close();
            return result;

        }

        public void WeatherAPICall(List<ContractCityList> citiList)
        {
            string apiResponse = string.Empty;
            try
            {
                if (citiList != null)
                {
                    foreach (var item in citiList)
                    {
                        var id = item.cityId;
                        var name = item.cityName;
                        /*Calling API http://openweathermap.org/api */
                        string apiKey = "aa69195559bd4f88d79f9aadeb77a8f6";
                        HttpWebRequest apiRequest =
                        WebRequest.Create("http://api.openweathermap.org/data/2.5/weather?id=" +
                        id + "&mode=xml" + "&appid=" + apiKey + "&units=metric") as HttpWebRequest;
                        using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
                        {
                            StreamReader reader = new StreamReader(response.GetResponseStream());
                            apiResponse = reader.ReadToEnd();
                        }
                        Createfolder();
                        SaveDataIntoFolder(name, apiResponse);
                    }

                }
            }
            catch (IOException ioex)
            {
                Console.WriteLine(ioex.Message);
            }
        }
    }
}

