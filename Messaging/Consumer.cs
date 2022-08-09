using System.Net.Security;
using System.Threading.Channels;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Messaging
{
    public class Consumer: IDisposable
    {
        private readonly string _queueName;
        private readonly string _hostName;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        public Consumer(string queueName)
        {
            _queueName = queueName;
            _hostName = "shark-01.rmq.cloudamqp.com";
            var factory = new ConnectionFactory
            {
                HostName = _hostName,
                VirtualHost = "ynlrftfm",
                UserName = "ynlrftfm", //Указать имя пользователя
                Password = "8kwCFzMD4mzMaQwTShEmtpiWM-kM994d", //Указать пароль
                Port = 5671,
                RequestedHeartbeat = TimeSpan.FromSeconds(10),
                Ssl = new SslOption
                {
                    Enabled = true,
                    AcceptablePolicyErrors = SslPolicyErrors.RemoteCertificateNameMismatch |
                                             SslPolicyErrors.RemoteCertificateChainErrors,
                    Version = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls11
                }
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void Receive(EventHandler<BasicDeliverEventArgs> receiveCallback)
        {
            _channel.ExchangeDeclare("direct_exchange", "direct");
            _channel.QueueDeclare(_queueName, false, false, false, null);
            _channel.QueueBind(_queueName,"direct_exchange", _queueName);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += receiveCallback;

            _channel.BasicConsume(_queueName, true, consumer);
        }


        public void Dispose()
        {
            _connection.Dispose();
            _channel.Dispose();
        }
    }
}