using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class InputManager : MonoBehaviour
{

    public static InputManager Instance;
    public PlayerControls input = null;
    private Vector2 moveVector = Vector2.zero;

    public Input customInput;

    public bool verticalMovement = false;
    public bool horizontalMovement = false;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            input = new PlayerControls();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator WaitForNetwork()
    {
        while (NetworkManager.Instance == null)
        {
            yield return null;
        }

        // NetworkManager.Instance.Runner.AddCallbacks(this);
    }

    private void OnEnable()
    {
        input.Enable();
        input.Player.Look.performed += onLook;
        input.Player.Move.performed += OnMove;
        input.Player.Move.canceled += OnMove;
        input.Player.Move.canceled += onMoveCancel;
        input.Player.Fire.performed += onFire;
    }

    private void OnDisable()
    {
        input.Player.Look.performed -= onLook;
        input.Player.Move.performed -= OnMove;
        input.Player.Move.canceled -= OnMove;
        input.Player.Move.canceled -= onMoveCancel;
        input.Player.Fire.performed -= onFire;


        if (NetworkManager.Instance.Runner != null)
        {
            // disabling the input map
            input.Disable();

            //NetworkManager.Instance.Runner.RemoveCallbacks(this);
        }

    }

    public void onLook(InputAction.CallbackContext value)
    {

        if (EventManager.instance != null)
        {
            EventManager.TriggerEvent("PlayerLook",
                new Dictionary<string, object> { { "value", value.ReadValue<Vector2>() } });
        }
    }

    public void onFire(InputAction.CallbackContext value)
    {
        if (EventManager.instance != null)
            EventManager.TriggerEvent("PlayerFire", null);

    }

    public void onMoveCancel(InputAction.CallbackContext value)
    {
        if (moveVector.x == 0) horizontalMovement = false;
        if (moveVector.y == 0) verticalMovement = false;
    }

    public void OnMove(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>().normalized;

        if (EventManager.instance != null)
            EventManager.TriggerEvent("PlayerMove", new Dictionary<string, object> { { "value", moveVector }, { "playerId", "1" } });
    }




}
