namespace ToDo.Domain.Results
{
    public class ErrorField : Error
    {
        public ErrorField(string key,
                          string message,
                          string field) 
            : base(key, message)
        {
            Field = field;
        }

        public string Field { get; private set; }
    }
}
