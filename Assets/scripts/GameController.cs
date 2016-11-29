using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

    public Text questionText;
    public Text scoreDisplayText;
    public SimpleObjectPool answerButtonObjectPool;
    public Transform answerButtonParent;
    public GameObject questionPanel;
    public GameObject roundEndPanel;
    public GameObject highScorePanel;
    public Text timeRemainingDisplayText;
    public Text roundEndText;
    public InputField highScoreName;
    public Button okButton;
    public Button tryAgainButton;

    private DataController dataController;
    private RoundData currentRoundData;
    private QuestionData[] questionPool;

    private bool isRoundActive;
    private float timeRemaining;
    private int questionIndex;

    private int playerScore;
    private float playerTime;
    private List<GameObject> answerButtonGameObjects = new List<GameObject>();


	// Use this for initialization
	void Start ()
    {
        dataController = FindObjectOfType<DataController>();
        currentRoundData = dataController.GetCurrentRoundData();
        questionPool = currentRoundData.questions;
        timeRemaining = currentRoundData.timeLimitInSeconds;
        UpdateTimeRemainingDisplay();

        playerScore = 0;
        playerTime = timeRemaining;
        questionIndex = 0;

        ShowQuestion();
        isRoundActive = true;

        highScorePanel.SetActive(false);

	}
	
    private void ShowQuestion()
    {
        RemoveAnswerButtons();
        QuestionData questionData = questionPool[questionIndex];
        questionText.text = questionData.questionText;

        for (int i = 0; i < questionData.answers.Length;i++)
        {
            GameObject answerButtonGameObject = answerButtonObjectPool.GetObject();
            answerButtonGameObjects.Add(answerButtonGameObject);
            answerButtonGameObject.transform.SetParent(answerButtonParent);
            
            AnswerButton answerButton = answerButtonGameObject.GetComponent<AnswerButton>();
            answerButton.Setup(questionData.answers[i]);
        }
    }

    private void RemoveAnswerButtons()
    {
        while(answerButtonGameObjects.Count > 0)
        {
            answerButtonObjectPool.ReturnObject(answerButtonGameObjects[0]);
            answerButtonGameObjects.RemoveAt(0);
        }
    }

    private void UpdateTimeRemainingDisplay()
    {
        timeRemainingDisplayText.text = "Time: " + Mathf.Round(timeRemaining).ToString();

    }

    private bool CheckHighScore()
    {
        int hsScore;
        float hsTime;
        
        playerTime = 90 - Mathf.Round(timeRemaining);

        if (PlayerPrefs.HasKey("HSName"))
        {
            hsScore = PlayerPrefs.GetInt("HSScore");
            hsTime = PlayerPrefs.GetFloat("HSTime");
        
            print("hscore: " + hsScore);
            print("playerScore: " + playerScore);

            print("hstime: " + hsTime);
            print("playertime: " + playerTime);

            if (playerScore > hsScore)
                return true;
            else if(playerScore == hsScore && playerTime < hsTime)
                    return true;
        }
        else
        {
            return true;
        }
        return false;
    }

    public void EndRound()
    {
        isRoundActive = false;
        questionPanel.SetActive(false);

        if(CheckHighScore())
        {
            highScorePanel.SetActive(true);
            tryAgainButton.gameObject.SetActive(false);

        }

        roundEndText.text = "You got " + playerScore/10 + " correct in " + playerTime + " secs";
        roundEndPanel.SetActive(true);

    }

    public void AnswerButtonClicked(bool isCorrect)
    {
        if(isCorrect)
        {
            playerScore += currentRoundData.pointsAddedForCorrectAnswer;
            scoreDisplayText.text = "Score: " + playerScore.ToString();

        }

        if(questionPool.Length > questionIndex + 1)
        {
            questionIndex++;
            ShowQuestion();
        }
        else
        {
            EndRound();
        }
    }

    public void ResetHighScore()
    {
        PlayerPrefs.SetInt("HSScore", 0);
        PlayerPrefs.SetFloat("HSTime", 0);
        PlayerPrefs.SetString("HSName", "");

        PlayerPrefs.Save();

    }

    public void SaveHighScore()
    {
        PlayerPrefs.SetInt("HSScore", playerScore);
        PlayerPrefs.SetFloat("HSTime", Mathf.Round(90 - timeRemaining));
        PlayerPrefs.SetString("HSName", highScoreName.text);

        PlayerPrefs.Save();

        ReturnMenu();       
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene("MenuScreen");
    }

	// Update is called once per frame
	void Update ()
    {
	    if(isRoundActive)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimeRemainingDisplay();

            if(timeRemaining <= 0f)
            {
                EndRound();
            }
        }
        else
        {
            if (highScoreName.text.Length < 1)
                okButton.enabled = false;
            else
                okButton.enabled = true;
        }
	}
}
