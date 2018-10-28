using SearchBot.Common;
using SearchBot.Connectors.RVM.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SearchBot.Connectors.RVM
{
    public class RVMConnector : IRVMConnector
    {

        IRequestHelper requestHelper;

        public RVMConnector(IRequestHelper requestHelper)
        {
            this.requestHelper = requestHelper;

        }
        public bool ResetPassword(string username, string newPassword)
        {

            try
            {
                var client = new HttpClient();

                var userId = GetRvmUserId(username);

                var response = PatchAsJsonAsync(client, $"http://rl-tstrvmiis02/MlmApi/v1.0/users({userId})", new resetpassword() { password = newPassword }).Result;

                return true;
            }
            catch (Exception)
            {

                throw;
            }


        }

        private string GetRvmUserId(string username)
        {

            requestHelper.Init("MlmApi");
            requestHelper.AuthenticationToken = ConfigurationManager.AppSettings["MlmApi_Token"];

            try
            {

                string testurl = $"v1.0/users?$filter={HttpUtility.UrlPathEncode($"name eq '{username}'")}";

                testurl = testurl.Replace("'", "%27");

                var mlmEmployees = requestHelper.GetAsync<UserAvmData>(testurl).Result;


                var employeeId = mlmEmployees.value.FirstOrDefault().id;


                return employeeId;
            }
            catch (Exception ex)
            {
                Console.WriteLine("the test");
            }

            return null;

        }

        private static Task<HttpResponseMessage> PatchAsJsonAsync<T>(HttpClient client, string requestUri, T value)
        {

            var content = new ObjectContent<T>(value, new JsonMediaTypeFormatter());
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri) { Content = content };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ConfigurationManager.AppSettings["MlmApi_Token"]);
            return client.SendAsync(request);
        }

    }

    public interface IRVMConnector
    {
        bool ResetPassword(string username, string newPassword);

        // string GetRvmUserId(string username);
    }

    public class resetpassword
    {
        public string password { get; set; }
    }
}
