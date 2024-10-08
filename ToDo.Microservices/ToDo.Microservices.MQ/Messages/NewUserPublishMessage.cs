using ToDo.Microservices.MQ.Models;

namespace ToDo.Microservices.MQ.Publishers
{
    [Serializable]
    public class NewUserPublishMessage : UserPublishMessage
    {
        public const string Exchange = "new_users_exchange";

        public NewUserPublishMessage(UserMQ user)
            : base(user)
        {
        }
    }
}
