// <copyright file="TeamJoinReceiver.cs" company="None">
//     All rights reserved.
// </copyright>
// <author>Sander Dings</author>

/// <summary>
/// The base class to receive and handle team join requests.
/// </summary>
public class TeamJoinReceiver : Receiver
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TeamJoinReceiver"/> class.
    /// </summary>
    /// <param name="host">Hostname, base is the local host.</param>
    public TeamJoinReceiver(string host = "localhost") : base(host)
    {
    }

    /// <summary>
    /// Starts listening to new team joins.
    /// </summary>
    /// <param name="teamName">The team we're in.</param>
    public void Listen(string teamName)
    {
        string exchange = "team-join-" + teamName;
        this.Channel.ExchangeDeclare(exchange, "fanout", false, false, true, false, false, null);
        string queueName = this.Channel.QueueDeclare();
        this.Channel.QueueBind(queueName, exchange, string.Empty, false, null);
        this.Consume(queueName, exchange);
    }
}