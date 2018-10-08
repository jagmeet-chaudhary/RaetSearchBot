using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBot.Model
{
    public class AuditChangeContextDto
    {
        public long EntityId { get; set; }
        public long AuditEntityId { get; set; }
        public string EntityName { get; set; }
        public string InitiatorId { get; set; }
        public string InitiatorName { get; set; }
        public string SubjectName { get; set; }
        public long? SubjectId { get; set; }
        public string ChangeType { get; set; }
        //[JsonConverter(typeof(ISODateTimeConvertor))]
        public DateTime ChangedDate { get; set; }

    }
}
