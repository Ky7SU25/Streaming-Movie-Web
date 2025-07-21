using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace StreamingMovie.Infrastructure.ExternalServices.Messages
{
    public class RabbitMqConnectionFactory
    {
        private RabbitMqOptions _options;
        private IConnection _connection;

        public RabbitMqConnectionFactory(IOptions<RabbitMqOptions> options)
        {
            _options = options.Value;
        }

        private async Task<IConnection> CreateConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _options.Host,
                UserName = _options.User,
                Password = _options.Password,
                Port = _options.Port,
            };

            return await factory.CreateConnectionAsync();
        }

        public async Task<IConnection> GetConnectionAsync()
        {
            if (_connection == null || !_connection.IsOpen)
            {
                _connection = await CreateConnection();
            }
            return _connection;
        }
    }
}
