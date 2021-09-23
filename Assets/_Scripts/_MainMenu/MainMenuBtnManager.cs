using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuBtnManager : MonoBehaviour
{
    public Database dataBase;
    public GameObject startBtnsHolder;
    public GameObject loginHolder;
    public List<TMP_InputField> loginInputFields;
    public GameObject createAccountHolder;
    public List<TMP_InputField> createAccountInputFields;

    // Start is called before the first frame update
    void Start()
    {
        startBtnsHolder.SetActive(true);
        loginHolder.SetActive(false);
        createAccountHolder.SetActive(false);
    }

    public void ToMenu()
    {
        startBtnsHolder.SetActive(true);
        loginHolder.SetActive(false);
        createAccountHolder.SetActive(false);
    }

    public void ToLogin()
    {
        startBtnsHolder.SetActive(false);
        loginHolder.SetActive(true);
        createAccountHolder.SetActive(false);

        dataBase.usernameField = loginInputFields[0];
        dataBase.passwordField = loginInputFields[1];
    }

    public void ToCreateAccount()
    {
        startBtnsHolder.SetActive(false);
        loginHolder.SetActive(false);
        createAccountHolder.SetActive(true);

        dataBase.usernameField = createAccountInputFields[0];
        dataBase.passwordField = createAccountInputFields[1];
    }
}
