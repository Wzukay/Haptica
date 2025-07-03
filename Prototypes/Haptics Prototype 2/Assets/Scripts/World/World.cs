using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Difficulty { Easy, Medium, Hard }

public class World : MonoBehaviour
{
    public Transform player;

    public Difficulty difficulty;
    public GameObject easyGO;
    public GameObject mediumGO;
    public GameObject hardGO;

    private Easy easy;
    private Medium medium;
    private Hard hard;

    public List<int> enemyCounts;
    public List<GameObject> enemies = new List<GameObject>();
    public List<int> spawns = new List<int>();
    public GameObject enemyPrefab;
    public GameObject projectilePrefab;
    public float timeToAttack = 2.5f;
    float currentTimer = 0;
    public Transform[] shooters;

    public TextMeshProUGUI difficultyText;

    private Difficulty currentDifficulty;
    private int difIndex = 0;

    void Awake()
    {
        easy = easyGO.GetComponent<Easy>();
        medium = mediumGO.GetComponent<Medium>();
        hard = hardGO.GetComponent<Hard>();
    }

    void Update()
    {
        currentTimer += Time.deltaTime;

        ChangeDifBasedOnIndex();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (difIndex + 1 > 2)
                difIndex = 0;
            else
                difIndex++;   
        }

        if (difficulty == Difficulty.Easy)
        {
            difficultyText.text = "Difficulty: Easy";
        }
        else if (difficulty == Difficulty.Medium)
        {
            difficultyText.text = "Difficulty: Medium";
        }
        else if (difficulty == Difficulty.Hard)
        {
            difficultyText.text = "Difficulty: Hard";
        }

        if (currentTimer > timeToAttack)
        {
            currentTimer = 0;
            Transform shooter = shooters[Random.Range(0, shooters.Length)];
            shooter.LookAt(player);

            if (projectilePrefab != null && shooter != null)
            {
                Vector3 pos = shooter.position + new Vector3(0, Random.Range(-.25f, .25f), 0);
                GameObject projectile = Instantiate(projectilePrefab, pos, shooter.rotation);
                Projectile p = projectile.GetComponent<Projectile>();
                p.thrownByPlayer = false;
                Physics.IgnoreCollision(projectile.GetComponent<Collider>(), shooter.GetComponent<Collider>());
            }
        }

        if (difficulty != currentDifficulty)
        {
            // Difficulty changed — handle transition
            SwitchDifficulty(currentDifficulty, difficulty);
            currentDifficulty = difficulty;
            spawns = new List<int>();

            for (int i = 0; i < enemies.Count; i++)
            {
                Destroy(enemies[i]);
            }
            
            enemies = new List<GameObject>();
        }
        else
        {
            for (int e = 0; e < enemies.Count; e++)
            {
                if (enemies[e] == null)
                {
                    enemies.RemoveAt(e);
                    spawns.RemoveAt(e);
                }
            }

            int maxIter = 1000;

            if (currentDifficulty == Difficulty.Easy)
            {
                if (enemies.Count == 0)
                {
                    currentTimer = 0;

                    for (int i = 0; i < enemyCounts[0]; i++)
                    {
                        int ok = 0;
                        int tries = 0;
                        int index;
                        while (true)
                        {
                            index = Random.Range(0, easy.checkpoints.Length);
                            tries++;
                            for (int j = 0; j < spawns.Count; j++)
                            {
                                if (index == spawns[j])
                                    ok = 1;
                            }

                            if (ok == 0)
                                break;

                            if (tries > maxIter)
                                break;
                        }

                        Transform pos = easy.checkpoints[index];
                        GameObject en = Instantiate(enemyPrefab, pos.position + new Vector3(0, 1, 0), pos.localRotation);
                        en.GetComponent<Enemy>().checkpoint = pos;
                        enemies.Add(en);
                        spawns.Add(index);
                    }
                }
            }
            else if (currentDifficulty == Difficulty.Medium)
            {
                if (enemies.Count == 0)
                {
                    currentTimer = 0;

                    for (int i = 0; i < enemyCounts[1]; i++)
                    {
                        int ok = 0;
                        int tries = 0;
                        int index;
                        while (true)
                        {
                            index = Random.Range(0, medium.checkpoints.Length);
                            tries++;
                            for (int j = 0; j < spawns.Count; j++)
                            {
                                if (index == spawns[j])
                                    ok = 1;
                            }

                            if (ok == 0)
                                break;

                            if (tries > maxIter)
                                break;
                        }

                        Transform pos = medium.checkpoints[index];
                        GameObject en = Instantiate(enemyPrefab, pos.position + new Vector3(0, 1, 0), Quaternion.identity);
                        en.GetComponent<Enemy>().checkpoint = pos;
                        enemies.Add(en);
                        spawns.Add(index);
                    }
                }
            }
            else if (currentDifficulty == Difficulty.Hard)
            {
                if (enemies.Count == 0)
                {
                    currentTimer = 0;

                    for (int i = 0; i < enemyCounts[2]; i++)
                    {
                        int ok = 0;
                        int tries = 0;
                        int index;
                        while (true)
                        {
                            index = Random.Range(0, hard.checkpoints.Length);
                            tries++;
                            for (int j = 0; j < spawns.Count; j++)
                            {
                                if (index == spawns[j])
                                    ok = 1;
                            }

                            if (ok == 0)
                                break;

                            if (tries > maxIter)
                                break;
                        }

                        Transform pos = hard.checkpoints[index];
                        GameObject en = Instantiate(enemyPrefab, pos.position + new Vector3(0, 1, 0), Quaternion.identity);
                        en.GetComponent<Enemy>().checkpoint = pos;
                        enemies.Add(en);
                        spawns.Add(index);
                    }
                }
            }
        }
    }

    void SwitchDifficulty(Difficulty from, Difficulty to)
    {
        if (from == Difficulty.Easy)
            easy.Disable();
        else if (from == Difficulty.Medium)
            medium.Disable();
        else if (from == Difficulty.Hard)
            hard.Disable();

        if (to == Difficulty.Easy)
            easy.enabled = true;
        else if (to == Difficulty.Medium)
            medium.enabled = true;
        else if (to == Difficulty.Hard)
            hard.enabled = true;
    }

    void ChangeDifBasedOnIndex()
    {
        if (difIndex == 0)
            difficulty = Difficulty.Easy;
        else if (difIndex == 1)
            difficulty = Difficulty.Medium;
        else if (difIndex == 2)
            difficulty = Difficulty.Hard;
    }
}