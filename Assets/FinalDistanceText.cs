using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinalDistanceText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtWhiteOutline, txtBlackOutline, txtDistance;

    private Animator anim;

    private void OnEnable(){
        EventManager.Instance.StartListening(Data.Events.OnFinalDistanceCalculated, Show);
    }
    private void OnDisable(){
        EventManager.Instance.StartListening(Data.Events.OnFinalDistanceCalculated, Show);
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Show(){
        StartCoroutine(WaitAndShow());
    }

    private IEnumerator WaitAndShow()
    {
        var distance = GameManager.Instance.GetDistance();
        distance = distance.Remove(distance.Length - 1, 1);
        txtWhiteOutline.text = $"FINAL DISTANCE \n{distance} METERS!";
        txtBlackOutline.text = txtWhiteOutline.text;
        txtDistance.text = txtWhiteOutline.text;

        yield return new WaitForSeconds(Data.DelayBeforeShowingFinalText);
        anim.SetTrigger("Show");
    }
    private void Hide(){
        anim.SetTrigger("Hide");
    }
}
