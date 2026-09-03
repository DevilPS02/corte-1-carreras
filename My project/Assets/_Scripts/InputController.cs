using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public static InputController instance;

    private InputAction moveInput;
    private InputAction brakeInput;

    [HideInInspector] public Vector2 movementVector;
    [HideInInspector] public bool isBraking;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        moveInput = InputSystem.actions.FindAction("Move");
        brakeInput = InputSystem.actions.FindAction("Interact");
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        GetInputMovement();
        isBraking = brakeInput.IsPressed();
    }

    public void GetInputMovement()
    {
        movementVector = moveInput.ReadValue<Vector2>();
    }
}
