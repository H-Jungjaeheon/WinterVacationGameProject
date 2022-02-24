using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Image fadePlan;
    public GameObject gameOverUI;
    // Start is called before the first frame update
    void Start()
    {

        
    }
    public void OnGameOver()
    {
        StartCoroutine(Fade(Color.clear, Color.black, 1));
        gameOverUI.SetActive(true);
    }
    IEnumerator Fade(Color from, Color to, float time)
    {
        float speed = 1 / time;
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime*speed;
            fadePlan.color = Color.Lerp(from,to,percent);
            yield return null;
        }
    }
    public void StartNewGame()
    {
        GameManager.Instance.isGameOver = false;
        SceneManager.LoadScene("Main1");
    }
    public void Title()
    {
        GameManager.Instance.isGameOver = false;
        SceneManager.LoadScene("Title");
    }
}
