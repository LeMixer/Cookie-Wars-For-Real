using System;
using UnityEngine;
using UnityEngine.Events;

public class ClickDownFor2 : MonoBehaviour
{   
    public float clickDuration = 1.3f;
    public UnityEvent OnClickDownFor2;

    bool clicking = false;
    float downTime = 0;

    void Start()
    {
        if(OnClickDownFor2 == null)
        {
            OnClickDownFor2 = new UnityEvent();
        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            downTime = 0;
            clicking =  true;    
        }

        if (clicking == true && Input.GetMouseButton(0))
        {
            downTime += Time.deltaTime;

            if(downTime >= clickDuration) 
            {
                clicking = false;
                OnClickDownFor2.Invoke();
            } 
        }

        if(clicking == true && Input.GetMouseButtonUp(0))
        {
            clicking = false;
        }
    }
}