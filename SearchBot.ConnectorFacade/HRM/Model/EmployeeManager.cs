using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBot.Connectors.HRM.Model
{

    //public class HrmOrganizationalUnit
    //{
    //    public Item[] Items { get; set; }
    //}

    public class HrmOrganizationalUnit
    {
        public OrgUnitVersion Version { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public bool Active { get; set; }
        public string ExternalId { get; set; }

        public long Id { get; set; }
    }

    public class OrgUnitVersion
    {
        public long OrganizationalUnitTypeId { get; set; }
        public long ParentId { get; set; }
        public long FinancialDimensionId { get; set; }
        public Manager[] Managers { get; set; }
        public Validityperiod ValidityPeriod { get; set; }
    }

    
    public class Manager
    {
        public long PersonId { get; set; }
        public string PersonNumber { get; set; }
        public string Name { get; set; }
        public string ExternalId { get; set; }
        public Gender Gender { get; set; }
        public bool IsPrimary { get; set; }
    }


}
