using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerMovement playerMovement;

    public Vector2 movementInput;

    public float verticalInput;
    public float horizontalInput;

    public bool sprintInput;
    public bool jumpInput;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();

            playerControls.PlayerMovement.Sprint.performed += i => sprintInput = true;
            playerControls.PlayerMovement.Sprint.canceled += i => sprintInput = false;

            playerControls.PlayerMovement.Jump.performed += i => jumpInput = true;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void HandleJumpingInput()
    {
        if (jumpInput)
        {
            jumpInput = false;
            playerMovement.HandleJump();
        }
    }

    void HandleOnMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
    }

    void HandleSprintingInput()
    {
        playerMovement.isSprinting = sprintInput;
    }

    public void HandleAllInputs()
    {
        HandleJumpingInput();
        HandleOnMovementInput();
        HandleSprintingInput();
    }

}
