using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public static class TargetDummyInput
{
    private static bool _init;
    private static InputSystem_Actions _actions;
    public static InputSystem_Actions Actions
    {
        get
        {
            if (!_init)
            {
                _actions = new InputSystem_Actions();
                _actions.Cursor.Enable();
                _actions.Player.Enable();
                _init = true;
            }
            return _actions;
        }
    }

    private static ControllerType _controllerType;
    public delegate void ControllerTypeChanged(ControllerType type);
    public static event ControllerTypeChanged OnControllerTypeChanged;

    public static ControllerType ControllerType => _controllerType;
    public static void SetControllerType(ControllerType type)
    {
        if(_controllerType == type) return;
        
        _controllerType = type;
        OnControllerTypeChanged?.Invoke(_controllerType);
    }
}

public enum ControllerType
{
    KeyboardAndMouse,
    Gamepad
}
