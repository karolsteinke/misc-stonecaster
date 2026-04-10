using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingAI : MonoBehaviour
{
    [SerializeField] private float speed = 0.56f;
    [SerializeField] private float acc = 5.0f;
    private Rigidbody2D _body;
    private BoxCollider2D _box;
    private Animator _anim;
    private bool _grounded = false;
    private bool _facingRight;
    private float _velX = 0;

    void Start() {
        _body = GetComponent<Rigidbody2D>();
        _box = GetComponent<BoxCollider2D>();
        _anim = GetComponent<Animator>();
    }

    void FixedUpdate() {
        //detect if grounded
        //its overlap area is under or above the feet, according to local scale
        Vector3 max = _box.bounds.max;
        Vector3 min = _box.bounds.min;
        Vector3 center = _box.bounds.center;
        Vector2 corner1 = new Vector2(max.x, center.y - 0.161f * transform.localScale.y);
        Vector2 corner2 = new Vector2(min.x, center.y - 0.240f * transform.localScale.y);
        Collider2D hit = Physics2D.OverlapArea(corner1, corner2);
        bool _groundedNewValue = hit != null;

        //pick new random facing direction when just landed
        if (!_grounded && _groundedNewValue) {
            _facingRight = Random.value > .5;
        }
        _grounded = _groundedNewValue;

        //lerp velocity vector to 'speed' value
        if (_grounded) {
            if (_facingRight)
                _velX = Mathf.Lerp(_velX, speed, acc * Time.deltaTime);
            else
                _velX = Mathf.Lerp(_velX, -speed, acc * Time.deltaTime);
        }
        else {
            _velX = Mathf.Lerp(_velX, 0, acc * Time.deltaTime);
        }
        //change direction when at the scene edge
        var posX = transform.position.x;
        if (posX > 3.1f) {
            _facingRight = false;
            _velX = -speed;
        }
        else if (posX < -3.1f) {
            _facingRight = true;
            _velX = speed;
        }
        _body.velocity = new Vector2(_velX, _body.velocity.y);

        //destroy if it escaped through the top of the scene
        var posY = transform.position.y;
        if (posY > 2.5f) {
            Messenger.Broadcast(GameEvent.ENEMY_ESCAPED);
            Destroy(this.gameObject);
        }

        //set animator's parameter
        _anim.SetFloat("velX", Mathf.Abs(_velX));

        //face sprite to velocity
        if (!Mathf.Approximately(_velX, 0)) {
            transform.localScale = new Vector3(Mathf.Sign(_velX), transform.localScale.y, transform.localScale.z);
        }
    }

    public void ScaleSpeedBy(float muliplier) {
        speed *= muliplier;
    }
}
