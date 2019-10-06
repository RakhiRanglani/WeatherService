using ExcelDataReader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using WeatherBusinessFactory;
using WeatherService.Models;
using BusinessWrapper;

namespace WeatherService.Controllers
{
    public class HomeController : Controller
    {

        public static IDataLayer _Weatherwrapperlayer;

        public HomeController() => _Weatherwrapperlayer = BusinessFactory.GetComponent<IDataLayer>();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {

                if (upload != null && upload.ContentLength > 0)
                {
                    DataSet result = _Weatherwrapperlayer.UploadExcelContent(upload);
                    return View(result.Tables[0]);
                }
                else
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                }
            }
            return View();
        }

    }
}
