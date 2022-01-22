using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set;}
    [SerializeField] Image FadIn;
    public bool IsBattleStart = false, IsStart = false;

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
        if(IsBattleStart == true && IsStart == false)
        {
            StartCoroutine("BattleStart");
            IsStart = true;
        }
    }
    IEnumerator BattleStart()
    {
        StartCoroutine("BattleStartFaidOut", 0.8f);
        yield return new WaitForSeconds(3f);
        StartCoroutine("BattleStartFaidIn", 0.8f);
        yield return null;
    }
    IEnumerator BattleStartFaidOut(float FaidTime)
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
    IEnumerator BattleStartFaidIn(float FaidTime)
    {
        Color color = FadIn.color;
        while (color.a > 0f)
        {
            color.a -= Time.deltaTime / FaidTime;
            FadIn.color = color;
            if (color.a <= 0f) color.a = 0f;
            yield return null;
        }
    }
}
