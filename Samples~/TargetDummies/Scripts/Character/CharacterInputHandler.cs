using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInputHandler : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private CharacterAttack attack;
    
    [SerializeField] private Animator _animator;
    private static readonly int AttackAnimationHash = Animator.StringToHash("Attack");
    
    private InputSystem_Actions _input;
    
    private void Start()
    {
        _input = TargetDummyInput.Actions;
        _input.Player.AddCallbacks(this);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _characterMovement.MoveInput = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            attack.Attack();
        }
    }
}
