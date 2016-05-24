// <copyright file="SenderType.cs" company="None">
//     All rights reserved.
// </copyright>
// <author>Sander Dings</author>

/// <summary>
/// The list of channels we can send to.
/// </summary>
public enum SenderType
{
    /// <summary>
    /// Sends whispers to a specific user.
    /// </summary>
    Whisper,

    /// <summary>
    /// Sends only to your team.
    /// </summary>
    Team,

    /// <summary>
    /// Sends to all users.
    /// </summary>
    All,

    /// <summary>
    /// Sends to all of your channels.
    /// </summary>
    System
}
