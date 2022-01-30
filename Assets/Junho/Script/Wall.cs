using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public GameObject btn;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (btn.GetComponent<Btn>().isOn)
        {
            gameObject.SetActive(false);
        }
        
    }
}
