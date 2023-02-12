using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Mellon.Services.Application
{
    public class LockProvider<T>
    {
        static readonly ConcurrentDictionary<T, SemaphoreSlim> lockDictionary =  new ConcurrentDictionary<T, SemaphoreSlim>();

        public LockProvider() { }

        /// <summary>
        /// Blocks the current thread (according to the given ID)
        /// until it can enter the LockProvider
        /// </summary>
        /// <param name="idToLock">the unique ID to perform the lock</param>
        public void Wait(T idToLock)
        {
            lockDictionary.GetOrAdd(idToLock, new SemaphoreSlim(1, 1)).Wait();
        }

        /// <summary>
        /// Asynchronously puts thread to wait (according to the given ID)
        /// until it can enter the LockProvider
        /// </summary>
        /// <param name="idToLock">the unique ID to perform the lock</param>
        public async Task WaitAsync(T idToLock)
        {
            await lockDictionary.GetOrAdd(idToLock, new SemaphoreSlim(1, 1)).WaitAsync();
        }


        /// <summary>
        /// Asynchronously puts thread to wait (according to the given ID)
        /// until it can enter the LockProvider
        /// </summary>
        /// <param name="idToLock">the unique ID to perform the lock</param>
        public async Task IsWaiting(T idToLock)
        {
            SemaphoreSlim semaphore;
             lockDictionary.TryGetValue(idToLock,out semaphore);
            if (semaphore.CurrentCount == 1)
            {
                await semaphore.WaitAsync();
            }
        }


        /// <summary>
        /// Releases the lock (according to the given ID)
        /// </summary>
        /// <param name="idToUnlock">the unique ID to unlock</param>
        public void Release(T idToUnlock)
        {
            SemaphoreSlim semaphore;
            if (lockDictionary.TryGetValue(idToUnlock, out semaphore))
                semaphore.Release();
        }
    }


        public interface IApprovaProcessLock
        {
            Task WaitAsync();
            void Release();
            Task IsWaiting();

    }

    public  class ApprovaProcessLock : IApprovaProcessLock
        {
            private static LockProvider<int> LockProvider =   new LockProvider<int>();

       
            public  async Task WaitAsync()
            {
                await LockProvider.WaitAsync(1);
            }
       
            public  void Release()
            {
                LockProvider.Release(1);
            }

            public async Task IsWaiting()
            {
                await LockProvider.IsWaiting(1);
            }
    }
}
