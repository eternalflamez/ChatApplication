// <copyright file="UserJoinInfo.cs" company="None">
//     All rights reserved.
// </copyright>
// <author>Sander Dings</author>

/// <summary>
/// A model class containing a name of a user and whether the user was added or removed.
/// </summary>
public class UserJoinInfo
{
    /// <summary>
    /// The name of the user.
    /// </summary>
    private string username;

    /// <summary>
    /// Was the user added or removed?
    /// </summary>
    private bool added;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserJoinInfo"/> class.
    /// </summary>
    /// <param name="username">The name of the user.</param>
    /// <param name="added">Was the user added or removed?</param>
    public UserJoinInfo(string username, bool added)
    {
        this.username = username;
        this.added = added;
    }

    /// <summary>
    /// Gets the name of the user.
    /// </summary>
    public string Username
    {
        get
        {
            return this.username;
        }
    }

    /// <summary>
    /// Gets a value indicating whether the user was added or removed.
    /// </summary>
    public bool Added
    {
        get
        {
            return this.added;
        }
    }
}
