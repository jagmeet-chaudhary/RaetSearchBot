using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBot.Connectors.HRM.Model
{

    public class PersonDetails
    {
        public string Id { get; set; }
        public string PersonNumber { get; set; }
        public string LastNameAtBirth { get; set; }
        public string PrefixLastNameAtBirth { get; set; }
        public string LastNameToUse { get; set; }
        public string PrefixLastNameToUse { get; set; }
        public string Initials { get; set; }
        public string FirstNames { get; set; }
        public string KnownAs { get; set; }
        public string DisplayName { get; set; }
        public string TitlePrefix { get; set; }
        public string TitleSuffix { get; set; }
        public string GenderId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfDeath { get; set; }
        public string CountryOfBirthId { get; set; }
        public string CountryOfBirth { get; set; }
        public string PrimaryNationalityId { get; set; }
        public string PrimaryNationality { get; set; }
        
    }

}
