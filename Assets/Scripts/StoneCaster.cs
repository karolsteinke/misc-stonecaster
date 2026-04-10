using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneCaster : MonoBehaviour
{
    [SerializeField] private float fireBasePower = 3.0f;
    [SerializeField] private GameObject stonePrefab;
    private Rigidbody2D _body;
    private Transform _crosshair;
    private StoneProjectile _stone;
    private Transform _fireBar;
    private Transform _fbRotationPivot;
    private Transform _fbScalePivot;
    private Vector3 _heading;
    private float _firePower;

    void Start() {
        _body = GetComponent<Rigidbody2D>();
        _crosshair = transform.GetChild(0).GetComponent<Transform>();
        _firePower = fireBasePower;
    }

    //MonoBehaviour.FixedUpdate has the frequency of the physics system (50fps)
    void FixedUpdate()
    {
        //turn off crosshair (and fire bar) if stone is casted
        if (_stone) {
            if (_crosshair.gameObject.activeSelf) {
                _crosshair.gameObject.SetActive(false);
            }
        }
        //turn on crosshair if there are none casted stone
        else {
            if (!_crosshair.gameObject.activeSelf) {
                _crosshair.gameObject.SetActive(true);
            }
        }

        //set firePower value and animate fireBar
        if (_fbRotationPivot != null) {
            //set firePower based on modifier and base value
            //calculate 'modifier' (0-1) - dot product of player's body and heading vector
            _heading = _stone.gameObject.transform.position - transform.position;
            float modifier = Mathf.Clamp(Vector3.Dot(_body.velocity.normalized,_heading.normalized), 0f, 1.0f);
            _firePower = Mathf.Lerp(_firePower, fireBasePower * (1 + modifier), 0.10f);
        
            //set fireBar rotation and length
            float angle = Mathf.Atan2(_heading.y, _heading.x);
            _fbRotationPivot.transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);
            _fbScalePivot.transform.localScale = new Vector3(_firePower, _firePower, 1.0f);
            _fireBar.gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, _firePower/fireBasePower-1.0f, 0f);
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.X)) {    
            //casting (instatiating) stone
            if ((_stone == null && _crosshair.gameObject.activeSelf)) {
                _stone = Instantiate(stonePrefab, _crosshair.position, _crosshair.rotation).GetComponent<StoneProjectile>();
                
                //set firebar which is part of stone prefab
                _fbRotationPivot = _stone.gameObject.transform.GetChild(0);
                _fbScalePivot = _fbRotationPivot.GetChild(0);
                _fireBar = _fbScalePivot.GetChild(0);
                _firePower = fireBasePower;
            }         
            //firing stone
            else if (!_stone.fired) {
                Destroy(_fbRotationPivot.gameObject);
                Vector3 direction = _heading / _heading.magnitude;
                _stone.Fire(direction * _firePower);
            }
        }
    }
}
