using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchBot.Model
{
    public enum ProcessStatus
    {
        /// <summary>
        /// xaml is running
        /// </summary>
        Running = 0,

        /// <summary>
        /// xaml is asleep
        /// </summary>
        Waiting = 1,

        /// <summary>
        /// Recoverable
        /// </summary>
        InError = 2,

        /// <summary>
        /// Completed with success
        /// </summary>
        Completed = 3,

        /// <summary>
        /// Deliverately stopped
        /// </summary>
        Canceled = 4,

        /// <summary>
        /// Crashed, not recoverable
        /// </summary>
        Terminated = 5,
    }

    public static class ProcessStatusExtensions
    {
        /// <summary>
        /// Composes the name of the LocalizableTest for each enum value
        /// </summary>
        /// <param name="processStatus"></param>
        /// <returns></returns>
        public static string GetLocalizableTextName(this ProcessStatus processStatus)
        {
            return $"{processStatus.ToString()}";
        }
    }
}
