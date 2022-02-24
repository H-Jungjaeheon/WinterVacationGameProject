using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerStats : MonoBehaviour
{
    public int[] stats = {1, 1, 1}; //체력 / 공격력 / 마나
    [SerializeField] private Slider ExpBar;
    [SerializeField] float Exp, MaxExp;
    public float LV;
    [SerializeField] Text expText;
    public bool Stateup = false;
    public GameObject statStartPos, statEndPos;
    public Text LvText;
    private void Start()
    {
        LV = 1;
        LvText.text = LV+"";
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
            //LvText.text = ""+LV;
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
            GameManager.Instance.StatUp.transform.DOMove(statStartPos.transform.position, 1f).SetEase(Ease.OutQuad);
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
            GameManager.Instance.StatUp.transform.DOMove(statStartPos.transform.position, 1f).SetEase(Ease.OutQuad);
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
            GameManager.Instance.StatUp.transform.DOMove(statStartPos.transform.position, 1f).SetEase(Ease.OutQuad);
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
        GameManager.Instance.StatUp.transform.DOMove(statEndPos.transform.position, 1f).SetEase(Ease.OutQuad);
    }
}
