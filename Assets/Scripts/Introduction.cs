using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Introduction : MonoBehaviour
{
    [SerializeField] private AudioClip introSound;
    [SerializeField] private GameObject player;

    private AudioSource introSource;

    private void Awake()
    {
        introSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        introSource.Play();
    }

    void Enable()
    {
        player.GetComponent<Controller>().isCinematic = false;
        player.GetComponent<PlayerSounds>().isCinematic = false;
    }
}
