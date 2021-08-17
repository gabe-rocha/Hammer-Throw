using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableBuilder : MonoBehaviour
{
    
    [SerializeField] private List<Transform> listElementPositions;
    [SerializeField] private GameObject elementTemplate;
    private List<GameObject> listElements = new List<GameObject>();

    private void OnEnable()
    {
        EventManager.Instance.StartListening(Data.Events.OnGameManagerReady, BuildTable);
    }
    private void OnDisable()
    {
        EventManager.Instance.StopListening(Data.Events.OnGameManagerReady, BuildTable);
    }

    private void BuildTable()
    {
        Transform parent = new GameObject().transform;
        parent.name = "Elements";

        for (var id = 0; id < listElementPositions.Count; id++)
        {
            GameObject element = Instantiate(elementTemplate, listElementPositions[id].position, Quaternion.identity, parent);
            element.GetComponent<Element>().Init(id+1, Data.dicElements.Keys.ElementAt(id), Data.dicElements.Values.ElementAt(id));
            listElements.Add(element);
        }

        GameManager.Instance.listOfElements = listElements;
        EventManager.Instance.TriggerEvent(Data.Events.OnTableReady);
    }
}