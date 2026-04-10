using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolutionTrigger : MonoBehaviour
{
    private enum States {
        VISABLE,
        TRANSPARENT,
        HIDDEN
    }
    [SerializeField] private GameObject particlesPrefab;
    [SerializeField] private GameObject mobPrefab;
    private States _state;
    private Transform _player;
    private SpriteRenderer _sprite;
    
    void Start() {
        _state = States.HIDDEN;
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _sprite = GetComponent<SpriteRenderer>();

        _sprite.enabled = false;
        StartCoroutine(RevealAfterWait());
    }

    void Update() {
        float sqrLen = (_player.position - transform.position).sqrMagnitude;
        States newState;
        
        if (sqrLen < 0.15f)     newState = States.HIDDEN;
        else if (sqrLen < 0.8f) newState = States.TRANSPARENT;
        else                    newState = States.VISABLE;

        if (_state != newState){
            switch(newState) {
                case States.HIDDEN:
                    _sprite.color = new Color(1.0f,1.0f,1.0f,0.0f);
                    break;
                case States.TRANSPARENT:
                    _sprite.color = new Color(1.0f,1.0f,1.0f,0.5f);
                    break;
                default:
                    _sprite.color = new Color(1.0f,1.0f,1.0f,1.0f);
                    break;
            }
            _state = newState;
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        ReactiveTarget mob = col.gameObject.GetComponent<ReactiveTarget>();
        //execute only for green and blue mobs
        if (mob && mob.transform.localScale.y == 1 && !mob.dying) {
            //create particles
            GameObject particles = GameObject.Instantiate(particlesPrefab) as GameObject;
            particles.transform.position = transform.position;

            //evolve old mob to new mob
            mob.Die();
            GameObject mobEvolved = GameObject.Instantiate(mobPrefab) as GameObject;
            mobEvolved.transform.position = transform.position;
        }
    }

    private IEnumerator RevealAfterWait() {
        yield return new WaitForSeconds(3.0f);
        _sprite.enabled = true;
    }
}
