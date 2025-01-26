using System;
using System.Collections.Generic;
using UnityEngine;

public class Introduction : MonoBehaviour
{
    [SerializeField] private GameObject player;

    void Enable()
    {
        player.GetComponent<Controller>().isCinematic = false;
        player.GetComponent<PlayerSounds>().isCinematic = false;
    }
}
