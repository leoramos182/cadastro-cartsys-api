namespace CadastroCartsys.Crosscutting.Results;

public class EnvelopDataResult<T> : EnvelopResult
{
    public T Data { get; set; }

    public EnvelopDataResult()
    {
        Success = true;
    }

    public static EnvelopDataResult<T> Ok(T data, bool success = true) => new EnvelopDataResult<T>
    {
        Success = success,
        Data = data
    };
}