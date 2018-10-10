using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBot.Connectors.HRM.Model
{

    public class SickLeave_orgUnits
    {
        public IList<SickLeave_orgUnit> Items { get; set; }
    }

    public class SickLeave_orgUnit
    {
        public string Id { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
    }

}
