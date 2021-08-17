using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textTimeRemaining;

    private float timerDuration, timeRemaining;
    private Coroutine timerCor;

    public float TimeRemaining { get; private set; }

    private void OnEnable(){
        EventManager.Instance.StartListening(Data.Events.OnElementDrawed, StartTimer);
        EventManager.Instance.StartListening(Data.Events.OnCorrectElementSelected, OnCorrectElementSelected);
    }
    private void OnDisable(){
        EventManager.Instance.StopListening(Data.Events.OnElementDrawed, StartTimer);
        EventManager.Instance.StopListening(Data.Events.OnCorrectElementSelected, OnCorrectElementSelected);
    }

    private void StartTimer(){
        timerDuration = Data.gameDuration;
        timeRemaining = timerDuration;
        timerCor = StartCoroutine(RunTimer());
        EventManager.Instance.TriggerEvent(Data.Events.OnTimerStarted);
    }

    public IEnumerator RunTimer(){
        var startTime = Time.time;

        while(Time.time < startTime + timerDuration){
            timeRemaining = timerDuration - Time.time - startTime;
            UpdateUI();
            yield return null;
        }

        EventManager.Instance.TriggerEvent(Data.Events.OnTimeRanOut);
    }

    private void UpdateUI(){
        textTimeRemaining.text = timeRemaining.ToString("0") + "s";
    }

    private void OnCorrectElementSelected(){
        if(timerCor != null){
            StopCoroutine(timerCor);
        }
    }
}
