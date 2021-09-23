using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrailDestroy : MonoBehaviour
{
    Vector3 direction;
    Vector3 playerPos;
    float distance = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = playerPos;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * (Time.deltaTime * 70);

        if (Vector3.Distance(transform.position, playerPos) >= distance)
        {
            Destroy(gameObject);
        }
    }

    public void GetVariables(Vector3 dir, Vector3 playerPos, float distance)
    {
        direction = dir;
        direction.z = 0;
        this.playerPos = playerPos;
        this.distance = distance;

        transform.up = dir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Wall")
        {
            Debug.Log("got here");
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Interactable")
        {
            BoxCollider2D[] colliders = collision.gameObject.GetComponents<BoxCollider2D>();
            if (GetComponent<BoxCollider2D>().IsTouching(colliders[0]))
            {
                Debug.Log("got here");
                Destroy(gameObject);
            }
        }
    }
}
