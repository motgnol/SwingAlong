using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlayerScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 50000f;
    [SerializeField] private float jumpForce = 5000;
    [SerializeField] private float climbStep = 7f;

    private Animator playerAnimator;

    private Vector2 moveInput;
    private float jumpInput;
    private Vector2 climbInput;
    private bool isGrounded;
    private bool isInWater;
    private bool canClimb;
    private bool rotatedAlready = false;

    private Rigidbody2D bodyRigidbody;
    


    void Start()
    {
        bodyRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {   //Bools for player movement
        isGrounded = GameManager.Instance.IsGrounded;
        isInWater = GameManager.Instance.IsInWater;
        canClimb = GameManager.Instance.CanClimb;
        rotatedAlready = GameManager.Instance.RotatedAlready;

        //Bools for animatioin
        playerAnimator.SetBool("Grounded", isGrounded);

        //Activate run animation
        if(moveInput.x == 0 ||!isGrounded)
            {
                playerAnimator.SetBool("Run", false);
            }
        if(moveInput.x != 0 && isGrounded)
            {
                playerAnimator.SetBool("Run", true);
            }

        //Activate jump animation
        if(jumpInput == 0 ||!isGrounded)
            {
                playerAnimator.SetBool("Jump", false);
            }
        if(jumpInput != 0 && isGrounded)
            {
                playerAnimator.SetBool("Jump", true);
            }

        //Activate climb animation
        if(canClimb && !isGrounded)
        {
            playerAnimator.SetBool("LadderContact", true);
        }
        else
        {
            playerAnimator.SetBool("LadderContact", false);
        }

        //Reset animations
        if(!isGrounded)
        {
            playerAnimator.SetBool("Run", false);
            playerAnimator.SetBool("Jump", false);
        }
        

        if(isInWater && !rotatedAlready)
        {
            transform.Rotate(0f, 180f, 0f);
            rotatedAlready =true;
            GameManager.Instance.RotatedAlready = rotatedAlready;
        }
        

        if(canClimb)
        {
            transform.Translate(0f, moveInput.y * climbStep * Time.deltaTime, 0f);
        }
    }

    void FixedUpdate()
    {
        if(isGrounded)
        {
            if(moveInput.x > 0 && rotatedAlready)
            {
                transform.Rotate(0f, -180f, 0f);
                rotatedAlready = false;
                GameManager.Instance.RotatedAlready = rotatedAlready;   
            }

            if(moveInput.x < 0 && !rotatedAlready)
            {
                transform.Rotate(0f, 180f, 0f);
                rotatedAlready = true;
                GameManager.Instance.RotatedAlready = rotatedAlready;  
            }

            bodyRigidbody.AddForce(new Vector2(moveInput.x * moveSpeed * Time.fixedDeltaTime, 0f), ForceMode2D.Force);
            bodyRigidbody.AddForce(new Vector2(0f, jumpInput * jumpForce * Time.fixedDeltaTime), ForceMode2D.Impulse);
        }

        
        if(bodyRigidbody.velocity.y > 0 && !isGrounded)
        {
            playerAnimator.SetBool("MovingUp", true);
            
        }
        else if(bodyRigidbody.velocity.y < 0 && !isGrounded)
        {
            playerAnimator.SetBool("MovingUp", false);
        }
        
    }

    void OnWalk(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        jumpInput = value.Get<float>();

        if(jumpInput > 0 && isGrounded)
        {
            GameManager.Instance.PlaySoundsEffects("boingClip");
        }
    }
}
