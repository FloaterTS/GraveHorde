using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public List<GameObject> zombiePrefabs;
    public Transform zombiesParentHolder;
    public float minSpawnInterval;
    public float maxSpawnInterval;
    public float minDistanceFromPlayerToSpawn;
    public float maxDistanceFromPlayerToSpawn;
    public float rateIncreaseAfter;
    public float rateIncreaseMultiplier;
    public float absoluteMinSpawnInterval;
    public bool spawnZombies = true;

    private GameObject[] spawnLocations;
    private GameObject player;
    private List<Vector3> spawnLocationsAtGoodDistance;

    void Start()
    {
        spawnLocations = GameObject.FindGameObjectsWithTag("Spawner");
        player = GameObject.FindWithTag("Player");

        spawnLocationsAtGoodDistance = new List<Vector3>();

        if (zombiePrefabs.Count > 0 && spawnLocations.Length > 0)
            StartSpawning();
        else
            Debug.Log("No zombie prefabs added or no spawner locations found. Spawner not spawning!");

        StartCoroutine(IncreaseSpawnRate());
    }

    void StartSpawning()
    {
        StartCoroutine(SpawnZombieCo());
    }

    IEnumerator SpawnZombieCo()
    {
        while (!spawnZombies)
            yield return null;

        float timeUntilNextSpawn = Random.Range(minSpawnInterval, maxSpawnInterval);
        yield return new WaitForSeconds(timeUntilNextSpawn);

        GameObject nextZombieTypeToSpawn = zombiePrefabs[Random.Range(0, zombiePrefabs.Count)];
        Vector3 nextSpawnLocation;

        UpdateSpawnLocationsFar();
        if(spawnLocationsAtGoodDistance.Count > 0)
            nextSpawnLocation = spawnLocationsAtGoodDistance[Random.Range(0, spawnLocationsAtGoodDistance.Count)];
        else
        {
            Debug.Log("No spawners outside player range or inside maxDistance spawn area at this moment.");
            StartCoroutine(SpawnZombieCo());
            yield break;
        }

        GameObject spawnedZombie = Instantiate(nextZombieTypeToSpawn, nextSpawnLocation, Quaternion.identity, zombiesParentHolder);

        spawnedZombie.GetComponent<Zombie>().chaseRadius = 10000f; // all spawned zombies chase the player immediately

        StartCoroutine(SpawnZombieCo());
    }

    void UpdateSpawnLocationsFar()
    {
        spawnLocationsAtGoodDistance.Clear();
        foreach(GameObject spawner in spawnLocations)
        {
            if (Vector3.Distance(player.transform.position, spawner.transform.position) >= minDistanceFromPlayerToSpawn &&
                Vector3.Distance(player.transform.position, spawner.transform.position) <= maxDistanceFromPlayerToSpawn)
                spawnLocationsAtGoodDistance.Add(spawner.transform.position);
        }
    }

    IEnumerator IncreaseSpawnRate()
    {
        yield return new WaitForSeconds(rateIncreaseAfter);
        minSpawnInterval /= rateIncreaseMultiplier;
        maxSpawnInterval /= rateIncreaseMultiplier;
        if(minSpawnInterval < absoluteMinSpawnInterval)
        {
            minSpawnInterval = absoluteMinSpawnInterval;
            if (maxSpawnInterval <= absoluteMinSpawnInterval)
                maxSpawnInterval = absoluteMinSpawnInterval + 1;
        }
        else
            StartCoroutine(IncreaseSpawnRate());
    }
}
