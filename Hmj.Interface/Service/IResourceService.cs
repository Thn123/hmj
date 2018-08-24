using System;
using System.Collections.Generic;

namespace Hmj.Interface
{
    public interface IResourceService
    {
        List<IResource> GetResourceByCode(string code);

        List<IResource> GetResourceByCode(string code, bool hasAllOption);

        List<IResource> GetResourceByCode(string code, string parentCode, bool hasAllOption);
        List<IResource> GetResourceByCode(string code, string parentCode, string regionid, string store, bool hasAllOption);
        List<IResource> GetResourceByCode(string code, string parentCode, bool hasAllOption, string Merchants_ID);

        //得到空闲技师信息 考虑预约冲突和排版的因素
        List<IResource> GetResourceByCode(string code, string parentCode, int storeId,int orgid, DateTime beginTime, DateTime endTime, bool? gender, bool hasAllOption);

        List<IResource> GetResourceByCode(string code, string parentCode, bool hasAllOption, double LowerAmt, int type);

    }
}
