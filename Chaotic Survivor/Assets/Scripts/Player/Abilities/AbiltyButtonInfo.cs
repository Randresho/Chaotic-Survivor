using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbiltyButtonInfo : MonoBehaviour
{
    public AbilityType type;
    public Image image;
    public Text abilityTitle;
    public Text abilityDescription;

    public void SetInfo(Sprite sprite, string title, string description)
    {
        image.sprite = sprite;
        abilityTitle.text = title;
        abilityDescription.text = description;
    }
}
