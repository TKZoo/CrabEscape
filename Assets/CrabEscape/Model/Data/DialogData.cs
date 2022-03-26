using System;
using UnityEngine;

[Serializable]
public class DialogData
{
    [SerializeField] private string[] _sentences;
    [SerializeField] private Sprite[] _narratorSprite;
    public string[] Sentences => _sentences;
    public Sprite[] NarratorSprite => _narratorSprite;
}
