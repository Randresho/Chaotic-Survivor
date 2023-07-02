using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AbiltyButtonInfo : MonoBehaviour
{
    public AbilityType type;
    public Image image;
    public TextMeshProUGUI abilityTitle;
    public TextMeshProUGUI abilityDescription;

    public void SetInfo(Sprite sprite, string title, string description)
    {
        image.sprite = sprite;
        abilityTitle.text = title;
        abilityDescription.text = description;
    }
}
