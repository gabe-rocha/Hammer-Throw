using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get => instance; set => instance = value; }

    public enum GameStates{
        Initializing,
        BuildingLevel,
        ReadyToPlay,
        Playing
    }

    public GameStates gameState = GameStates.BuildingLevel;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        
    }
}
