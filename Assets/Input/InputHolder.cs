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

    private void Start()
    {
        if(GameStatus.IsMobile && !GameStatus.IsJoystick) IsAutoFire = true;

        GameStatus.DisconectedJoystick.AddListener(OnDeviceLost);
    }

    public void OnDeviceLost()
    {
        UIManager.PauseGame?.Invoke();

        if(GameStatus.IsMobile && !GameStatus.IsJoystick)
        {
            DisableTouchControls.EnableTouchControls?.Invoke();
            IsAutoFire = true;
        }
    }

    #region Accel
    public void GetAcceleration(InputAction.CallbackContext context)
    {
        Acceleration = context.ReadValue<float>();
    }
    public void SetAccelTouch()
    {
        Acceleration = 1;
    }
    public void StopAccelTouch()
    {
        Acceleration = 0;
    }
    public void SetReverseTouch()
    {
        Acceleration = -1;
    }
    #endregion

    #region turning
    bool touchTurning = false;
    bool touchDirection = false;
    public void GetTurning(InputAction.CallbackContext context)
    {
        //if (touchTurning) return;
        Turning = context.ReadValue<float>();
    }
    public void GetDirection(InputAction.CallbackContext context)
    {
        //if (touchDirection) return;
        Direction = context.ReadValue<Vector2>();
    }
    public void GetDirectionTouch(Vector2 direction)
    {
        if (GameStatus.IsJoystick) return;
        Direction = direction;
    }
    public void SetTrueTouchDirection()
    {
        touchDirection = true;
    }
    public void SetFalseTouchDirection()
    {
        touchDirection = false;
    }
    public void SetTurnRightTouch()
    {
        touchTurning = true;
        Turning = 1;
    }    
    public void SetTurnLeftTouch()
    {
        touchTurning = true;
        Turning = -1;
    }
    public void StopTurningTouch()
    {
        touchTurning = false;
        Turning = 0;
    }

    #endregion

    #region Firing
    public void GetFiring(InputAction.CallbackContext context)
    {
        IsFiring = context.performed;
    }
    public void SetFiringTouch()
    {
        IsFiring = true;
    }
    public void StopFiringTouch()
    {
        IsFiring = false;
    }
    #endregion

    #region Special
    public void SetSpecial(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Special?.Invoke();
        }

        IsSpecial = context.performed;
    }
    public void SetSpecialTouch()
    {
        Special?.Invoke();
    }
    #endregion

    #region Pause

    public void SetPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Pause?.Invoke();
        }

        IsPause = context.performed;
    }
    public void SetPauseTouch()
    {       
        Pause?.Invoke();
    }
    #endregion

    public void SetAutoFire(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsAutoFire = !IsAutoFire;
        }
        if(GameStatus.IsMobile && !GameStatus.IsJoystick) 
            IsAutoFire = true;
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