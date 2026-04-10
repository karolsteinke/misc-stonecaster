using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadcastingPickup : MonoBehaviour
{
    [SerializeField] private GameObject particlesPrefab;

    void OnTriggerEnter2D(Collider2D other) {
        //only when player is triggering pickup
        if (other.gameObject.GetComponent<PlatformerPlayer>()) {
            //create particles
            GameObject particles = GameObject.Instantiate(particlesPrefab) as GameObject;
            particles.transform.position = transform.position;

            Messenger<BroadcastingPickup>.Broadcast(GameEvent.PICKUP_COLLECTED, this);
        }
    }

    public void Die() {
        Destroy(gameObject);
    }
}
