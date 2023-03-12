using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum timerActions { Destroy, Active, }
public class Timer : MonoBehaviour
{
    [SerializeField] private float setTimer = 0f;
    [SerializeField] private timerActions timerActions;
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if(setTimer > 0f)
            setTimer -= Time.fixedDeltaTime;
        else
        {
            switch (timerActions)
            {
                case timerActions.Destroy:
                    gameObject.SetActive(false);
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
