using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum timerActions { Destroy, Active, }
public class Timer : MonoBehaviour
{
    [SerializeField] private float setTimer = 0f;
    [SerializeField] private float setMaxTimer = 0f;
    [SerializeField] private timerActions timerActions;
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if(setTimer < setMaxTimer)
            setTimer += Time.fixedDeltaTime;
        else
        {
            switch (timerActions)
            {
                case timerActions.Destroy:
                    gameObject.SetActive(false);
                    setTimer = 0f;
                    break;
                case timerActions.Active:
                    //Aun no se que hara
                    break;
                default:
                    break;
            }
        }
    }

}
