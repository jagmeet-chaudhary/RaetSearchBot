using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBot.Connectors.HRM.Model
{

    public class OrgUnitDetails
    {
        public OrgDetail[] Property1 { get; set; }
    }

    public class OrgDetail
    {
        public Version Version { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public bool Active { get; set; }
        public string ExternalId { get; set; }
        public Extensions Extensions { get; set; }
        public abc[] Metadata { get; set; }
    }


    public class abc
    {
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string Owner { get; set; }
        public bool Versioned { get; set; }
        public string DataType { get; set; }
        public List List { get; set; }
        public IdData Group { get; set; }
        public int Sequence { get; set; }
        public bool Mandatory { get; set; }
        public bool ReadOnly { get; set; }
        public bool Hidden { get; set; }
        public string DefaultValue { get; set; }
    }


    public class IdData
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string Owner { get; set; }
    }

}
