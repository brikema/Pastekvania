using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody2D rigidBody;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 25f;
    private bool isFacingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rigidBody.velocity = new Vector2(horizontal * speed, rigidBody.velocity.y);
        if(isFacingRight && horizontal < 0f){
            Flip();
        }
        else if(!isFacingRight && horizontal > 0f){
            Flip();
        }

    }

    private bool IsGrounded(){
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
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
        if(context.performed && IsGrounded()){
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpingPower);
        }

        if(context.canceled && rigidBody.velocity.y > 0f){
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpingPower * 0.5f);
        }
    }
}
