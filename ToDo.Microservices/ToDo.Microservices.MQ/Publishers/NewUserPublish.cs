using ToDo.Microservices.MQ.Models;

namespace ToDo.Microservices.MQ.Publishers
{
    [Serializable]
    public class NewUserPublish : UserPublish
    {
        public const string Exchange = "new_users_exchange";

        public NewUserPublish(UserMQ user)
            : base(user)
        {
        }
    }
}
