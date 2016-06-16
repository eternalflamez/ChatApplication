// <copyright file="Program.cs" company="None">
//     All rights reserved.
// </copyright>
// <author>Sander Dings</author>

namespace GameChatHost
{
    using System;
    using System.Collections.Generic;
    using Logics;
    using Models;

    /// <summary>
    /// The main program class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The lock variable for the list.
        /// </summary>
        private static object listLock = new object();

        /// <summary>
        /// The list of logged in users. Saved in a dictionary so improve lookup speed.
        /// </summary>
        private Dictionary<string, byte> userNames = new Dictionary<string, byte>();

        /// <summary>
        /// The sender object we use to send replies.
        /// </summary>
        private Sender sender = new Sender();
        
        /// <summary>
        /// Gets called when the user opens the application.
        /// </summary>
        /// <param name="args">Extra arguments for when you run the application.</param>
        public static void Main(string[] args)
        {
            Program p = new Program();
            Receiver r = new Receiver();
            r.ReceiveEvent += p.MessageReceived;

            r.StartExchangeListen("userlogin");
            r.StartExchangeListen("userlogout");

            string s = "";

            while ((s = Console.ReadLine()) != "exit")
            {
                p.AddUser(s);
            }

        }

        /// <summary>
        /// Adds a user to the list.
        /// </summary>
        /// <param name="userName">The username to add.</param>
        private void AddUser(string userName)
        {
            lock (listLock)
            {
                if(!this.userNames.ContainsKey(userName))
                {
                    this.userNames.Add(userName, 0);
                }
            }

            UserChange userChange = new UserChange(userName, true);
            this.sender.StartExchangeSend("userchange", userChange);
        }

        /// <summary>
        /// Removes a user from the list.
        /// </summary>
        /// <param name="userName">The username to remove.</param>
        private void RemoveUser(string userName)
        {
            lock (listLock)
            {
                this.userNames.Remove(userName);
            }

            UserChange userChange = new UserChange(userName, false);
            this.sender.StartExchangeSend("userchange", userChange);
        }

        /// <summary>
        /// The function to call when a message is received.
        /// </summary>
        /// <param name="s">The sender object.</param>
        /// <param name="e">The values associated with the event.</param>
        private void MessageReceived(object s, Receiver.ConsumerEventArgs e)
        {
            switch (e.Channel)
            {
                case "userlogin":
                    this.sender.StartExchangeSend("newuserlist", this.userNames);
                    this.AddUser(e.Message);

                    Console.WriteLine(e.Message + " joined.");
                    break;

                case "userlogout":
                    this.RemoveUser(e.Message);
                    Console.WriteLine(e.Message + " left.");
                    break;

                default:
                    // Error.
                    Console.WriteLine("Error resolving channel: " + e.Channel);
                    return;
            }
        }
    }
}
