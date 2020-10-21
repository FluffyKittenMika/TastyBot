using System.Threading.Tasks;
using Utilities.LoggingService;

namespace Utilities.TasksUtilities
{
    public static class AsyncUtility { 
        public static void PerformAsyncTaskWithoutAwait(this Task task) 
        {
            var dummy = task.ContinueWith(t => Logging.LogErrorMessage("AsyncUtility", "An error has occurred trying to run AsyncMethod: " + t.Exception.Message), TaskContinuationOptions.OnlyOnFaulted); 
        } 
    }
}