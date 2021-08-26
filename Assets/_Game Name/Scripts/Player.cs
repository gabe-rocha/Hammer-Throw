using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Animator anim;
    private void OnEnable(){
        EventManager.Instance.StartListening(Data.Events.OnGameStarted, GameStarted);
        EventManager.Instance.StartListening(Data.Events.OnCorrectElementSelected, ThrowHammer);
    }
    private void OnDisable(){
        EventManager.Instance.StopListening(Data.Events.OnGameStarted, GameStarted);
        EventManager.Instance.StopListening(Data.Events.OnCorrectElementSelected, ThrowHammer);
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }


    private void GameStarted(){
        anim.SetTrigger("Spin");
    }

    private void ThrowHammer(){
        anim.SetTrigger("Throw");
    }
}
