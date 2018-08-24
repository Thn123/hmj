using Hmj.Entity;
using System.Collections.Generic;

namespace Hmj.Interface
{
    public interface ICommonService
    {
        FILES UploadFile(string ext,string contentType,byte[] data,string url);

        FILES UploadFile(string ext, string contentType, byte[] data, string url, string bendword,
            string bagword, string smllword,string fansid);

        FILES UploadFile(string ext, string contentType, byte[] data, string url,string remark);

        FILES UploadFile(string ext,string uploadimg, string contentType, byte[] data, string url, string remark);

        FILES UploadFile2(string ext, string contentType, byte[] data, string url);

        FILES GetUploadFile(int id);

       

       

         //产品 项目 大类
        List<PROD_CATEGORY> getCATEGORY(int prodType,int orgId);

        

        //*发送预约短信接口*//
        int SendBookMessage(int orgId, int storeId, int bid, string cname, string phone, string creater);

        //存款短信
        int SendSaveAmtMessage(int orgId, int storeId, int cid, decimal buyAmt, string creater, int stype);
        string InsertGood(string fansID, string fileID);
        int InsertStore();
        int InsertCity();
    }
}
