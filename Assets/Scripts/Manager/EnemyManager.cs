using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : Singleton<EnemyManager>
{
    //巡逻点
    [SerializeField]private Transform[] patrolPoints;
    //出生点
    [SerializeField] private Transform[] spawnPoints;

    [Header("该关卡的敌人")]
    public List<EnemyWave> enemyWaves;

    public int currentWaveIndex;
    public int currentEnemyCount;

    private void Update()
    {
        
        
        //在Enemy的脚本中控制currentEnemyCount的数量，start的时候++，onDestroy时--
        if (currentEnemyCount == 0)
        {
            StartCoroutine(StartNextWaveCoroutine());
        }
    }


    IEnumerator StartNextWaveCoroutine()
    {
        if (currentWaveIndex >= enemyWaves.Count)
            yield break;
        List<EnemyData> currentEnemyWave = enemyWaves[currentWaveIndex].enemies;
        foreach (EnemyData enemyData in currentEnemyWave)
        {
            for (int i = 0; i < enemyData.enemyCount; i++)
            {
                GameObject enemy = Instantiate(enemyData.enemyprefab, GetRandomSpawnPoint(), Quaternion.identity);
                //TODO:将巡逻点传给Enemy
                
                yield return new WaitForSeconds(enemyData.spawnInterval);
            }
        }
        currentWaveIndex++;
        
    }

    private Vector3 GetRandomSpawnPoint()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        return spawnPoint.position;
    }
}

[System.Serializable]
public class EnemyData
{
    public GameObject enemyprefab;
    public int enemyCount;
    public float spawnInterval;
}

public class EnemyWave
{
    public List<EnemyData> enemies;
}