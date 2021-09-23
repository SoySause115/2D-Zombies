using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffImage : MonoBehaviour
{
    public GameObject btnHolder;
    int totalBtns;
    List<GameObject> btns = new List<GameObject>();

    private void Start()
    {
        totalBtns = btnHolder.transform.childCount;
        for (int i = 0; i < totalBtns; i++)
        {
            btns.Add(btnHolder.transform.GetChild(i).gameObject);
        }
    }

    public void TurnOffOtherImages(GameObject ignoredObject)
    {
        for (int i = 0; i < btns.Count; i++)
        {
            if (btns[i].gameObject != ignoredObject)
            {
                btns[i].GetComponent<HoverOverBtn>().childImg.SetActive(false);
            }
        }
    }

    public void TurnOffAllImages()
    {
        for (int i = 0; i < btns.Count; i++)
        {
            btns[i].GetComponent<HoverOverBtn>().childImg.SetActive(false);
        }
    }
}
