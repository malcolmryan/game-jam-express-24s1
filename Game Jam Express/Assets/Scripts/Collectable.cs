using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    [SerializeField] private AudioSource audioPrefab;

    void OnTriggerEnter2D(Collider2D collider) {
        AudioSource a = Instantiate(audioPrefab, transform.position, transform.rotation);
        Destroy(a.gameObject, a.clip.length);
        Destroy(gameObject);        
    }

}
