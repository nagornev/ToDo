using ToDo.Microservices.MQ.Models;

namespace ToDo.Microservices.MQ.Publishers
{
    [Serializable]
    public abstract class UserPublishMessage
    {
        public UserPublishMessage(UserMQ user)
        {
            User = user;
        }

        public UserMQ User { get; private set; }
    }
}
