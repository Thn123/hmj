using Hmj.Common;
using Hmj.Common.Exceptions;
using Hmj.Common.Utils;
using Hmj.DataAccess.Repository;
using Hmj.Entity;
using Hmj.Interface;
using Hmj.WebService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using WeChatCRM.Common.Utils;

namespace Hmj.Business.ServiceImpl
{
    public class CommonService : ICommonService
    {
        private CommonRepository _repo;
        private BcjBookRepository _book;
        public CommonService()
        {
            _repo = new CommonRepository();
            _book = new BcjBookRepository();
        }
        public FILES UploadFile(string ext, string contentType, byte[] data, string url)
        {
            FILES fileEntity = null;
            try
            {
                string newName = Guid.NewGuid().ToString("d") + ext;

                string filePath = string.Concat(AppConfig.UploadTMP,
                    DateTime.Today.ToString("yyyyMMdd"), "\\");

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string fullName = filePath + newName;
                using (FileStream steam = new FileStream(fullName, FileMode.CreateNew, FileAccess.Write))
                {
                    steam.Write(data, 0, data.Length);
                }

                fileEntity = new FILES();
                fileEntity.CONTENT_TYPE = contentType;
                fileEntity.FILE_NAME = fullName;
                fileEntity.FILE_SIZE = data.Length;
                fileEntity.IS_GOOD = 0;
                fileEntity.ID = (int)_repo.Insert(fileEntity);

                fileEntity.FILE_URL = url + "/" + fileEntity.ID;
                _repo.Update(fileEntity);

            }
            catch (Exception ex)
            {
                throw new BOException("上传图片发生意外错误，请稍后重试，或联系管理员", ex);
            }
            return fileEntity;
        }

        public FILES UploadFile(string ext, string contentType, byte[] data, string url, string bendword,
            string bagword, string smllword, string fansId)
        {
            FILES fileEntity = null;
            try
            {
                string newName = Guid.NewGuid().ToString("d") + ext;

                string filePath = string.Concat(AppConfig.UploadTMP, DateTime.Today.ToString("yyyyMMdd"), "\\");

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string fullName = filePath + newName;

                Stream st = new MemoryStream(data);

                //using (FileStream steam = new FileStream(fullName, FileMode.CreateNew, FileAccess.Write))
                //{
                //    //steam.Write(data, 0, data.Length);
                //StreamHelper.MakeThumbnail(st, fullName, 20, 20, true);
                StreamHelper.GetPicThumbnail(st, fullName, 20, 20, 30);
                //}

                fileEntity = new FILES();
                fileEntity.BEND_WORD = bendword;
                fileEntity.BAG_WORD = bagword;
                fileEntity.FANS_ID = int.Parse(string.IsNullOrEmpty(fansId) ? "0" : fansId);
                fileEntity.SMALL_WORD = smllword;
                fileEntity.CREATE_TIME = DateTime.Now;

                fileEntity.CONTENT_TYPE = contentType;
                fileEntity.FILE_NAME = fullName;
                fileEntity.FILE_SIZE = data.Length;
                fileEntity.IS_GOOD = 0;

                fileEntity.REMARK = fullName.Split('\\')[fullName.Split('\\').Length - 1];
                fileEntity.ID = (int)_repo.Insert(fileEntity);

                fileEntity.FILE_URL = url + "/" + fileEntity.ID;
                _repo.Update(fileEntity);

            }
            catch (Exception ex)
            {
                throw new BOException("上传图片发生意外错误，请稍后重试，或联系管理员", ex);
            }
            return fileEntity;
        }

        public FILES UploadFile(string ext, string contentType, byte[] data, string url, string remark)
        {
            FILES fileEntity = null;
            try
            {
                string newName = Guid.NewGuid().ToString("d") + ext;

                string filePath = string.Concat(AppConfig.UploadTMP, DateTime.Today.ToString("yyyyMMdd"), "\\");

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string fullName = filePath + newName;
                using (FileStream steam = new FileStream(fullName, FileMode.CreateNew, FileAccess.Write))
                {
                    steam.Write(data, 0, data.Length);
                }

                fileEntity = new FILES();
                fileEntity.CONTENT_TYPE = contentType;
                fileEntity.FILE_NAME = fullName;
                fileEntity.FILE_SIZE = data.Length;
                fileEntity.FILE_URL = "<!--只有图片存放到外部时才有用哦--!>";
                fileEntity.IS_GOOD = 0;
                if (remark != "")
                    fileEntity.REMARK = remark;
                else
                    fileEntity.REMARK = fullName.Split('\\')[fullName.Split('\\').Length - 1];
                fileEntity.ID = (int)_repo.Insert(fileEntity);

                fileEntity.FILE_URL = url + "/" + fileEntity.ID;
                _repo.Update(fileEntity);

            }
            catch (Exception ex)
            {
                throw new BOException("上传图片发生意外错误，请稍后重试，或联系管理员", ex);
            }
            return fileEntity;
        }

        public FILES UploadFile(string ext, string uploadimg, string contentType, byte[] data, string url, string remark)
        {
            FILES fileEntity = null;
            try
            {
                string newName = Guid.NewGuid().ToString("d") + ext;

                string filePath = string.Concat(uploadimg, DateTime.Today.ToString("yyyyMMdd"), "\\");

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string fullName = filePath + newName;
                using (FileStream steam = new FileStream(fullName, FileMode.CreateNew, FileAccess.Write))
                {
                    steam.Write(data, 0, data.Length);
                }

                fileEntity = new FILES();
                fileEntity.CONTENT_TYPE = contentType;
                fileEntity.FILE_NAME = fullName;
                fileEntity.FILE_SIZE = data.Length;
                fileEntity.FILE_URL = "<!--只有图片存放到外部时才有用哦--!>";
                fileEntity.IS_GOOD = 0;
                if (remark != "")
                    fileEntity.REMARK = remark;
                else
                    fileEntity.REMARK = fullName.Split('\\')[fullName.Split('\\').Length - 1];
                fileEntity.ID = (int)_repo.Insert(fileEntity);

                fileEntity.FILE_URL = url + "/" + fileEntity.ID;
                _repo.Update(fileEntity);

            }
            catch (Exception ex)
            {
                throw new BOException("上传图片发生意外错误，请稍后重试，或联系管理员", ex);
            }
            return fileEntity;
        }

        public FILES UploadFile2(string ext, string contentType, byte[] data, string url)
        {
            FILES fileEntity = null;
            try
            {
                string newName = Guid.NewGuid().ToString("d") + ext;

                string filePath = string.Concat(AppConfig.UploadTMP, DateTime.Today.ToString("yyyyMMdd"), "\\");

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string fullName = filePath + newName;
                using (FileStream steam = new FileStream(fullName, FileMode.CreateNew, FileAccess.Write))
                {
                    steam.Write(data, 0, data.Length);
                }

                fileEntity = new FILES();
                fileEntity.CONTENT_TYPE = contentType;
                fileEntity.FILE_NAME = fullName;
                fileEntity.FILE_SIZE = data.Length;
                fileEntity.FILE_URL = "<!--只有图片存放到外部时才有用哦--!>";
                fileEntity.IS_GOOD = 0;

                fileEntity.ID = (int)_repo.Insert(fileEntity);

                fileEntity.FILE_URL = url + "/" + fileEntity.ID;
                _repo.Update(fileEntity);

            }
            catch (Exception ex)
            {
                throw new BOException("上传图片发生意外错误，请稍后重试，或联系管理员", ex);
            }
            return fileEntity;
        }

        public FILES GetUploadFile(int id)
        {
            return _repo.GetFILES(id);
        }



        public List<PROD_CATEGORY> getCATEGORY(int prodType, int orgId)
        {
            throw new NotImplementedException();
        }

        public int SendBookMessage(int orgId, int storeId, int bid, string cname, string phone, string creater)
        {
            throw new NotImplementedException();
        }

        public int SendSaveAmtMessage(int orgId, int storeId, int cid, decimal buyAmt, string creater, int stype)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 插入点赞数据
        /// </summary>
        /// <param name="fansID"></param>
        /// <param name="fileID"></param>
        /// <returns></returns>
        public string InsertGood(string fansID, string fileID)
        {
            return _repo.InsertGood(fansID, fileID);

        }

        /// <summary>
        /// 插入门店
        /// </summary>
        /// <returns></returns>
        public int InsertStore()
        {
            try
            {
                dt_GetMd_req req = new dt_GetMd_req();

                TVKO[] ko = new TVKO[]
                {
                new TVKO() { VKORG = "C004" },
                new TVKO() { VKORG = "C024" }
                };

                req.ZTVKO = ko;

                dt_GetMd_res resStore = WebApiHelp.GetMd(req);

                JavaScriptSerializer js = new JavaScriptSerializer();

                foreach (ZSAL_MD stroeModel in resStore.INFO_MD)
                {
                    Thread.Sleep(100);
                    //如果门店是激活状态
                    if (stroeModel.VTWEG != "28" && stroeModel.VTWEG != "11")
                    {
                        int id = _repo.CheckStore(stroeModel.ZMD_ID);
                        if (id < 1)
                        {
                            TenAPI api = null;

                            if (!string.IsNullOrEmpty(stroeModel.STRAS))
                            {
                                string json = NetHelper.HttpRequest("http://apis.map.qq.com/ws/geocoder/v1/?key=MYVBZ-AO3R3-KVS3G-3OBPF-VNHE5-7VF7M&address=" +
                                    stroeModel.STRAS + "", "",
                                   "GET", 2000, Encoding.UTF8, "application/json");

                                if (!json.Contains("查询无结果"))
                                {
                                    api = js.Deserialize<TenAPI>(json);

                                    if (api.status != 0)
                                    {
                                        api = null;
                                    }
                                }
                            }

                            BCJ_STORES model = new BCJ_STORES()
                            {
                                ADDRESS = stroeModel.STRAS,
                                CITY = stroeModel.CITY_CODE,
                                CITY_CATE = stroeModel.ZCITY_TYPE,
                                NAME = stroeModel.ZMD_MC,
                                POS_CODE = stroeModel.ZMD_ID,
                                KUNNR_SH = stroeModel.ZKUNNR_SH,
                                TEL = stroeModel.TELF1,
                                STORE_TYPE = stroeModel.ZMD_TYPE,
                                LONGITUDE = api == null ? "无" : api.result.location.lng,
                                LATITUDE = api == null ? "无" : api.result.location.lat,
                                STATUS = string.IsNullOrEmpty(stroeModel.ZSTATUS) ? -1 : int.Parse(stroeModel.ZSTATUS)
                            };

                            _repo.Insert(model);
                        }
                        //更新门店信息
                        else
                        {
                            BCJ_STORES model = new BCJ_STORES()
                            {
                                ID = id,
                                ADDRESS = stroeModel.STRAS,
                                CITY = stroeModel.CITY_CODE,
                                CITY_CATE = stroeModel.ZCITY_TYPE,
                                NAME = stroeModel.ZMD_MC,
                                POS_CODE = stroeModel.ZMD_ID,
                                KUNNR_SH = stroeModel.ZKUNNR_SH,
                                TEL = stroeModel.TELF1,
                                STATUS = string.IsNullOrEmpty(stroeModel.ZSTATUS) ? -1 : int.Parse(stroeModel.ZSTATUS)
                            };
                            _repo.Update(model);
                        }
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 加载城市信息
        /// </summary>
        /// <returns></returns>
        public int InsertCity()
        {
            List<BCJ_CITY> citys = _book.GetCity();

            JavaScriptSerializer js = new JavaScriptSerializer();

            foreach (var city in citys)
            {
                Thread.Sleep(1000);
                string json = NetHelper.HttpRequest("http://apis.map.qq.com/ws/geocoder/v1/?key=MYVBZ-AO3R3-KVS3G-3OBPF-VNHE5-7VF7M&address=" +
                                    city.CITY_NAME + "", "",
                                   "GET", 2000, Encoding.UTF8, "application/json");
                TenAPI api = null;
                if (!json.Contains("查询无结果"))
                {
                    api = js.Deserialize<TenAPI>(json);

                    if (api.status != 0)
                    {
                        api = null;
                    }
                }
                else
                {

                }

                _book.Update(new BCJ_CITY()
                {
                    ID = city.ID,
                    CITY_CODE = city.CITY_CODE,
                    CITY_NAME = city.CITY_NAME,
                    LATITUDE = api == null ? "无" : api.result.location.lat,
                    LONGITUDE = api == null ? "无" : api.result.location.lng
                });
            }

            return 1;
        }
    }
}
