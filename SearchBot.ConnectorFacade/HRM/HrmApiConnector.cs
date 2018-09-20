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
        public HrmApiConnector(IRequestHelper requestHelper,ITokenProvider tokenProvider)
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

        private void PrepareRequest()
        {
            requestHelper.Init("HrmBaseUri");
            requestHelper.AuthenticationToken = tokenProvider.GetToken();
            var tenantId = "188a2e34-410b-41af-a501-8e99482a8e8e";
            requestHelper.AddClientHeader("x-raet-tenant-id", tenantId);
        }
         
    }
}
