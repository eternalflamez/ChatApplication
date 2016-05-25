// <copyright file="MessageController.cs" company="None">
//     All rights reserved.
// </copyright>
// <author>Sander Dings</author>
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Consumes messages to display to the user.
/// </summary>
public class MessageController : Singleton<MessageController>
{
    /// <summary>
    /// The Text to display UI messages in.
    /// </summary>
    [SerializeField]
    private Text textArea;

    /// <summary>
    /// A list of SenderButtons.
    /// </summary>
    [SerializeField]
    private List<SenderButton> senderButtons;

    /// <summary>
    /// The InputField that the user types into.
    /// </summary>
    [SerializeField]
    private InputField inputField;

    /// <summary>
    /// The type of messages we're displaying right now.
    /// </summary>
    private SenderType senderType = SenderType.All;

    /// <summary>
    /// A list of messages per sender type.
    /// </summary>
    private Dictionary<SenderType, string> messages;

    /// <summary>
    /// When true, the UI needs to be changed next frame.
    /// </summary>
    private bool dirtyUI;

    /// <summary>
    /// When true, the buttons needs to be changed next frame.
    /// </summary>
    private bool dirtyButtons = true;

    /// <summary>
    /// When true, the textbox with user text is going to get cleared.
    /// </summary>
    private bool dirtyInput = false;

    /// <summary>
    /// Sends a message to the UI for display.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <param name="senderType">The type of message we're showing to the user.</param>
    public void Send(string message, SenderType senderType)
    {
        if (this.messages[senderType] != string.Empty || senderType == SenderType.System)
        {
            message = "\r\n" + message;
        }

        if (senderType != SenderType.System)
        {
            this.messages[senderType] += message;
        }
        else
        {
            var values = Enum.GetValues(typeof(SenderType));

            foreach (SenderType value in values)
            {
                this.messages[value] += message;
            }
        }

        this.UpdateText();
    }

    /// <summary>
    /// Changes the message type we're displaying right now.
    /// </summary>
    /// <param name="newSenderType">The new type of messages we're going to display.</param>
    public void ChangeSenderType(SenderType newSenderType)
    {
        this.senderType = newSenderType;
        this.UpdateText();
        this.UpdateButtons();
    }

    /// <summary>
    /// Use this for initialization.
    /// </summary>
    private void Start()
    {
        this.messages = new Dictionary<SenderType, string>();
        foreach (SenderType type in Enum.GetValues(typeof(SenderType)))
        {
            this.messages[type] = string.Empty;
        }
    }

    /// <summary>
    /// Sets the dirty flag for UI, so it's going to get changed next frame.
    /// </summary>
    private void UpdateText()
    {
        this.dirtyUI = true;
    }

    /// <summary>
    /// Sets the dirty flag for buttons, so it's going to get changed next frame.
    /// </summary>
    private void UpdateButtons()
    {
        this.dirtyButtons = true;
    }

    /// <summary>
    /// Clears the input field.
    /// </summary>
    private void ClearText()
    {
        this.dirtyInput = true;
    }

    /// <summary>
    /// Called each frame.
    /// </summary>
    private void Update()
    {
        if (this.dirtyUI)
        {
            this.textArea.text = this.messages[this.senderType];
            this.dirtyUI = false;
        }

        if (this.dirtyButtons)
        {
            foreach (SenderButton senderButton in this.senderButtons)
            {
                senderButton.Highlight(this.senderType);
            }

            this.dirtyButtons = false;
        }

        if (this.dirtyInput)
        {
            this.inputField.text = string.Empty;
            this.dirtyInput = false;
        }
    }
}
