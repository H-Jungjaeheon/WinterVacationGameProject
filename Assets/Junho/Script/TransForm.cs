using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransForm : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = GameManager.Instance.TPlayer.position;
    }
}
