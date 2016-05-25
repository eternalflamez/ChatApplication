// <copyright file="SenderButton.cs" company="None">
//     All rights reserved.
// </copyright>
// <author>Sander Dings</author>
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A model to link a button to a SenderType in the Unity UI.
/// </summary>
[System.Serializable]
public class SenderButton
{
    /// <summary>
    /// The button we're linking.
    /// </summary>
    [SerializeField]
    private Button button;

    /// <summary>
    /// The SenderType that belongs to the button.
    /// </summary>
    [SerializeField]
    private SenderType senderType;

    /// <summary>
    /// Gets the button.
    /// </summary>
    public Button Button
    {
        get { return this.button; }
    }

    /// <summary>
    /// Gets the SenderType.
    /// </summary>
    public SenderType SenderType
    {
        get { return this.senderType; }
    }
    
    /// <summary>
    /// Highlights the button if it's the correct type.
    /// </summary>
    /// <param name="type">The type to compare to.</param>
    public void Highlight(SenderType type)
    {
        if (this.senderType == type)
        {
            this.Button.image.color = Color.red;
        }
        else
        {
            this.Button.image.color = Color.white;
        }
    }
}