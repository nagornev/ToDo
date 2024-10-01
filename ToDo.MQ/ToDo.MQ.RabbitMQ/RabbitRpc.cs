using RabbitMQ.Client;

namespace ToDo.MQ.RabbitMQ
{
    internal class RabbitRpc
    {
        private const int _timeout = 15000;

        public RabbitRpc(IModel channel)
        {
            Channel = channel;
            Source = new TaskCompletionSource<IEnumerable<byte>>();

            StartTimer();
        }

        public IModel Channel { get; private set; }

        public TaskCompletionSource<IEnumerable<byte>> Source { get; private set; }

        private void StartTimer()
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += (sender, args) =>
            {
                if (!Source.Task.IsCompleted)
                {
                    Source.SetException(new TimeoutException());
                    Channel.Close();
                    Channel.Dispose();
                }
            };
            timer.AutoReset = false;
            timer.Enabled = false;
            timer.Interval = _timeout;

            timer.Start();
        }
    }
}
