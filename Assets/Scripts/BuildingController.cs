using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    public GameObject fireParticleSystem = null;
    public AudioSource fireAudioSource = null;
   

    private void Awake()
    {
        fireAudioSource = GetComponent<AudioSource>();
        
    }



}
