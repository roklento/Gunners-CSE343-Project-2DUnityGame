using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RandomPowerGenerate : NetworkBehaviour
{
    [SerializeField] private GameObject[] buff;
    [SerializeField] private float minTime;
    [SerializeField] private float maxTime;
    private float time, spawnTime;
    private Vector2 spawnPos;
    private int randomNum;
    private GameObject randomGameObject;


    // Start is called before the first frame update
    void Start()
    {
        SetRandomTime();
        time = minTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsServer){ return; }
        time += Time.fixedDeltaTime;

        if(time >= spawnTime)
        {
            SetRandomTime();
            SpawnObject();
        }

        spawnPos = new Vector2(Random.Range(-10, 10), 4);
        randomNum = Random.Range(0,buff.Length);
    }

    void SpawnObject()
    {
        time = 0;

        randomGameObject = buff[randomNum];

        GameObject spawnedObject = Instantiate(randomGameObject, spawnPos, Quaternion.identity);
        spawnedObject.GetComponent<NetworkObject>().Spawn(true);
    }

    void SetRandomTime()
    {
        spawnTime = Random.Range(minTime, maxTime);
    }
}
