// <copyright file="TeamJoinManager.cs" company="None">
//     All rights reserved.
// </copyright>
// <author>Sander Dings</author>

using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The main controller for team joining and leaving.
/// </summary>
public class TeamJoinManager : Singleton<TeamJoinManager>
{
    /// <summary>
    /// The text label that shows the team name.
    /// </summary>
    [SerializeField]
    private Text teamNameText;

    /// <summary>
    /// The text label that shows all the player names in the team.
    /// </summary>
    [SerializeField]
    private Text playerNamesText;

    /// <summary>
    /// The list of usernames of people in our team.
    /// </summary>
    private List<string> userNames;

    /// <summary>
    /// The receiver for this team.
    /// </summary>
    private TeamJoinReceiver teamJoinReceiver;

    /// <summary>
    /// The sender for this team.
    /// </summary>
    private TeamJoinSender teamJoinSender;

    /// <summary>
    /// The name of the team we're in right now.
    /// </summary>
    private string teamName = string.Empty;

    /// <summary>
    /// True when we are currently in a team, otherwise false.
    /// </summary>
    private bool inATeam = false;

    /// <summary>
    /// When this is true, the UI is getting updated next frame.
    /// </summary>
    private bool dirtyUI = false;

    /// <summary>
    /// Gets the current name of the team we're in.
    /// </summary>
    public string TeamName
    {
        get { return this.teamName; }
    }

    /// <summary>
    /// Used for initializing the object.
    /// </summary>
    public void Start()
    {
        this.userNames = new List<string>();
    }

    /// <summary>
    /// Called once per frame.
    /// </summary>
    public void Update()
    {
        if (this.dirtyUI)
        {
            this.teamNameText.text = (this.teamName == string.Empty) ? "None" : this.TeamName;

            string text = string.Empty;

            for (int i = 0; i < this.userNames.Count; i++)
            {
                string name = this.userNames[i];

                if (i != 0)
                {
                    name = "\r\n" + name;
                }

                text += name;
            }

            this.playerNamesText.text = text;

            this.dirtyUI = false;
        }
    }

    /// <summary>
    /// Sets the current team name if applicable. This should be called from the UI.
    /// </summary>
    /// <param name="input">The InputField to read from.</param>
    public void SetTeamName(InputField input)
    {
        string text = input.text;

        if (text != string.Empty)
        {
            this.JoinTeam(text);
        }

        input.text = string.Empty;
    }

    /// <summary>
    /// Leaves the current team.
    /// </summary>
    public void LeaveTeam()
    {
        if (this.inATeam)
        {
            this.teamJoinSender.Leave(this.teamName);

            this.teamJoinReceiver.Disconnect();
            this.teamJoinSender.Disconnect();
            
            this.teamName = "";
            this.userNames = new List<string>();

            this.dirtyUI = true;
        }
    }

    /// <summary>
    /// Joins a specific team. Does nothing if we're already in that team.
    /// </summary>
    /// <param name="name">The name of the team to join.</param>
    private void JoinTeam(string name)
    {
        if (name == this.TeamName)
        {
            return;
        }

        this.LeaveTeam();
        
        this.userNames = new List<string>();

        this.teamJoinReceiver = new TeamJoinReceiver();
        this.teamJoinReceiver.ReceiveEvent += this.MessageReceived;
        this.teamJoinReceiver.Listen(name);

        this.teamJoinSender = new TeamJoinSender();
        this.teamJoinSender.Join(name);

        ChatReceiverManager.Instance.SetupTeamChat(name);

        this.teamName = name;
        this.inATeam = true;
        this.dirtyUI = true;
    }

    /// <summary>
    /// The function to call when a message is received.
    /// </summary>
    /// <param name="s">The sender object.</param>
    /// <param name="e">The values associated with the event.</param>
    private void MessageReceived(object s, ChatReceiver.ConsumerEventArgs e)
    {
        TeamJoinInfo joinInfo = JsonConvert.DeserializeObject<TeamJoinInfo>(e.Message);
        
        if (joinInfo.Joined && !this.userNames.Contains(joinInfo.UserName))
        {
            MessageController.Instance.Send(joinInfo.UserName + " joined.", SenderType.Team);

            // If someone joined and is not in the list yet.
            this.userNames.Add(joinInfo.UserName);

            // We better tell them we're here too. Unless ofcourse, we're them.
            if (joinInfo.UserName != UserController.Instance.UserName && !joinInfo.Reply)
            {
                this.teamJoinSender.Join(this.teamName, true);
            }
        }
        else if (!joinInfo.Joined && this.userNames.Contains(joinInfo.UserName))
        {
            MessageController.Instance.Send(joinInfo.UserName + " left.", SenderType.Team);

            // If someone left and was still in the list.
            this.userNames.Remove(joinInfo.UserName);
        }

        this.dirtyUI = true;
    }

    /// <summary>
    /// Called when we leave the game. We should stop listening here.
    /// </summary>
    private void OnApplicationQuit()
    {
        this.LeaveTeam();
        UserChangeSender userChangeSender = new UserChangeSender();
        userChangeSender.Logout();
    }
}
