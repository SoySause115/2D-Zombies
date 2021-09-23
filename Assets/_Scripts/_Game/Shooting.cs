using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class Shooting : MonoBehaviour
{
    Gun currentWeapon;
    public GameObject gameManager;
    public GameObject playerCanvas;
    public GameObject expGainedAnim;
    public GameObject bulletTrail;

    public string GetCurrentWeapon()
    {
        //reverse dictionary lookup (input value, get key)
        return gameManager.GetComponent<GunArsenal>().arsenal2.FirstOrDefault(x => x.Value == currentWeapon).Key;
    }

    public void SetCurrentWeapon(string weapon)
    {
        currentWeapon = gameManager.GetComponent<GunArsenal>().arsenal2[weapon];
    }

    // Start is called before the first frame update
    void Start()
    {
        SetCurrentWeapon("pistol");
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWeapon.fireTimer > 0)
        {
            currentWeapon.fireTimer -= Time.deltaTime;
        }

        if (Input.GetMouseButton(0) && currentWeapon.fireTimer <= 0)
        {
            //list of bullets coming from the gun
            List<RaycastHit2D> hit = new List<RaycastHit2D>(gameManager.GetComponent<GunArsenal>().GunShots(transform.position, currentWeapon));

            //goes through every ray
            for (int i = 0; i < hit.Count; i++)
            {
                //if the collider hits something
                if (hit[i].collider != null)
                {
                    if (hit[i].collider.tag == "Enemy")
                    {
                        bool enemyDead = false;
                        for (int j = 0; j < gameManager.GetComponent<GameManager>().enemies.Count; j++)
                        {
                            if (hit[i].collider.gameObject == gameManager.GetComponent<GameManager>().enemies[j])
                            {
                                gameManager.GetComponent<GameManager>().health[j] -= currentWeapon.Damage;
                                if (gameManager.GetComponent<GameManager>().health[j] <= 0)
                                {
                                    gameManager.GetComponent<Scores>().score += 100;
                                    GetComponent<Player>().expGained += 115;
                                    expGainedAnim.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = "+115 Exp!";
                                    Instantiate(expGainedAnim, playerCanvas.transform);
                                    enemyDead = true;
                                    break;
                                }
                                else
                                {
                                    gameManager.GetComponent<Scores>().score += 10;
                                }
                            }
                        }
                        if (enemyDead)
                        {
                            break;
                        }
                    }
                }
            }

            //rate of fire
            currentWeapon.ResetTimer();
        }
    }
}
