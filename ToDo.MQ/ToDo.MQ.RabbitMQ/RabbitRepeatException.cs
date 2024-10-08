namespace ToDo.MQ.RabbitMQ
{
    internal class RabbitRepeatException : Exception
    {
        public RabbitRepeatException(int delay)
        {
            Delay = delay;
        }

        public int Delay { get; private set; }
    }
}
