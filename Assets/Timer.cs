using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textTimeRemaining;
    public float TimeRemaining { get => timeRemaining; private set => timeRemaining = value; }


    private float timerDuration, timeRemaining;
    private bool timerRunning = false;

    private void OnEnable(){
        EventManager.Instance.StartListening(Data.Events.OnElementDrawed, StartTimer);
        EventManager.Instance.StartListening(Data.Events.OnCorrectElementSelected, OnCorrectElementSelected);
    }
    private void OnDisable(){
        EventManager.Instance.StopListening(Data.Events.OnElementDrawed, StartTimer);
        EventManager.Instance.StopListening(Data.Events.OnCorrectElementSelected, OnCorrectElementSelected);
    }

    private void StartTimer(){
        timerRunning = true;
        timerDuration = Data.gameDuration;
        timeRemaining = timerDuration;
        StartCoroutine(RunTimer());
        EventManager.Instance.TriggerEvent(Data.Events.OnTimerStarted);
    }

    public IEnumerator RunTimer(){
        var startTime = Time.time;

        while(Time.time < startTime + timerDuration && timerRunning){
            timeRemaining = timerDuration - Time.time - startTime;
            UpdateUI();
            yield return null;
        }

        if(timerRunning)
            EventManager.Instance.TriggerEvent(Data.Events.OnTimeRanOut);
    }

    private void UpdateUI(){
        textTimeRemaining.text = timeRemaining.ToString("0") + "s";
    }

    private void OnCorrectElementSelected(){
        timerRunning = false;
    }
}
