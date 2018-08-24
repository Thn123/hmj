using SPACRM.Entity;
using SPACRM.Entity.PageSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPACRM.Interface.Service
{
    public interface ISystemSetService
    {
        PagedList<SYS_DICT> QueryGoodsList(PageView view);
    }
}
