using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBot.Model
{
    public class TaskDto
    {
        //private ProcessStatus processStatusEnum;

        /// <summary>
        /// Gets or sets the task identifier.
        /// </summary>
        /// <value>
        /// The task identifier.
        /// </value>
        public long Id { get; set; }
        /// <summary>
        /// Gets or sets the task identifier.
        /// </summary>
        /// <value>
        /// The task identifier.
        /// </value>
        public Guid ProcessId { get; set; }
        /// <summary>
        /// Gets or sets the name of the task.
        /// </summary>
        /// <value>
        /// The name of the task.
        /// </value>
        public string ShortName { get; set; }
        /// <summary>
        /// Gets or sets the display name of the task.
        /// </summary>
        /// <value>
        /// The display name of the task.
        /// </value>
        public string FullName { get; set; }
        /// <summary>
        /// Gets or sets the related UI.
        /// </summary>
        /// <value>
        /// The related UI.
        /// </value>
        public string RelatedUi { get; set; }
        /// <summary>
        /// Gets or sets the name of the status.
        /// </summary>
        /// <value>
        /// The name of the status.
        /// </value>
        public string StatusShortName { get; set; }
        /// <summary>
        /// Gets or sets the status label.
        /// </summary>
        /// <value>
        /// The status label.
        /// </value>
        public string StatusFullName { get; set; }
        /// <summary>
        /// Gets or sets the Process Display Name
        /// </summary>
        public string ProcessFullName { get; set; }
        /// <summary>
        /// Gets or sets the process description.
        /// </summary>
        /// <value>
        /// The process description.
        /// </value>
        public string ProcessDescription { get; set; }
        /// <summary>
        /// Gets or sets the date creation
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the date creation
        /// </summary>
        public DateTime? StatusChangeDate { get; set; }

        /// <summary>
        /// Gets or sets the task actions.
        /// </summary>
        /// <value>
        /// The task actions.
        /// </value>
        public IEnumerable<TaskOptionDto> TaskActions { get; set; }

        /// <summary>
        ///The result of the user's action selection.
        /// </summary>
        public TaskOptionDto Result { get; set; }

        /// <summary>
        /// Gets or sets the process originator user full name
        /// </summary>
        public string ProcessOriginatorFullName { get; set; }

        /// <summary>
        /// Gets or sets the subject user reference
        /// </summary>
        public string SubjectReferenceId { get; set; }

        /// <summary>
        /// Gets or sets the process status.
        /// </summary>
        /// <value>
        /// The process status.
        /// </value>
        public string ProcessStatus { get; set; }

        /// <summary>
        /// Gets or sets the current step name
        /// </summary>
        public string CurrentStepName { get; set; }

        /// <summary>
        /// Gets or sets the entity uris
        /// </summary>
        public string EntityUris { get; set; }

        /// <summary>
        /// Gets or sets the completed by.
        /// </summary>
        /// <value>
        /// The completed by.
        /// </value>
        public string CompletedBy { get; set; }

        public string ProcessSubjectFullName { get; internal set; }

        public long? CustomEntityId { get; internal set; }

        public string CompositeTaskName { get; set; }

        public ProcessStatus ProcessStatusEnum { get; set; }

       
       


    }
}

