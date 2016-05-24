// <copyright file="UserChangeSender.cs" company="None">
//     All rights reserved.
// </copyright>
// <author>Sander Dings</author>

using System.Text;

/// <summary>
/// Sends messages relating to logging in and logging out so other users can know our online status.
/// </summary>
public class UserChangeSender : Sender
{
    /// <summary>
    /// Sends a message to the server that we're online.
    /// </summary>
    public void Login()
    {
        this.ExchangeSend("userlogin", UserController.Instance.UserName);
    }

    /// <summary>
    /// Sends a message to the server that we're going offline.
    /// </summary>
    public void Logout()
    {
        this.ExchangeSend("userlogout", UserController.Instance.UserName);
    }

    /// <summary>
    /// Starts sending a message.
    /// </summary>
    /// <param name="exchange">The exchange to send to.</param>
    /// <param name="username">The info to send.</param>
    private void ExchangeSend(string exchange, string username)
    {
        this.Channel.ExchangeDeclare(exchange, "fanout");
        byte[] body = Encoding.UTF8.GetBytes(username);

        this.Channel.BasicPublish(exchange, string.Empty, null, body);
    }
}
