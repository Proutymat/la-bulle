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
    [SerializeField] private float minDuration = 0f;
    [SerializeField] private float maxDuration = 1f;
    [SerializeField] private float lowPassFilterInsideFS = 3000f;
    [SerializeField] private float lowPassFilterOutsideFS = 5000f;
    private float currentDuration = 0f;
    
    [Header("Ambience")]
    [SerializeField] private AudioClip ambienceBasic;
    [SerializeField] private AnimationCurve _ambianceCurve = AnimationCurve.Linear(0,0,1,1);
    [SerializeField] private float maxDistance = 20f; 
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private Transform bubbleTransform;
    [SerializeField] private float constantVolume = 1f; 
    [SerializeField] private float maxVolume = 1f;       
    
    [SerializeField] private float lowPassFilterInside = 1000f;
    [SerializeField] private float lowPassFilterOutside = 5000f;
    [SerializeField] private float duration = 2f;

    private bool isPlayerInside = false; //IsPlayerInside Bubble
    
    private AudioSource footstepSource;
    private AudioSource ambienceSource;
    private Controller playerController;
    
    public bool isCinematic = true;

    private void Start()
    {
        footstepSource = GetComponents<AudioSource>()[0];
        ambienceSource = GetComponents<AudioSource>()[1];
        
        playerController = GetComponent<Controller>();
        currentDuration = maxDuration;
        maxDuration = Random.Range(minDuration, maxDuration);
        
        ambienceSource.Play();
    }

    private void Update()
    {
        if(isCinematic == true)return;
        if (playerController.IsWalking == true)
        {
            currentDuration -= Time.deltaTime;
            //Debug.Log(currentDuration);
            if (currentDuration <= 0)
            {
                PlayFootSteps();
                currentDuration = maxDuration;
            }
        }
        if (isPlayerInside)
        {
            ambienceSource.volume = constantVolume * maxVolume; 
        }
        else
        {
            float distance = Vector3.Distance(bubbleTransform.position, transform.position);
            float volume = Mathf.Clamp01(1 - (distance - minDistance) / (maxDistance - minDistance));
            ambienceSource.volume = _ambianceCurve.Evaluate(volume) * maxVolume;
        }
    }
    void PlayFootSteps()
    {
        AudioClip clip = null;
        clip = basicFS[Random.Range(0, basicFS.Count)]; 
        footstepSource.clip = clip;
        footstepSource.volume = Random.Range(volumeMinFS, volumeMaxFS); 
        footstepSource.pitch = Random.Range(0.8f, 1.2f);
        footstepSource.Play();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bubble")
        {
            isPlayerInside = true;
            audioMixer.SetFloat("lowPassFilterFS", lowPassFilterInsideFS);
            audioMixer.SetFloat("lowPassFilterBreath", lowPassFilterInsideFS);
            StartCoroutine(PlayInsideBubble());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Bubble")
        {
            isPlayerInside = false;
            audioMixer.SetFloat("lowPassFilterFS", lowPassFilterOutsideFS);
            audioMixer.SetFloat("lowPassFilterBreath", lowPassFilterOutsideFS);
            ambienceSource.Play();
            StartCoroutine(PlayOutsideBubble());
        }
    }
    
    private IEnumerator PlayInsideBubble()
    {
        float currentTime = 0;
        float currentVol;
        audioMixer.GetFloat("lowPassFilter", out currentVol);
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
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, lowPassFilterOutside, currentTime / duration);
            audioMixer.SetFloat("lowPassFilter", newVol);
            yield return null;
        }
    }
}
