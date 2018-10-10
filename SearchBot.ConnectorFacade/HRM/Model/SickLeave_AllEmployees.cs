using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBot.Connectors.HRM.Model
{

    public class SickLeave_AllEmployees
    {
        public SickLeave_AllEmployee[] Property1 { get; set; }
    }

    public class SickLeave_AllEmployee
    {
        public string DisplayName { get; set; }
        public string ContractId { get; set; }
        public string ExternalId { get; set; }
    }

}
