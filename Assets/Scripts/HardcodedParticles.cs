using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardcodedParticles : MonoBehaviour
{
    [SerializeField] private AudioClip particlesSound;
    private AudioSource _soundSource;

    void Start() {
        _soundSource = GetComponent<AudioSource>();
        _soundSource.PlayOneShot(particlesSound);

        for (int i=0; i < gameObject.transform.childCount; i++) {
            Transform pivot = gameObject.transform.GetChild(i);
            Transform particle = pivot.GetChild(0);

            /*
            pivot.localEulerAngles = new Vector3(
                pivot.localEulerAngles.x,
                pivot.localEulerAngles.y,
                pivot.localEulerAngles.z + Random.value * 90.0f
            );
            */
            
            Vector3 offset = particle.position - transform.position;
            particle.gameObject.GetComponent<Rigidbody2D>().velocity = offset * 80.0f;
        }
        StartCoroutine(WaitAndDie());
    }

    void FixedUpdate() {
        for (int i=0; i < gameObject.transform.childCount; i++) {
            Rigidbody2D particle = gameObject.transform.GetChild(i).GetChild(0).GetComponent<Rigidbody2D>();
            particle.velocity *= 0.88f;
        }
    }

    private IEnumerator WaitAndDie() {
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }
}
