using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    [SerializeField] AudioClip coinPickup;
    [SerializeField] int pointsPerPickup = 100;
    bool wasCollected = false;
    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player" && !wasCollected)
        {
            wasCollected = true;
            FindObjectOfType<GameSession>().AddToScore(pointsPerPickup);
            AudioSource.PlayClipAtPoint(coinPickup,Camera.main.transform.position);
            Destroy(gameObject);
        }
    }
}
