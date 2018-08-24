using Hmj.DataAccess.MiniProgram;
using Hmj.Entity;
using Hmj.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hmj.Business
{
    public class MiniProgramService: IMiniProgramService
    {
        MiniProgramRepository _repo;
        public MiniProgramService()
        {
            _repo = new MiniProgramRepository();
        }

        public long insertMiniKey(Mini_sessionkey model)
        {
            return _repo.Insert(model);
        }

    }
}
