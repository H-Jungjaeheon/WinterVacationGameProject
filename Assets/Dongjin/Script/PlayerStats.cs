using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int[] stats = { 1, 1, 1}; //체력 / 공격력 / 마나
    [SerializeField] float Exp, MaxExp, LV;
    public bool Stateup = false;
    private void Start()
    {
        LV = 1;
        Exp = 0;
        MaxExp = 100;
    }
    private void Update()
    {
        if(Exp >= MaxExp)
        {
            GameManager.Instance.LevelUp = true;
            LV += 1;
            Exp = 0;
            MaxExp += 20;
            Stateup = true;
            Invoke("StatUp", 3f);
        }
    }
    public void HpUpgrade()
    {
        if(Stateup == true)
        {
            stats[0] += 1;
            Stateup = false;
            GameManager.Instance.StatUp.SetActive(false);
            GameManager.Instance.LevelUp = false;
        }
    }
    public void DmgUpgrade()
    {
        if (Stateup == true)
        {
            stats[1] += 2;
            Stateup = false;
            GameManager.Instance.StatUp.SetActive(false);
            GameManager.Instance.LevelUp = false;
        }
    }
    public void ManaUpgrade()
    {
        if (Stateup == true)
        {
            stats[2] += 10;
            Stateup = false;
            GameManager.Instance.StatUp.SetActive(false);
            GameManager.Instance.LevelUp = false;
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
