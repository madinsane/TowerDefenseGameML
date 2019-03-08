using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public static int EnemiesAlive { get; set; }
    public static int OpponentEnemiesAlive { get; set; }

    [Header("Enemies")]
    public GameObject enemySetPrefab;

    [Header("Setup")]
    public Transform spawnPoint;
    public float enemyDelay = 0.2f;
    public int valuePerWave = 25;
    public int baseValue = 0;
    public Text waveText;
    public bool playerSpawner = true;

    public float timeBetweenWaves = 5f;
    private float countdown = 2f;

    private Enemy[] enemies;
    private int maxIterations;
    private SortedList<int, int> nextWave;

    private int waveNumber = 1;

    void Start()
    {
        enemies = enemySetPrefab.GetComponentsInChildren<Enemy>();
        maxIterations = 20;
        nextWave = new SortedList<int, int>();
        PrepareWave();
    }

    void Update()
    {
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }
        if (playerSpawner)
        {
            if (EnemiesAlive <= 0)
            {
                countdown -= Time.deltaTime;
                EnemiesAlive = 0;
            }
        } else
        {
            if (OpponentEnemiesAlive <= 0)
            {
                countdown -= Time.deltaTime;
                OpponentEnemiesAlive = 0;
            }
        }
    }

    void PrepareWave()
    {
        
        int currentValue = baseValue + (valuePerWave * waveNumber);
        System.Random random = new System.Random();
        int index = 0;
        int enemyValue = 0;
        int iterations = 0;
        while (currentValue > 0)
        {
            index = random.Next(0, enemies.Length);
            enemyValue = enemies[index].unit.baseValue;
            if (enemyValue <= baseValue + (valuePerWave * waveNumber))
            {
                currentValue -= enemyValue;
                if (nextWave.ContainsKey(index))
                {
                    nextWave[index]++;
                } else
                {
                    nextWave.Add(index, 1);
                }
            }
            else
            {
                iterations++;
                if (iterations > maxIterations)
                {
                    break;
                }
            }
        }
        UpdateWaveText();
    }

    void UpdateWaveText()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("Next Wave");
        string name;
        foreach (var pair in nextWave)
        {
            name = enemies[pair.Key].unit.name;
            sb.AppendLine(name + " \t" + pair.Value);
        }
        waveText.text = sb.ToString();
    }

    IEnumerator SpawnWave()
    {
        //Debug.Log("Wave Incoming: " + waveNumber);
        foreach (var pair in nextWave)
        {
            for (int i = 0; i < pair.Value; i++)
            {
                SpawnEnemy(pair.Key);
                yield return new WaitForSeconds(enemyDelay);
            }
        }
        waveNumber++;
        nextWave.Clear();
        PrepareWave();
    }

    void SpawnEnemy(int index)
    {
        Enemy enemy = Instantiate(enemies[index], spawnPoint.position, spawnPoint.rotation).GetComponent<Enemy>();
        if (playerSpawner)
        {
            EnemiesAlive++;
        } else
        {
            OpponentEnemiesAlive++;
        }
        enemy.PartOfWave = true;
        enemy.PlayerOwned = playerSpawner;
    }
}
