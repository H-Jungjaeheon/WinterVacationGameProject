using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver2 : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneManager.LoadScene("Main1");
    }
    public void Title()
    {
        SceneManager.LoadScene("Title");
    }
}
