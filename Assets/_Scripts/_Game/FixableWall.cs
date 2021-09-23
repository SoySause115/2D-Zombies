using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FixableWall : MonoBehaviour
{
    //health of the wall
    int maxHealth = 100;
    [SerializeField]
    int health = 100;

    //timer for fixing the wall
    int timeBetweenFix = 2;
    float fixTimer = 0;

    //timer for breaking the wall
    int timeBetweenBroken = 2;
    float brokenTimer = 0;

    bool canBeFixed = false;
    bool canBeBroken = false;

    GameManager gameManager;
    BoxCollider2D insideCollider; //where players can collide to fix the wall
    BoxCollider2D outsideCollider; //where enemies can collide to break the wall

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        BoxCollider2D[] colliders = GetComponents<BoxCollider2D>();
        insideCollider = colliders[1];
        outsideCollider = colliders[2];
    }

    // Update is called once per frame
    void Update()
    {
        //timers
        if (fixTimer > 0)
        {
            fixTimer -= Time.deltaTime;
        }
        if (brokenTimer > 0)
        {
            brokenTimer -= Time.deltaTime;
        }

        //if the wall is completely broken
        if (health <= 0)
        {
            for (int i = 0; i < gameManager.enemies.Count; i++)
            {
                //allow the enemy to go through it
                Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), gameManager.enemies[i].GetComponent<CircleCollider2D>(), true);
            }
        }
        else
        {
            for (int i = 0; i < gameManager.enemies.Count; i++)
            {
                //don't allow the enemy to go through it
                Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), gameManager.enemies[i].GetComponent<CircleCollider2D>(), false);
            }
        }

        //if the wall can be fixed && user press C && fixable timer is at 0 && health isn't max
        if (canBeFixed && Input.GetKey(KeyCode.C) && fixTimer <= 0 && health < maxHealth)
        {
            health += 20;
            gameManager.GetComponent<Scores>().score += 10;
            fixTimer = timeBetweenFix;
        }

        //if an enemy is next to the wall && broken timer is at 0 && health is above 0
        if (canBeBroken && brokenTimer <= 0 && health > 0)
        {
            health -= 20;
            brokenTimer = timeBetweenBroken;
        }

        //change the alpha of the wall to indicate damage (0 - 1)
        transform.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, (float)health / maxHealth);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                if (collision.IsTouching(insideCollider))
                {
                    canBeFixed = true;
                }
                return;

            case "Enemy":
                if (collision.IsTouching(outsideCollider))
                {
                    canBeBroken = true;
                }
                return;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                canBeFixed = false;
                return;

            case "Enemy":
                canBeBroken = false;
                return;
        }        
    }
}
