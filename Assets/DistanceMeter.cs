using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DistanceMeter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textDistance;
    private float startX, endX;
    private GameObject hammer;

    private void OnEnable(){
        EventManager.Instance.StartListeningWithGOParam(Data.Events.OnHammerThrown, OnHammerThrown);
        EventManager.Instance.StartListeningWithGOParam(Data.Events.OnHammerHitGround, OnHammerHitGround);
    }


    private void OnDisable(){
        EventManager.Instance.StopListeningWithGOParam(Data.Events.OnHammerThrown, OnHammerThrown);
        EventManager.Instance.StopListeningWithGOParam(Data.Events.OnHammerHitGround, OnHammerHitGround);
    }

    private void Start()
    {
        textDistance.text = "0.00m";
    }

    private void Update(){
        if(hammer != null)
            textDistance.text = (hammer.transform.position.x - startX).ToString("0.00" + "m");
    }

    private void OnHammerThrown(GameObject hammer)
    {
        this.hammer = hammer;
        startX = hammer.transform.position.x;
    }
    private void OnHammerHitGround(GameObject hammer){
        endX = hammer.transform.position.x;
    }
}
