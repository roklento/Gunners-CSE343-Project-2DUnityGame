using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDie : MonoBehaviour
{
    [SerializeField] private GameObject grenade;
    void Start()
    {
        Destroy(gameObject, 1f);
    }
}
