﻿
using ToDo.MQ.Abstractions;

namespace ToDo.Microservices.Identity.API.Backgrounds
{
    public class MessageQueueBackground : BackgroundService
    {
        public MessageQueueBackground(IMessageQueueClient messageQueue)
        {
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken) => Task.CompletedTask;
    }
}
