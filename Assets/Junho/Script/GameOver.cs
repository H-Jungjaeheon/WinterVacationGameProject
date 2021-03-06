using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public Image fadePlan;
    public void OnGameOver()
    {
        StartCoroutine(Fade(Color.clear, Color.black, 1.5f));
        SceneManager.LoadScene("GameOver");
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
    
}
