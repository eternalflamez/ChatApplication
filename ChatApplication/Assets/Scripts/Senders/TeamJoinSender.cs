// <copyright file="TeamJoinSender.cs" company="None">
//     All rights reserved.
// </copyright>
// <author>Sander Dings</author>
using System.Text;
using Newtonsoft.Json;

/// <summary>
/// Handles joining teams so we notify the other users we just joined that team.
/// </summary>
public class TeamJoinSender : Sender
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TeamJoinSender"/> class.
    /// </summary>
    /// <param name="host">The host to connect to.</param>
    public TeamJoinSender(string host = "localhost") : base(host)
    {
    }

    /// <summary>
    /// Sends a join message to everyone already in the team.
    /// </summary>
    /// <param name="teamName">The name of the team to join.</param>
    /// <param name="reply">True if this is a reply, otherwise false.</param>
    /// <param name="joined">True if the user joined, false if the user left.</param>
    public void Join(string teamName, bool reply = false, bool joined = true)
    {
        string exchange = "team-join-" + teamName;
        this.Channel.ExchangeDeclare(exchange, "fanout", false, false, true, false, false, null);
        string message = this.SerializeMessage(UserName, joined, reply);
        byte[] body = Encoding.UTF8.GetBytes(message);

        this.Channel.BasicPublish(exchange, string.Empty, null, body);
    }

    /// <summary>
    /// Sends a join message to everyone already in the team. This function has a different name to avoid confusion.
    /// </summary>
    /// <param name="teamName">The team to leave.</param>
    public void Leave(string teamName)
    {
        this.Join(teamName, false, false);
    }

    /// <summary>
    /// Serializes a message.
    /// </summary>
    /// <param name="message">The message to serialize.</param>
    /// <param name="joined">True when the user joined the team, false if the user left.</param>
    /// <param name="reply">True when this is a reply.</param>
    /// <returns>The string serialized into jSon.</returns>
    private string SerializeMessage(string message, bool joined, bool reply)
    {
        return JsonConvert.SerializeObject(new TeamJoinInfo(message, joined, reply));
    }
}