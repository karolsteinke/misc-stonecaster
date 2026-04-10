using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMovement : MonoBehaviour
{
    [SerializeField] private enum MovementEnum {
        PosOnAbsoluteTime = 0,
        VelIgnoringFramerate = 1,
        VelIncludingFramerate = 2,
        PosOnFrames =3
    }
    [SerializeField] private MovementEnum movementType = MovementEnum.PosOnAbsoluteTime;
    [SerializeField] private float expectedFPS = 60.0f;
    private Rigidbody2D _body;
    private float prevClock = 0.0f;

    void Start() {
        _body = GetComponent<Rigidbody2D>();
        if (movementType == MovementEnum.VelIgnoringFramerate) {
            //vel = 0.76f should mean it moves 0.76f distance every sec, so 6,08f on 8sec?
            //theoretically it lags more if framerate is lower?
            _body.velocity = new Vector2(0.76f, 0.0f);
        }
    }

    void Update() {
        float clock = Time.time % 8.0f;
        
        //move to start at full clock cycle
        if (prevClock > clock) {
            _body.position = new Vector2(-3.04f, _body.position.y);
        }
        prevClock = clock;

        if (movementType == MovementEnum.PosOnAbsoluteTime) {
            //it's alway on accurate position based on time passed since scene start
            //6.08f is distance to be travelled
            float posX = clock / 8.0f * 6.08f - 3.04f;
            _body.position = new Vector2(posX,_body.position.y);
        }
        else if (movementType == MovementEnum.VelIncludingFramerate) {
            //velocity is adjusted constantly to include fluctuating framerate
            //vel should be 0.76f (avgFPS * deltaTime should be 1)
            _body.velocity = new Vector2(0.76f * expectedFPS * Time.deltaTime, 0.0f);
        }
        else if (movementType == MovementEnum.PosOnFrames) {
            //it changes position every frame by const value
            //6.08units is distance to be travelled in 8 sec
            float posX = _body.position.x + 6.08f / (8.0f * expectedFPS);
            _body.position = new Vector2(posX, _body.position.y);
        }
    }
}
