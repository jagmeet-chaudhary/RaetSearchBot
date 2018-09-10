using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBot.Connectors.HRM.Model
{
        public class HrmEmployee
        {
            public Person[] Items { get; set; }
        }

        public class Person
        {
            public Contract Contract { get; set; }
            public long Id { get; set; }
            public string Number { get; set; }
            public string GivenName { get; set; }
            public string Prefix { get; set; }
            public string FamilyName { get; set; }
            public string BirthName { get; set; }
            public string PrefixAtBirth { get; set; }
            public string FamilyNameAtBirth { get; set; }
            public string Initials { get; set; }
            public string ExternalId { get; set; }
            public Gender Gender { get; set; }
            public string DisplayName { get; set; }
            public DateTime DateOfBirth { get; set; }
            public string CommunicationLanguage { get; set; }
            public Extensions Extensions { get; set; }
            public Metadata[] Metadata { get; set; }
        }

        public class Contract
        {
            public long Id { get; set; }
            public string ExternalId { get; set; }
            public string ContractNumber { get; set; }
            public Company Company { get; set; }
            public Location Location { get; set; }
            public string Country { get; set; }
            public Organizationalunit OrganizationalUnit { get; set; }
            public DateTime HireDate { get; set; }
            public DateTime DischargeDate { get; set; }
            public Workpattern WorkPattern { get; set; }
            public Recurringcompensation[] RecurringCompensation { get; set; }
            public Contracttype ContractType { get; set; }
            public Expensepolicy ExpensePolicy { get; set; }
            public Jobprofile JobProfile { get; set; }
            public string ReportType { get; set; }
        }

        public class Company
        {
            public long Id { get; set; }
            public string ShortName { get; set; }
            public string FullName { get; set; }
        }

        public class Location
        {
            public long Id { get; set; }
            public string ShortName { get; set; }
            public string FullName { get; set; }
        }

        public class Organizationalunit
        {
            public long Id { get; set; }
            public string ShortName { get; set; }
            public string FullName { get; set; }
        }

        public class Workpattern
        {
            public bool WorkPatternInWeeks { get; set; }
            public int WorkPatternStartDay { get; set; }
            public Day[] Days { get; set; }
            public int WorkPatternStartSelection { get; set; }
        }

        public class Day
        {
            public int DayIndexNumber { get; set; }
            public int Hours { get; set; }
        }

        public class Contracttype
        {
            public long Id { get; set; }
            public string ShortName { get; set; }
            public string FullName { get; set; }
        }

        public class Expensepolicy
        {
            public long Id { get; set; }
            public string ShortName { get; set; }
            public string FullName { get; set; }
        }

        public class Jobprofile
        {
            public long Id { get; set; }
            public string ShortName { get; set; }
            public string FullName { get; set; }
        }

        public class Recurringcompensation
        {
            public Paycomponent PayComponent { get; set; }
            public long PayComponentId { get; set; }
            public int Value { get; set; }
            public string Text { get; set; }
        }

        public class Paycomponent
        {
            public Paycomponenttype PayComponentType { get; set; }
            public Currency Currency { get; set; }
            public Frequency Frequency { get; set; }
            public Version Version { get; set; }
            public long Id { get; set; }
            public string ShortName { get; set; }
            public string FullName { get; set; }
        }

        public class Paycomponenttype
        {
            public long Id { get; set; }
            public string ShortName { get; set; }
            public string FullName { get; set; }
        }

        public class Currency
        {
            public int Decimals { get; set; }
            public long Id { get; set; }
            public string ShortName { get; set; }
            public string FullName { get; set; }
        }

        public class Frequency
        {
            public long Id { get; set; }
            public string ShortName { get; set; }
            public string FullName { get; set; }
        }

        public class Version
        {
            public long VersionId { get; set; }
            public string Description { get; set; }
            public int FixedValue { get; set; }
            public bool CompensationMetrics { get; set; }
            public Validityperiod ValidityPeriod { get; set; }
        }

        public class Validityperiod
        {
            public DateTime VersionValidFrom { get; set; }
            public DateTime VersionValidTo { get; set; }
        }

        public class Gender
        {
            public long Id { get; set; }
            public string ShortName { get; set; }
            public string FullName { get; set; }
        }

        public class Extensions
        {
            public Vendor Vendor { get; set; }
            public Custom Custom { get; set; }
        }

        public class Vendor
        {
        }

        public class Custom
        {
        }

        public class Metadata
        {
            public string ShortName { get; set; }
            public string FullName { get; set; }
            public string Owner { get; set; }
            public bool Versioned { get; set; }
            public string DataType { get; set; }
            public List List { get; set; }
            public Group Group { get; set; }
            public int Sequence { get; set; }
            public bool Mandatory { get; set; }
            public bool Hidden { get; set; }
            public string DefaultValue { get; set; }
        }

        public class List
        {
            public string ShortName { get; set; }
            public string FullName { get; set; }
            public Listvalue[] ListValues { get; set; }
        }

        public class Listvalue
        {
            public string FullName { get; set; }
            public string Index { get; set; }
        }

        public class Group
        {
            public long Id { get; set; }
            public string ShortName { get; set; }
            public string FullName { get; set; }
            public string Owner { get; set; }
        }

    }

