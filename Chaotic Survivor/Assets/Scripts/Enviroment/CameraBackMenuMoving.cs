using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBackMenuMoving : MonoBehaviour
{
    [SerializeField] private Transform[] cameraPos;
    [SerializeField] private float speedMove;
    [SerializeField] private float waitTimer;
    [SerializeField] private int target;
    private bool isOnPlace = false;
    private float speedCur;
    // Start is called before the first frame update
    void Awake()
    {
        target = Random.Range(0, cameraPos.Length);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, cameraPos[target].position) < 0.001f)
        {
            if(!isOnPlace)
            {
                StartCoroutine(ChangeCameraPos());
            }
        }
        else
        { 
            speedCur = speedMove * Time.fixedDeltaTime;
            transform.position = Vector3.MoveTowards(transform.position, cameraPos[target].position, speedCur);
        }
    }

    IEnumerator ChangeCameraPos()
    {        
        isOnPlace = true;
        yield return new WaitForSeconds(waitTimer);
        target = Random.Range(0, cameraPos.Length);
        //target = (target + 1) % cameraPos.Length;
        isOnPlace = false;
    }
}
