using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrivacyPage : MonoBehaviour
{
    [SerializeField] private Toggle isAccept;
    [SerializeField] private Button acceptBtn;

    void Awake()
    {
        acceptBtn.interactable = isAccept.isOn;        
    }

    public void AcceptPrivacy()
    {
        acceptBtn.interactable = isAccept.isOn;
    }
}
