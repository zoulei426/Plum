using Plum.Notify;
using Plum.Object;
using Prism.Ioc;
using Refit;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Plum
{
    /// <summary>
    /// ApiExceptionResolverExtensions
    /// </summary>
    public static class ApiExceptionResolverExtensions
    {
        private class ApiExceptionResolver
        {
            private readonly INotifier notifier;

            public ApiExceptionResolver(INotifier notifier)
            {
                this.notifier = notifier;
            }

            public async Task<bool> RunApiInternal(Task task, Action onSuccessCallback)
            {
                try
                {
                    await task;
                    onSuccessCallback?.Invoke();
                }
                catch (ApiException ex)
                {
                    if (ex.Content.IsNullOrEmpty())
                    {
                        notifier.Error(ex.Message);
                        return false;
                    }
                    notifier.Error(ex.Content.ToObject<ErrorResponse>().ToString());
                    return false;
                }
                catch (HttpRequestException httpRequestException)
                {
                    notifier.Error(httpRequestException.ToDetailString());
                    return false;
                }
                return true;
            }

            public async Task<T> RunApiInternal<T>(Task<T> task)
            {
                try
                {
                    return await task;
                }
                catch (ApiException ex)
                {
                    if (ex.Content.IsNullOrEmpty())
                    {
                        notifier.Error(ex.Message);
                        return default(T);
                    }
                    notifier.Error(ex.Content.ToObject<ErrorResponse>().ToString());
                }
                catch (HttpRequestException httpRequestException)
                {
                    notifier.Error(httpRequestException.ToDetailString());
                }

                return default;
            }
        }

        private static IContainerProvider container;

        /// <summary>
        /// Sets the unity container.
        /// </summary>
        /// <param name="container">The container.</param>
        public static void SetUnityContainer(IContainerProvider container) => ApiExceptionResolverExtensions.container = container;

        /// <summary>
        /// Runs the API.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="onSuccessCallback">The on success callback.</param>
        /// <returns></returns>
        public static Task<bool> RunApi(this Task task, Action onSuccessCallback = null) =>
            container.Resolve<ApiExceptionResolver>().RunApiInternal(task, onSuccessCallback);

        /// <summary>
        /// Runs the API.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task">The task.</param>
        /// <returns></returns>
        public static Task<T> RunApi<T>(this Task<T> task) =>
            container.Resolve<ApiExceptionResolver>().RunApiInternal(task);
    }
}