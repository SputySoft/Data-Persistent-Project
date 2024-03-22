using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class UI_MenuHandler : MonoBehaviour
{
    public TMP_Text BestPlayer;
    Dictionary<string, int> ranking = new Dictionary<string, int>();

    //Update the score
    public void Start()
    {
        UpdateScoreBoard();
    }

    public void StartGame()
    {

        SceneManager.LoadScene(1);
    }

    public void SetPlayerName(string input)
    {

        DataManager.instance.playerName = input;

        //string dataManager = DataManager.instance.playerName;

        //Debug.Log(dataManager);
    }

    private void UpdateScoreBoard ()
    {
        ranking.Clear();
        ranking = DataManager.instance.scoreBoard;

        string scoreText = "Top players:\n";

        var topThreeScores = ranking.OrderByDescending(pair=> pair.Value).Take(3).ToList();
        foreach (var score in topThreeScores)
        {
            scoreText+= $"{score.Key}: {score.Value}\n";
        }
        BestPlayer.text = scoreText;
    }

}
