using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBot.Connectors.HRM.Model
{

    public class HrmOrganizationalUnits
    {
        public OrgUnit[] Items { get; set; }
    }

    public class OrgUnit
    {
        public OrgUnitVersion Version { get; set; }
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public bool Active { get; set; }
        public string ExternalId { get; set; }
        public Extensions Extensions { get; set; }
        public Metadata[] Metadata { get; set; }
    }

    public class OrgUnitVersion
    {
        public int VersionId { get; set; }
        public Parent Parent { get; set; }
        public Organizationalunittype OrganizationalUnitType { get; set; }
        public Financialdimension FinancialDimension { get; set; }
        public Manager[] Managers { get; set; }
        public int OrganizationalUnitTypeId { get; set; }
        public int ParentId { get; set; }
        public int FinancialDimensionId { get; set; }
        public Validityperiod ValidityPeriod { get; set; }
    }

    public class Parent
    {
        public string ShortName { get; set; }
        public string FullName { get; set; }
    }

    public class Organizationalunittype
    {
        public string ShortName { get; set; }
        public string FullName { get; set; }
    }

    public class Financialdimension
    {
        public string ShortName { get; set; }
        public string FullName { get; set; }
    }


    public class Manager
    {
        public int PersonId { get; set; }
        public string PersonNumber { get; set; }
        public string Name { get; set; }
        public string ExternalId { get; set; }
        public Gender Gender { get; set; }
    }


}
