using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerMove : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public PlayerTrail playerTrail;
    public float delay;
    public int order;

    void Start()
    {
        spriteRenderer.sortingOrder = order;
        UpdatePosition();
    }

    void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector3? point = playerTrail.GetPointAt(delay);        

        if (point.HasValue) {
            transform.position = point.Value;
        }
    }
}
