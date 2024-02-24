using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHolder : MonoBehaviour
{
    //[field: SerializeField] public InputSO Input { get; private set; }
    public static InputHolder Instance;

    public float Acceleration;
    public float Turning;
    public Vector2 Direction;
    public bool IsAutoFire = false;
    public bool IsFiring;
    public bool IsAutoFiring;
    public bool IsSpecial;
    public event Action Special;
    public bool IsPause;
    public event Action Pause;
    public bool IsCancelUI;
    public event Action CancelUI;
    public bool IsDisableUI;
    public event Action DisableUI;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    public void GetAcceleration(InputAction.CallbackContext context)
    {
        Acceleration = context.ReadValue<float>();
    }

    public void GetTurning(InputAction.CallbackContext context)
    {
        Turning = context.ReadValue<float>();
    }

    public void GetDirection(InputAction.CallbackContext context)
    {
        Direction = context.ReadValue<Vector2>();
    }

    public void GetFiring(InputAction.CallbackContext context)
    {
        IsFiring = context.performed;
    }

    public void SetSpecial(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Special?.Invoke();
        }

        IsSpecial = context.performed;
    }

    public void SetPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Pause?.Invoke();
        }

        IsPause = context.performed;
    }

    public void SetAutoFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsAutoFire = !IsAutoFire;
        }
    }

    public void SetCancelUI(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            CancelUI?.Invoke();
        }

        IsCancelUI = context.performed;
    }

    public void SetDisableUI(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DisableUI?.Invoke();
        }

        IsDisableUI = context.performed;
    }
}