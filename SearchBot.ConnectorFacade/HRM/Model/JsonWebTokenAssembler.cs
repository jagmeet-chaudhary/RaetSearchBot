
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace SearchBot.Connectors.HRM.Model
{
    public class JsonWebTokenAssembler
    {
        private const string UNIQUE_NAME = "unique_name";
        private const string IDENT = "ident";
        private const string UPN = "upn";
        private const string TENANT_ID = "tid";
        private const string SOURCE_ID = "sourceid";
        private const string SOURCE_SYSTEM = "sourcesystem";
        private const string CLIENT_ID = "client_id";
        private const string ISS = "iss";
        private const string SESSION_ID = "pi.sri";
        private const string DOMAIN = "";
        private const string UID = "uid";
        private const string FAMILYNAME = "family_name";

        public JsonWebToken ListOfClaimsToJsonWebToken(IEnumerable<Claim> claims)
        {


            JsonWebToken jsonToken = new JsonWebToken
            {
                Iss = GetIss(claims),
                TenantId = GetTenantId(claims),
                Name = GetUserName(claims),
                SourceId = GetSourceId(claims),
                SourceSystem = GetSourceSystem(claims),
                ClientId = GetClientId(claims),
                SessionId = GetSessionId(claims),
                Domain = GetDomain(claims),
                Uid = GetUid(claims)
            };

            return jsonToken;
        }

        private string GetUserName(IEnumerable<Claim> claims)
        {
            var name = claims.ToList().Find(c => c.Type == "name")?.Value;
            if (name == null)
                name = string.Empty;
            return name;
        }

        private string GetTenantId(IEnumerable<Claim> claims)
        {
            return claims.ToList().Find(c => c.Type == TENANT_ID)?.Value;
        }

        private string GetSourceId(IEnumerable<Claim> claims)
        {
            var sourceId = claims.ToList().Find(c => c.Type == SOURCE_ID)?.Value;
            if (sourceId == null)
                sourceId = string.Empty;
            return sourceId;
        }

        private string GetSourceSystem(IEnumerable<Claim> claims)
        {
            var sourceSystem = claims.ToList().Find(c => c.Type == SOURCE_SYSTEM)?.Value;
            if (sourceSystem == null)
                sourceSystem = string.Empty;
            return sourceSystem;
        }

        private string GetClientId(IEnumerable<Claim> claims)
        {
            var clientId = claims.ToList().Find(c => c.Type == CLIENT_ID)?.Value;
            if (clientId == null)
                clientId = string.Empty;
            return clientId;
        }

        private string GetIss(IEnumerable<Claim> claims)
        {
            var clientId = claims.ToList().Find(c => c.Type == ISS)?.Value;
            if (clientId == null)
                clientId = string.Empty;
            return clientId;
        }

        private string GetSessionId(IEnumerable<Claim> claims)
        {
            var sessionId = claims.ToList().Find(c => c.Type == SESSION_ID)?.Value;
            if (sessionId == null)
                sessionId = string.Empty;
            return sessionId;
        }

        private string GetDomain(IEnumerable<Claim> claims)
        {
            var ident = claims.ToList().Find(c => c.Type == IDENT || c.Type == UPN)?.Value;
            if (ident == null)
                ident = string.Empty;

            var regex = new Regex("@.+$");
            var domain = regex.Match(ident).ToString();

            return domain;
        }

        private string GetUid(IEnumerable<Claim> claims)
        {
            var uid = claims.ToList().Find(c => c.Type == UID || c.Type == FAMILYNAME)?.Value ?? Guid.Empty.ToString();
            return uid;
        }

    }
}
