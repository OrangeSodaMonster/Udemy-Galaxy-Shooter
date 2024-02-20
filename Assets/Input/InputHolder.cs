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

    public event Action Special;
    public event Action Pause;
    public event Action CancelUI;

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
    }

    public void SetPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Pause?.Invoke();
        }
    }

    public void SetAutoFire(InputAction.CallbackContext context)
    {
        if (context.performed)
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
    }
}