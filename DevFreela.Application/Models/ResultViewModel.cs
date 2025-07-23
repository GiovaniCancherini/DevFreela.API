namespace DevFreela.Application.Models
{
    public class ResultViewModel
    {
        public ResultViewModel(bool isSucess = true, string message = "")
        {
            IsSucess = isSucess;
            Message = message;
        }
        public bool IsSucess { get; private set; }
        public string Message { get; private set; }

        // Factory methods for creating instances of ResultViewModel (no data return)
        public static ResultViewModel Success()
            => new();

        public static ResultViewModel Failure(string message)
            => new(false, message);
    }

    public class ResultViewModel<T> : ResultViewModel
    {
        public ResultViewModel(T? data, bool isSucess = true, string message = "")
            : base(isSucess, message)
        {
            Data = data;
        }
        public T? Data { get; private set; }

        public static ResultViewModel<T> Success(T data)
            => new (data);

        public static ResultViewModel<T> Failure(string message)
            => new (default, false, message);
    }
}
