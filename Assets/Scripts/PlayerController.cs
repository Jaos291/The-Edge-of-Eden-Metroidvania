using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    [Header("Ground and Movement Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float walkSpeed;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundChecky = 0.2f;
    [SerializeField] private float groundCheckx = 0.2f;
    [SerializeField] private LayerMask whatIsGround;

    private float xAxis;
    private CharacterAnimatorController characterAnimatorController;

    private void Start()
    {
        characterAnimatorController = GetComponent<CharacterAnimatorController>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(xAxis * walkSpeed, rb.linearVelocityY);
        characterAnimatorController.SetAnimationState("Walking", rb.linearVelocityX != 0 && isGrounded());
        characterAnimatorController.SetAnimationState("Jumping", !isGrounded());
    }

    #region PLAYERCONTROLLER

    public void Move(InputAction.CallbackContext context)
    {
        xAxis = context.ReadValue<Vector2>().x;
        Flip();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (characterAnimatorController.ReturnCurrentAnimation().Equals("Attacking1"))
            {
                characterAnimatorController.SetAnimationState("Attacking2", true);
            }
            else
            {
                characterAnimatorController.SetAnimationState("Attacking1", true);
            }
        }

    }

    private bool isGrounded()
    {
        if (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundChecky, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckx, 0, 0), Vector2.down, groundChecky, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckx, 0, 0), Vector2.down, groundChecky, whatIsGround))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    private void Flip()
    {
        if (xAxis < 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
        }
        else if (xAxis > 0)
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
        }
    }

    #endregion


    //OLD METHODS
    /*private void Update()
    {
        GetInputs();
        Move();
        Jump();
    }

    private void GetInputs()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
    }

    private void Jump()
    {
        if (Input.GetButtonUp("Jump") && rb.linearVelocityY > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, 0);
        }

        if (Input.GetButtonDown("Jump") && Grounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
        }
    }

    private void Move()
    {
        rb.linearVelocity = new Vector2(walkSpeed * xAxis, rb.linearVelocityY);
    }

    private bool Grounded()
    {
        if (Physics2D.Raycast(groundCheckPoint.position, Vector2.down, groundChecky, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckx,0,0), Vector2.down, groundChecky, whatIsGround)
            || Physics2D.Raycast(groundCheckPoint.position + new Vector3(-groundCheckx, 0, 0), Vector2.down, groundChecky, whatIsGround))
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }*/
}
