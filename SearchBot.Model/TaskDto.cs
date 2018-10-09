using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBot.Model
{
    public class TaskDto
    {

        public string Id { get; set; }
        public string ProcessId { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public string RelatedUi { get; set; }
        public string StatusShortName { get; set; }
        public string StatusFullName { get; set; }
        public string ProcessFullName { get; set; }
        public string ProcessDescription { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime StatusChangeDate { get; set; }
        public IList<TaskOptionDto> TaskActions { get; set; }
        public TaskOptionDto Result { get; set; }
        public string ProcessOriginatorFullName { get; set; }
        public string SubjectReferenceId { get; set; }
        public string ProcessStatus { get; set; }
        public string CurrentStepName { get; set; }
        public string EntityUris { get; set; }
        public string CompletedBy { get; set; }
        public string ProcessSubjectFullName { get; set; }
        public int CustomEntityId { get; set; }
        public string CompositeTaskName { get; set; }
        public int ProcessStatusEnum { get; set; }

        public string UserImage { get; set; }

    }
}

