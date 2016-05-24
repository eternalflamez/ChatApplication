// <copyright file="TeamJoinInfo.cs" company="None">
//     All rights reserved.
// </copyright>
// <author>Sander Dings</author>

/// <summary>
/// Holds information about a user that joined or left a team.
/// </summary>
public class TeamJoinInfo
{
    /// <summary>
    /// The username of the user.
    /// </summary>
    private string userName;

    /// <summary>
    /// True if the user joined, false if the user left.
    /// </summary>
    private bool joined;

    /// <summary>
    /// Indicates whether or not this is a reply.
    /// </summary>
    private bool reply;

    /// <summary>
    /// Initializes a new instance of the <see cref="TeamJoinInfo"/> class.
    /// </summary>
    /// <param name="userName">The username of the user that just joined.</param>
    /// <param name="joined">The joined status.</param>
    /// <param name="reply">Is this a reply?</param>
    public TeamJoinInfo(string userName, bool joined, bool reply)
    {
        this.userName = userName;
        this.joined = joined;
        this.reply = reply;
    }
    
    /// <summary>
    /// Gets the username of the user.
    /// </summary>
    public string UserName
    {
        get { return this.userName; }
    }

    /// <summary>
    /// Gets a value indicating whether or not the user joined or left.
    /// </summary>
    public bool Joined
    {
        get { return this.joined; }
    }

    /// <summary>
    /// Gets a value indicating whether or not this is a reply.
    /// </summary>
    public bool Reply
    {
        get { return this.reply; }
    }
}