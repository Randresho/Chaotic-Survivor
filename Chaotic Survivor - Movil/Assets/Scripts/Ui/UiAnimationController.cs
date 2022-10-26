using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class UiAnimationController : MonoBehaviour
{
    public UIParallelAnimation[] parallelAnimation = null;
    public UIFixedAnimation[] fixedAnimation = null;
    [SerializeField] private float timer = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < parallelAnimation.Length; i++)
        {
            parallelAnimation[i].duration = timer;
            if (parallelAnimation[i] == null)
                Debug.Log("No tengo parallel animation" + i);
        }
        for (int i = 0; i < fixedAnimation.Length; i++)
        {
            fixedAnimation[i].duration = timer;
            if (fixedAnimation == null)
                Debug.Log("No tengo fixed animation" + i);
        }
    }

    public void PlayAnimationParallel()
    {
        StartCoroutine(playAnimationPa());
    }

    IEnumerator playAnimationPa()
    {
        //showPanel.SetActive(on);
        yield return new WaitForSeconds(timer) ;
        for (int i = 0; i < parallelAnimation.Length; i++)
        {
            parallelAnimation[i].PlayReverse();
        }
    }

    public void PlayAnimationFixed()
    {
        StartCoroutine(playAnimationFi());
    }

    IEnumerator playAnimationFi()
    {
        //showPanel.SetActive(on);
        yield return new WaitForSeconds(timer);
        for (int i = 0; i < fixedAnimation.Length; i++)
        {
            fixedAnimation[i].PlayReverse();
        }
    }
}
