// <copyright file="Sender.cs" company="None">
//     All rights reserved.
// </copyright>
// <author>Sander Dings</author>

namespace GameChatHost.Logics
{
    using System.Text;
    using Models;
    using Newtonsoft.Json;
    using RabbitMQ.Client;

    /// <summary>
    /// The base sender class.
    /// </summary>
    public class Sender
    {
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
        /// Initializes a new instance of the <see cref="Sender"/> class.
        /// </summary>
        /// <param name="host">The host to connect to.</param>
        public Sender(string host = "localhost")
        {
            this.Connect(host);
        }
        
        /// <summary>
        /// Gets the current channel we're connected to.
        /// </summary>
        protected IModel Channel
        {
            get { return this.channel; }
        }

        /// <summary>
        /// Gets the current connection we have.
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
            this.factory.HostName = host;

            this.connection = this.factory.CreateConnection();
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
        /// Sends a message to a specific exchange. Used for everything with more than one user.
        /// </summary>
        /// <param name="exchange">The exchange to send to.</param>
        /// <param name="message">The message to send.</param>
        public void StartExchangeSend(string exchange, object message)
        {
            this.Channel.ExchangeDeclare(exchange, "fanout");
            string endmessage = this.SerializeMessage(message);
            byte[] body = Encoding.UTF8.GetBytes(endmessage);

            this.Channel.BasicPublish(exchange, string.Empty, null, body);
        }

        /// <summary>
        /// Serializes a message.
        /// </summary>
        /// <param name="message">The message to change to jSon.</param>
        /// <returns>The message, but in jSon.</returns>
        private string SerializeMessage(object message)
        {
            return JsonConvert.SerializeObject(message);
        }
    }
}