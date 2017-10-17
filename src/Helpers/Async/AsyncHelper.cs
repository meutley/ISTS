using System;
using System.Threading;
using System.Threading.Tasks;

namespace ISTS.Helpers.Async
{
    public static class AsyncHelper
    {
        private static readonly TaskFactory _taskFactory =
            new TaskFactory(
                CancellationToken.None,
                TaskCreationOptions.None,
                TaskContinuationOptions.None,
                TaskScheduler.Default);

        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return AsyncHelper._taskFactory
                .StartNew<Task<TResult>>(func)
                .Unwrap<TResult>()
                .GetAwaiter()
                .GetResult();
        }

        public static void RunSync(Func<Task> func)
        {
            AsyncHelper._taskFactory
                .StartNew<Task>(func)
                .Unwrap()
                .GetAwaiter()
                .GetResult();
        }
    }
}