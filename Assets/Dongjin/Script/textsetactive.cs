 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textsetactive : MonoBehaviour
{
    public bool on;
    private void LateUpdate()
    {
        if (on && GameManager.Instance.isPause == false)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
