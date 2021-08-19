using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Element : MonoBehaviour    
{
    [SerializeField] private GameObject goHighlightedInGameHelper, goHighlightedMouseOver, goSelected, goTooltip, goFader;
    [SerializeField] private TextMeshProUGUI txtNumber, txtSymbol, txtTooltipName;
    
    private int id;
    private string strSymbol, strName;
    private bool isForcedHighlighted;

    public void Init(int id, string strSymbol, string strName){
        this.id = id;
        this.strSymbol = strSymbol;
        this.strName = strName;

        txtNumber.text = id.ToString();
        txtSymbol.text = strSymbol;
        txtTooltipName.text = strName;

        isForcedHighlighted = false;
    }

    public void Deselect(){
        goSelected.SetActive(false);
    }

    private void OnMouseOver()
    {
        if(!isForcedHighlighted)
            goFader.SetActive(false);

        goHighlightedMouseOver.SetActive(true);
        goTooltip.SetActive(true);
    }

    private void OnMouseUp()
    {
        Debug.Log($"Element {id} selected");

        // if(GameManager.Instance.elementSelectedByPlayer == this)
            // return;
        
        // goHighlightedMouseOver.SetActive(false);
        // goSelected.SetActive(true);
        
        // if(GameManager.Instance.elementSelectedByPlayer != null)
            // GameManager.Instance.elementSelectedByPlayer.Deselect();
        
        GameManager.Instance.elementSelectedByPlayer = this;
        EventManager.Instance.TriggerEvent(Data.Events.OnElementSelected);
        // isForcedHighlighted = true;
    }

    private void OnMouseExit()
    {
        if(!isForcedHighlighted)
            goFader.SetActive(true);
        
        goHighlightedMouseOver.SetActive(false);
        goTooltip.SetActive(false);
    }

    public void ForceHighlight(bool isOn){
        isForcedHighlighted = isOn;
        // goHighlightedMouseOver.SetActive(isOn);
        // goSelected.SetActive(isOn);
        goFader.SetActive(!isOn);
    }
}
