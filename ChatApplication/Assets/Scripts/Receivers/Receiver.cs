// <copyright file="Receiver.cs" company="None">
//     All rights reserved.
// </copyright>
// <author>Sander Dings</author>

using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

/// <summary>
/// The base receiver class.
/// </summary>
public abstract class Receiver
{
    /// <summary>
    /// The EventHandler to call when we receive a message.
    /// </summary>
    private EventHandler<ConsumerEventArgs> receiveEvent;

    /// <summary>
    /// The ConnectionFactory which creates the connection to RabbitMQ.
    /// </summary>
    private ConnectionFactory factory = new ConnectionFactory();

    /// <summary>
    /// The connection we receive messages on.
    /// </summary>
    private IConnection connection;

    /// <summary>
    /// The channel we receive messages on.
    /// </summary>
    private IModel channel;

    /// <summary>
    /// Initializes a new instance of the <see cref="Receiver"/> class.
    /// </summary>
    /// <param name="host">Hostname, base is the local host.</param>
    public Receiver(string host = "localhost")
    {
        this.Connect(host);
    }
    
    /// <summary>
    /// Gets or sets the event to call when a message is received.
    /// </summary>
    public EventHandler<ConsumerEventArgs> ReceiveEvent
    {
        get { return this.receiveEvent; }
        set { this.receiveEvent = value; }
    }

    /// <summary>
    /// Gets the channel we receive messages on.
    /// </summary>
    protected IModel Channel
    {
        get { return this.channel; }
    }

    /// <summary>
    /// Gets the connection we receive messages on.
    /// </summary>
    protected IConnection Connection
    {
        get { return this.connection; }
    }
    
    /// <summary>
    /// Connects to a host.
    /// </summary>
    /// <param name="host">Host to connect to.</param>
    public void Connect(string host = "localhost")
    {
        this.connection = this.factory.CreateConnection(host);
        this.channel = this.connection.CreateModel();
    }

    /// <summary>
    /// Disconnects the user from the channel and connection.
    /// </summary>
    public void Disconnect()
    {
        this.channel.Close();
        this.connection.Close();
    }

    /// <summary>
    /// Disconnects the user from the connection.
    /// </summary>
    public void CloseConnection()
    {
        this.connection.Close();
    }
    
    /// <summary>
    /// Sets up the channel to be consumed.
    /// </summary>
    /// <param name="queueName">The queue to consume.</param>
    /// <param name="channelname">The name of the queue to report when messages are received.</param>
    protected void Consume(string queueName, string channelname)
    {
        this.Channel.BasicConsume(
            queue: queueName,
            noAck: true,
            filter: null,
            consumer: this.CreateConsumer(channelname));
    }

    /// <summary>
    /// Creates a new consumer object.
    /// </summary>
    /// <param name="channelname">The channel name to report when a message is received.</param>
    /// <returns>A basic consumer object with it's receiving event already attached.</returns>
    private EventingBasicConsumer CreateConsumer(string channelname)
    {
        EventingBasicConsumer consumer = new EventingBasicConsumer();

        consumer.Received += (model, ea) =>
        {
            byte[] body = ea.Body;
            string message = Encoding.UTF8.GetString(body);

            if (ReceiveEvent != null)
            {
                ConsumerEventArgs e = new ConsumerEventArgs(message, ea.Exchange);
                ReceiveEvent(this, e);
            }
        };

        return consumer;
    }

    /// <summary>
    /// Wrapper class for event arguments for the event where a message is consumed.
    /// </summary>
    public class ConsumerEventArgs : EventArgs
    {
        /// <summary>
        /// Holds the username and message in jSon.
        /// </summary>
        private string message;

        /// <summary>
        /// Holds the channel name for later grouping.
        /// </summary>
        private string channel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsumerEventArgs"/> class.
        /// </summary>
        /// <param name="message">The message we just received. Contains username and message in jSon.</param>
        /// <param name="channel">The channel we received this from, used for grouping messages.</param>
        public ConsumerEventArgs(string message, string channel)
        {
            this.message = message;
            this.channel = channel;
        }

        /// <summary>
        /// Gets the message of this event.
        /// </summary>
        public string Message
        {
            get { return this.message; }
        }

        /// <summary>
        /// Gets the channel of this event.
        /// </summary>
        public string Channel
        {
            get { return this.channel; }
        }
    }
}