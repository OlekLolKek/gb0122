using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;


public sealed class GameController : MonoBehaviour
{
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private HudView _hudView;
    [SerializeField] private Data _data;
    
    private Controllers _controllers;
    private float _matchCountdown;

    private void Start()
    {
        _controllers = new Controllers();

        var spawnPoints = Object.FindObjectsOfType<PlayerSpawnPointView>();

        var playerFactory = new PlayerFactory(_data.PlayerData, spawnPoints);
        var cameraFactory = new CameraFactory(_data.CameraData);

        var inputModel = new InputModel(_data.InputData);
        var playerModel = new PlayerModel(playerFactory);
        var cameraModel = new CameraModel(cameraFactory);

        var inputController = new InputController(inputModel);

        var playerController = new PlayerController(playerModel, inputModel,
            _data.PlayerData, _hudView, spawnPoints);

        var cameraController = new CameraController(cameraModel, _data.CameraData,
            playerModel, inputModel);

        var weaponController = new WeaponController(inputModel, _weaponData,
            cameraModel, playerModel, _hudView);

        var cursorController = new CursorController();

        _controllers
            .Add(inputController)
            .Add(playerController)
            .Add(cameraController)
            .Add(weaponController)
            .Add(cursorController);

        if (PhotonNetwork.IsMasterClient || !PhotonNetwork.IsConnected)
        {
            var aiController = new AiController(_data.BotData, _weaponData.AssaultRifleData);
            _controllers.Add(aiController);
            
            LoadingCountdown().ToObservable().Subscribe();
        }

        _controllers.Initialize();
    }

    // init
    // fake loading
    // start cooldown
    // start
    // match length timer
    // match end
    // aftermatch cooldown
    
    private IEnumerator LoadingCountdown()
    {
        _controllers.ChangeMatchState(MatchState.LoadingCountdown);
        yield return new WaitForSeconds(_data.MatchData.TimeToLoad);

        _controllers.ChangeMatchState(MatchState.StartCountdown);
        _matchCountdown = _data.MatchData.MatchStartCountdown;
        var endOfFrame = new WaitForEndOfFrame();

        while (_matchCountdown > 0)
        {
            Debug.Log($"Starting in {_matchCountdown}");
            yield return endOfFrame;
        }

        _controllers.ChangeMatchState(MatchState.MatchProcess);
        _matchCountdown = _data.MatchData.MatchLength;
        while (_matchCountdown > 0)
        {
            Debug.Log($"Ending in {_matchCountdown}");
            yield return endOfFrame;
        }

        _controllers.ChangeMatchState(MatchState.MatchEndCountdown);
        _matchCountdown = _data.MatchData.MatchEndCountdown;
        while (_matchCountdown > 0)
        {
            Debug.Log($"Going to menu in {_matchCountdown}");
            yield return endOfFrame;
        }

        Disconnect().ToObservable().Subscribe();
    }

    private IEnumerator Disconnect()
    {
        PhotonNetwork.LeaveRoom();

        while (PhotonNetwork.NetworkClientState == ClientState.Leaving)
        {
            yield return 0;
        }
        
        PhotonNetwork.LoadLevel(_data.MatchData.MainMenuScene);
    }

    private void Update()
    {
        var deltaTime = Time.deltaTime;
        _controllers.Execute(deltaTime);

        _matchCountdown -= deltaTime;
    }

    private void LateUpdate()
    {
        var deltaTime = Time.deltaTime;
        _controllers.LateExecute(deltaTime);
    }

    private void FixedUpdate()
    {
        _controllers.FixedExecute();
    }

    private void OnDestroy()
    {
        _controllers.Cleanup();
    }
}