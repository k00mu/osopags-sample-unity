namespace Osopags.Core
{
    public interface IResult<TSuccess, TError>
    {
        bool IsError { get; }
        TSuccess Success { get; }
        TError Error { get; }
    }

    public class Result<TSuccess, TError> : IResult<TSuccess, TError>
    {
        public bool IsError { get; private set; }
        public TSuccess Success { get; private set; }
        public TError Error { get; private set; }

        public static Result<TSuccess, TError> CreateSuccess(TSuccess success)
        {
            return new Result<TSuccess, TError>
            {
                IsError = false,
                Success = success
            };
        }

        public static Result<TSuccess, TError> CreateError(TError error)
        {
            return new Result<TSuccess, TError>
            {
                IsError = true,
                Error = error
            };
        }
    }

    public delegate void ResultCallback<TSuccess, TError>(IResult<TSuccess, TError> result);

    public delegate void SuccessDelegate<TSuccess>(TSuccess success);
    public delegate void ErrorDelegate<TError>(TError error);
}