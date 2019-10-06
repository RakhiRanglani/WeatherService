using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace BusinessWrapper
{
   public interface IDataLayer
    {
        DataSet UploadExcelContent(HttpPostedFileBase upload);
        List<ContractCityList> ConvertDataSetToList(DataSet _excelResult);

        void WeatherAPICall(List<ContractCityList> citiList);
        void Createfolder();
        void SaveDataIntoFolder(string citiName, string response);
        string GetFormattedXml(string xml);
    }
}
