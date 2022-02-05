using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electricityscript : MonoBehaviour
{
    private GameObject E;
    [SerializeField] float times;
    private void Start()
    {
        E = transform.GetChild(0).gameObject;
        StartCoroutine("ElectricityOnOff");
    }
    IEnumerator ElectricityOnOff()
    {
        E.gameObject.SetActive(false);
        yield return new WaitForSeconds(times);
        E.gameObject.SetActive(true);
        yield return new WaitForSeconds(times);
        StartCoroutine("ElectricityOnOff");
        yield return null;
    }
}
