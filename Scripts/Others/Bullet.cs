using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int bulletDamage = 10;
    //[SerializeField] private ParticleSystem hitWall = null;
    //[SerializeField] private ParticleSystem hitZombie = null;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            return;
        if(collision.gameObject.CompareTag("Zombie"))
        {
            Zombie zombie = collision.gameObject.GetComponent<Zombie>();
            if (zombie != null)
                zombie.Damaged(bulletDamage);
            else
                Debug.LogError("Zombie missing zombie script");
        }
        gameObject.SetActive(false);
    }
}
