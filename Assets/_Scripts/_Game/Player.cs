using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public GameObject healthTxt;
    [HideInInspector]
    public int health = 2;
    float decreaseHealthTimer;
    float invincibilityTimer = 2f;

    public int expGained = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthTxt.GetComponent<TextMeshProUGUI>().text = "Health: " + health;

        if (decreaseHealthTimer > 0)
        {
            decreaseHealthTimer -= Time.deltaTime;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (decreaseHealthTimer <= 0)
            {
                health--;
                decreaseHealthTimer = invincibilityTimer;
            }
        }
    }
}
