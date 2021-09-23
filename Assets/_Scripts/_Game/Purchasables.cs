using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Purchasables : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject gameManager;
    public GameObject player;
    public int pointsToBuy;
    bool canBuy = false;
    bool displayAlternateText = false;

    enum PurchasableType
    {
        Wallbuy,
        Door,
        MysteryBox
    };
    [SerializeField]
    PurchasableType purchasableType;

    [Tooltip("Only set if purchasableType is WallBuy (reads fist entry) or MysteryBox")]
    public List<string> weapon;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        //if the player has walked up to the purchasable
        if (canBuy)
        {
            //displays if the "Not enough points!" text is not showing
            if (!displayAlternateText)
            {
                if (purchasableType == PurchasableType.Wallbuy)
                {
                    text.text = weapon[0] + " - " + pointsToBuy + " points\nPress E to buy!";
                }
                else if (purchasableType == PurchasableType.Door)
                {
                    text.text = "Door - " + pointsToBuy + " points\nPress E to buy!";
                }
                else if (purchasableType == PurchasableType.MysteryBox)
                {
                    text.text = "Mystery Box - " + pointsToBuy + " points\nPress E to buy!";
                }
            }

            //if the player presses E and has enough points to buy
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (gameManager.GetComponent<Scores>().score >= pointsToBuy)
                {
                    //if the type is a wallbuy
                    if (purchasableType == PurchasableType.Wallbuy)
                    {
                        gameManager.GetComponent<Scores>().score -= pointsToBuy;
                        player.GetComponent<Shooting>().SetCurrentWeapon(weapon[0]);
                    }
                    else if (purchasableType == PurchasableType.Door)
                    {
                        gameManager.GetComponent<Scores>().score -= pointsToBuy;
                        Destroy(gameObject);
                    }
                    else if (purchasableType == PurchasableType.MysteryBox)
                    {
                        gameManager.GetComponent<Scores>().score -= pointsToBuy;
                        player.GetComponent<Shooting>().SetCurrentWeapon(weapon[Random.Range(0, weapon.Count)]);
                    }
                    else
                    {
                        Debug.Log("Something went wrong buying!");
                    }
                }
                else
                {
                    displayAlternateText = true;
                    StartCoroutine(NotEnoughPointsDisplay());
                }
            }
        }
    }

    //if the player doesn't have enough points, display this
    IEnumerator NotEnoughPointsDisplay()
    {
        text.text = "Not enough points!";
        yield return new WaitForSeconds(1f);
        displayAlternateText = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            canBuy = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (displayAlternateText)
            {
                StopCoroutine(NotEnoughPointsDisplay());
            }
            canBuy = false;
            text.text = "";
        }
    }
}
