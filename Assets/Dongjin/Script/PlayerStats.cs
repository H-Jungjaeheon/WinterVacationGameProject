using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private int[] stats = {1,1};
    public int stateup;
    public void HpUpgrade()
    {
        if(stateup != 0)
       {
            stats[0] += 1;
        }
    }
    public void DmgUpgrade()
    {
        if (stateup != 0)
        {
            stats[1] += 1;
        }
    }
}
