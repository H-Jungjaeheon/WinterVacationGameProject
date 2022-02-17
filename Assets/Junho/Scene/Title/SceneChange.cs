using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
public class SceneChange : MonoBehaviour
{
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    { 
        text = GetComponent<Text>();
        GameObject.Find("ExText").GetComponent<Text>().color = new Color(text.color.r, text.color.b, text.color.g, 0);
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
        StartCoroutine(Ex());
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
    IEnumerator Ex()
    {
        GameObject.Find("ExText").GetComponent<Text>().DOFade(1, 1);
        yield return new WaitForSeconds(3f);
        GameObject.Find("ExText").GetComponent<Text>().DOFade(0, 1);

    }
}
