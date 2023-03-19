using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class LinkHandlerForTMPText : MonoBehaviour, IPointerClickHandler
{
    private TMP_Text _tmpTextBox;
    private Camera _camera;

    public delegate void ClickOnLinkEvent (string text);
    public static event ClickOnLinkEvent OnClickedOnLinkEvent;

    // Start is called before the first frame update
    void Awake()
    {
        _tmpTextBox = GetComponent<TMP_Text>();
        _camera = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 mousePosition = new Vector3(eventData.position.x, eventData.position.y, 0);
        var linkTaggedText = TMP_TextUtilities.FindIntersectingLink(_tmpTextBox, mousePosition, _camera);

        if (linkTaggedText == -1) 
            return;

        TMP_LinkInfo linkInfo = _tmpTextBox.textInfo.linkInfo[linkTaggedText];

        string linkID = linkInfo.GetLinkID();
        if(linkID.Contains("www"))
        {
            Application.OpenURL(linkID);
            return;
        }

        OnClickedOnLinkEvent?.Invoke(linkInfo.GetLinkText());
    }
}
