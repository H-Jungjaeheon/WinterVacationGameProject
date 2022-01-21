using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Scenetest : MonoBehaviour
{
    private bool d = false;
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(d == false) { 
                SceneManager.LoadScene("test");
                DontDestroyOnLoad(this.gameObject);
                d = true;
            }
            else
            {
                SceneManager.LoadScene("EnemyTest");
                DontDestroyOnLoad(this.gameObject);
                d = false;
            }
        }
    }
}
