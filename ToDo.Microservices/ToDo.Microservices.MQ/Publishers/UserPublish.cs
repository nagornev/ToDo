using ToDo.Microservices.MQ.Models;

namespace ToDo.Microservices.MQ.Publishers
{
    [Serializable]
    public abstract class UserPublish
    {
        public UserPublish(UserMQ user)
        {
            User = user;
        }

        public UserMQ User { get; private set; }
    }
}
