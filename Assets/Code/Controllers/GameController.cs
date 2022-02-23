using UnityEngine;


public sealed class GameController : MonoBehaviour
{
    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private HudView _hudView;
    [SerializeField] private Data _data;
    private Controllers _controllers;

    private void Start()
    {
        _controllers = new Controllers();
            
        var playerFactory = new PlayerFactory(_data.PlayerData);
        var cameraFactory = new CameraFactory(_data.CameraData);

        var inputModel = new InputModel(_data.InputData);
        var playerModel = new PlayerModel(playerFactory);
        var cameraModel = new CameraModel(cameraFactory);

        var inputController = new InputController(inputModel);
            
        var playerController = new PlayerController(playerModel, inputModel, 
            _data.PlayerData, _hudView);
            
        var cameraController = new CameraController(cameraModel, _data.CameraData, 
            playerModel, inputModel);

        var weaponController = new WeaponController(inputModel, _weaponData, 
            cameraModel, playerModel);

        var cursorController = new CursorController();

        _controllers.Add(inputController);
        _controllers.Add(playerController);
        _controllers.Add(cameraController);
        _controllers.Add(weaponController);
        _controllers.Add(cursorController);

        _controllers.Initialize();
    }

    private void Update()
    {
        var deltaTime = Time.deltaTime;
        _controllers.Execute(deltaTime);
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