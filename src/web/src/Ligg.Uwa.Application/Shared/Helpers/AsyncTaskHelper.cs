using System;
using System.Threading.Tasks;

namespace Ligg.Uwa.Application.Shared
{
    public class AsyncTaskHelper
    {
        public static void StartTask(Action action)
        {
            try
            {
                Action newAction = () =>
                { };
                newAction += action;
                Task task = new Task(newAction);
                task.Start();
            }
            catch (Exception ex)
            {
                //LogHelper.Error(ex);
            }
        }
    }
}
