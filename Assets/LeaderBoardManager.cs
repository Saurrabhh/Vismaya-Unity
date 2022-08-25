using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System.Linq;
using TMPro;
public class LeaderBoardManager : MonoBehaviour
{

    public DatabaseReference database;

    public Transform leaderBoardContent;
    public GameObject leaderBoard;
    //public TextMeshProUGUI nameText;
    //public TextMeshProUGUI moneyText;

    // Start is called before the first frame update
    void Start()
    {
        database = FirebaseManager.database;
        StartCoroutine(LoadLeaderBoardData());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LeaderBoardButton()
    {
        StartCoroutine(LoadLeaderBoardData());
    }

    //public void LeaderBoardElements(string _name, int _money)
    //{
        //nameText.text = _name;
        ///moneyText.text = _money.ToString();
    //}

    private IEnumerator LoadLeaderBoardData()
    {
        while (true)
        {
            var DbTask = database.Child("Users").OrderByChild("expPoints").GetValueAsync();
            yield return new WaitUntil(predicate: () => DbTask.IsCompleted);

            if (DbTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DbTask.Exception}");
            }

            else
            {
                DataSnapshot snapshot = DbTask.Result;  //retrieve data

                //destroy any existing leaderboard element
                foreach (Transform child in leaderBoardContent.transform)
                {
                    Destroy(child.gameObject);
                }


                //loop through every user ids
                foreach (DataSnapshot dataSnapshot in snapshot.Children.Reverse<DataSnapshot>())
                {
                    string username = dataSnapshot.Child("name").Value.ToString();
                    int money = int.Parse(dataSnapshot.Child("expPoints").Value.ToString());
                    Debug.Log(username);
                    //instantiate new scoreboard element
                    GameObject leaderBoardElement = Instantiate(leaderBoard);

                    leaderBoardElement.transform.SetParent(leaderBoardContent.transform);
                    TextMeshProUGUI[] list = leaderBoardElement.GetComponentsInChildren<TextMeshProUGUI>();
                    list[0].GetComponent<TextMeshProUGUI>().text = username;
                    list[1].GetComponent<TextMeshProUGUI>().text = money.ToString();
                    leaderBoardElement.SetActive(true);
                    //leaderBoardElement.GetComponent<LeaderBoardManager>().LeaderBoardElements(username, money);
                    //leaderBoardElement.GetComponent<LeaderBoardManager>().LeaderBoardElements(username, money);
                }


            }

            yield return new WaitForSeconds(5);

        }

    }

}
