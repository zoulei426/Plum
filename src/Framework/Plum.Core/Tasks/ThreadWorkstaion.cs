using System;
using System.Threading;

namespace Plum.Tasks
{
    public static class ThreadWorkstaion
    {
        #region Properties

        public static int MaxThreadCount
        {
            get { return pool.MaxThreadCount; }
            set { pool.MaxThreadCount = value; }
        }

        private static readonly ThreadPool pool = new ThreadPool();

        #endregion Properties

        #region Fields

        #endregion Fields

        #region Ctor

        #endregion Ctor

        #region Methods

        #region Methods - Public

        public static void Start(Action<object> callback, object state)
        {
            pool.Start(callback, state);
        }

        public static void Start<T>(Action<T> callback, T state)
        {
            pool.Start(callback, state);
        }

        public static void Start(Action callback, bool useThreadPool = true)
        {
            pool.Start(callback, useThreadPool);
        }

        public static void Start(Action callback, int maxThreadCount)
        {
            pool.Start(callback, maxThreadCount);
        }

        public static void Start(Action callback, ApartmentState state)
        {
            pool.Start(callback, state);
        }

        #endregion Methods - Public

        #region Methods - Private

        #endregion Methods - Private

        #endregion Methods
    }
}