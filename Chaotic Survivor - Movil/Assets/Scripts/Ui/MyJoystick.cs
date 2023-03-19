using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyJoystick : MonoBehaviour
{
    private Camera _camera;
    [SerializeField] private GameObject leftJoystick = null;

    public delegate void ClickOnLinkEvent(string text);
    public static event ClickOnLinkEvent OnClickedOnLinkEvent;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Vector2 pointerPosition = eventData.pressEventCamera.ScreenToWorldPoint(Input.mousePosition);

        leftJoystick.SetActive(true);
        leftJoystick.transform.position = pointerPosition;
    }

    public virtual void OnPointerUp(PointerEventData eventData) 
    {
        leftJoystick.SetActive(false);
        leftJoystick.transform.position = Vector2.zero;
    }
}
