using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DistanceMeter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textDistance;
    [SerializeField] private Animator anim;
    private float startX, endX;
    private GameObject hammer;
    private bool hammerLanded = false;

    private void OnEnable(){
        EventManager.Instance.StartListening(Data.Events.OnStartButtonPressed, Init);
        EventManager.Instance.StartListeningWithGOParam(Data.Events.OnHammerThrown, OnHammerThrown);
        EventManager.Instance.StartListeningWithGOParam(Data.Events.OnHammerHitGround, OnHammerHitGround);
    }


    private void OnDisable(){
        EventManager.Instance.StopListening(Data.Events.OnStartButtonPressed, Init);
        EventManager.Instance.StopListeningWithGOParam(Data.Events.OnHammerThrown, OnHammerThrown);
        EventManager.Instance.StopListeningWithGOParam(Data.Events.OnHammerHitGround, OnHammerHitGround);
    }

    private void Init()
    {
        anim.SetTrigger("Show");
        textDistance.text = "0.00m";
    }

    private void Update(){
        if(hammer != null && !hammerLanded)
            textDistance.text = (hammer.transform.position.x - startX).ToString("0.00" + "m");
        else
            textDistance.text = (endX - startX).ToString("0.00" + "m");
    }

    private void OnHammerThrown(GameObject hammer)
    {
        this.hammer = hammer;
        startX = hammer.transform.position.x;
    }
    private void OnHammerHitGround(GameObject hammer){
        if(hammerLanded)
            return; //we want just the first hit

        hammerLanded = true;
        endX = hammer.transform.position.x;

        EventManager.Instance.TriggerEvent(Data.Events.OnFinalDistanceCalculated);
    }

    internal string GetDistance()
    {
        return (endX - startX).ToString("0.00" + "m");
    }
}
