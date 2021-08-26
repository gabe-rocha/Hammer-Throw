using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textTimeRemaining;
    [SerializeField] private GameObject powerMeterNeedle;
    [SerializeField] private Animator anim;
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
        Debug.Log(  "Timer started");
        
        anim.SetTrigger("Show");
        timerRunning = true;
        timerDuration = Data.gameDuration;
        timeRemaining = timerDuration;
        StartCoroutine(RunTimer());
        EventManager.Instance.TriggerEvent(Data.Events.OnTimerStarted);
    }

    public IEnumerator RunTimer(){

        yield return new WaitForSeconds(1.0f); //Wait for the elements to scale up
        var startTime = Time.time;

        while(Time.time < startTime + timerDuration && timerRunning){
            timeRemaining -= Time.deltaTime;
            UpdateUI(); 
            yield return null;
        }

        if(timerRunning)
            EventManager.Instance.TriggerEvent(Data.Events.OnTimeRanOut);
    }

    private void UpdateUI(){
        var pos = powerMeterNeedle.GetComponent<RectTransform>().localPosition;
        pos.y = timeRemaining * 14.3f;
        powerMeterNeedle.GetComponent<RectTransform>().localPosition = pos;

        textTimeRemaining.text = timeRemaining.ToString("00:00.000");
    }

    private void OnCorrectElementSelected(){
        timerRunning = false;
    }
}
