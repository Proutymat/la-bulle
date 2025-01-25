using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Audio;
using UnityEngine.Video;

public class PlayerSounds : MonoBehaviour
{
    [Header("Footsteps")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private List<AudioClip> basicFS;
    [SerializeField] private List<AudioClip> bubbleFS;
    [SerializeField] private float volumeMinFS = 0.5f;
    [SerializeField] private float volumeMaxFS = 0.8f;
    
    [Header("Ambience")]
    [SerializeField] private AudioClip ambienceBasic;
    
    [SerializeField] private float lowPassFilterInside = 1000f;
    [SerializeField] private float lowPassFilterOutside = 5000f;
    [SerializeField] private float duration = 2f;

    private bool isPlayerInside = false; //IsPlayerInside Bubble
    
    private AudioSource footstepSource;
    private AudioSource ambienceSource;
    private Controller playerController;

    private float maxDuration = 1f;
    private float currentDuration = 0f;

    private void Start()
    {
        footstepSource = GetComponents<AudioSource>()[0];
        ambienceSource = GetComponents<AudioSource>()[1];
        
        playerController = GetComponent<Controller>();
        currentDuration = maxDuration;
        maxDuration = Random.Range(0.7f, 1.1f);
        
        ambienceSource.Play();
    }

    private void Update()
    {
        if (playerController._isWalking == true)
        {
            currentDuration -= Time.deltaTime;
            //Debug.Log(currentDuration);
            if (currentDuration <= 0)
            {
                PlayFootSteps();
                currentDuration = maxDuration;
            }
        }
    }
    void PlayFootSteps()
    {
        AudioClip clip = null;
        
        if (isPlayerInside == true)
        {
            clip = bubbleFS[Random.Range(0, bubbleFS.Count)];
            footstepSource.clip = clip;
            footstepSource.volume = Random.Range(volumeMinFS, volumeMaxFS);
            footstepSource.pitch = Random.Range(0.8f, 1.2f);
            footstepSource.Play();
        }
        else
        {
            clip = basicFS[Random.Range(0, basicFS.Count)];
            footstepSource.clip = clip;
            footstepSource.volume = Random.Range(volumeMinFS, volumeMaxFS);
            footstepSource.pitch = Random.Range(0.8f, 1.2f);
            footstepSource.Play();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bubble")
        {
                //Debug.Log("Enter");
                StartCoroutine(PlayInsideBubble());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Bubble")
        {
            //Debug.Log("Exit");
            ambienceSource.Play();
            StartCoroutine(PlayOutsideBubble());
        }
    }
    
    private IEnumerator PlayInsideBubble()
    {
        float currentTime = 0;
        float currentVol;
        audioMixer.GetFloat("lowPassFilter", out currentVol);
        Debug.Log(currentVol);
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, lowPassFilterInside, currentTime / duration);
            audioMixer.SetFloat("lowPassFilter", newVol);
            yield return null;
        }
    }
    private IEnumerator PlayOutsideBubble()
    {
        float currentTime = 0;
        float currentVol;
        audioMixer.GetFloat("lowPassFilter", out currentVol);
        Debug.Log(currentVol);
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, lowPassFilterOutside, currentTime / duration);
            audioMixer.SetFloat("lowPassFilter", newVol);
            yield return null;
        }
    }
}
