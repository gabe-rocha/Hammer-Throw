using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Element : MonoBehaviour    
{
    [SerializeField] private GameObject goHighlightedInGameHelper, goHighlightedMouseOver, goSelected, goTooltip;
    [SerializeField] private TextMeshProUGUI txtNumber, txtSymbol, txtTooltipName;
    
    private int id;
    private string strSymbol, strName;
    private bool isSelected;

    public void Init(int id, string strSymbol, string strName){
        this.id = id;
        this.strSymbol = strSymbol;
        this.strName = strName;

        txtNumber.text = id.ToString();
        txtSymbol.text = strSymbol;
        txtTooltipName.text = strName;

        isSelected = false;
    }

    public void Deselect(){
        goSelected.SetActive(false);
        isSelected = false;
    }

    private void OnMouseOver()
    {
        if(!isSelected)
            goHighlightedMouseOver.SetActive(true);

        goTooltip.SetActive(true);
    }

    private void OnMouseUp()
    {
        Debug.Log($"Element {id} selected");

        if(GameManager.Instance.elementSelectedByPlayer == this)
            return;
        
        goHighlightedMouseOver.SetActive(false);
        goSelected.SetActive(true);
        
        if(GameManager.Instance.elementSelectedByPlayer != null)
            GameManager.Instance.elementSelectedByPlayer.Deselect();
        
        GameManager.Instance.elementSelectedByPlayer = this;
        EventManager.Instance.TriggerEvent(Data.Events.OnElementSelected);
        isSelected = true;
    }

    private void OnMouseExit()
    {
        goHighlightedMouseOver.SetActive(false);
        goTooltip.SetActive(false);
    }
}
