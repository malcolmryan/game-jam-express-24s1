using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadHazard : MonoBehaviour
{
    public enum Spread {
        BUTTER, JAM
    };

    [SerializeField] private Spread spread;
    [SerializeField] private float duration;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerMove player = collider.GetComponent<PlayerMove>();
        player.AddSpread(spread, duration);
    }
}
