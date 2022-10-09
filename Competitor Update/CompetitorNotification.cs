
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompetitorNotification : MonoBehaviour
{   
    private Vector3 originalPosition;

    [SerializeField] private CanvasGroup spawnNotification;
    [SerializeField] private CanvasGroup competitorProgress;
    private RectTransform rectTransform;

    //[SerializeField] public GameObject competitor;
    public Competitor competitorScript;

    private bool startShowingContainer = false;
    private bool containerShown = false;
    private bool fadeOut = false;
    float fadeOutTimer = 0;
    private bool showProgressBar = false;

    [SerializeField] public Image progressBar;
	private float progressBarWidth;
    private float progressBarHeight;

    public void ResetContainer(){
        transform.position = originalPosition;
        startShowingContainer = false;
        containerShown = false;
        fadeOut = false;
        fadeOutTimer = 0;
        showProgressBar = false;
    }

    public void ShowContainer(){
        originalPosition = transform.position;
        rectTransform = GetComponent<RectTransform>();
        progressBarHeight = progressBar.rectTransform.rect.height; //
		progressBarWidth = progressBar.rectTransform.rect.width; //
        competitorScript = GameObject.FindGameObjectWithTag("Competitor").GetComponent<Competitor>();

        startShowingContainer = true;
    }

    private bool DelayCheck(float delayLength){
        fadeOutTimer += Time.deltaTime;
        if (fadeOutTimer >= delayLength){
            fadeOutTimer = 0.0f;
            return true;
        }
        return false;
    }

    private void Update(){

        if (startShowingContainer){
            Vector2 thisPosition = rectTransform.anchoredPosition; 
            thisPosition.x -= Time.deltaTime * 600;
            rectTransform.anchoredPosition = thisPosition;
            
            if (DelayCheck(1.15f)){
                startShowingContainer = false;
            }
            
        }
        

        if (containerShown | DelayCheck(3.0f)){
            fadeOut = true;
        }

        if (fadeOut){
            spawnNotification.alpha -= Time.deltaTime/2;
            if (spawnNotification.alpha <= 0){
                fadeOut = false;
                showProgressBar = true;
            }
        }

        if (showProgressBar){
            competitorProgress.alpha += Time.deltaTime;
            if(competitorProgress.alpha >= 1){
                showProgressBar = false;
            }
        }

        UpdateProgressBar(competitorScript.GetRemainingDistanceInPercentage());
   
    }

    private void UpdateProgressBar(float remainingPercentage){
        if (remainingPercentage >= 1){
            remainingPercentage = 1;
        } else if (remainingPercentage <= 0.001){
            remainingPercentage = 0;
        }
        float progressBarCurrentWidth = progressBarWidth - remainingPercentage * progressBarWidth;
        progressBar.rectTransform.sizeDelta = new Vector2(progressBarCurrentWidth, progressBarHeight);
    }

}

