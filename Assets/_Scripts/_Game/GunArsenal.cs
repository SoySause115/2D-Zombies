using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun
{
    //variables
    float rateOfFire;
    public float fireTimer = 0;
    float bulletTravelDistance;
    int bullets;
    float bulletSpread;
    int damage;

    //constructor
    public Gun(float rateOfFire, float bulletTravelDistance, int bullets, float bulletSpread, int damage)
    {
        this.rateOfFire = rateOfFire;
        this.bulletTravelDistance = bulletTravelDistance;
        this.bullets = bullets;
        this.bulletSpread = bulletSpread;
        this.damage = damage;
    }

    //methods
    public void ResetTimer()
    {
        fireTimer = rateOfFire;
    }

    //sets and gets
    public float BulletTravelDistance
    {
        get => bulletTravelDistance;
    }

    public int NumOfBullets
    {
        get => bullets;
    }

    public float BulletSpread
    {
        get => bulletSpread;
    }

    public float Damage
    {
        get => damage;
    }
}

public class GunArsenal : MonoBehaviour
{
    public GameObject bulletTrail;
    [HideInInspector]
    public Dictionary<string, Gun> arsenal2 = new Dictionary<string, Gun> {
        { "pistol", new Gun(0.5f, 10f, 1, 5f, 1) },
        { "shotgun", new Gun(1f, 5f, 3, 100f, 5) },
        { "machinegun", new Gun(0.15f, 7f, 1, 20f, 3) }
    };

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<RaycastHit2D> GunShots(Vector3 origin, Gun selectedGun)
    {
        List<RaycastHit2D> shots = new List<RaycastHit2D>();
        //mouse position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        //for all of the bullets the gun has selected
        for (int i = 0; i < selectedGun.NumOfBullets; i++)
        {
            //offset from the center so that the bullet doesn't always go in a straight line
            float offset = Random.Range(-selectedGun.BulletSpread, selectedGun.BulletSpread + 1) * Mathf.Deg2Rad;
            //direction of the ray
            Vector3 dir = transform.TransformDirection(mousePosition - origin + new Vector3(offset, offset, 0)).normalized;
            shots.Add(Physics2D.Raycast(origin, dir, selectedGun.BulletTravelDistance));
            Debug.DrawRay(origin, dir * selectedGun.BulletTravelDistance, Color.red, 0.1f);

            GameObject bulletTrailTemp = Instantiate(bulletTrail);
            bulletTrailTemp.GetComponent<BulletTrailDestroy>().GetVariables(dir, origin, selectedGun.BulletTravelDistance);
        }

        return shots;
    }
}
