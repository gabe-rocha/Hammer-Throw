using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonPlayAgain : MonoBehaviour
{
    [SerializeField] private Image imageFadeOut;
    private Animator anim;


    private void OnEnable(){
        EventManager.Instance.StartListening(Data.Events.OnFinalDistanceCalculated, Show);
    }
    private void OnDisable(){
        EventManager.Instance.StopListening(Data.Events.OnFinalDistanceCalculated, Show);
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
        yield return new WaitForSeconds(Data.DelayBeforeShowingFinalButtons);
        anim.SetTrigger("Show");
    }

    // private void Hide(){
    //     anim.SetTrigger("Hide");
    // }

    public void OnButtonPlayAgainPressed(){
        StartCoroutine(FadeOutSceneAndReload());
    }

    private IEnumerator FadeOutSceneAndReload()
    {
        var startTime = Time.time;
        while(Time.time < startTime + Data.fadeOutSpeed){
             var color = imageFadeOut.color;
             color.a += 1f / Data.fadeOutSpeed * 1f / Time.deltaTime;
            // imageFadeOut.CrossFadeAlpha(1f, Data.fadeOutSpeed, true);
             imageFadeOut.color = color;
            yield return null;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
