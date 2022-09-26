using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityBarkScript : MonoBehaviour
{
    [TextArea]
    [SerializeField] string inspectorNote = "This component will disable itself if no audio clips are assigned. ALSO, this can be functionally disabled by calling the DisableBark() function. This is useful for situations such as the player hitting the npc causing them to no longer react to player proximity";

    // for the sake of simplicty, each pedestrian will only ever say one line when the player get's too close (at runtime start, the line that they will say for the rest of the game will be selected from the list)
    [SerializeField] AudioClip[] audioClipOptions;
    AudioSource audioSource;
    int randomSeed;
    float cooldownTimer = 0;
    bool onCooldown = false;
    bool barkDisabled = false;


    public void DisableBark()
    {
        barkDisabled = true;
    }

    void Start()
    {
        if (audioClipOptions.Length == 0)
        {
            //if no audioclips assigned, this componenet will disable itself
            this.enabled = false;
        }

        audioSource = this.GetComponent<AudioSource>();
        randomSeed = Random.Range(0, audioClipOptions.Length);
    }


    void Update()
    {
        if (onCooldown)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer > 3)
            {
                onCooldown = false;
                cooldownTimer = 0;
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(!barkDisabled && !onCooldown && other.CompareTag("Player"))
        {
            audioSource.PlayOneShot(audioClipOptions[randomSeed]);
            onCooldown = true;
        }
    }
}
