namespace ToDo.Domain.Results
{
    public class Error : IError
    {
        public Error(string key, string message)
        {
            Key = key;
            Message = message;
        }

        public string Key { get; private set; }

        public string Message { get; private set; }
    }
}
