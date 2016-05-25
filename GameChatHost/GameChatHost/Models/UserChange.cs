// <copyright file="UserChange.cs" company="None">
//     All rights reserved.
// </copyright>
// <author>Sander Dings</author>

namespace GameChatHost.Models
{
    /// <summary>
    /// The user change model that contains a username and whether the user was added or removed.
    /// </summary>
    public class UserChange
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
        /// Initializes a new instance of the <see cref="UserChange"/> class.
        /// </summary>
        /// <param name="username">The name of the user.</param>
        /// <param name="added">Was the user added or removed?</param>
        public UserChange(string username, bool added)
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
}
