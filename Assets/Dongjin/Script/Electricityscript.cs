using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electricityscript : MonoBehaviour
{
    private GameObject E0;
    private GameObject E1;
    [SerializeField] float times;
    private void Start()
    {
        E0 = this.transform.GetChild(0).gameObject;
        E1 = E0.transform.GetChild(0).gameObject;
        StartCoroutine("ElectricityOnOff");
    }
    IEnumerator ElectricityOnOff()
    {
        E0.gameObject.SetActive(false);
        E1.gameObject.SetActive(false);
        yield return new WaitForSeconds(times);
        E0.gameObject.SetActive(true);
        yield return new WaitForSeconds(times);
        E1.gameObject.SetActive(true);
        yield return new WaitForSeconds(times);
        StartCoroutine("ElectricityOnOff");
        yield return null;
    }
}
