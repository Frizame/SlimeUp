using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static PlayerInputActions;

public interface IInputReader
{
    void EnablePlayerActions();
}

public class InputReader : MonoBehaviour, IInputReader, IPlayerActions
{
    public event UnityAction InteractPressed = delegate { };
    public event UnityAction InteractReleased = delegate { };

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

    public void OnMove(InputAction.CallbackContext context) { }
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
    public void OnLook(InputAction.CallbackContext context) { }
    public void OnNext(InputAction.CallbackContext context) { }
    public void OnPrevious(InputAction.CallbackContext context) { }
    public void OnSprint(InputAction.CallbackContext context) { }

    #endregion
}
