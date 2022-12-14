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

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
    }

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

        spawnPos = new Vector2(Random.Range(-15, 15), 6);
        randomNum = Random.Range(0,buff.Length);
    }

    void SpawnObject()
    {
        time = 0;

        randomGameObject = buff[randomNum];

        GameObject spawnedObject = Instantiate(randomGameObject, spawnPos, Quaternion.identity);
        spawnedObject.GetComponent<NetworkObject>().Spawn(true);
        Destroy(spawnedObject, 5f);
    }

    void SetRandomTime()
    {
        spawnTime = Random.Range(minTime, maxTime);
    }
}
