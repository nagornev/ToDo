namespace ToDo.Domain.Results
{
    public class Error : IError
    {
        public Error(int code, string key, string message)
        {
            Code = code;
            Key = key;
            Message = message;
        }

        public int Code { get; private set; }

        public string Key { get; private set; }

        public string Message { get; private set; }

    }
}
