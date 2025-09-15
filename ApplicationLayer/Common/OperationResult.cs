namespace ApplicationLayer.Common;

public class OperationResult<T>
{
    public bool IsSuccess { get; }
    public string Error { get; }
    public T? Data { get; }
    public T? Value => Data; // Alias så både Data och Value funkar

    private OperationResult(bool isSuccess, T? data, string error)
    {
        IsSuccess = isSuccess;
        Data = data;
        Error = error;
    }

    // Primära fabrikmetoder
    public static OperationResult<T> Ok(T data) => new(true, data, string.Empty);
    public static OperationResult<T> Fail(string error) => new(false, default, error);

    // Alias (om något ställe använder dessa)
    public static OperationResult<T> Success(T data) => Ok(data);
    public static OperationResult<T> Failure(string error) => Fail(error);
}
