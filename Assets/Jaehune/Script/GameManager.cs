using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    
    public int Stage = 1; //현재 챕터(스테이지)
    [SerializeField] Image FadIn, BattleStartImage; //전투 시작시 띄우는 페이드인, 빨간 칼 연출 이미지
    public bool IsBattleStart = false, IsCamMove = false, AttackOk = false, IsBattlePlace = false, isPause, isRoom, LevelUp = false, isGetKey=false, isManaBarrier,isEunsin = false, isTrapBarrier=false; //전투 시작, 전투 카메라 이동, 공격 가능, 전투 장소 띄우기 여부 판단, 플레이어 열쇠 여부
    [SerializeField] bool IsStart = false; //전투 시작 여부 판단2
    public Text BattleSkillText; //전투 중 공격 or 스킬 이름 표시 텍스트
    public GameObject BattleButtonUi, BattleSkillBackGround, StatUp; //전투용 버튼, 전투용 버튼 배경 오브젝트, 스탯 업그레이드 창
    
    [SerializeField] private Slider hpBar, manaBar; //플레이어 hp, 감염수치 바
    public float curHp = 100, curMana = 100, maxHp = 100, maxMana = 100, BattleEndCount = 0, stackDamage = 0; //체력, 감염수치,최대 체력, 최대 감염수치
    
    [SerializeField] GameObject Player, menuPanel; //플레이어, 메뉴
   
    private Color PanelAlpha;
    private Image PanelImage;

    private void Awake()
    {
        BattleButtonUi.SetActive(false);
        Instance = this;
        BattleSkillText.text = "";
        BattleSkillBackGround.SetActive(false);
        StatUp.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (curHp>=maxHp)
        {
            curHp = maxHp;
        }
        if (curMana >= maxMana)
        {
            curMana = maxMana;
        }

        curHp = maxHp - stackDamage;

        if (IsBattleStart == true && IsStart == false && BattleEndCount == 0)
        {
            StartCoroutine("BattleStart");
            IsStart = true;
        }
        if (IsBattleStart == false && IsStart == true && LevelUp == false)
        {
            StartCoroutine("BattleEnd");
            IsStart = false;
        }
        HandleSlider();
        if (BattleEndCount <= 0)
        {
            BattleEndCount = 0;
        }
        else
        {
            BattleEndCount -= Time.deltaTime;
        }
    }
    IEnumerator BattleStart()
    {
        StartCoroutine("BattleStartFaidOut", 0.8f);
        yield return new WaitForSeconds(0.8f);
        Player.SetActive(false);
        IsCamMove = true;
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
        BattleEndCount = 1f;
        StartCoroutine("BattleStartFaidIn", 0.8f);
        yield return new WaitForSeconds(1f);
        BattleManager.Instance.IsEnemyDead = false;
        Player.SetActive(false);
        Player.SetActive(true);
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
        if (IsBattleStart == false && LevelUp == false&& isManaBarrier==false)
        {
            curMana -= Time.deltaTime*0.25f;
        }
        hpBar.value = curHp / maxHp;
        manaBar.value = curMana / maxMana;
    }
    public void StopButton()
    {
        PanelImage = menuPanel.GetComponent<Image>();
        if (isPause)
        {
            PanelAlpha.a = 0f;
            PanelImage.color = PanelAlpha;
            isPause = false;
            Time.timeScale = 1f;
        }
        else
        {
            PanelAlpha.a = 0.7f;
            PanelImage.color = PanelAlpha;
            isPause = true;
            Time.timeScale = 0f;
        }
    }
}
