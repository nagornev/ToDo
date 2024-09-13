using Newtonsoft.Json;

namespace ToDo.Microservices.Middleware.Identities
{
    public class IdentityOutput
    {
        public class OutputError
        {
            public OutputError(int code,
                               string key,
                               string message)
            {
                Code = code;
                Key = key;
                Message = message;
            }


            [JsonProperty("code")]
            public int Code { get; private set; }

            [JsonProperty("key")]
            public string Key { get; private set; }

            [JsonProperty("message")]
            public string Message { get; private set; }
        }

        public IdentityOutput(bool success, 
                              Guid? user = null,
                              OutputError error = null)
        {
           
            Success = success;
            User = user;
            Error = error;
        }



        [JsonProperty("success")]
        public bool Success { get; private set; }

        [JsonProperty("content")]
        public Guid? User { get; private set; }

        [JsonProperty("error")]
        public OutputError? Error { get; private set; }

        public bool ShouldSerializeUser()
        {
            return User != null;
        }

        public bool ShouldSerializeError()
        {
            return Error != null;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
