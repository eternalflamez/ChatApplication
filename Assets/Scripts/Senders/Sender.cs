// <copyright file="Sender.cs" company="None">
//     All rights reserved.
// </copyright>
// <author>Sander Dings</author>
using RabbitMQ.Client;

/// <summary>
/// The base sender class.
/// </summary>
public abstract class Sender
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
    /// The username of the current user.
    /// </summary>
    private string userName;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Sender"/> class.
    /// </summary>
    /// <param name="host">The host to connect to.</param>
    public Sender(string host = "localhost")
    {
        this.Connect(host);
        this.userName = UserController.Instance.UserName;
    }

    /// <summary>
    /// Gets the username of the current user.
    /// </summary>
    protected string UserName
    {
        get { return this.userName; }
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
}
