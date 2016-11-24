using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    // Declare public variables
    //
    //Horizontal movement variables
    public float BaseSpeed;
    public float Acceleration;
    public float AccelCoefficient;
    public float MaxSpeed;
    // Jump variables
    public float JumpHeight;
    public float WallJump;
    public float RunJumpBonus;
    public float AirLag;
    public Transform groundPoint;
    public Transform groundPoint2;
    public Transform groundPoint3;
    public float radius;
    public LayerMask groundMask;
    // Declare RB2D variable
    Rigidbody2D rb2D;
    // Declare remaining class variables
    bool isGrounded;
    bool touchWallR;
    bool touchWallL;
    bool wallJumpR;
    bool wallJumpL;
    bool isRunning;
    bool isIdle;
    float tempAcceleration;
    float curVelocity;
    float preVelocity;
      
    void Start () {

    //Initialize RB2D
        rb2D = GetComponent<Rigidbody2D>();
        tempAcceleration = Acceleration;
    }

    void Update() {

        // Move Left or Right
        isRunning = Input.GetKey(KeyCode.LeftControl);
        float horzInput = Input.GetAxis("Horizontal");
        // Declare idle sensor
        if (isRunning || horzInput != 0)
        {
            isIdle = false;
        } else if (!isRunning && horzInput == 0)
        {
            isIdle = true;
        }
      
        if (!isIdle && isRunning)
        {
            Vector2 moveDir = new Vector2(Input.GetAxis("Horizontal") * BaseSpeed * tempAcceleration, rb2D.velocity.y);
            rb2D.velocity = moveDir;
            if (BaseSpeed * tempAcceleration < MaxSpeed)
            {
                tempAcceleration = tempAcceleration * AccelCoefficient;
            }
        } else if (!isIdle && !isRunning)
        {
            Vector2 moveDir = new Vector2(Input.GetAxis("Horizontal") * BaseSpeed, rb2D.velocity.y);
            rb2D.velocity = moveDir;
            tempAcceleration = Acceleration;
        }
        // Reset run acceleration 
        if (isIdle)
        {
            tempAcceleration = Acceleration;
        } else if(isRunning && horzInput == 0)
        {
            tempAcceleration = Acceleration;
        } else if(!isGrounded)
            {
                tempAcceleration = Acceleration;
            }
     
        // Declare ground sensor
        if(Physics2D.OverlapCircle(groundPoint.position, radius, groundMask))
        {
            isGrounded = true;
        } 
        // Declare wall sensors
        if(Physics2D.OverlapCircle(groundPoint2.position, radius, groundMask))
        {
            touchWallR = true;
        } else if(Physics2D.OverlapCircle(groundPoint3.position, radius, groundMask))
        {
            touchWallL = true;
        }

        // Flip player access on input
        if(Input.GetAxis("Horizontal") == 1)
        {
            transform.localScale = new Vector3(1, 1, 1);
        } else if(Input.GetAxis("Horizontal") == -1)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
//        Debug.Log("Base Speed (" + BaseSpeed + ") * Temp Acceleration (" + tempAcceleration + ") >= MaxSpeed (" + MaxSpeed + ") [" + (BaseSpeed * tempAcceleration));
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            if (BaseSpeed * tempAcceleration >= MaxSpeed && isGrounded)
            {
                rb2D.AddForce(new Vector2(-10, JumpHeight + RunJumpBonus));
            } else if(rb2D.velocity.x <= BaseSpeed && isGrounded)
            {
                rb2D.AddForce(new Vector2(0, JumpHeight));
            } else if(touchWallR && !isGrounded && !wallJumpR)
            {
                rb2D.AddForce(new Vector2(0, WallJump));
                wallJumpR = true;
                wallJumpL = false;
            } else if(touchWallL && !isGrounded && !wallJumpL)
            {
                rb2D.AddForce(new Vector2(0, WallJump));
                wallJumpL = true;
                wallJumpR = false;
            }
            Debug.Log(wallJumpL + " / " + wallJumpR);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundPoint.position, radius);
        Gizmos.DrawWireSphere(groundPoint2.position, radius);
        Gizmos.DrawWireSphere(groundPoint3.position, radius);
    }
}
