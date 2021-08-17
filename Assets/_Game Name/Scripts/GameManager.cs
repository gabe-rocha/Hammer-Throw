using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get => instance; set => instance = value; }


    [SerializeField] private GameObject tableBuilderPrefab;
    [SerializeField] private GameObject elementDrawerPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject timerPrefab;
    [SerializeField] private GameObject hammerPrefab;

    private TableBuilder tableBuilder;
    private ElementDrawer elementDrawer;
    private Player player;
    private Timer timer;
    private Hammer hammer;
    [SerializeField] private Transform mainCanvas;

    
    internal Element elementSelectedByPlayer, elementDrawedByTheGame;
    internal List<GameObject> listOfElements;
    internal GameObject elementDrawed;
    

    private void OnEnable()
    {
        EventManager.Instance.StartListening(Data.Events.OnDrawerReady, OnDrawerReady);
        EventManager.Instance.StartListening(Data.Events.OnElementSelected, OnElementSelected);        
        EventManager.Instance.StartListening(Data.Events.OnTimerStarted, OnTimerStarted);
        EventManager.Instance.StartListening(Data.Events.OnTimeRanOut, OnTimeRanOut);
    }


    private void OnDisable()
    {
        EventManager.Instance.StopListening(Data.Events.OnDrawerReady, OnDrawerReady);
        EventManager.Instance.StopListening(Data.Events.OnElementSelected, OnElementSelected);
        EventManager.Instance.StopListening(Data.Events.OnTimeRanOut, OnTimerStarted);
        EventManager.Instance.StopListening(Data.Events.OnTimeRanOut, OnTimeRanOut);
    }

    void Awake(){
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(this.gameObject);

            Application.targetFrameRate = -1;

        } else {
            Destroy(this);
        }
    }

    void Start(){
        InstantiateComponents();
        EventManager.Instance.TriggerEvent(Data.Events.OnGameManagerReady);
    }

    private void InstantiateComponents()
    {
        var go = Instantiate(tableBuilderPrefab);
        tableBuilder = go.GetComponent<TableBuilder>();

        go = Instantiate(playerPrefab);
        player = go.GetComponent<Player>();

        go = Instantiate(elementDrawerPrefab, mainCanvas);
        elementDrawer = go.GetComponent<ElementDrawer>();
        
        go = Instantiate(timerPrefab, mainCanvas);
        timer = go.GetComponent<Timer>();
        
        go = Instantiate(hammerPrefab, player.gameObject.transform.position, Quaternion.identity);
        hammer = go.GetComponent<Hammer>();
    }

    private void OnElementSelected()
    {
        //Compare elements
        if(elementSelectedByPlayer == elementDrawedByTheGame){
            Debug.Log("WIN");
            EventManager.Instance.TriggerEvent(Data.Events.OnCorrectElementSelected);
        }
    }

    private void OnDrawerReady(){
        var elementDrawedId = elementDrawer.Draw();
        elementDrawedByTheGame = listOfElements[elementDrawedId-1].GetComponent<Element>();
        EventManager.Instance.TriggerEvent(Data.Events.OnElementDrawed);
    }
    
    private void OnTimeRanOut()
    {
        EventManager.Instance.TriggerEvent(Data.Events.OnGameOver);
    }

    internal float GetRemainingTime()
    {
        return timer.TimeRemaining;
    }
    
    private void OnTimerStarted()
    {
        EventManager.Instance.TriggerEvent(Data.Events.OnGameStarted);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            EventManager.Instance.TriggerEvent(Data.Events.OnCorrectElementSelected);
        }
    }
}