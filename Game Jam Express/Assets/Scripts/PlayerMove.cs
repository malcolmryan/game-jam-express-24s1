using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private Actions actions;
    private InputAction moveAction;

    void Start()
    {
        actions = new Actions();
        moveAction = actions.map.move;
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
        
    }
}
