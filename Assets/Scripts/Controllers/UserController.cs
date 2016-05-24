// <copyright file="UserController.cs" company="None">
//     All rights reserved.
// </copyright>
// <author>Sander Dings</author>

using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The user controller. Manages all things that belong to the user.
/// </summary>
public class UserController : Singleton<UserController>
{
    /// <summary>
    /// The username of the current user.
    /// </summary>
    [SerializeField]
    private string userName;

    /// <summary>
    /// The text field that contains the username.
    /// </summary>
    [SerializeField]
    private Text userNameText;

    /// <summary>
    /// The text field prefab to hold a username.
    /// </summary>
    [SerializeField]
    private Text playerNamesText;

    /// <summary>
    /// The Transform to be the parent of the texts.
    /// </summary>
    [SerializeField]
    private Transform textParent;

    /// <summary>
    /// The list of Text fields holding the usernames.
    /// </summary>
    private Dictionary<string, Text> userNameHolders;

    /// <summary>
    /// The receiver for user changes.
    /// </summary>
    private ChatReceiver changeReceiver;

    /// <summary>
    /// The receiver for the complete list of users.
    /// </summary>
    private ChatReceiver userListReceiver;

    /// <summary>
    /// The list of names to add.
    /// </summary>
    private List<string> namesToAdd;

    /// <summary>
    /// The list of names to remove.
    /// </summary>
    private List<string> namesToRemove;

    /// <summary>
    /// Gets the username of the current user.
    /// </summary>
    public string UserName
    {
        get { return this.userName; }
    }

    /// <summary>
    /// Used for initialization.
    /// </summary>
    public void Awake()
    {
        this.namesToAdd = new List<string>();
        this.namesToRemove = new List<string>();
        this.userNameHolders = new Dictionary<string, Text>();

        this.userName = Random.Range(100000000, 999999999).ToString();
        this.userNameText.text = this.userName;
        
        this.changeReceiver = new ChatReceiver();
        this.changeReceiver.ReceiveEvent += this.UpdateReceived;
        this.changeReceiver.StartExchangeListen("userchange");

        this.userListReceiver = new ChatReceiver();
        this.userListReceiver.ReceiveEvent += this.UserListReceived;
        this.userListReceiver.StartExchangeListen("newuserlist");

        UserChangeSender sender = new UserChangeSender();
        sender.Login();
        sender.Disconnect();
    }

    /// <summary>
    /// Called once per frame, used to update the UI.
    /// </summary>
    public void Update()
    {
        lock(namesToAdd)
        {
            for (int i = 0; i < namesToAdd.Count; i++)
            {
                // If we're gonna delete it anyway, why go through the effort of creating the object?
                if (!namesToRemove.Contains(namesToAdd[i]))
                {
                    Text text = Instantiate(playerNamesText);
                    text.text = namesToAdd[i];

                    text.transform.SetParent(textParent);

                    userNameHolders.Add(namesToAdd[i], text);
                }
            }

            namesToAdd.Clear();
        }

        lock (namesToRemove)
        {
            for (int i = 0; i < namesToRemove.Count; i++)
            {
                if (userNameHolders.ContainsKey(namesToRemove[i]))
                {
                    userNameHolders.Remove(namesToRemove[i]);
                    Destroy(userNameHolders[namesToRemove[i]]);
                }
            }

            namesToRemove.Clear();
        }
    }

    /// <summary>
    /// Adds a user to the list.
    /// </summary>
    /// <param name="userName">The username to add.</param>
    private void AddUser(string userName)
    {
        lock (this.namesToAdd)
        {
            this.namesToAdd.Add(userName);
        }
    }

    /// <summary>
    /// Removes a user from the list.
    /// </summary>
    /// <param name="userName">The username to remove.</param>
    private void RemoveUser(string userName)
    {
        lock (this.namesToRemove)
        {
            this.namesToRemove.Remove(userName);
        }
    }

    /// <summary>
    /// Overwrites the users dictionary with new values.
    /// </summary>
    /// <param name="usernames">The new list of usernames.</param>
    private void OverwriteUsers(Dictionary<string, byte> usernames)
    {
        lock (this.namesToAdd)
        {
            foreach (string s in usernames.Keys)
            {
                this.namesToAdd.Add(UserName);
            }
        }
    }

    /// <summary>
    /// The function to call when a message is received.
    /// </summary>
    /// <param name="s">The sender object.</param>
    /// <param name="e">The values associated with the event.</param>
    private void UpdateReceived(object s, Receiver.ConsumerEventArgs e)
    {
        UserJoinInfo joinInfo = JsonConvert.DeserializeObject<UserJoinInfo>(e.Message);

        if (joinInfo.Added)
        {
            this.AddUser(e.Message);
        }
        else
        {
            this.RemoveUser(e.Message);
        }
    }

    /// <summary>
    /// The function to call when a list of users is received.
    /// </summary>
    /// <param name="s">The sender object.</param>
    /// <param name="e">The values associated with the event.</param>
    private void UserListReceived(object s, Receiver.ConsumerEventArgs e)
    {
        Dictionary<string, byte> users = JsonConvert.DeserializeObject<Dictionary<string, byte>>(e.Message);

        this.OverwriteUsers(users);
    }
}
