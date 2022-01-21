using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    
    [SerializeField]
    private Slider hpBar;
    private float maxHp = 100;
    private float curHp = 100;


    [SerializeField]
    private Slider surviveBar;
    private float maxSurvive = 100;
    private float curSurvive = 100;

    

    Obj obj;
    // Start is called before the first frame update
    
    void Start()
    {
        hpBar.value = (float)curHp / (float)maxHp;
        surviveBar.value = (float)curSurvive / (float)maxSurvive;
        
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            curHp -= 10; 
        }
        HandleSlider();
        curSurvive -= 0.001f;
        
    }
    private void HandleSlider()
    {
        hpBar.value = (float)curHp / (float)maxHp;
        surviveBar.value = (float)curSurvive / (float)maxSurvive;

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Obj"))
        {
            if (Obj.Instance.isIt == true)
            {
                if (Input.GetKey(KeyCode.F))
                {
                    Debug.Log("f≈∞ ¥©∏ß");
                    //Interaction.gameObject.SetActive(false);
                    
                    Obj.Instance.isIt = false;
                }

            }

        }

    }
    

}
