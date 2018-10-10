using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBot.Connectors.HRM.Model
{


    public class SickLeave_Employees
    {
        public SickLeave_Employee[] Property1 { get; set; }
    }

    public class SickLeave_Employee
    {
        public string Id { get; set; }
        public string Displayname { get; set; }
        public string ContractId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string SickLeaveTypeId { get; set; }
        public Sickleavetype SickLeaveType { get; set; }
        public string SickLeaveStatus { get; set; }
    }

    public class Sickleavetype
    {
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string ReferenceId { get; set; }
    }
}
