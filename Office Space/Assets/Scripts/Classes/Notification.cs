using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Notification
{
    public string Text { get; set; }
    public bool Read { get; set; }

    /// <summary>
    /// New Notification.
    /// </summary>
    /// <param name="text"></param>
    public Notification (string text)
    {
        Text = text;
        Read = false;
    }
}
