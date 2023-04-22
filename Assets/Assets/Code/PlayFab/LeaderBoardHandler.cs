using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderBoardHandler : MonoBehaviour
{
    public GameObject rowPrefab;
    public GameObject leaderBoardGameObject;

    [Header("UI Stages")]
    [SerializeField] private GameObject stageLeaderBoard;
    [SerializeField] private Transform stageTable;
    [SerializeField] private Image stageButton;

    [Header("UI Kills")]
    [SerializeField] private GameObject killsLeaderBoard;
    [SerializeField] private Transform killsTable;
    [SerializeField] private Image killsButton;

    [Header("UI Gems")]
    [SerializeField] private GameObject gemsLeaderBoard;
    [SerializeField] private Transform gemsTable;
    [SerializeField] private Image gemsButton;


    private Color leaderboardClickedColor = new Color(.5f, .5f, .84f, 1f);
    private Color leaderboardNotClicked = new Color(.5f, .5f, .84f, .35f);

    //stageLeaderboard//stageLeaderboard//stageLeaderboard//stageLeaderboard//stageLeaderboard
    public void SendStageLeaderBoard(int stage)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
             {
                 new StatisticUpdate
                 {
                     StatisticName = "StageLeaderboard",
                     Value = stage
                 }
             }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }
    //stageLeaderboard//stageLeaderboard//stageLeaderboard//stageLeaderboard//stageLeaderboard




    //KillsLeaderBoard//KillsLeaderBoard//KillsLeaderBoard//KillsLeaderBoard//KillsLeaderBoard
    public void SendKillsLeaderBoard(int kills)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
             {
                 new StatisticUpdate
                 {
                     StatisticName = "killsLeaderBoard",
                     Value = kills
                 }
             }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }
    //KillsLeaderBoard//KillsLeaderBoard//KillsLeaderBoard//KillsLeaderBoard//KillsLeaderBoard




    //gemsLeaderBoard//gemsLeaderBoard//gemsLeaderBoard//gemsLeaderBoard//gemsLeaderBoard
    public void SendGemsLeaderBoard(int gems)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
             {
                 new StatisticUpdate
                 {
                     StatisticName = "gemsLeaderBoard",
                     Value = gems
                 }
             }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }
    //gemsLeaderBoard//gemsLeaderBoard//gemsLeaderBoard//gemsLeaderBoard//gemsLeaderBoard

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successfull leaderboard sent");
    }


    void OnError(PlayFabError error)
    {
        Debug.Log("Error while logging in/creating account!");
        Debug.Log(error.GenerateErrorReport());
    }





    //getting the leaderboards

    //stageLeaderboard//stageLeaderboard//stageLeaderboard//stageLeaderboard//stageLeaderboard
    public void openStageLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "StageLeaderboard",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnStageLeaderBoardGet, OnError);
    }

    public void OnStageLeaderBoardGet(GetLeaderboardResult result)
    {
        pauseGame();
        closeAll();

        foreach(Transform item in stageTable)
        {
            Destroy(item.gameObject);
        }


        foreach (var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPrefab, stageTable);
            TextMeshProUGUI[] texts  = newGo.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = NumberAbrev.ParseDouble((double)item.Position + 1);
            texts[1].text = item.DisplayName;
            texts[2].text = NumberAbrev.ParseDouble((double)item.StatValue);      
        }

        leaderBoardGameObject.SetActive(true);
        stageLeaderBoard.SetActive(true);
        stageButton.color = leaderboardClickedColor;
    }
    //stageLeaderboard//stageLeaderboard//stageLeaderboard//stageLeaderboard//stageLeaderboard




    //KillsLeaderBoard//KillsLeaderBoard//KillsLeaderBoard//KillsLeaderBoard//KillsLeaderBoard
    public void openKillsLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "killsLeaderBoard",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnKillsLeaderBoardGet, OnError);
    }

    public void OnKillsLeaderBoardGet(GetLeaderboardResult result)
    {
        closeAll();

        foreach (Transform item in killsTable)
        {
            Destroy(item.gameObject);
        }


        foreach (var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPrefab, killsTable);
            TextMeshProUGUI[] texts = newGo.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = NumberAbrev.ParseDouble((double)item.Position + 1);
            texts[1].text = item.DisplayName;
            texts[2].text = NumberAbrev.ParseDouble((double)item.StatValue);
        }

        leaderBoardGameObject.SetActive(true);
        killsLeaderBoard.SetActive(true);
        killsButton.color = leaderboardClickedColor;
    }

    //KillsLeaderBoard//KillsLeaderBoard//KillsLeaderBoard//KillsLeaderBoard//KillsLeaderBoard



    //gemsLeaderBoard//gemsLeaderBoard//gemsLeaderBoard//gemsLeaderBoard//gemsLeaderBoard
    public void openGemsLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "gemsLeaderBoard",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnGemsLeaderBoardGet, OnError);
    }

    public void OnGemsLeaderBoardGet(GetLeaderboardResult result)
    {
        closeAll();

        foreach (Transform item in gemsTable)
        {
            Destroy(item.gameObject);
        }


        foreach (var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPrefab, gemsTable);
            TextMeshProUGUI[] texts = newGo.GetComponentsInChildren<TextMeshProUGUI>();
            texts[0].text = NumberAbrev.ParseDouble((double)item.Position + 1);
            texts[1].text = item.DisplayName;
            texts[2].text = NumberAbrev.ParseDouble((double)item.StatValue);
        }

        leaderBoardGameObject.SetActive(true);
        gemsLeaderBoard.SetActive(true);
        gemsButton.color = leaderboardClickedColor;
    }

    //gemsLeaderBoard//gemsLeaderBoard//gemsLeaderBoard//gemsLeaderBoard//gemsLeaderBoard





    public void closeAll()
    {
        stageLeaderBoard.SetActive(false);
        killsLeaderBoard.SetActive(false);
        gemsLeaderBoard.SetActive(false);

        stageButton.color = leaderboardNotClicked;
        killsButton.color = leaderboardNotClicked;
        gemsButton.color = leaderboardNotClicked;
    }


    public void exit()
    {
        resumeGame();
        closeAll();
        leaderBoardGameObject.SetActive(false);
    }


    void pauseGame()
    {
        Time.timeScale = 0;
    }
    void resumeGame()
    {
        Time.timeScale = 1;
    }
}
