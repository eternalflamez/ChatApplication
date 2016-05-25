// <copyright file="ChatReceiverManager.cs" company="None">
//     All rights reserved.
// </copyright>
// <author>Sander Dings</author>

using System;
using Newtonsoft.Json;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Main class for managing ChatReceivers, making sure the user receives all sorts of messages.
/// </summary>
[RequireComponent(typeof(UserController))]
public class ChatReceiverManager : Singleton<ChatReceiverManager>
{
    /// <summary>
    /// The controller that sends messages to the UI.
    /// </summary>
    private MessageController messageController;

    /// <summary>
    /// The list of chat receivers.
    /// </summary>
    private List<ChatReceiver> chatReceivers = new List<ChatReceiver>();

    /// <summary>
    /// Gets the current time and formats it for chat.
    /// </summary>
    /// <returns>The time between two straight brackets.</returns>
    public static string GetTime()
    {
        DateTime now = DateTime.Now;

        return string.Format("[{0}:{1}]", now.Hour, now.Minute);
    }

    /// <summary>
    /// Used once to setup the team chat receiver.
    /// </summary>
    /// <param name="teamName">The name of the team to connect to.</param>
    public void SetupTeamChat(string teamName)
    {
        ChatReceiver receiver = this.CreateReceiver();

        // Listen to new whispers to this person.
        receiver.StartExchangeListen("team-" + teamName);

        chatReceivers.Add(receiver);
    }

    /// <summary>
    /// Use this for initialization.
    /// </summary>
    private void Start()
    {
        this.SetupWhispers();
        
        this.SetupAllChat();
        this.messageController = MessageController.Instance;
    }

    /// <summary>
    /// Used once to setup the whispers receiver.
    /// </summary>
    private void SetupWhispers()
    {
        ChatReceiver receiver = this.CreateReceiver();

        // Listen to new whispers to this person.
        receiver.StartBasicListen("whisper-" + UserController.Instance.UserName);

        chatReceivers.Add(receiver);
    }
    
    /// <summary>
    /// Used once to setup the all chat receiver.
    /// </summary>
    private void SetupAllChat()
    {
        ChatReceiver receiver = this.CreateReceiver();
        receiver.StartExchangeListen("all");

        chatReceivers.Add(receiver);
    }

    /// <summary>
    /// The function to call when a message is received.
    /// </summary>
    /// <param name="s">The sender object.</param>
    /// <param name="e">The values associated with the event.</param>
    private void MessageReceived(object s, Receiver.ConsumerEventArgs e)
    {
        try
        {
            UserMessage userMessage = JsonConvert.DeserializeObject<UserMessage>(e.Message);
            SenderType typeToSendTo;

            if (e.Channel == string.Empty)
            {
                typeToSendTo = SenderType.Whisper;
            }
            else
            {
                switch (e.Channel[0])
                {
                    case 't':
                        typeToSendTo = SenderType.Team;
                        break;

                    case 'a':
                        typeToSendTo = SenderType.All;
                        break;

                    default:
                        // Should never happen
                        typeToSendTo = SenderType.System;
                        Debug.LogError("Couldn't resolve channel on ChatReceiverManager. " + e.Message + " || channel: " + e.Channel);
                        break;
                }
            }

            string message = string.Format("{0} {1}: {2}", GetTime(), userMessage.Username, userMessage.Message);

            this.messageController.Send(message, typeToSendTo);
            
            Debug.Log("Processed: " + e.Message + ", which was added to channel: " + e.Channel);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error converting the received message:" + e.Message);
            Debug.Log(ex.Message);
        }
    }

    /// <summary>
    /// Creates a new receiver.
    /// </summary>
    /// <returns>The newly created receiver object.</returns>
    private ChatReceiver CreateReceiver()
    {
        ChatReceiver cr = new ChatReceiver();
        cr.ReceiveEvent += this.MessageReceived;
        return cr;
    }

    private void OnApplicationQuit()
    {
        foreach (ChatReceiver receiver in chatReceivers)
        {
            receiver.Disconnect();
        }
    }
}