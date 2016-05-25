// <copyright file="ChatSender.cs" company="None">
//     All rights reserved.
// </copyright>
// <author>Sander Dings</author>
using System.Text;
using Newtonsoft.Json;

/// <summary>
/// Sends messages through rabbitMQ.
/// </summary>
public class ChatSender : Sender
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChatSender"/> class.
    /// </summary>
    public ChatSender() : base()
    {
    }
    
    /// <summary>
    /// Sends a message to a specific exchange. Used for everything with more than one user.
    /// </summary>
    /// <param name="exchange">The exchange to send to.</param>
    /// <param name="message">The message to send.</param>
    public void StartExchangeSend(string exchange, string message)
    {
        this.Channel.ExchangeDeclare(exchange, "fanout");
        message = this.SerializeMessage(message);
        byte[] body = Encoding.UTF8.GetBytes(message);

        this.Channel.BasicPublish(exchange, string.Empty, null, body);
    }

    /// <summary>
    /// Sends a message to a specific queue. Used for whispers mostly.
    /// </summary>
    /// <param name="queue">The name of the queue to send to.</param>
    /// <param name="message">The message to send.</param>
    public void StartSimpleSend(string queue, string message)
    {
        this.Channel.QueueDeclare(queue, false);
        message = this.SerializeMessage(message);
        byte[] body = Encoding.UTF8.GetBytes(message);

        this.Channel.BasicPublish(string.Empty, queue, null, body);
    }
    
    /// <summary>
    /// Serializes a message.
    /// </summary>
    /// <param name="message">The message to change to jSon.</param>
    /// <returns>The message, but in jSon.</returns>
    private string SerializeMessage(string message)
    {
        return JsonConvert.SerializeObject(new UserMessage(this.UserName, message));
    }
}
