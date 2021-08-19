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

    private int totalElements, highlightAmount;
    private List<Element> listHighlightedElements = new List<Element>();

    private void OnEnable()
    {
        EventManager.Instance.StartListening(Data.Events.OnGameManagerReady, BuildTable);
        EventManager.Instance.StartListening(Data.Events.OnElementDrawed, HighlightElements);
    }
    private void OnDisable()
    {
        EventManager.Instance.StopListening(Data.Events.OnGameManagerReady, BuildTable);
        EventManager.Instance.StopListening(Data.Events.OnElementDrawed, HighlightElements);
    }

    private void HighlightElements()
    {
        InvokeRepeating("Highlight", 0, 5);
    }

    private void Highlight()
    {
        //Unhighlight everything
        foreach (var element in listHighlightedElements)
            element.ForceHighlight(false);
        listHighlightedElements.Clear();

        //Highlight the drawed element
        Element alwaysHighlight = GameManager.Instance.elementDrawedByTheGame;
        alwaysHighlight.ForceHighlight(true);
        listHighlightedElements.Add(alwaysHighlight);

        for (var i = 0; i < highlightAmount; i++)
        {
            int rng = UnityEngine.Random.Range(0, totalElements);
            Element elementToBeHighlighted = listElements[rng].GetComponent<Element>();
            if (!listHighlightedElements.Contains(elementToBeHighlighted))
            {
                listHighlightedElements.Add(elementToBeHighlighted);
                elementToBeHighlighted.ForceHighlight(true);
            }
            else
            {
                i--;
            }
        }

        //Every time this is called we highlighted half less elements
        highlightAmount /= 2;
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
        totalElements = listElements.Count;
        highlightAmount = totalElements / 2;

        GameManager.Instance.listOfElements = listElements;
        EventManager.Instance.TriggerEvent(Data.Events.OnTableReady);
    }
}