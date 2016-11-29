using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScreenController : MonoBehaviour
{
    public Text highScoreText;

    private DataController dataController;

    void Start()
    {
        int hsScore;
        float hsTime;
        string hsName;

        highScoreText.text = "";

        if (PlayerPrefs.HasKey("HSName"))
        {
            hsScore = PlayerPrefs.GetInt("HSScore");
            hsTime = PlayerPrefs.GetFloat("HSTime");
            hsName = PlayerPrefs.GetString("HSName");
            if(hsName.Length > 0)
                highScoreText.text = hsName + " got " + hsScore/10 + " correct in " + hsTime + " secs";
        }
    }

    public void StartGame()
    {
        dataController = FindObjectOfType<DataController>();
        dataController.Shuffle();
        SceneManager.LoadScene("MainScene");
    }
}
