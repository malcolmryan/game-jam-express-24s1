using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float jumpSpeed = 25;
    [SerializeField] private float jumpSpin = 360;

    private Actions actions;
    private InputAction moveAction;
    private InputAction jumpAction;
    private Rigidbody2D rigidbody;
    private AudioSource jumpAudio;

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
        Vector2 move = moveAction.ReadValue<Vector2>();
        Vector3 velocity = rigidbody.velocity;
        velocity.x = move.x * moveSpeed;

        if (jumpAction.WasPerformedThisFrame()) 
        {
            jumpAudio.Play();
            velocity.y = jumpSpeed;
            rigidbody.angularVelocity = jumpSpin * move.x;
        }

        rigidbody.velocity = velocity;
    }

    void OnTriggerEnter2D(Collider2D collider) {

    }

}
