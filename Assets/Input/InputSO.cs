using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "MyInputSO", menuName = "MySOs/InputSO")]
public class InputSO : ScriptableObject
{
    public float Acceleration;
    public float Turning;
    public bool IsFiring;
    public bool IsFiringMissiles;
    public bool IsSpecialing;
    public bool IsAutoFire = false;
    public bool IsPausing = false;

    public void GetAcceleration(InputAction.CallbackContext context)
    {
        Acceleration = context.ReadValue<float>();
        //Debug.Log("Acceleration: " + Acceleration);
    }

    public void GetTurning(InputAction.CallbackContext context)
    {
        Turning = context.ReadValue<float>();
        //Debug.Log("Turn: " + Turning);
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
}
