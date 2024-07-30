using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    public string SpeakerName;
    public Sprite SpeakerSprite;
    [SerializeField] List<string> lines;

    public List<string> Lines
    {
        get { return lines; }
    }
}