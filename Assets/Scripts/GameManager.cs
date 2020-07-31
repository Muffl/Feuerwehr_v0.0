using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float _extinquishTime = 3.0f;
    [SerializeField] private float _delayTimeBetweenTwoFires = 3.0f;
    

   

    private BuildingController[] _buildings = null;
    private BuildingController _actualBuildingOnFire = null;
    public BuildingController ActualBuildingOnFire
    {
        get
        {
            return _actualBuildingOnFire;
        }
    }

    private float _actualExtinquishTime = 0.0f;

    private void Start()
    {
        _buildings = FindObjectsOfType<BuildingController>();
        PutNextBuildingOnFire();
    }

    private void PutNextBuildingOnFire()
    {
        
        
        _actualBuildingOnFire = _buildings[Random.Range(0, _buildings.Length)];
        _actualBuildingOnFire.fireParticleSystem.SetActive(true);
        _actualBuildingOnFire.tag = "OnFire";
        _actualBuildingOnFire.fireAudioSource.Play();
       

    }

    public  void ExtinquishFire()
    {
        _actualExtinquishTime += Time.deltaTime;
        //_actualBuildingOnFire.fireParticleSystem.SetActive(false);
        if (_actualExtinquishTime >= _extinquishTime)
        {
            _actualBuildingOnFire.fireParticleSystem.SetActive(false);
            _actualBuildingOnFire.tag = "Untagged";
            _actualExtinquishTime = 0.0f;
            _actualBuildingOnFire.fireAudioSource.Stop();
        

            Invoke("PutNextBuildingOnFire", _delayTimeBetweenTwoFires); 
        }

    }
}

