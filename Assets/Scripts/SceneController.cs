using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{   
    [SerializeField] private GameObject spawnerPrefab;
    [SerializeField] private GameObject pickupPrefab;
    [SerializeField] private float redSpawnTime = 5.0f;
    private Vector3[] _redSpawnSpots = new Vector3[] {
        new Vector3(2.72f,1.6f,0),
        new Vector3(-2.72f,1.6f,0)
    };
    private Vector3[] _pickupSpots = new Vector3[] {
        new Vector3(0,.96f,0),
        new Vector3(0,-0.32f,0),
        new Vector3(1.44f,1.28f,0),
        new Vector3(-1.44f,1.28f,0),
        new Vector3(3.04f,-0.64f,0),
        new Vector3(-3.04f,-0.64f,0),
        new Vector3(1.12f,0,0),
        new Vector3(-1.12f,0,0)
    };
    private float _time;
    private BroadcastingPickup[] _pickups = new BroadcastingPickup[2];
    
    void Start() {
        //ignore collisions between mobs
        Physics2D.IgnoreLayerCollision(6, 6);

        Messenger<BroadcastingPickup>.AddListener(GameEvent.PICKUP_COLLECTED, UpdatePickups);
        Messenger.AddListener(GameEvent.PLAYER_HIT, GameOver);
        
        CreateSpawner(MobSpawner.MobTypes.Green, _redSpawnSpots[0]);
        StartCoroutine(CreatePickups());
    }

    void OnDestroy() {
        Messenger<BroadcastingPickup>.RemoveListener(GameEvent.PICKUP_COLLECTED, UpdatePickups);
        Messenger.RemoveListener(GameEvent.PLAYER_HIT, GameOver);
    }

    void Update() {
        //timer for red spawners
        if (_time < redSpawnTime) {
            _time += Time.deltaTime;
        }
        else {
            int randIdx = Random.Range(0, _redSpawnSpots.Length);
            CreateSpawner(MobSpawner.MobTypes.Green, _redSpawnSpots[randIdx]);
            _time = 0;
        }
    }

    //creates pickups, positions are random from predefined array
    private IEnumerator CreatePickups() {
        yield return new WaitForSeconds(2.0f);
        int randIdx = -1;
        for (int i=0; i<_pickups.Length; i++) {
            GameObject pickup = Instantiate(pickupPrefab) as GameObject;
            //put at random position, make sure it's different second time
            int rand;
            do rand = Random.Range(0, _pickupSpots.Length); while (rand == randIdx);
            randIdx = rand;
            pickup.transform.position = _pickupSpots[randIdx];
            _pickups[i] = pickup.GetComponent<BroadcastingPickup>();
        }
    }

    //to be called on GameEvent.PICKUP_COLLECTED
    //erases pickup and creates blue spawner at second pickup
    private void UpdatePickups(BroadcastingPickup broadcaster) {
        foreach (BroadcastingPickup pickup in _pickups) {
            if (pickup != null) {
                //create blue spawner at second pickup (that one which player isn't collecting)
                if (pickup != broadcaster) {
                    CreateSpawner(MobSpawner.MobTypes.Blue, pickup.transform.position);
                }
                pickup.Die();
            }
        }
        StartCoroutine(CreatePickups());
    }

    //creates given type of spawner at pos
    private void CreateSpawner(MobSpawner.MobTypes mobType, Vector3 pos) {
        GameObject spawner = Instantiate(spawnerPrefab) as GameObject;
        spawner.GetComponent<MobSpawner>().mobType = mobType;
        spawner.transform.position = pos;
    }

    private void GameOver() {
        //Deactive player
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerPlayer>().Die();
    }

    //to be called with button press
    public void RestartGame() {
        SceneManager.LoadScene("Level01");
    }
}
