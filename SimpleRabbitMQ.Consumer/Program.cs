
using RabbitMQ.Client;

ConnectionFactory factory = new();
factory.Uri = new("amqps://efithflb:8XLVmInfj3Q37bnvn6yBCUb_1upLf86Y@shrimp.rmq.cloudamqp.com/efithflb");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.QueueDeclare(queue: "example-queue", exclusive: false);
