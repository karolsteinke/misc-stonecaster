using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public int stage {get; private set;} = 0;
    [SerializeField] private Text scoreLabel;
    [SerializeField] private Text stageLabel;
    [SerializeField] private GameObject gameOverMesssage;
    private int _score = 0;

    void Start()
    {
        Messenger<BroadcastingPickup>.AddListener(GameEvent.PICKUP_COLLECTED, OnPickupCollected);
        Messenger.AddListener(GameEvent.ENEMY_ESCAPED, OnEnemyEscaped);
        Messenger.AddListener(GameEvent.PLAYER_HIT, ShowGameOver);
    }

    void OnDestroy() {
        Messenger<BroadcastingPickup>.RemoveListener(GameEvent.PICKUP_COLLECTED, OnPickupCollected);
        Messenger.RemoveListener(GameEvent.ENEMY_ESCAPED, OnEnemyEscaped);
        Messenger.RemoveListener(GameEvent.PLAYER_HIT, ShowGameOver);
    }

    private void OnPickupCollected(BroadcastingPickup broadcaster) {
        _score++;
        stage = _score / 3 + 1;
        scoreLabel.text = "" + _score;
        stageLabel.text = "S T A G E   " + stage;
        if (!scoreLabel.gameObject.activeSelf) {
            scoreLabel.gameObject.SetActive(true);
        }
    }

    private void OnEnemyEscaped() {
        //decrease score if score is > 0 & game is live
        if (_score>0 && !gameOverMesssage.activeSelf) {
            _score--;
            stage = _score / 3 + 1;
            scoreLabel.text = "" + _score;
            stageLabel.text = "S T A G E   " + stage;
        }
    }

    private void ShowGameOver() {
        gameOverMesssage.SetActive(true);
    }
}
