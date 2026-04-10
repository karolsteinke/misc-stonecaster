using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    public enum MobTypes {
        Green = 0,
        Blue = 1
    }
    public MobTypes mobType = MobTypes.Green;
    [SerializeField] private float spawnTime = 1.5f;
    [SerializeField] private GameObject[] mobPrefabs = new GameObject[2];
    private GameObject controller;

    void Start() {
        StartCoroutine(SpawnAndDie());
        controller = GameObject.FindGameObjectWithTag("GameController");
    }

    private IEnumerator SpawnAndDie() {
        GameObject mob;
        yield return new WaitForSeconds(spawnTime);
        mob = Instantiate(mobPrefabs[(int)mobType]) as GameObject;
        mob.transform.position = transform.position;
        float speedModifier = 0.75f + controller.GetComponent<UIController>().stage*0.25f;
        mob.GetComponent<WanderingAI>().ScaleSpeedBy(speedModifier);
        
        Destroy(gameObject);
    }

}
