using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaUp : MonoBehaviour
{
    private void useitem()
    {
        GameObject.Find("GameManager").GetComponent<PlayerStats>().HpUpgrade();
    }
}
