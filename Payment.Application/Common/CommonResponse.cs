namespace Payment.Application.Common;

public class BaseResponse<T>
{
    public T Data { get; set; }
    public Dictionary<string, string[]> Errors { get; set; }

    public BaseResponse(T data)
    {
        Data = data;
    }

    public void AddError(string name, string message)
    {
        if (Errors is null)
        {
            Errors = new Dictionary<string, string[]>();
        }

        if (Errors.ContainsKey(name))
        {
            Errors[name].Append(message);
        }
        else
        {
            Errors[name] = new[] { message };
        }
    }
}
