using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DataController : MonoBehaviour
{

    public RoundData[] allRoundData;

    public void Shuffle()
    {
        for (int i = 0; i < allRoundData.Length; i++)
        {
            ShuffleQuestions(allRoundData[i].questions);
        }
    }


    // Use this for initialization
    void Start ()
    {
        DontDestroyOnLoad(gameObject);
     
        SceneManager.LoadScene("MenuScreen");
    }
	
    // Update is called once per frame
    void Update ()
    {
	
    }

    public RoundData GetCurrentRoundData()
    {
        return allRoundData[0];
    }

    private void ShuffleQuestions(QuestionData[] q)
    {
        for(int n = 0; n < q.Length; n++)
        {
            QuestionData tmp = q[n];
            int r = Random.Range(n, q.Length);
            q[n] = q[r];
            q[r] = tmp;
        }
    }

    
}
