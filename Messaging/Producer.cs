using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Messaging
{
    public class Producer: IDisposable
    {
        private readonly string _queueName;
        private readonly string _hostName;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        public Producer(string queueName)
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
            _channel.ExchangeDeclare("direct_exchange", "direct", false, false, null);

        }

        public void Send(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish("direct_exchange", _queueName, null, body);
        }

        public void Dispose()
        {
            _connection.Dispose();
            _channel.Dispose();
        }
    }
}
