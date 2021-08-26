using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerMeter : MonoBehaviour
{
    [SerializeField] private Animator anim;

     private void OnEnable(){
        EventManager.Instance.StartListening(Data.Events.OnStartButtonPressed, Show);
        EventManager.Instance.StartListening(Data.Events.OnCorrectElementSelected, Hide);
    }


    private void OnDisable(){
        EventManager.Instance.StopListening(Data.Events.OnStartButtonPressed, Show);
        EventManager.Instance.StopListening(Data.Events.OnCorrectElementSelected, Hide);
    }

    private void Show()
    {
        anim.SetTrigger("Show");
    }

    private void Hide(){
        anim.SetTrigger("Hide");
    }
}
