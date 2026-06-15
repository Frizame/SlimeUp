using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerInputActions;

public interface IInputReader
{
    void EnablePlayerActions();
}

public class InputReader : ScriptableObject, IInputReader, IPlayerActions
{
    public event UnityAction InteractPressed = delegate { };
    public event UnityAction InteractReleased = delegate { };

    public event UnityAction<Vector2> MoveChanged = delegate { };
    public event UnityAction<Vector2> LookChanged = delegate { };

    /// <summary>Latest movement axis value (WASD / left stick), range -1..1 per axis.</summary>
    public Vector2 MoveInput { get; private set; }

    /// <summary>Latest look delta (mouse / right stick) for this frame.</summary>
    public Vector2 LookInput { get; private set; }

    public PlayerInputActions inputActions;
    public void EnablePlayerActions()
    {
        if (inputActions == null)
        {
            inputActions = new PlayerInputActions();
            inputActions.Player.SetCallbacks(this);
        }
        inputActions.Enable();
    }

    public void DisablePlayerActions()
    {
        if (inputActions != null)
        {
            inputActions.Player.Disable();
        }
    }
    
    public string GetInteractDisplayString()
    {
        if (inputActions == null)
        {
            return "";
        }

        foreach (InputBinding binding in inputActions.Player.Interact.bindings)
        {
            if (binding.groups != null && binding.groups.Contains("Keyboard"))
            {
                return InputControlPath.ToHumanReadableString(
                    binding.effectivePath,
                    InputControlPath.HumanReadableStringOptions.OmitDevice);
            }
        }

        return "";
    }

    #region Player Actions

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
        MoveChanged.Invoke(MoveInput);
    }
    public void OnAttack(InputAction.CallbackContext context) { }
    public void OnCrouch(InputAction.CallbackContext context) { }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InteractPressed.Invoke();
        }
        else if (context.canceled)
        {
            InteractReleased.Invoke();
        }
    }
    public void OnJump(InputAction.CallbackContext context) { }
    public void OnLook(InputAction.CallbackContext context)
    {
        LookInput = context.ReadValue<Vector2>();
        LookChanged.Invoke(LookInput);
    }
    public void OnNext(InputAction.CallbackContext context) { }
    public void OnPrevious(InputAction.CallbackContext context) { }
    public void OnSprint(InputAction.CallbackContext context) { }

    #endregion
}
