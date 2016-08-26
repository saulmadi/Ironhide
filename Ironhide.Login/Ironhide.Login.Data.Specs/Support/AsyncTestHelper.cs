using System.Threading.Tasks;

namespace Ironhide.Login.Data.Specs.Support
{
    public static class AsyncTestHelper
    {
        public static T WaitForResult<T>(this Task<T> task)
        {
            T result = default(T);
            task.ContinueWith(t => result = task.Result).Wait();
            return result;
        }
    }
}