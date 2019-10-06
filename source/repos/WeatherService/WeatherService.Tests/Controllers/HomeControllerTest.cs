using System;
using ExcelDataReader;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WeatherService;
using WeatherService.Controllers;
using BusinessWrapper;
using WeatherBusinessFactory;
using System.IO;
using System.Web;
using System.Net;

namespace WeatherService.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        string apiResponse = "";


        [TestMethod]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        // Test case to call the Wep API for test data
        [TestMethod]
        public void WeatherAPICall()
        {
            //Arrange
            
            List<ContractCityList> list = new List<ContractCityList>();
            ContractCityList model = new ContractCityList();
            model.cityId = 292223;
            model.cityName = "Dubai";
            list.Add(model);
            //Act
            foreach (var item in list)
            {
                var id = item.cityId;
                var name = item.cityName;
                /*Calling API http://openweathermap.org/api */
                string apiKey = "aa69195559bd4f88d79f9aadeb77a8f6";
                System.Net.HttpWebRequest apiRequest =
                WebRequest.Create("http://api.openweathermap.org/data/2.5/weather?id=" +
                id + "&mode=xml" + "&appid=" + apiKey + "&units=metric") as HttpWebRequest;
                using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    apiResponse = reader.ReadToEnd();
                }

            }
            // Assert
            Assert.IsNotNull(apiResponse);
        }
    // Test case to save the weather data in to folder in text file .
        [TestMethod]
        public void SaveDataIntoFolder(string Testresponse)
        {
            //Arrange 
            Testresponse = apiResponse;
            var citiName = "Dubai";
            String Todaysdate = DateTime.Now.ToString("dd-MMM-yyyy");
            string nestedsubdir = @"C:\\Temp\WeatherServiceData\\" + Todaysdate;
            string FileName = string.Format(citiName + " -{0:yyyy-MM-dd_hh-mm-ss-tt}.txt", DateTime.Now);
            if (!Directory.Exists(nestedsubdir))
            {
                DirectoryInfo mi = Directory.CreateDirectory(nestedsubdir);
            }
            //Act
            // Use Combine again to add the file name to the path.
            var pathString = System.IO.Path.Combine(nestedsubdir, FileName);
            using (StreamWriter swriter = new StreamWriter(pathString))
            {
                swriter.Write(Testresponse);
            }
            // Assert
            Assert.IsNotNull(Testresponse);
        }
    }
}
