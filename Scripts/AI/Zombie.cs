using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public enum ZombieState
{
    idle,
    chasing,
    attacking,
    dead
}

public class Zombie : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float chaseRadius = 6f;
    public float attackRadius = 1f;
    public float attackDuration = 0.8f; //attack animation duration
    public float attackRadiusRangeMultiplier = 3f;
    public float deathDissapearanceTime;
    public int attackDamage;
    public int maxHealth;
    public bool dissapearAfterDeath;
    public List<GameObject> pickupPrefabs;

    private Transform playerPosition;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private PlayerHealth playerHealth;
    private ZombieState zombieState;
    private readonly float minMovingVelocity = 0.1f;
    private int currentHealth;


    void Start()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.speed = moveSpeed;
        navMeshAgent.stoppingDistance = attackRadius;

        currentHealth = maxHealth;

        zombieState = ZombieState.idle;
    }

    void Update()
    {
        if (zombieState != ZombieState.dead)
        {
            CheckMovement();
            CheckPlayerPosition();
        }
    }

    void CheckMovement()
    {
        if (Mathf.Abs(navMeshAgent.velocity.x) > minMovingVelocity ||
            Mathf.Abs(navMeshAgent.velocity.z) > minMovingVelocity)
            animator.SetBool("isMoving", true);
        else
            animator.SetBool("isMoving", false);
    }

    void CheckPlayerPosition()
    {
        float distanceFromPlayer = Vector3.Distance(transform.position, playerPosition.position);

        if (distanceFromPlayer <= chaseRadius && zombieState != ZombieState.attacking)
        {
            ChasePlayer();
            if (distanceFromPlayer <= attackRadius)
                StartCoroutine(AttackPlayer());
        }
        else
        {
            if (zombieState == ZombieState.chasing)
                StopChasing();
        }
    }

    void ChasePlayer()
    {
        // transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        zombieState = ZombieState.chasing;
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(playerPosition.position);
    }

    void StopChasing()
    {
        navMeshAgent.ResetPath();
        navMeshAgent.isStopped = true;
        zombieState = ZombieState.idle;
    }

    IEnumerator AttackPlayer()
    {
        StopChasing();

        transform.LookAt(new Vector3(playerPosition.position.x, transform.position.y, playerPosition.position.z));

        zombieState = ZombieState.attacking;

        animator.SetTrigger("attack");
        yield return new WaitForSeconds(attackDuration);

        if (zombieState == ZombieState.dead)
            yield break;

        if (Vector3.Distance(transform.position, playerPosition.position) < attackRadius * attackRadiusRangeMultiplier)
            playerHealth.TakeDamage(attackDamage);

        zombieState = ZombieState.chasing;
    }

    public void Damaged(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0 && zombieState != ZombieState.dead)
        {
            Die();
            ScoreScript.scoreValue += 100;
        }
    }

    private void Die()
    {
        int rng = Random.Range(1, 3);

        if (rng == 1)
            SoundManagerScript.PlaySound("zombiedeath1");
        else
            SoundManagerScript.PlaySound("zombiedeath2");
        if (zombieState == ZombieState.idle)
            animator.SetTrigger("dieFront");
        else
            animator.SetTrigger("dieBack");

        zombieState = ZombieState.dead;

        navMeshAgent.isStopped = true;
        Destroy(navMeshAgent);
        Destroy(GetComponent<Collider>());
        Destroy(this);
        if (dissapearAfterDeath)
            Destroy(gameObject, deathDissapearanceTime);
        
        SpawnRandomItem();
    }

    void SpawnRandomItem()
    {
        if (pickupPrefabs != null)
        {
            var random = Random.Range(0, pickupPrefabs.Count);
            var nextPickupToSpawn = pickupPrefabs[random];
            if (nextPickupToSpawn != null)
            {
                Instantiate(nextPickupToSpawn, transform.position + new Vector3(0,0.5f,0), Quaternion.identity);
            }
        }
    }
}
