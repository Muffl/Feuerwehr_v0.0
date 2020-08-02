using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float _extinquishTime = 3.0f;
    [SerializeField] private float _delayTimeBetweenTwoFires = 3.0f;
    [SerializeField] private float _gameTime = 300.0f;

    [SerializeField] private TMP_Text _countdownLabel = null;
    [SerializeField] private TMP_Text _pointLabel = null;

    [SerializeField] private GameObject _menuePanel = null;
    [SerializeField] private GameObject _ingameUI = null;
    [SerializeField] private GameObject _gameOverPanel = null;
    [SerializeField] private TMP_Text _gameOverPointsLabel = null;
    [SerializeField] private GameObject _pausePanel = null;


    private BuildingController[] _buildings = null;
    private BuildingController _actualBuildingOnFire = null;

    private float _actualExtinquishTime = 0.0f;
    private float _actualGameTime = 0.0f;

    private int _points = 0;

    private bool _isPause = false;
    private bool _isGameStarted = false;

    public BuildingController ActualBuildingOnFire
    {
        get
        {
            return _actualBuildingOnFire;
        }
    }

    

    private void Start()
    {
        _buildings = FindObjectsOfType<BuildingController>();
        PutNextBuildingOnFire();
        _actualGameTime = _gameTime;

        AdjustPointLabel();

        Time.timeScale = 0.0f;
        _ingameUI.SetActive(false);
        _menuePanel.SetActive(true);
        _gameOverPanel.SetActive(false);
        _pausePanel.SetActive(false);

    }

    private void Update()
    {
        _actualGameTime -= Time.deltaTime; 
        if(_actualGameTime <= 0.0f)
        {
            _actualGameTime = 0.0f;
            Time.timeScale = 0.0f;

            _ingameUI.SetActive(false);
            _gameOverPanel.SetActive(true);
            _gameOverPointsLabel.text = string.Format("Points: {0:D3}", _points);

            Cursor.lockState = CursorLockMode.None;

            _isGameStarted = false;
        }

        _countdownLabel.text = string.Format("Time: {0:D1}:{1:D2}",
                                            Mathf.FloorToInt(_actualGameTime /60.0f),
                                            Mathf.FloorToInt(_actualGameTime % 60.0f));

        if (_isGameStarted && Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPause)
            {
                OnContinueButtonClick();
            }
            else
            {
                _pausePanel.SetActive(true);
                Time.timeScale = 0.0f;
                Cursor.lockState = CursorLockMode.None;
            }

            _isPause = !_isPause;
        }
        Cursor.visible = Cursor.lockState != CursorLockMode.Locked;
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
            _points++;
            AdjustPointLabel();
        }

    }

    private void AdjustPointLabel()
    {
        _pointLabel.text = string.Format("Points: {0:D3}", _points);
    }


    public void OnStartButtonClick()
    {
        Time.timeScale = 1.0f;
        _menuePanel.SetActive(false);
        _ingameUI.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        _isGameStarted = true;
    }

    public void OnExitButtonClick()
    {
        Application.Quit();
    }

    public void OnMenueButtonClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnContinueButtonClick()
    {
        _pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
    }
}

