using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using MongoDB.Driver;
using MongoDB.Bson;

public class Database : MonoBehaviour
{
    //database information
    MongoClient client = new MongoClient("mongodb+srv://dbUser:applesauce@cluster0.jkocz.mongodb.net/UsersDB?retryWrites=true&w=majority");
    IMongoDatabase database;
    IMongoCollection<BsonDocument> loginCollection;
    IMongoCollection<BsonDocument> statsCollection;

    //input fields
    [HideInInspector]
    public TMP_InputField usernameField;
    [HideInInspector]
    public TMP_InputField passwordField;

    BsonDocument loggedInUser = new BsonDocument();

    // Start is called before the first frame update
    void Start()
    {
        database = client.GetDatabase("UsersDB");
        loginCollection = database.GetCollection<BsonDocument>("LoginCollection");
        statsCollection = database.GetCollection<BsonDocument>("StatsCollection");
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Lobby")
        {
            GameObject.Find("UsernameTxt").GetComponent<TextMeshProUGUI>().text = loggedInUser.GetElement("username").Value.ToString();
            GameObject.Find("LvlTxt").GetComponent<TextMeshProUGUI>().text = "Level: " + loggedInUser.GetElement("level").Value.ToString();
            GameObject.Find("ExpTxt").GetComponent<TextMeshProUGUI>().text = "Exp: " + loggedInUser.GetElement("exp").Value.ToString() + "/100";
        }
    }

    public async void CreateAccount()
    {
        if (!string.IsNullOrEmpty(usernameField.text) && !string.IsNullOrEmpty(passwordField.text))
        {
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("usernameCheck", usernameField.text.ToLower());
            var existingLogin = loginCollection.Find(filter).FirstOrDefault();

            if (existingLogin == null || existingLogin.GetElement("usernameCheck").Value.ToString() != usernameField.text.ToLower())
            {
                await loginCollection.InsertOneAsync(new BsonDocument
                {
                    { "usernameCheck", usernameField.text.ToLower() },
                    { "username", usernameField.text },
                    { "password", passwordField.text }
                });
                await statsCollection.InsertOneAsync(new BsonDocument {
                    { "usernameCheck", usernameField.text.ToLower() },
                    { "username", usernameField.text },
                    { "level", 1 },
                    { "exp", 0 }
                });
                ClearInputFields();
                Debug.Log("setup complete");
                return;
            }
            
            Debug.Log("username is taken");
            ClearInputFields();
        }
    }

    public async void Login()
    {
        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("usernameCheck", usernameField.text.ToLower());
        var existingLogin = loginCollection.Find(filter).FirstOrDefault();

        //if nothing in either field
        if (usernameField.text == "" || usernameField.text == "")
        {
            Debug.Log("insert all information");
            return;
        }

        if (existingLogin != null || existingLogin.GetElement("usernameCheck").Value.ToString() == usernameField.text.ToLower())
        {
            if (existingLogin.GetElement("password").Value.ToString() == passwordField.text)
            {
                Debug.Log("logged in successfully");
                loggedInUser = statsCollection.Find(filter).FirstOrDefault();
                SceneManager.LoadScene(1);
                return;
            }
        }

        Debug.Log("wrong account credidentials");
        ClearInputFields();
    }

    public async void UpdateExp(int expGained)
    {
        int totalExp = loggedInUser.GetElement("exp").Value.ToInt32() + expGained;
        int level = loggedInUser.GetElement("level").Value.ToInt32();

        while (totalExp >= 100)
        {
            totalExp -= 100;
            level += 1;
        }

        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("usernameCheck", loggedInUser.GetElement("usernameCheck").Value.ToString());

        //update the exp and level
        UpdateDefinition<BsonDocument> update = Builders<BsonDocument>.Update.Set("exp", totalExp);
        await statsCollection.UpdateOneAsync(filter, update);
        update = Builders<BsonDocument>.Update.Set("level", level);
        await statsCollection.UpdateOneAsync(filter, update);

        loggedInUser = statsCollection.Find(filter).FirstOrDefault();
    }

    private void ClearInputFields()
    {
        usernameField.text = "";
        passwordField.text = "";
    }
}
