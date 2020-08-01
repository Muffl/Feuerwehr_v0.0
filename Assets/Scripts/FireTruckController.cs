using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTruckController : MonoBehaviour

{
    [SerializeField] private GameManager _gameManager = null;
    [SerializeField] private float _speed = 2.5f;
    [SerializeField] private float _rotationSpeed = 1.5f;

    [SerializeField] private Transform _waterCannon = null;
    [SerializeField] private float _waterCannonRotationSpeed = 60.0f;
    [SerializeField] private ParticleSystem _waterCannonParticleSystem = null;
    [SerializeField] private Transform _cameraMountRight = null;
    [SerializeField] private Transform _cameraMountLeft = null;
    [SerializeField] private Transform _arrowMount = null;

    private float _horizontal = 0.0f;
    private float _vertical = 0.0f;

    private Rigidbody _rigidbody = null;
    private ParticleSystem.EmissionModule _emmision;
    private AudioSource _audioSource = null;

    private bool _isCameraLeft = false;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _emmision = _waterCannonParticleSystem.emission;
        _audioSource = GetComponent<AudioSource>();

        Cursor.lockState = CursorLockMode.Locked;
       
    }

    private void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        _waterCannon.Rotate(Vector3.up, Input.GetAxis("Mouse X") * _waterCannonRotationSpeed * Time.deltaTime);

        _emmision.enabled = Input.GetButton("Fire1");


        if (Input.GetButtonDown("Fire2"))
        {
            if (_isCameraLeft)
                Camera.main.transform.SetParent(_cameraMountRight, false);
            else
                Camera.main.transform.SetParent(_cameraMountLeft, false);

            _isCameraLeft = !_isCameraLeft;
        }


        if (_emmision.enabled)
        {
            if (!_audioSource.isPlaying)
                _audioSource.Play();

            RaycastHit hit;
            if (Physics.Raycast(_waterCannon.position, _waterCannon.forward, out hit, 10.0f,
                LayerMask.GetMask("Buildings")))
            {
                if (hit.collider.CompareTag("OnFire"))
                {
                    _gameManager.ExtinquishFire();
                }

            }
            

           
        }
        else
        {
            _audioSource.Stop();

        }



        //
        //if (Input.GetButtonDown("Fire2"))
        //{
        //    if (_isCameraLeft)
        //        Camera.main.transform.SetParent(_cameraMountRight, false);
        //    else
        //        Camera.main.transform.SetParent(_cameraMountLeft, false);

        //    _isCameraLeft = !_isCameraLeft;
        //}
        //


        _arrowMount.rotation = Quaternion.LookRotation(_gameManager.ActualBuildingOnFire.transform.
                position - transform.position);


    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = transform.forward * _vertical * _speed;
        _rigidbody.angularVelocity = transform.up * _horizontal * _rotationSpeed;
    }

}
