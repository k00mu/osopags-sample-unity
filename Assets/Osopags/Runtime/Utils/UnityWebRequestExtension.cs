using System.Runtime.CompilerServices;
using UnityEngine.Networking;

namespace Osopags.Utils
{
    public static class UnityWebRequestExtensions
    {
        public struct UnityWebRequestAwaiter : INotifyCompletion
        {
            private UnityWebRequestAsyncOperation asyncOp;
#pragma warning disable IDE0052 // Remove unread private members
            private bool isDone;
#pragma warning restore IDE0052 // Remove unread private members

            public UnityWebRequestAwaiter(UnityWebRequestAsyncOperation asyncOp)
            {
                this.asyncOp = asyncOp;
                isDone = false;
            }

            public bool IsCompleted => asyncOp.isDone;

            public void OnCompleted(System.Action continuation)
            {
                var self = this;
                asyncOp.completed += _ =>
                {
                    self.isDone = true;
                    continuation();
                };
            }

            public UnityWebRequest GetResult()
            {
                return asyncOp.webRequest;
            }
        }

        public static UnityWebRequestAwaiter GetAwaiter(this UnityWebRequestAsyncOperation asyncOp)
        {
            return new UnityWebRequestAwaiter(asyncOp);
        }
    }
}