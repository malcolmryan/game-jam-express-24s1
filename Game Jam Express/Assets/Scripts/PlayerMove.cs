using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float jumpSpeed = 25;
    [SerializeField] private float jumpSpin = 360;
    [SerializeField] private float restoreSpin = 180;

    [SerializeField] private Vector3 groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float coyoteTime = 0.1f;

    private Actions actions;
    private InputAction moveAction;
    private InputAction jumpAction;
    new private Rigidbody2D rigidbody;
    private AudioSource jumpAudio;
    private float wasOnGround = float.NegativeInfinity;
    private float wasJumpPressed = float.NegativeInfinity;

    void Awake()
    {
        actions = new Actions();
        moveAction = actions.map.move;
        jumpAction = actions.map.jump;

        rigidbody = GetComponent<Rigidbody2D>();
        jumpAudio = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        actions.map.Enable();
    }

    void OnDisable()
    {
        actions.map.Disable();
    }

    void Update()
    {
        CheckOnGround();

        if (jumpAction.WasPerformedThisFrame()) {
            wasJumpPressed = Time.time;
        }

        Vector2 move = moveAction.ReadValue<Vector2>();
        Vector3 velocity = rigidbody.velocity;
        velocity.x = move.x * moveSpeed;

        if (Time.time - wasOnGround < coyoteTime) 
        {
            if (Time.time - wasJumpPressed < coyoteTime) {
                jumpAudio.Play();
                velocity.y = jumpSpeed;
                rigidbody.angularVelocity = jumpSpin * move.x;
            }
            
            if (velocity.y <= 0) {
                // add force to return to upright
                float angle = transform.localEulerAngles.z;
                angle = (angle + 180) % 360 - 180;  // in range -180 to 180
                rigidbody.angularVelocity = -restoreSpin * angle / 180;
            }
        }

        rigidbody.velocity = velocity;
    }

    private void Jump() 
    {
    }


    private void CheckOnGround() {
        Vector2 point = transform.position + groundCheckPoint;
        Collider2D collider = Physics2D.OverlapCircle(point, groundCheckRadius, groundLayer); 
        if (collider != null)
        {
            wasOnGround = Time.time;
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {

    }

    void OnDrawGizmos() {
        bool onGround = (Time.time - wasOnGround < coyoteTime);
        Gizmos.color = (onGround  ? Color.green : Color.yellow);
        Gizmos.DrawSphere(transform.position + groundCheckPoint, groundCheckRadius);
    }

}
