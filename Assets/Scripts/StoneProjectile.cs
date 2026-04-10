using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneProjectile : MonoBehaviour
{
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip fireSound;
    public bool fired {get; private set;}
    public bool dying {get; private set;}
    private Rigidbody2D _body;
    private BoxCollider2D _box;
    private SpriteRenderer _sprite;
    private AudioSource _soundSource;

    void Start() {
        _body = GetComponent<Rigidbody2D>();
        _box = GetComponent<BoxCollider2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _soundSource = GetComponent<AudioSource>();
        fired = false;
        dying = false;
    }

    
    void FixedUpdate() {
        //destroy if outside the scene
        var posX = transform.position.x;
        if (posX > 3.2f || posX < -3.2f) {
            Destroy(this.gameObject);
        }
    }

    public void Fire(Vector3 direction) {
        _soundSource.PlayOneShot(fireSound);
        _body.constraints = RigidbodyConstraints2D.None;
        _body.constraints = RigidbodyConstraints2D.FreezeRotation;
        _box.usedByEffector = false;
        _body.AddForce(direction, ForceMode2D.Impulse);
        fired = true;
    }

    void OnCollisionEnter2D(Collision2D col) {
        //inform any reactiveTarget that it was hit
        ReactiveTarget target = col.gameObject.GetComponent<ReactiveTarget>();
        if (target != null) {
            target.ReactToHit();
        }

        //erase yourself
        if (!dying) {
            _soundSource.PlayOneShot(hitSound);
            _sprite.color = new Color(.8f,.8f,.8f,.5f);
            dying = true;
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die() {
        yield return new WaitForSeconds(.5f);
        Destroy(this.gameObject);
    }
}
