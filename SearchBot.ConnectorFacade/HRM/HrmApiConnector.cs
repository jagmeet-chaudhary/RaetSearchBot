using SearchBot.Common;
using SearchBot.Common.Exceptions;
using SearchBot.Connectors.HRM.Model;
using SearchBot.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SearchBot.Connectors.HRM
{
    public class HrmApiConnector : IHrmConnector
    {
        private IRequestHelper requestHelper;
        private ITokenProvider tokenProvider;

        public HrmApiConnector(IRequestHelper requestHelper, ITokenProvider tokenProvider)
        {
            this.requestHelper = requestHelper;
            this.tokenProvider = tokenProvider;
        }

        public Employee GetManagerForEmployee(Employee employee, string token)
        {
            try
            {
                var searchResult = SearchEmployees(employee, token).FirstOrDefault();
                requestHelper.Init("HrmBaseUri");
                requestHelper.AuthenticationToken = token;
                var result = requestHelper.GetWithResponseAsync($"/api/organizationalunits").Result;
                var hrmOrganizationUnits = requestHelper.GetAsync<List<HrmOrganizationalUnit>>($"/api/organizationalunits").Result;
                var manager = hrmOrganizationUnits.FirstOrDefault(x => x.Id == employee.OrgUnitId).Version.Managers.FirstOrDefault();
                return new Employee()
                {
                    FirstName = manager.Name,
                };
            }
            catch (Exception ex)
            {

                ProcessException(ex, "GetManagerForEmployee");
            }
            return null;
        }

        public List<Employee> SearchEmployees(Employee employeeToSearch, string token)
        {
            try
            {
                requestHelper.Init("HrmBaseUri");
                requestHelper.AuthenticationToken = token;
                var hrmEmployees = requestHelper.GetAsync<HrmEmployees>("api/employees").Result;
                var searchResult = new List<Employee>();
                Predicate<Person> p = x => false;
                if (!String.IsNullOrWhiteSpace(employeeToSearch.FirstName) && !String.IsNullOrWhiteSpace(employeeToSearch.LastName))
                {
                    p = x => x.FamilyName?.ToUpper() == employeeToSearch?.LastName.ToUpper() && x.BirthName?.ToUpper() == employeeToSearch.FirstName.ToUpper();
                }
                else if (!String.IsNullOrWhiteSpace(employeeToSearch.FirstName))
                {
                    p = x => x.BirthName?.ToUpper() == employeeToSearch.FirstName.ToUpper();
                }
                else if (!String.IsNullOrWhiteSpace(employeeToSearch.LastName))
                {
                    p = x => x.FamilyName?.ToUpper() == employeeToSearch?.LastName.ToUpper() || x.GivenName?.ToUpper() == employeeToSearch.LastName.ToUpper();
                }

                var filteredResults = hrmEmployees.Items.Where(x => p(x));

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
            catch (Exception ex)
            {

                ProcessException(ex, "SearchEmployees");
            }
            return null;
        }

        public AuditChangeContextDto GetOrgUnitChangeByDates(string orgUnitName, string start, string end, string token)
        {
            try
            {
                requestHelper.Init("HrmBaseUri");
                requestHelper.AuthenticationToken = token;

                string testurl = $"api/auditreader/odm/?$count=true&$top=25&$skip=0&$filter=EntityName%20eq%20%27HRM_OrganizationalUnit%27%20and%20ChangedDate%20ge%20{start}%20and%20ChangedDate%20le%20{end}&$orderby=ChangedDate%20desc";
                var auditChanges = requestHelper.GetAsync<OdataAuditContextDto>(testurl).Result;
                return auditChanges.Items.FirstOrDefault(s => s.SubjectName.ToLower() == orgUnitName);
            }
            catch (Exception ex)
            {

                ProcessException(ex, "GetOrgUnitByName");
            }

            return null;
        }
        public AuditChangeContextDto GetOrgUnitByName(string orgUnitName, string token)
        {
            try
            {
                requestHelper.Init("HrmBaseUri");
                requestHelper.AuthenticationToken = token;

                string testurl = "api/auditreader/odm/?$count=true&$top=25&$skip=0&$filter=EntityName%20eq%20%27HRM_OrganizationalUnit%27%20and%20ChangedDate%20ge%202016-07-31T18:30:00Z%20and%20ChangedDate%20le%202018-09-19T18:29:59Z&$orderby=ChangedDate%20desc";
                var hrmEmployees = requestHelper.GetAsync<OdataAuditContextDto>(testurl).Result;
                return hrmEmployees.Items.FirstOrDefault(s => s.SubjectName.ToLower() == orgUnitName);
            }
            catch (Exception ex)
            {

                ProcessException(ex, "GetOrgUnitByName");
            }

            return null;
        }

        public ResultTaskDto GetPendingTaskForEmployee(string token)
        {
            try
            {
                requestHelper.Init("WorkFlowBaseUri");
                requestHelper.AuthenticationToken = token;

                string testurl = "api/tasks/Pending?$count=true&$top=5&$skip=0";
                var pendingTask = requestHelper.GetAsync<ResultTaskDto>(testurl).Result;
                return pendingTask;
            }
            catch (Exception ex)
            {

                ProcessException(ex, "GetPendingTaskForEmployee");
            }
            return null;
        }

        public string GetUserImage(string externalId, string token)
        {
            requestHelper.Init("UIApiUrl");
            requestHelper.AuthenticationToken = token;

            try
            {
                string apiUrl = $"api/photos/person?externalId={externalId}";
                var userImage = requestHelper.GetAsync<string>(apiUrl).Result;
                return userImage;
            }
            catch (Exception ex)
            {
                ProcessException(ex, "GetUserImage");
            }

            return null;
        }

        public PersonDetails GetUserDetails(string externalId, string token)
        {
            try
            {
                requestHelper.Init("HrmBaseUri");
                requestHelper.AuthenticationToken = token;

                string apiUrl = $"api/persons?externalId={externalId}";
                var personDetails = requestHelper.GetAsync<PersonDetails>(apiUrl).Result;
                return personDetails;
            }
            
            catch (Exception ex)
            {
                ProcessException(ex, "GetUserDetals");
            }
            return null;
    
        }

        public IList<SickLeave_Employee> GetSickLeaveEmployees(string from, string to, string token)
        {
            try
            {
                requestHelper.Init("HrmBaseUri");
                requestHelper.AuthenticationToken = token;

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

                ProcessException(ex, "Sickleave");
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

        private void ProcessException(Exception ex,string logPrefix)
        {
            LogFactory.Log.Error($"{logPrefix} : Exception : {ex.Message}");

            if (ex.InnerException is NotAuthorizedException)
                throw ex;
        }
    }
}