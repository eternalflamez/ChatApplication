// <copyright file="ChatReceiver.cs" company="None">
//     All rights reserved.
// </copyright>
// <author>Sander Dings</author>

/// <summary>
/// Basic class to handle receiving messages from various queue-types from RabbitMQ.
/// </summary>
public class ChatReceiver : Receiver
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChatReceiver"/> class.
    /// </summary>
    /// <param name="host">Hostname, base is the local host.</param>
    public ChatReceiver(string host = "localhost") : base(host)
    {
    }

    /// <summary>
    /// Starts a basic listener on a specified queue. Should only be used for one way channels.
    /// </summary>
    /// <param name="queue">The queue to listen on.</param>
    public void StartBasicListen(string queue)
    {
        this.Channel.QueueDeclare(queue, false);
        this.Consume(queue, queue);
    }

    /// <summary>
    /// Starts a listener that listens to an exchange. This allows multiple people to listen to the same exchange.
    /// </summary>
    /// <param name="exchange">The exchange to listen to.</param>
    public void StartExchangeListen(string exchange)
    {
        this.Channel.ExchangeDeclare(exchange, "fanout");
        string queueName = this.Channel.QueueDeclare();
        this.Channel.QueueBind(queueName, exchange, string.Empty, false, null);
        this.Consume(queueName, exchange);
    }
}