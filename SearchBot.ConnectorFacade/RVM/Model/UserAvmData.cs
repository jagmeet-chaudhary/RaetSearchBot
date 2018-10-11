using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SearchBot.Connectors.RVM.Model
{
    public class UserAvmData
    {
        public UserAvmDataForEmployee[] value { get; set; }
    }

    public class UserAvmDataForEmployee
    {
        public string personId { get; set; }
        public string name { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public bool active { get; set; }
        public bool autoInactive { get; set; }
        public string raetUserId { get; set; }
        public string creationDate { get; set; }
        public string endDate { get; set; }
        public Userrole[] userRoles { get; set; }
        public string id { get; set; }
    }

    public class Userrole
    {
        public string roleId { get; set; }
        public string userGroupId { get; set; }
        public bool basic { get; set; }
    }



}
