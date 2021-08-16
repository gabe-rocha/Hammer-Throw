using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableBuilder : MonoBehaviour
{
    
    [SerializeField] private List<Transform> listElementPositions;
    [SerializeField] private List<GameObject> listElements;
    [SerializeField] private GameObject elementTemplate;

    private IEnumerator Start()
    {
         yield return new WaitUntil(()=>GameManager.Instance.gameState == GameManager.GameStates.BuildingLevel);
         BuildTable();
    }

    private void BuildTable()
    {
        Transform parent = new GameObject().transform;
        parent.name = "Elements";
        foreach (var t in listElementPositions)
        {
            GameObject element = Instantiate(elementTemplate, t.position, Quaternion.identity, parent);
            listElements.Add(element);
        }
    }
}
