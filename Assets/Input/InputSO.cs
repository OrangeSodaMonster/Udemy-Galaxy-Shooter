using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "MyInputSO", menuName = "MySOs/InputSO")]
public class InputSO_ : ScriptableObject
{
    public float Acceleration;
    public float Turning;
    public bool IsFiring;
    public bool IsFiringMissiles;
    public bool IsSpecialing;
    public bool IsAutoFire = false;
    public bool IsPausing = false;
    public bool IsClickUI = false;
    public bool IsCancelUI = false;
    public Vector2 Direction;

    public void GetAcceleration(InputAction.CallbackContext context)
    {
        Acceleration = context.ReadValue<float>();

        if(Mathf.Abs(Acceleration) > 0.85f)
            Acceleration = 1 * Mathf.Sign(Acceleration);
    }

    public void GetTurning(InputAction.CallbackContext context)
    {
        Turning = context.ReadValue<float>();

        if (Mathf.Abs(Turning) > 0.85f)
            Turning = 1 * Mathf.Sign(Turning);
        //Debug.Log("Turn: " + Turning);
    }

    public void GetDirection(InputAction.CallbackContext context)
    {
        Direction = context.ReadValue<Vector2>();
    }

    public void GetFiring(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsFiring = true;
        }
        else if (context.canceled)
        {
            IsFiring = false;
        }
        //if (IsFiring) { Debug.Log("Firing"); }
    }

    public void GetFiringMissiles(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsFiringMissiles = true;
        }
        else if (context.canceled)
        {
            IsFiringMissiles = false;
        }
        //if (IsFiringMissiles) { Debug.Log("FiringMissiles"); }
    }
    public void GetSpecialing(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsSpecialing = true;
        }
        else if (context.canceled)
        {
            IsSpecialing = false;
        }
        //if (IsSpecialing) { Debug.Log("Specialing"); }
    }

    public void GetPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsPausing = true;
        }
        else if (context.canceled)
        {
            IsPausing = false;
        }
    }

    public void GetAutoFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsAutoFire = !IsAutoFire;
        }
    }

    public void GetCancelUI(InputAction.CallbackContext context)
    {
        IsCancelUI = context.performed;
    }

    public void GetClickUI(InputAction.CallbackContext context)
    {
        IsClickUI = context.performed;
    }
}
