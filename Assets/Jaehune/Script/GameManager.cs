using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; set;}
    public int Stage = 1;
    [SerializeField] Image FadIn, BattleStartImage; //전투 시작시 띄우는 페이드인, 빨간 칼 연출 이미지
    public bool IsBattleStart = false, IsCamMove = false, AttackOk = false, IsBattlePlace = false; //전투 시작, 전투 카메라 이동, 공격 가능, 전투 장소 띄우기 여부 판단
    [SerializeField] bool IsStart = false; //전투 시작 여부 판단2
    [SerializeField] private Slider hpBar, surviveBar; //플레이어 hp, 감염수치 바
    public Text BattleSkillText; //전투 중 공격 or 스킬 이름 표시 텍스트
    public GameObject BattleButtonUi, BattleSkillBackGround; //전투용 버튼, 전투용 버튼 배경 오브젝트
    [SerializeField] float maxHp = 100, maxSurvive = 100; //최대 체력, 최대 감염수치
    public float curHp = 100, curSurvive = 0; //체력, 감염수치
    [SerializeField] GameObject Player;

    private void Awake()
    {
        BattleButtonUi.SetActive(false);
        Instance = this;
        BattleSkillText.text = "";
        BattleSkillBackGround.SetActive(false);
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
        yield return new WaitForSeconds(0.8f);
        Player.SetActive(false);
        yield return new WaitForSeconds(2.2f);
        BattleButtonUi.SetActive(true);
        StartCoroutine("BattleStartFaidIn", 0.8f);
        yield return new WaitForSeconds(1f);
        IsBattlePlace = true;
        yield return new WaitForSeconds(2f);
        IsBattlePlace = false;
        yield return new WaitForSeconds(1.2f);
        AttackOk = true;
        yield return null;
    }
    IEnumerator BattleEnd()
    {
        AttackOk = false;
        yield return new WaitForSeconds(2.5f);
        StartCoroutine("BattleStartFaidOut", 1f);
        yield return new WaitForSeconds(0.5f);
        BattleButtonUi.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        IsCamMove = false;
        yield return new WaitForSeconds(2f);
        Player.SetActive(true);
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
