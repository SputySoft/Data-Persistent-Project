using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public string playerName;

    public int score;

    public Dictionary<string, int> scoreBoard = new Dictionary<string, int>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);

        LoadScoresFromJson();


    }

    //add new scores (called in game over MainManager)

    public void AddOrUpdateScore(string playerName, int score)
    {
        // does player exist and is the score superior ?
        if (scoreBoard.ContainsKey(playerName) && scoreBoard[playerName] < score)
        {
            scoreBoard[playerName] = score;

        }
        else
        {
            scoreBoard.Add(playerName, score);
        }
    }

    [System.Serializable]
    public class ScoreEntry
    {
        public string playerName;
        public int score;

        public ScoreEntry(string name, int score)
        {
            this.playerName = name;
            this.score = score;
        }
    }

    [System.Serializable]
    public class ScoreList
    {
        public List<ScoreEntry> scores = new List<ScoreEntry>();
    }

    public void SaveScoresToJson()
    {
        ScoreList scoreList = new ScoreList();


        // just keep the ten first entries
        var topTenScores = scoreBoard.OrderByDescending(pair => pair.Value)
                             .Take(10)
                             .ToDictionary(pair => pair.Key, pair => pair.Value);
        scoreBoard = topTenScores;

        //construct score list from scoreboard for correct serialization in json file

        foreach (var entry in scoreBoard)
        {
            scoreList.scores.Add(new ScoreEntry(entry.Key, entry.Value));
        }

        string json = JsonUtility.ToJson(scoreList, true);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/scores.json", json);
        Debug.Log("Scores saved to " + Application.persistentDataPath + "/scores.json");
    }

    public void LoadScoresFromJson()
    {
        string path = Application.persistentDataPath + "/scores.json";
        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            ScoreList scoreList = JsonUtility.FromJson<ScoreList>(json);
            scoreBoard.Clear();
            foreach (var entry in scoreList.scores)
            {
                scoreBoard[entry.playerName] = entry.score;
            }
            Debug.Log("Scores loaded from " + path);
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
        }
    }
}


   