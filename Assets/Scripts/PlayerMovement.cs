using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    InputManager inputManager;

    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody rb;

    public float movementSpeed = 7f;
    public float runSpeed = 5f;
    public float sprintSpeed = 10f;

    public bool isSprinting;

    public float rotationSpeed = 15f;

    Animator anim;

    public float jumpHeight = 7f;
    public float gravityIntensity = -10f;

    public LayerMask whatIsGround;
    public float inAirTimer;
    public float fallingSpeed;

    // Powerups
    public float powerJumpHeight = 14f;
    public float baseJumpHeight;
    bool hasJumpPowerUp;
    [SerializeField] float powerUpTimer = 10f;

    bool hasDoubleJump;
    int numberOfJumps = 1;

    private void Awake()
    {
        baseJumpHeight = jumpHeight;

        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        cameraObject = Camera.main.transform;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void HandleMovement()
    {
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (isSprinting)
        {
            anim.SetFloat("Sprint", 1);
            movementSpeed = sprintSpeed;
        }
        else
        {
            anim.SetFloat("Sprint", 0);
            movementSpeed = runSpeed;
        }

        moveDirection *= movementSpeed;

        rb.velocity = moveDirection;

        if(moveDirection.magnitude == 0)
        {
            anim.SetBool("idle", true);
        }
        else
        {
            anim.SetBool("idle", false);
        }

    }

    void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputManager.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputManager.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if(targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    public bool IsGrounded()
    {
        RaycastHit hit;
        Vector3 sphereCastPos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        bool groundCheck = Physics.SphereCast(sphereCastPos, 0.2f, Vector3.down, out hit, 0.4f, whatIsGround);
        return groundCheck;
    }

    public void HandleJump()
    {
        if (IsGrounded() || numberOfJumps > 1)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            numberOfJumps--;

            float jumpVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);

            rb.AddForce(0, jumpVelocity, 0, ForceMode.Impulse);
        }
    }

    public void HandleFalling()
    {
        if (!IsGrounded())
        {
            anim.SetBool("isFalling", true);
            anim.SetFloat("yVelocity", rb.velocity.y);

            inAirTimer = inAirTimer + Time.deltaTime;
            rb.AddForce(Vector3.down * fallingSpeed * inAirTimer);
        }
        else
        {
            anim.SetBool("isFalling", false);
            inAirTimer = 0;

            if (hasDoubleJump)
            {
                numberOfJumps = 2;
            }
            else
            {
                numberOfJumps = 1;
            }

        }

    }

    public void HandleAllMovement()
    {
        HandleFalling();

        if (IsGrounded())
        {
            HandleMovement();
            HandleRotation();
        }
    }

    public void SetPowerJump()
    {
        if (!hasJumpPowerUp)
        {
            hasJumpPowerUp = true;
            jumpHeight = powerJumpHeight;
            StartCoroutine(PowerJumpTimer());
        }
    }

    IEnumerator PowerJumpTimer()
    {
        yield return new WaitForSeconds(powerUpTimer);
        hasJumpPowerUp = false;
        jumpHeight = baseJumpHeight;
    }

    public void SetDoubleJump()
    {
        if (!hasDoubleJump)
        {
            hasDoubleJump = true;
            StartCoroutine(DoubleJumpTimer());
        }
    }

    IEnumerator DoubleJumpTimer()
    {
        yield return new WaitForSeconds(powerUpTimer);
        hasDoubleJump = false;
    }

}
