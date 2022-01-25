using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set;}
    [SerializeField] Image FadIn, BattleStartImage;
    public bool IsBattleStart = false, IsCamMove = false, AttackOk = false; //IsMove = true
    [SerializeField] bool IsStart = false;
    [SerializeField] private Slider hpBar, surviveBar;
    private float maxHp = 100, maxSurvive = 100;
    public float curHp = 100, curSurvive = 0;

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
            IsCamMove = true;
        }
        if(IsBattleStart == false && IsStart == true)
        {
            StartCoroutine("BattleEnd");
            IsStart = false;
        }
        hpBar.value = (float)curHp / (float)maxHp;
        surviveBar.value = (float)curSurvive / (float)maxSurvive;
        HandleSlider();
    }
    IEnumerator BattleStart()
    {
        StartCoroutine("BattleStartFaidOut", 0.8f);
        yield return new WaitForSeconds(3f);
        StartCoroutine("BattleStartFaidIn", 0.8f);
        AttackOk = true;
        yield return null;
    }
    IEnumerator BattleEnd()
    {
        AttackOk = false;
        StartCoroutine("BattleStartFaidOut", 0.8f);
        yield return new WaitForSeconds(1f);
        IsCamMove = false;
        yield return new WaitForSeconds(2f);
        StartCoroutine("BattleStartFaidIn", 0.8f);
        yield return null;
    }
    IEnumerator BattleStartFaidOut(float FaidTime)
    {
        Color color = FadIn.color;
        Color color2 = BattleStartImage.color;
        while (color.a < 1f && color2.a < 1f)
        {
            color.a += Time.deltaTime / FaidTime;
            color2.a += Time.deltaTime / FaidTime;
            FadIn.color = color;
            BattleStartImage.color = color2;
            if (color.a >= 1f && color2.a >= 1f)
            {
                color.a = 1f;
                color2.a = 1f;
            }
            yield return null;
        }
    }
    IEnumerator BattleStartFaidIn(float FaidTime)
    {
        Color color = FadIn.color;
        Color color2 = BattleStartImage.color;
        while (color.a > 0f && color2.a > 0f)
        {
            color.a -= Time.deltaTime / FaidTime;
            color2.a -= Time.deltaTime / FaidTime;
            FadIn.color = color;
            BattleStartImage.color = color2;
            if (color.a <= 0f && color2.a <= 0f)
            {
                color.a = 0f;
                color2.a = 0f;
            }
            yield return null;
        }
    }
    private void HandleSlider()
    {
        if(IsBattleStart == false)
        {
            curSurvive += Time.deltaTime;
        }
        hpBar.value = (float)curHp / (float)maxHp;
        surviveBar.value = (float)curSurvive / (float)maxSurvive;
    }
}
