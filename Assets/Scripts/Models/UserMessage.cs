// <copyright file="UserMessage.cs" company="None">
//     All rights reserved.
// </copyright>
// <author>Sander Dings</author>

/// <summary>
/// Model class for a user message.
/// </summary>
public class UserMessage
{
    /// <summary>
    /// The message that a user sent us.
    /// </summary>
    private string message;
    
    /// <summary>
    /// The username of the message we got sent.
    /// </summary>
    private string username;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="UserMessage"/> class.
    /// </summary>
    /// <param name="username">The username of the sender of the message.</param>
    /// <param name="message">The content of the message.</param>
    public UserMessage(string username, string message)
    {
        this.message = message;
        this.username = username;
    }

    /// <summary>
    /// Gets the message.
    /// </summary>
    public string Message
    {
        get { return this.message; }
    }

    /// <summary>
    /// Gets the username associated with message.
    /// </summary>
    public string Username
    {
        get { return this.username; }
    }
}
