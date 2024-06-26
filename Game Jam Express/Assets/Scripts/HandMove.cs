using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMove : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();        
    }

    void Update()
    {
        rigidbody.velocity = Vector3.right * speed;        
    }
}
