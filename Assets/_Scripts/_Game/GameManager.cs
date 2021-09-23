using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject enemyObj;
    public List<GameObject> spawnLoc;
    [HideInInspector]
    public List<GameObject> enemies;
    [HideInInspector]
    public List<float> health;
    int maxHealth = 0;
    int maxEnemies = 0;
    bool isSpawningWave = false;
    //int maxEnemiesOnScreen = 20;

    int wave = 0;
    public TextMeshProUGUI waveTxt;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        wave++;
        waveTxt.text = "Wave: " + wave;
        isSpawningWave = true;
        maxHealth += 4;
        maxEnemies += 5;
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < maxEnemies; i++)
        {
            GameObject instEnemy = Instantiate(enemyObj, spawnLoc[Random.Range(0, spawnLoc.Count)].transform.position, Quaternion.identity);
            enemies.Add(instEnemy);
            health.Add(maxHealth);
            yield return new WaitForSeconds(0.8f);
        }
        isSpawningWave = false;
    }

    // Update is called once per frame
    void Update()
    {
        //detecting enemy death
        for (int i = 0; i < enemies.Count; i++)
        {
            if (health[i] <= 0)
            {
                Destroy(enemies[i]);
                enemies.RemoveAt(i);
                health.RemoveAt(i);
            }
        }

        //next wave
        if (enemies.Count == 0 && !isSpawningWave)
        {
            StartCoroutine(SpawnWave());
        }

        if (player.GetComponent<Player>().health <= 0)
        {
            GameObject.Find("Database").GetComponent<Database>().UpdateExp(player.GetComponent<Player>().expGained);
            SceneManager.LoadScene(1);
        }
    }
}
