using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float jumpSpin = 360;
    [SerializeField] private float jumpHeight = 10;
    [SerializeField] private float restoreSpin = 180;

    [SerializeField] private Vector3 groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float coyoteTime = 0.1f;

    private float jumpSpeed;
    private Actions actions;
    private InputAction moveAction;
    private InputAction jumpAction;
    new private Rigidbody2D rigidbody;
    private AudioSource jumpAudio;
    private float wasOnGround = float.NegativeInfinity;
    private float wasJumpPressed = float.NegativeInfinity;

    private float butterTimer = 0;
    private float jamTimer = 0;
    private float jamDuration = 0;
    [SerializeField] private AnimationCurve jamCurve;

    void Awake()
    {
        actions = new Actions();
        moveAction = actions.map.move;
        jumpAction = actions.map.jump;

        rigidbody = GetComponent<Rigidbody2D>();
        jumpAudio = GetComponent<AudioSource>();

        // v^2 = u^2 + 2as
        // Let v = 0 (velocity at highest point)
        // u^2 = -2as
        // u = sqrt(-2as)

        float a = Physics2D.gravity.y * rigidbody.gravityScale;
        jumpSpeed = Mathf.Sqrt(-2 * a * jumpHeight);
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

        if (butterTimer > 0) 
        {
            butterTimer -= Time.deltaTime;
        }
        if (jamTimer > 0) 
        {
            jamTimer -= Time.deltaTime;
        }

        if (jumpAction.WasPerformedThisFrame()) {
            wasJumpPressed = Time.time;
        }

        Vector2 move = moveAction.ReadValue<Vector2>();
        Vector3 velocity = rigidbody.velocity;
        float speed = moveSpeed;

        if (jamTimer > 0) 
        {
            float t = jamCurve.Evaluate(1 - jamTimer / jamDuration);
            speed *= t;
        }

        if (butterTimer > 0) 
        {
            // you can't slow down or turn while buttered
            float sign = Mathf.Sign(velocity.x);
            float mx = move.x * speed * sign;
            float vx = velocity.x * sign;
            vx = Mathf.Max(vx, mx) * sign;;
            velocity.x = vx;
        }
        else {
            velocity.x = move.x * speed;
        }



        if (Time.time - wasOnGround < coyoteTime) 
        {
            if (Time.time - wasJumpPressed < coyoteTime) {
                wasJumpPressed = float.NegativeInfinity;
                if (!jumpAudio.isPlaying) 
                {
                    jumpAudio.Play();
                }
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

    private void CheckOnGround() 
    {
        Vector2 point = transform.position + groundCheckPoint;
        Collider2D collider = Physics2D.OverlapCircle(point, groundCheckRadius, groundLayer); 
        if (collider != null)
        {
            wasOnGround = Time.time;
        }
    }

    public void AddSpread(SpreadHazard.Spread spread, float duration) 
    {
        switch (spread)
        {
            case SpreadHazard.Spread.BUTTER:
                butterTimer = duration;
                break;
            case SpreadHazard.Spread.JAM:
                jamTimer = duration;
                jamDuration = duration;
                break;
        }
    }

    void OnDrawGizmos() {
        bool onGround = (Time.time - wasOnGround < coyoteTime);
        Gizmos.color = (onGround  ? Color.green : Color.yellow);
        Gizmos.DrawSphere(transform.position + groundCheckPoint, groundCheckRadius);
    }

}
