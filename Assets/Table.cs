using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] private Animator anim;
    private void OnEnable(){
        EventManager.Instance.StartListening(Data.Events.OnStartButtonPressed, Init);
        EventManager.Instance.StartListening(Data.Events.OnCorrectElementSelected, Hide);
    }
    private void OnDisable(){
        EventManager.Instance.StopListening(Data.Events.OnStartButtonPressed, Init);
        EventManager.Instance.StopListening(Data.Events.OnCorrectElementSelected, Hide);
    }

    private void Init(){
        anim.SetTrigger("Show");
    }

    private void Hide(){
        anim.SetTrigger("Hide");
    }
}
