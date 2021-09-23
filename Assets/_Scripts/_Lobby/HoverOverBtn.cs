using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverOverBtn : MonoBehaviour, IPointerEnterHandler
{
    public GameObject hoverManager;
    public GameObject childImg;

    private void Start()
    {
        childImg.transform.localPosition = new Vector3(180, 35, 0);
        childImg.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        childImg.SetActive(true);
        hoverManager.GetComponent<TurnOffImage>().TurnOffOtherImages(gameObject);
    }
}
