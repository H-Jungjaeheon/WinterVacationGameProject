using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EL : MonoBehaviour
{
    private bool Dam;
    void FixdUpdate()
    {
        if(Dam)
        {
            GameManager.Instance.stackDamage += 10;
            Dam = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Dam = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Dam = false;
    }
}
