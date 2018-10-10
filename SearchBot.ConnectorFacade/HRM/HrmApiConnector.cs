using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchBot.Common;
using SearchBot.Model;
using SearchBot.Connectors.HRM.Model;

namespace SearchBot.Connectors.HRM
{
    public class HrmApiConnector : IHrmConnector
    {
        IRequestHelper requestHelper;
        ITokenProvider tokenProvider;
        public HrmApiConnector(IRequestHelper requestHelper, ITokenProvider tokenProvider)
        {
            this.requestHelper = requestHelper;
            this.tokenProvider = tokenProvider;
        }

        public Employee GetManagerForEmployee(Employee employee)
        {
            var searchResult = SearchEmployees(employee).FirstOrDefault();
            PrepareRequest();
            var hrmOrganizationUnits = requestHelper.GetAsync<HrmOrganizationalUnits>($"api/employees/{searchResult.Id}/Manager").Result;
            var manager = hrmOrganizationUnits.Items.FirstOrDefault().Version.Managers.FirstOrDefault();
            return new Employee()
            {
                FirstName = manager.Name,
                
            };
         }
        public List<Employee> SearchEmployees(Employee employeeToSearch)
        {

            PrepareRequest();
            var hrmEmployees = requestHelper.GetAsync<HrmEmployees>("api/employees").Result;
            var searchResult =new  List<Employee>();
            foreach (var hrmEmployee in hrmEmployees.Items.Where(x => x.FamilyName == employeeToSearch.LastName || x.BirthName == employeeToSearch.FirstName))
            {
                var employee = new Employee()
                {
                    Id = hrmEmployee.Id,
                    FirstName = hrmEmployee.BirthName,
                    LastName = hrmEmployee.FamilyName
                };
                searchResult.Add(employee);
            }

            return searchResult;
        }

        public AuditChangeContextDto GetOrgUnitByName(string orgUnitName, string token)
        {
            requestHelper.Init("HrmBaseUri");
            requestHelper.AuthenticationToken = tokenProvider.GetToken();
            var tenantId = "188a2e34-410b-41af-a501-8e99482a8e8e";
            requestHelper.AddClientHeader("x-raet-tenant-id", tenantId);
            try
            {
                string testurl = "api/auditreader/odm/?$count=true&$top=25&$skip=0&$filter=EntityName%20eq%20%27HRM_OrganizationalUnit%27%20and%20ChangedDate%20ge%202016-07-31T18:30:00Z%20and%20ChangedDate%20le%202018-09-19T18:29:59Z&$orderby=ChangedDate%20desc";
                var hrmEmployees = requestHelper.GetAsync<OdataAuditContextDto>(testurl).Result;
                return hrmEmployees.Items.FirstOrDefault(s => s.SubjectName.ToLower() == orgUnitName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("the test");
            }

            return null;
        }

        public ResultTaskDto GetPendingTaskForEmployee(string token)
        {
            requestHelper.Init("WorkFlowBaseUri");
            requestHelper.AuthenticationToken = token;
            //var tenantId = "188a2e34-410b-41af-a501-8e99482a8e8e";
            //requestHelper.AddClientHeader("x-raet-tenant-id", tenantId);
            try
            {
                string testurl = "api/tasks/Pending?$count=true&$top=5&$skip=0";
                var pendingTask = requestHelper.GetAsync<ResultTaskDto>(testurl).Result;
                return pendingTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine("the test");
            }



            return null;
        }

        public string GetUserImage(string externalId, string token)
        {
            requestHelper.Init("UIApiUrl");
            requestHelper.AuthenticationToken = token;
            //var tenantId = "188a2e34-410b-41af-a501-8e99482a8e8e";
            //requestHelper.AddClientHeader("x-raet-tenant-id", tenantId);
            try
            {
                string apiUrl = $"api/photos/person?externalId={externalId}";
                var userImage = requestHelper.GetAsync<string>(apiUrl).Result;
                return userImage;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed in getting the user Image");
            }


            return null;
        }

        public PersonDetails GetUserDetails(string externalId, string token)
        {
            requestHelper.Init("HrmBaseUri");
            requestHelper.AuthenticationToken = token;

            try
            {
                string apiUrl = $"api/persons?externalId={externalId}";
                var personDetails = requestHelper.GetAsync<PersonDetails>(apiUrl).Result;
                return personDetails;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed in getting the user Image");
            }


            return null;
        }

        public Employee GetManagerForEmployee(Employee employee, string token)
        {
            throw new NotImplementedException();
        }

        public IList<SickLeave_Employee> GetSickLeaveEmployees(string from, string to, string token)
        {
            requestHelper.Init("HrmBaseUri");
            requestHelper.AuthenticationToken = token;

            try
            {
                string apiUrl = $"api/sickleaves/organizationalunits";
                var SickLeave_orgUnits = requestHelper.GetAsync<IList<SickLeave_orgUnit>>(apiUrl).Result;

                var orgUnit = SickLeave_orgUnits != null && SickLeave_orgUnits.Count() > 0 ? SickLeave_orgUnits[0] : null;

                if (orgUnit == null)
                    return null;

                string allEmployeesURI = $"/api/sickleaves/employees?orgUnitId={orgUnit.Id}";
                var sickLeave_AllEmployees = requestHelper.GetAsync<IList<SickLeave_AllEmployee>>(allEmployeesURI).Result;


                string leaveUri = $"api/sickleaves/organizationalunits/{orgUnit.Id}/sickleaves?startDate={from}&endDate={to}";
                var details = requestHelper.GetAsync<IList<SickLeave_Employee>>(leaveUri).Result;

                foreach (var employee in details)
                {
                    employee.Displayname = sickLeave_AllEmployees.FirstOrDefault(x => x.ContractId == employee.ContractId)?.DisplayName;
                }
                

                return details;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed in getting the user Image");
            }

            return null;
        }

        private void PrepareRequest()
        {
            requestHelper.Init("HrmBaseUri");
            requestHelper.AuthenticationToken = tokenProvider.GetToken();
            var tenantId = "188a2e34-410b-41af-a501-8e99482a8e8e";
            requestHelper.AddClientHeader("x-raet-tenant-id", tenantId);
        }
         
        public string GetOrgUnitIdByPassingName(string OrgUnitName, string token)
        {
            requestHelper.Init("HrmBaseUri");
            requestHelper.AuthenticationToken = token;

            try
            {
                //string apiUrl = $"api/organizationalunits";
                string apiUrl = $"api/sickleaves/organizationalunits/27/sickleaves?startDate='2010-01-01'&endDate='2020-01-01'";
                var details = requestHelper.GetAsync<OrgUnitDetails>(apiUrl).Result;
                //var orgUnitId = details.Property1.FirstOrDefault(x => x.Metadata.Where(z => z.FullName.ToLower() == OrgUnitName));
                return "";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed in getting the user Image");
            }


            return null;
        }
    }
}
