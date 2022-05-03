using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Player
    public Rigidbody2D rigidBody;
    public BoxCollider2D boxCollider2D;
    public PlayerInput playerInput;

    public LayerMask platformLayer;

    // Déplacement normal
    private float horizontal;
    [SerializeField] private float speed;
    private bool isFacingRight = true;
    [SerializeField] private float jumpingPower;
    private bool isJumping;
    private bool jumpCooldown;

    // Jump Buffer
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    // Coyote Time
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private bool isPlayerDead = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayerDead){
            return;
        }

        // Gestion CoyoteTime
        if(IsGrounded()){
            coyoteTimeCounter = coyoteTime;
        } else {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Gestion Jump Buffer
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f && !jumpCooldown)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpingPower);
            jumpBufferCounter = 0f;

            StartCoroutine(JumpCooldown());

            if (!isJumping)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.5f);
            }
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if(isFacingRight && horizontal < 0f){
            Flip();
        }
        else if(!isFacingRight && horizontal > 0f){
            Flip();
        }
    }

    private void FixedUpdate()
    {
        rigidBody.velocity = new Vector2(horizontal * speed, rigidBody.velocity.y);
    }

    private bool IsGrounded(){
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, .1f, platformLayer);
        return raycastHit2D.collider != null;
    }

    private void Flip(){
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context){
        horizontal = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context){
        if(isPlayerDead){
            return;
        }
        // rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpingPower);
        // context.performed

        // TODO : Faire en sorte que le saut n'augmente la vélocité que vers le haut
        // TODO : faire petit saut 

        if (context.performed)
        {
            isJumping = true;
            jumpBufferCounter = jumpBufferTime;
        }

        if(context.canceled) {
            isJumping = false;

            if (rigidBody.velocity.y > 0f)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.5f);
                coyoteTimeCounter = 0f;
            }
        }
    }

    private IEnumerator JumpCooldown()
    {
        jumpCooldown = true;
        yield return new WaitForSeconds(0.01f);
        jumpCooldown = false;
    }
}
