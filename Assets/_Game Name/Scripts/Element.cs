using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;


public class Element : MonoBehaviour    
{
    [SerializeField] private GameObject goHighlightedInGameHelper, goHighlightedMouseOver, goSelected, goTooltip, goFader;
    [SerializeField] private TextMeshProUGUI txtNumber, txtSymbol, txtTooltipName;
    
    private int id;
    private string strSymbol, strName;
    private bool isForcedHighlighted;
    private SpriteRenderer spriteFader;

    private GameObject highlightedMouseOver, tooltip;
    private Animator anim;


    private void OnEnable(){
        EventManager.Instance.StartListening(Data.Events.OnCorrectElementSelected, Hide);
    }
    private void OnDisable(){
        EventManager.Instance.StopListening(Data.Events.OnCorrectElementSelected, Hide);
    }

    private void Hide()
    {
        anim.SetTrigger("Hide");
    }

    private void Awake()
    {
        spriteFader = GetComponent<SpriteRenderer>();
        // transform.localScale = Vector3.zero;
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        highlightedMouseOver = Instantiate(goHighlightedMouseOver);
        highlightedMouseOver.transform.position = transform.position;
        highlightedMouseOver.SetActive(false);

        if(name.Contains('(')){
            id = int.Parse(name.Split('(', ')')[1]) - 1;
            txtTooltipName.text = Data.dicElements.ElementAt(id).Value;
        }
        tooltip = Instantiate(goTooltip);
        tooltip.transform.position = transform.position;
        tooltip.SetActive(false);
        isForcedHighlighted = false;
    }

    public void Deselect(){
        goSelected.SetActive(false);
    }

    // private void OnMouseOver()
    // {
    //     if(!isForcedHighlighted)
    //         goFader.SetActive(false);

    //     goHighlightedMouseOver.SetActive(true);
    //     goTooltip.SetActive(true);
    // }
    private void OnMouseOver()
    {
        if(!isForcedHighlighted)
            spriteFader.enabled = false;

        highlightedMouseOver.SetActive(true);
        tooltip.SetActive(true);
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

    // private void OnMouseExit()
    // {
    //     if(!isForcedHighlighted)
    //         goFader.SetActive(true);
        
    //     goHighlightedMouseOver.SetActive(false);
    //     goTooltip.SetActive(false);
    // }
    private void OnMouseExit()
    {
        if(!isForcedHighlighted)
            spriteFader.enabled = true;
        
        highlightedMouseOver.SetActive(false);
        tooltip.SetActive(false);
    }

    // public void ForceHighlight(bool isOn){
    //     isForcedHighlighted = isOn;
    //     // goHighlightedMouseOver.SetActive(isOn);
    //     // goSelected.SetActive(isOn);
    //     goFader.SetActive(!isOn);
    // }
    public void ForceHighlight(bool isOn){
        isForcedHighlighted = isOn;
        // goHighlightedMouseOver.SetActive(isOn);
        // goSelected.SetActive(isOn);
        spriteFader.enabled = !isOn;
    }

    internal void Show()
    {
        StartCoroutine(ScaleUp());
    }

    IEnumerator ScaleUp(){
        yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 0.5f));

        anim.SetTrigger("Show");

        // while(transform.localScale.x < 1)
        // {
        //     var newScale = transform.localScale;
        //     newScale.x += 0.1f * Time.deltaTime;
        //     newScale.y += 0.1f * Time.deltaTime;
        //     transform.localScale = newScale;
        // }
    }
}
