using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class CursorInputHandler : MonoBehaviour
{
    private const int MaxHits = 20;
    private const float MaxDistance = 500;
    
    public static Vector3 MousePosition { get; private set; }
    public static Vector2 MouseScreenPosition { get; private set; }
    
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private LayerMask groundLayers;
    
    private InputSystem_Actions _input;
    
    private readonly RaycastHit[] _hits = new RaycastHit[MaxHits];
    private int _hitCount;

    private bool _dampingCursor;
    private float _dampTime;

    private void Awake()
    {
        if(_mainCamera == null) _mainCamera = Camera.main;
    }

    private void Start()
    {
        _input = TargetDummyInput.Actions;
        InputSystem.onAnyButtonPress.Call(OnAnyInput);
    }
    
    public void Update()
    {
        if (TargetDummyInput.ControllerType == ControllerType.KeyboardAndMouse)
        {
            MouseScreenPosition = _input.Cursor.MousePosition.ReadValue<Vector2>();
        }
        else
        {
            MouseScreenPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
        }

        Ray ray = _mainCamera.ScreenPointToRay(MouseScreenPosition);
            
        _hitCount = Physics.RaycastNonAlloc(ray.origin, ray.direction, _hits, MaxDistance, groundLayers);
        if (_hitCount > 0)
        {
            MousePosition = _hits[0].point;
        }
    }
    
    private void OnAnyInput(InputControl obj)
    {
        if (obj.device == Keyboard.current || obj.device == Mouse.current)
        {
            TargetDummyInput.SetControllerType(ControllerType.KeyboardAndMouse);
        }
        else
        {
            TargetDummyInput.SetControllerType(ControllerType.Gamepad);
        }
    }
}
