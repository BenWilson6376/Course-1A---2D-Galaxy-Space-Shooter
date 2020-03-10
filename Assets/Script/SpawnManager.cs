using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private float spawnTime = 3f;
    [SerializeField]
    private GameObject enemyContainer;
    [SerializeField]
    private GameObject[] powerUps;


    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    { 
        yield return new WaitForSeconds(2f);
      
        while (_stopSpawning == false) {
            GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-9.5f, 9.5f), 9f, 0), Quaternion.identity);
            newEnemy.transform.SetParent(enemyContainer.transform);
            yield return new WaitForSeconds(spawnTime);
        }

    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(2f);
        //every 3-7 seconds, spawn in a powerup
        while(_stopSpawning == false) {
            int randomPowerUp = Random.Range(0, powerUps.Length);
            yield return new WaitForSeconds(Random.Range(3, 7));
            Instantiate(powerUps[randomPowerUp], new Vector3(Random.Range(-9.5f, 9.5f), 9f, 0), Quaternion.identity);
        }
    }


    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
         
}
