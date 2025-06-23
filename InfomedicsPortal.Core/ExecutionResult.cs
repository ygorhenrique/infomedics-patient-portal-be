namespace InfomedicsPortal.Core;

public readonly struct ExecutionResult<T>
{
    public T? Result { get; }
    public bool IsSuccess { get; }
    public string? Message { get; }

    private ExecutionResult(T? result, bool success, string? message = null)
    {
        this.Result = result;
        this.IsSuccess = success;
        this.Message = message;
    }
    
    private ExecutionResult(bool success, string? message = null)
    {
        this.IsSuccess = success;
        this.Message = message;
    }

    public static ExecutionResult<T> Success(T result)
    {
        return new ExecutionResult<T>(result, true);
    }
    
    public static ExecutionResult<T> Failure(string message)
    {
        return new ExecutionResult<T>(false, message);
    }
}