using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace BikeAround.Service
{
    public static class TaskExtensions
    {
        public static void WaitAndUnwrapException(this Task task)
        {
            try
            {
                task.Wait();
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.Count == 1)
                {
                    throw ex.InnerExceptions[0];
                }
                else
                {
                    throw ex;
                }
            }
        }

        public static T WaitAndUnwrapException<T>(this Task<T> task)
        {
            try
            {
                task.Wait();
                return task.Result;
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.Count == 1)
                {
                    ExceptionDispatchInfo.Capture(ex.InnerExceptions[0]).Throw();
                }
                throw;
            }
        }
    }
}