using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void StartBtn()
    {
        SceneManager.LoadScene("Main");
    }

    public void SettingBtn()
    {

    }
    public void ExBtn()
    {

    }
    public void QuitBtn()
    {
        Application.Quit();
    }
    public void TitleBtn()
    {
        SceneManager.LoadScene("Title");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
