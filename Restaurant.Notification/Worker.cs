using System.Collections.Concurrent;
using System.Text;
using Messaging;

namespace Restaurant.Notification
{
    public class Worker: BackgroundService
    {

        private readonly Consumer _consumer;

        public Worker()
        {
            _consumer = new Consumer("BookingNotification");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Receive((sender, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[x] Received {message}");
            });
        }
    }
}
