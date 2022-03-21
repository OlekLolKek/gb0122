using System.Collections;
using Photon.Pun;
using UniRx;
using UnityEngine;


public sealed class GameController : MonoBehaviour
{
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private HudView _hudView;
    [SerializeField] private Data _data;
    [SerializeField] private PhotonView _photonView;
    
    private Controllers _controllers;
    private float _matchCountdown;

    private void Start()
    {
        var musicPlayer = FindObjectOfType<MusicPlayer>();
        
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
            .Add(musicPlayer)
            .Add(inputController)
            .Add(playerController)
            .Add(cameraController)
            .Add(weaponController)
            .Add(cursorController);
        
        _controllers.Initialize();

        if (PhotonNetwork.IsMasterClient || !PhotonNetwork.IsConnected)
        {
            var aiController = new AiController(_data.BotData, _weaponData.AssaultRifleData);
            _controllers.Add(aiController);
            
            LoadingCountdown().ToObservable().Subscribe();
        }
    }

    private IEnumerator LoadingCountdown()
    {
        _photonView.RPC(nameof(StartFakeLoading), RpcTarget.All);
        
        yield return new WaitForSeconds(_data.MatchData.TimeToLoad);

        _photonView.RPC(nameof(StartCountdown), RpcTarget.All);

        yield return new WaitForSeconds(_data.MatchData.MatchStartCountdown);

        _photonView.RPC(nameof(StartMatchProcess), RpcTarget.All);

        yield return new WaitForSeconds(_data.MatchData.MatchLength);

        _photonView.RPC(nameof(StartMatchEndCountdown), RpcTarget.All);

        yield return new WaitForSeconds(_data.MatchData.MatchEndCountdown);
        
        Disconnect();
    }

    [PunRPC]
    private void StartFakeLoading()
    {
        _controllers.ChangeMatchState(MatchState.LoadingCountdown);
        _hudView.SetStartCountdown(true, _data.MatchData.MatchStartCountdown);
    }
    
    [PunRPC]
    private void StartCountdown()
    {
        _controllers.ChangeMatchState(MatchState.StartCountdown);

        StartingTimer().ToObservable().Subscribe();
    }

    private IEnumerator StartingTimer()
    {
        _matchCountdown = _data.MatchData.MatchStartCountdown;
        while (_matchCountdown > 0)
        {
            _hudView.SetStartCountdown(true, _matchCountdown);
            yield return 1;
        }
    }
    
    [PunRPC]
    private void StartMatchProcess()
    {
        _hudView.SetStartCountdown(false, _matchCountdown);
        _controllers.ChangeMatchState(MatchState.MatchProcess);
    }
    
    [PunRPC]
    private void StartMatchEndCountdown()
    {
        _controllers.ChangeMatchState(MatchState.EndScreen);

        EndingTimer().ToObservable().Subscribe();
    }
    
    private IEnumerator EndingTimer()
    {
        _matchCountdown = _data.MatchData.MatchEndCountdown;
        while (_matchCountdown > 0)
        {
            _hudView.SetEndCountdown(true, _matchCountdown);
            yield return 1;
        }
    }

    private void Disconnect()
    {
        _controllers.ChangeMatchState(MatchState.MainMenu);
        
        PhotonNetwork.LoadLevel(_data.MatchData.MainMenuSceneIndex);
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