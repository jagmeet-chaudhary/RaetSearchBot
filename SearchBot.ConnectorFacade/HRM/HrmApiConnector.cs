using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SearchBot.Common;
using SearchBot.Model;
using SearchBot.Connectors.HRM.Model;
using System.Linq.Expressions;

namespace SearchBot.Connectors.HRM
{
    public class HrmApiConnector : IHrmConnector
    {
        IRequestHelper requestHelper;
        ITokenProvider tokenProvider;
        public HrmApiConnector(IRequestHelper requestHelper,ITokenProvider tokenProvider)
        {
            this.requestHelper = requestHelper;
            this.tokenProvider = tokenProvider;
        }
        public Employee GetManagerForEmployee(Employee employee)
        {
            var searchResult = SearchEmployees(employee).FirstOrDefault();
            PrepareRequest();
            var result = requestHelper.GetWithResponseAsync($"/api/organizationalunits").Result;
            var hrmOrganizationUnits = requestHelper.GetAsync<List<HrmOrganizationalUnit>>($"/api/organizationalunits").Result;
            var manager = hrmOrganizationUnits.FirstOrDefault(x => x.Id == employee.OrgUnitId).Version.Managers.FirstOrDefault();
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
            Predicate<Person> p = x=>false;
            if (!String.IsNullOrWhiteSpace(employeeToSearch.FirstName) && !String.IsNullOrWhiteSpace(employeeToSearch.LastName))
            {
                p = x => x.FamilyName?.ToUpper() == employeeToSearch?.LastName.ToUpper() && x.BirthName?.ToUpper() == employeeToSearch.FirstName.ToUpper();
            }
            else if(!String.IsNullOrWhiteSpace(employeeToSearch.FirstName))
            {
                 p = x =>  x.BirthName?.ToUpper() == employeeToSearch.FirstName.ToUpper();
            }
            else if(!String.IsNullOrWhiteSpace(employeeToSearch.LastName))
            {
                 p = x => x.FamilyName?.ToUpper() == employeeToSearch?.LastName.ToUpper() || x.GivenName?.ToUpper() == employeeToSearch.LastName.ToUpper();
            }
            
            var filteredResults = hrmEmployees.Items.Where(x=>p(x));

            
            foreach (var hrmEmployee in filteredResults)
            {
                var employee = new Employee()
                {
                    OrgUnitId = hrmEmployee.Contract.OrganizationalUnit.Id,
                    Id = hrmEmployee.Id,
                    FirstName = hrmEmployee.BirthName,
                    LastName = hrmEmployee.FamilyName
                };
                searchResult.Add(employee);
            };

            return searchResult;
        }


        public AuditChangeContextDto GetOrgUnitByName(string orgUnitName)
        {
            requestHelper.Init("HrmBaseUri");
            requestHelper.AuthenticationToken = tokenProvider.GetToken();
            var tenantId = "188a2e34-410b-41af-a501-8e99482a8e8e";
            requestHelper.AddClientHeader("x-raet-tenant-id", tenantId);
            try
            {
                string testurl = "api/auditreader/odm/?$count=true&$top=25&$skip=0&$filter=EntityName%20eq%20%27HRM_OrganizationalUnit%27%20and%20ChangedDate%20ge%202018-07-31T18:30:00Z%20and%20ChangedDate%20le%202018-09-19T18:29:59Z&$orderby=ChangedDate%20desc";
                var hrmEmployees = requestHelper.GetAsync<OdataAuditContextDto>(testurl).Result;
                return hrmEmployees.Items.FirstOrDefault(s => s.SubjectName.ToLower() == orgUnitName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("the test");
            }



            return null;
        }

        public ResultTaskDto GetPendingTaskForEmployee()
        {
            requestHelper.Init("WorkFlowBaseUri");
            requestHelper.AuthenticationToken = tokenProvider.GetToken();
            var tenantId = "188a2e34-410b-41af-a501-8e99482a8e8e";
            requestHelper.AddClientHeader("x-raet-tenant-id", tenantId);
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

        private void PrepareRequest()
        {
            requestHelper.Init("HrmBaseUri");
            requestHelper.AuthenticationToken = tokenProvider.GetToken();
            var tenantId = "188a2e34-410b-41af-a501-8e99482a8e8e";
            requestHelper.AddClientHeader("x-raet-tenant-id", tenantId);
        }
         
    }
}
