using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electricityscript : MonoBehaviour
{
    private GameObject E0;
    private GameObject E1;
    [SerializeField] float times;
    [SerializeField] bool countELstart;
    private void Start()
    {
        E0 = this.transform.GetChild(0).gameObject;
        E1 = this.transform.GetChild(1).gameObject;
        if(countELstart != true)
            StartCoroutine("ElectricityOnOff");
    }
    IEnumerator ElectricityOnOff()
    {
        E0.gameObject.SetActive(false);
        E1.gameObject.SetActive(false);
        yield return new WaitForSeconds(times);
        E0.gameObject.SetActive(true);
        yield return new WaitForSeconds(times);
        E0.gameObject.SetActive(false);
        E1.gameObject.SetActive(true);
        yield return new WaitForSeconds(times);
        E0.gameObject.SetActive(false);
        E1.gameObject.SetActive(false);
        if (countELstart != true)
            StartCoroutine("ElectricityOnOff");
        yield return null;
    }
}
