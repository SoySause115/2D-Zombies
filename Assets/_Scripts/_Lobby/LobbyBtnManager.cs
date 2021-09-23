using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyBtnManager : MonoBehaviour
{
    public GameObject selectGameBtns;
    public GameObject mapSelections;

    private void Start()
    {
        selectGameBtns.SetActive(true);
        mapSelections.SetActive(false);
    }

    public void ToMapSelection()
    {
        selectGameBtns.SetActive(false);
        mapSelections.SetActive(true);
    }

    public void ToSelectGameBtns()
    {
        GameObject.Find("HoverManager").GetComponent<TurnOffImage>().TurnOffAllImages();
        selectGameBtns.SetActive(true);
        mapSelections.SetActive(false);
    }

    public void ToMap1()
    {
        SceneManager.LoadScene(2);
    }
}
