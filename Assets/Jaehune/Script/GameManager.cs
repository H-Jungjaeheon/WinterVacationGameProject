using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance { get; set;}
    [SerializeField] Image FadIn;
    [SerializeField] bool IsBattleStart = false;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(IsBattleStart == true)
        {
            StartCoroutine("BattleStart");
        }
    }
    IEnumerator BattleStart(float FaidTime)
    {
        Color color = FadIn.color;
        while (color.a < 1.0f)
        {
            color.a += Time.deltaTime / FaidTime;
            FadIn.color = color;
            if (color.a >= 1f) color.a = 1f;
            yield return null;
        }
    }

}
