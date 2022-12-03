using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDie : MonoBehaviour
{
    [SerializeField]private GameObject bulletDieParticulEffect;
 
    void Start()
    {
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        float dieTime = 1f;
        yield return new WaitForSeconds(dieTime);
        Die();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log(collision.gameObject.name);
            SpawnParticleEffects(collision.GetContact(0).point, collision.GetContact(0).normal);
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
    void SpawnParticleEffects(Vector2 spawnPos, Vector2 spawnDirection)
    {
        if (bulletDieParticulEffect != null)
        {
            Instantiate(bulletDieParticulEffect, spawnPos, Quaternion.LookRotation(spawnDirection));
        }
    }
}
