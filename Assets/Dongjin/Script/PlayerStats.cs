using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int[] stats = { 1, 1, 1}; //ü�� / ���ݷ� / ����
    [SerializeField] private Slider ExpBar;
    [SerializeField] float Exp, MaxExp, LV;
    [SerializeField] Text expText;
    public bool Stateup = false;
    private void Start()
    {
        LV = 1;
        Exp = 0;
        MaxExp = 100;
    }
    private void Update()
    {
        HandleSlider();
        StatsSetting();
        if (Exp >= MaxExp)
        {
            GameManager.Instance.LevelUp = true;
            LV += 1;
            Exp = 0;
            MaxExp += 20;
            Stateup = true;
            Invoke("StatUp", 3f);
        }
    }
    private void StatsSetting()
    {
        for(int i =0;i<stats.Length;i++)
        {
            GameObject.Find("stats").transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = "" + stats[i];
        }
        GameObject.Find("stats").transform.GetChild(3).transform.GetChild(0).GetComponent<Text>().text = "" + LV;
    }
    public void HpUpgrade()
    {
        if(Stateup == true)
        {
            stats[0] += 1;
            Stateup = false;
            GameManager.Instance.StatUp.SetActive(false);
            GameManager.Instance.LevelUp = false;
            GameManager.Instance.maxHp += 10;
        }
    }
    public void DmgUpgrade()
    {
        if (Stateup == true)
        {
            stats[1] += 1;
            Stateup = false;
            GameManager.Instance.StatUp.SetActive(false);
            GameManager.Instance.LevelUp = false;
        }
    }
    private void HandleSlider()
    {
        ExpBar.value = Exp / MaxExp;
        expText.text = Exp + "/" + MaxExp;
    }
    public void ManaUpgrade()
    {
        if (Stateup == true)
        {
            stats[2] += 1;
            Stateup = false;
            GameManager.Instance.StatUp.SetActive(false);
            GameManager.Instance.LevelUp = false;
            GameManager.Instance.maxMana += 10;
        }
    }
    public void ExpUp(float expidx)
    {
        Exp += expidx;
    }
    void StatUp()
    {
        GameManager.Instance.StatUp.SetActive(true);
    }
}
