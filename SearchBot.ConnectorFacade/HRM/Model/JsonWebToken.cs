using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBot.Connectors.HRM.Model
{
    public class JsonWebToken
    {
        public string Iss { get; set; }
        public string TenantId { get; set; }
        public string Name { get; set; }
        public string SourceId { get; set; }
        public string SourceSystem { get; set; }
        public string ClientId { get; set; }
        public string SessionId { get; set; }
        public string Domain { get; set; }
        public string Uid { get; set; }
        public JsonWebToken()
        {
        }

        public JsonWebToken(string tenantId)
        {
            this.TenantId = tenantId;
            this.Iss = string.Empty;
            this.Name = string.Empty;
            this.SourceId = string.Empty;
            this.SourceSystem = string.Empty;
            this.ClientId = string.Empty;
            this.SessionId = string.Empty;
            this.Domain = string.Empty;
            this.Uid = Guid.Empty.ToString();
        }
    }
}
