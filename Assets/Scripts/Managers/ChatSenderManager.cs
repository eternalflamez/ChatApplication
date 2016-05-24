// <copyright file="ChatSenderManager.cs" company="None">
//     All rights reserved.
// </copyright>
// <author>Sander Dings</author>
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the ChatSenders to send messages over multiple queues.
/// </summary>
public class ChatSenderManager : MonoBehaviour
{
    /// <summary>
    /// The text field to read text from.
    /// </summary>
    [SerializeField]
    private Text textfield;

    /// <summary>
    /// The Controller which sends messages to the UI.
    /// </summary>
    private MessageController messageController;

    /// <summary>
    /// A sender to use to send messages over rabbitMQ.
    /// </summary>
    private ChatSender chatSender;

    /// <summary>
    /// The channel we're sending to.
    /// </summary>
    private SenderType senderType;

    /// <summary>
    /// Sends a message by reading the text field and judging which type we're using.
    /// </summary>
    public void Send()
    {
        string message = this.textfield.text;
        string[] split = message.Split(' ');
        string potentionalCommand = split[0];
        string text = this.ConnectStringsFromArray(split, 1);
        string to = string.Empty;
        
        if (split.Length != 1)
        {
            switch (potentionalCommand)
            {
                case "/w":
                    if (split.Length <= 2)
                    {
                        this.messageController.Send("Invalid whisper, no username or text was found.", SenderType.System);
                        return;
                    }

                    to = split[1];
                    text = this.ConnectStringsFromArray(split, 2);
                    this.senderType = SenderType.Whisper;
                    break;
                case "/t":
                    this.senderType = SenderType.Team;
                    break;
                case "/a":
                    this.senderType = SenderType.All;
                    break;
                default:
                    text = this.DefaultProcess(split);
                    to = split[0];
                    break;
            }
        }
        else
        {
            text = this.DefaultProcess(split);
            to = split[0];
        }

        if (text == string.Empty)
        {
            return;
        }

        switch (this.senderType)
        {
            case SenderType.Whisper:
                if (to != UserController.Instance.UserName)
                {
                    this.messageController.Send(ChatReceiverManager.GetTime() + " To " + to + ": " + text, SenderType.Whisper);
                    this.SendWhisper(to, text);
                }

                break;
            case SenderType.Team:
                string teamname = TeamJoinManager.Instance.TeamName;

                if (teamname != string.Empty)
                {
                    this.chatSender.StartExchangeSend("team-" + teamname, text);
                }
                else
                {
                    messageController.Send("[System]: You have not joined a team yet!", SenderType.System);
                }
                
                break;
            case SenderType.All:
                this.chatSender.StartExchangeSend("all", text);
                break;
        }
    }

    /// <summary>
    /// Sends a whisper to a specific person.
    /// </summary>
    /// <param name="target">Name of the person to send to.</param>
    /// <param name="text">The message we're sending.</param>
    public void SendWhisper(string target, string text)
    {
        this.chatSender.StartSimpleSend("whisper-" + target, text);
    }

    /// <summary>
    /// Sets a new sender type from the UI buttons.
    /// </summary>
    /// <param name="value">The new senderType from the enum list.</param>
    public void SetNewSenderType(int value)
    {
        this.senderType = (SenderType)value;
        this.messageController.ChangeSenderType(this.senderType);
    }

    /// <summary>
    /// Use this for initialization
    /// </summary>
    private void Start()
    {
        this.chatSender = new ChatSender();
        this.senderType = SenderType.All;

        this.messageController = MessageController.Instance;
    }

    /// <summary>
    /// Processes a default send.
    /// </summary>
    /// <param name="split">The original message, split.</param>
    /// <returns>The text to send.</returns>
    private string DefaultProcess(string[] split)
    {
        if (split.Length < 2 && this.senderType == SenderType.Whisper)
        {
            this.messageController.Send("Invalid whisper, no username or text was found.", SenderType.System);
            return string.Empty;
        }

        int start = 0;

        if (this.senderType == SenderType.Whisper)
        {
            start = 1;
        }

        string text = this.ConnectStringsFromArray(split, start);

        return text;
    }

    /// <summary>
    /// Reads an array and reattaches the strings using the delimiter.
    /// </summary>
    /// <param name="array">The array to reattach.</param>
    /// <param name="start">The first position in the array to reattach.</param>
    /// <param name="delimiter">The string to glue all the pieces together with.</param>
    /// <returns>A string containing all the array pieces starting at the start position, glued together with the delimiter.</returns>
    private string ConnectStringsFromArray(string[] array, int start, string delimiter = " ")
    {
        string result = string.Empty;

        for (int i = start; i < array.Length; i++)
        {
            result += array[i];

            if (i != array.Length - 1)
            {
                result += delimiter;
            }
        }

        return result;
    }
}
