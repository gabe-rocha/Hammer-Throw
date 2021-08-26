using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ElementDrawer : MonoBehaviour
{

    // [SerializeField] private TextMeshProUGUI txtElementToFind;
    [SerializeField] private TextMeshPro txtElementToFind;
    private Dictionary<string,string> dicElements = new Dictionary<string, string>();
    private int numElements;
    private Animator anim;

    private void OnEnable(){
        EventManager.Instance.StartListening(Data.Events.OnTableReady, Init);
        EventManager.Instance.StartListening(Data.Events.OnStartButtonPressed, Show);
        EventManager.Instance.StartListening(Data.Events.OnCorrectElementSelected, OnCorrectElementSelected);
    }
    private void OnDisable(){
        EventManager.Instance.StopListening(Data.Events.OnTableReady, Init);
        EventManager.Instance.StopListening(Data.Events.OnStartButtonPressed, Show);
        EventManager.Instance.StopListening(Data.Events.OnCorrectElementSelected, OnCorrectElementSelected);
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Init(){
        dicElements = Data.dicElements;
        numElements = dicElements.Count;
        EventManager.Instance.TriggerEvent(Data.Events.OnDrawerReady);
    }

    public int Draw(){
        var elementId = Random.Range(1, numElements+1);
        txtElementToFind.text = dicElements.Values.ElementAt(elementId - 1);
        
        Debug.Log($"Element drawed: {txtElementToFind.text}");
        // EventManager.Instance.TriggerEvent(Data.Events.OnElementDrawed); //we will let the gamemanager call this

        return elementId;
    }


    private void OnCorrectElementSelected(){
        Hide();
    }

    private void Show(){
        anim.SetTrigger("Show");
    }
    private void Hide(){
        txtElementToFind.text = "";
        anim.SetTrigger("Hide");
    }
}
