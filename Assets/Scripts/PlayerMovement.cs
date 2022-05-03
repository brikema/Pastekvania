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
    [SerializeField] private float jumpingPower;
    private bool isFacingRight = true;

    // Jump Buffer
    private float jumpBufferTime = 0.000000000000111f;
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
        if(coyoteTimeCounter != 0){
            // print(coyoteTimeCounter);
        }
        if(isPlayerDead){
            return;
        }
        rigidBody.velocity = new Vector2(horizontal * speed, rigidBody.velocity.y);
        if(isFacingRight && horizontal < 0f){
            Flip();
        }
        else if(!isFacingRight && horizontal > 0f){
            Flip();
        }

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

    public void Jump(){

        // rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpingPower);
        // context.performed

        // TODO : Faire en sorte que le saut n'augmente la vélocité que vers le haut
        // TODO : faire petit saut 

        if(IsGrounded()) {
            coyoteTimeCounter = coyoteTime;
        } else {
            coyoteTimeCounter = Time.deltaTime;
        }

        if(coyoteTimeCounter > 0f && playerInput.events.player.["Jump"].WasPressedThisFrame()) {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpingPower);
            coyoteTimeCounter = 0f;
        }

        if(coyoteTimeCounter > 0f && playerInput.actions["Jump"].WasPressedThisFrame()) {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * .5f);
            coyoteTimeCounter = 0f;
        }

        // if(IsGrounded() && context.performed) {
        //     rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.5f);
        // }

    }
}
