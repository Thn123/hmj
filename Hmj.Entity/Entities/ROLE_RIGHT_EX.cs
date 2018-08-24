using System.Collections.Generic;
namespace Hmj.Entity.Entities
{
    public class ROLE_RIGHT_EX
    {
        private SYS_ROLE role = new SYS_ROLE();
        public SYS_ROLE ROLE
        {
            get { return role; }
            set { role = value; }
        }

        private SYS_RIGHT right = new SYS_RIGHT();
        public SYS_RIGHT RIGHT
        {
            get { return right; }
            set { right = value; }
        }

        private SYS_ROLE_RIGHT rr = new SYS_ROLE_RIGHT();
        public SYS_ROLE_RIGHT RR
        {
            get { return rr; }
            set { rr = value; }
        }

        private List<LISTRIGHT_EX> listright;

        public List<LISTRIGHT_EX> LISTRIGHT
        {
            get { return listright; }
            set { listright = value; }
        }



    }
}