using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int[] stats = {1,1}; //체력 / 공격력
    private float exp, MaxExp;
    public int stateup;
    private void Update()
    {
        if(exp >= 100)
        {
            exp -= 100;
            stateup += 1;
        }
    }
    public void HpUpgrade()
    {
        if(stateup != 0)
       {
            stats[0] += 1;
            stateup--;
        }
    }
    public void DmgUpgrade()
    {
        if (stateup != 0)
        {
            stats[1] += 1;
            stateup--;
        }
    }
    public void ExpUp(float expidx)
    {
        exp += expidx;
    }
}
