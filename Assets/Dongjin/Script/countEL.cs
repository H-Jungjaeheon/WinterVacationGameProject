using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class countEL : MonoBehaviour
{
    [SerializeField] float times;
    private GameObject E;
    void Start()
    {
        StartCoroutine("ElectricityOnOff");
    }
    IEnumerator ElectricityOnOff()
    {
        for(int i = 0;i<this.transform.childCount;i++)
        {
            this.transform.GetChild(i).GetComponent<Electricityscript>().StartCoroutine("ElectricityOnOff");
            yield return new WaitForSeconds(1.5f);
        }
        yield return new WaitForSeconds(times);
        StartCoroutine("ElectricityOnOff");
        yield return null;
    }    
}
