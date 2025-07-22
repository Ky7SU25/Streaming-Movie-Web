using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using StreamingMovie.Application.Interfaces.ExternalServices.Messages;

namespace StreamingMovie.Infrastructure.ExternalServices.Messages
{
    public class RabbitMqPublisher : IQueuePublisher, IDisposable
    {
        private IConnection _connection;
        private IChannel _channel;
        private RabbitMqConnectionFactory _factory;
        private RabbitMqOptions _options;

        public RabbitMqPublisher(
            RabbitMqConnectionFactory factory,
            IOptions<RabbitMqOptions> options
        )
        {
            _factory = factory;
            _options = options.Value;
        }

        public async Task Publish(VideoProcessingMessage message)
        {
            _connection = await _factory.GetConnectionAsync();
            if (_connection == null)
                throw new Exception("Connection is null");

            _channel = await _connection.CreateChannelAsync();
            if (_channel == null)
                throw new Exception("Channel is null");
            await _channel.QueueDeclareAsync(
                queue: _options.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false
            );
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);
            try
            {
                await _channel.BasicPublishAsync(
                    exchange: "",
                    routingKey: _options.QueueName,
                    basicProperties: new BasicProperties(),
                    mandatory: true,
                    body: body
                );

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
