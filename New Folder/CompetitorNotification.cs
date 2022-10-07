
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompetitorNotification : MonoBehaviour
{
    [SerializeField] private CanvasGroup spawnNotification;
    [SerializeField] private CanvasGroup competitorProgress;

    [SerializeField] public GameObject competitor;
    public Competitor competitorScript;

    [SerializeField] private bool fadeOut = false;
    float fadeOutTimer = 0;
    private bool showProgressBar = false;

    [SerializeField] public Image progressBar;
	private float progressBarWidth;
    private float progressBarHeight;



    private void Start(){
        progressBarHeight = progressBar.rectTransform.rect.height; //
		progressBarWidth = progressBar.rectTransform.rect.width; //
        competitorScript = competitor.GetComponent<Competitor>();

        //fadeOut = true;
    }


    public void showContainer(){

    }

    private bool DelayCheck(float delayLength){
        fadeOutTimer += Time.deltaTime;
        if (fadeOutTimer >= delayLength){
            return true;
        }
        return false;
    }

    private void FadeOutText(){
        fadeOut = true;
    }

    private void FadeInProgressBar(){
        showProgressBar = true;
    }

    private void Update(){
        
        if (DelayCheck(3.0f)){
            FadeOutText();
        }

        if (fadeOut){
            spawnNotification.alpha -= Time.deltaTime/2;
            if (spawnNotification.alpha <= 0){
                fadeOut = false;
                FadeInProgressBar();
            }
        }

        if (showProgressBar){
            competitorProgress.alpha += Time.deltaTime/2;
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
        float progressBarCurrentWidth = remainingPercentage * progressBarWidth;
        float reducedAmount = progressBarWidth - progressBarCurrentWidth;
        progressBar.rectTransform.sizeDelta = new Vector2(progressBarWidth - reducedAmount, progressBarHeight);
    }

}

