using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
public class SceneChange : MonoBehaviour
{
    public Text exText;
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Awake()
    { 
        text = GetComponent<Text>();
    }
    public void StartBtn()
    {
        SceneManager.LoadScene("Main1");
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
        exText.GetComponent<Text>().DOFade(1, 1);
        yield return new WaitForSeconds(3f);
        exText.GetComponent<Text>().DOFade(0, 1);

    }
}
