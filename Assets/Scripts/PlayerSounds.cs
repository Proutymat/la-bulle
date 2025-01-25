using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerSounds : MonoBehaviour
{
    [Header("Footsteps")]
    [SerializeField] private List<AudioClip> basicFS;
    [SerializeField] private List<AudioClip> bubbleFS;
    [SerializeField] private float volumeMinFS = 0.5f;
    [SerializeField] private float volumeMaxFS = 0.8f;
    
    [Header("Ambience")]
    [SerializeField] private AudioClip ambienceBasic;
    [SerializeField] private AudioClip ambienceBubble;
    [SerializeField] private float maxDistance = 20f; //max distance to hear sound
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private Transform bubbleTransform;
    [SerializeField] private float constantVolume = 0.25f; //constant value if in bubble
    [SerializeField] private float maxVolume = 0.35f; //max volume if outside bubble
    [SerializeField] private bool noSound = false;

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
        if (isPlayerInside == true)
        {
            ambienceSource.volume = constantVolume;
        }
        else
        {
            float distance = Vector3.Distance(bubbleTransform.position, transform.position);

            float volume = Mathf.Clamp01(1 - (distance - minDistance) / (maxDistance - minDistance));

            ambienceSource.volume = volume * maxVolume;
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
            if (noSound == true)
            {
                ambienceSource.Stop();
            }
            else
            {
                //Debug.Log("Enter");
                ambienceSource.clip = ambienceBubble;
                ambienceSource.Play();
                isPlayerInside = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Bubble")
        {
            //Debug.Log("Exit");
            ambienceSource.clip = ambienceBasic;
            ambienceSource.Play();
            isPlayerInside = false;
        }
    }
}
