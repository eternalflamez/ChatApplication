// <copyright file="Singleton.cs" company="None">
//     All rights reserved.
// </copyright>
// <author>Sander Dings</author>
using UnityEngine;

/// <summary>
/// Generic singleton class, allows for finding instances of a single class (of which there should be only one).
/// </summary>
/// <typeparam name="T">The class to make a singleton of.</typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// The instance of the singleton.
    /// </summary>
    private static T instance;

    /// <summary>
    /// Gets or sets the current instance of the Singleton. Logs an error if it cannot find one.
    /// </summary>
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));
                if (instance == null)
                {
                    Debug.LogError("Error: Could not find an instance of " + typeof(T));
                }
            }

            return instance;
        }

        protected set
        {
            instance = value;
        }
    }
}
