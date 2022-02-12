using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    
    public int Stage = 1; //���� é��(��������)
    [SerializeField] Image FadIn, BattleStartImage; //���� ���۽� ���� ���̵���, ���� Į ���� �̹���
    public bool IsBattleStart = false, IsCamMove = false, AttackOk = false, IsBattlePlace = false, isPause, isRoom, LevelUp = false, isGetKey=false, isManaBarrier,isEunsin = false, isTrapBarrier=false , isBurns=false; //���� ����, ���� ī�޶� �̵�, ���� ����, ���� ��� ���� ���� �Ǵ�, �÷��̾� ���� ����
    [SerializeField] bool IsStart = false; //���� ���� ���� �Ǵ�2
    public Text BattleSkillText; //���� �� ���� or ��ų �̸� ǥ�� �ؽ�Ʈ
    public GameObject BattleButtonUi, BattleSkillBackGround, StatUp; //������ ��ư, ������ ��ư ��� ������Ʈ, ���� ���׷��̵� â
    
    [SerializeField] private Slider hpBar, manaBar; //�÷��̾� hp, ������ġ ��
    public float curHp = 100, curMana = 100, maxHp = 100, maxMana = 100, BattleEndCount = 0, stackDamage = 0, damageabsorption=0, defense=0; //ü��, ������ġ,�ִ� ü��, �ִ� ������ġ,���������,����
    
    [SerializeField] GameObject Player, menuPanel; //�÷��̾�, �޴�
   
    private Color PanelAlpha;
    private Image PanelImage;

    [SerializeField] public Text manaText, hpText;
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
        StartCoroutine(BattleStartFaidOut(0.8f));
        yield return new WaitForSeconds(0.8f);
        Player.SetActive(false);
        IsCamMove = true;
        yield return new WaitForSeconds(2.2f);
        BattleButtonUi.SetActive(true);
        StartCoroutine(BattleStartFaidIn(0.8f));
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
        StartCoroutine(BattleStartFaidOut(1));
        yield return new WaitForSeconds(0.5f);
        BattleButtonUi.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        IsCamMove = false;
        yield return new WaitForSeconds(2f);
        Player.SetActive(true);
        BattleEndCount = 1f;
        StartCoroutine(BattleStartFaidIn(0.8f));
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
        hpText.text = curHp + "/" + maxHp;
        manaBar.value = curMana / maxMana;
        float mana = (float)(Math.Truncate(curMana)/1);
        manaText.text = mana + "/" + maxMana;
    }
    public void StopButton()
    {
        PanelImage = menuPanel.GetComponent<Image>();
        if (isPause)
        {
            PanelAlpha.a = 0f;
            PanelImage.color = PanelAlpha;
            PanelImage.raycastTarget = false;
            isPause = false;
            Time.timeScale = 1f;
        }
        else
        {
            PanelAlpha.a = 0.7f;
            PanelImage.color = PanelAlpha;
            PanelImage.raycastTarget = true;
            isPause = true;
            Time.timeScale = 0f;
        }
    }
    IEnumerator ManaBarrier()
    {
        isManaBarrier = true;
        yield return new WaitForSeconds(10f);
        isManaBarrier = false;
    }
    IEnumerator TrapBarrier()
    {
        isTrapBarrier = true;
        yield return new WaitForSeconds(10f);
        isTrapBarrier = false;
    }
    public IEnumerator speedPotion()
    {
        GameObject.Find("Player").GetComponent<Player>().speed = 10;
        yield return new WaitForSeconds(5.0f);
        GameObject.Find("Player").GetComponent<Player>().speed = 5;
    }
    public IEnumerator Eunsincnt()
    {
        isEunsin = true;
        yield return new WaitForSeconds(10f);
        isEunsin = false;
    }
    public void useitem(int itemidx)
    {
        switch (itemidx)
        {
            case 0:
                stackDamage -= 10;
                Debug.Log(curHp);
                break;
            case 1:
                curMana += 10;
                break;
            case 2:
                Debug.Log("speed");
                StartCoroutine(speedPotion());
                break;
            case 3:
                Debug.Log("manaBarrier");

                StartCoroutine(ManaBarrier());
                break;
            case 4:
                Debug.Log("Eunsin");
                StartCoroutine(Eunsincnt());
                break;
            case 5:
                Debug.Log("TrapBarrier");

                StartCoroutine(TrapBarrier());
                break;
            case 6:
                gameObject.GetComponent<PlayerStats>().stats[0] += 1;
                maxHp += gameObject.GetComponent<PlayerStats>().stats[0] * 10;
                break;
            case 7:
                gameObject.GetComponent<PlayerStats>().stats[1] += 1;
                break;
            case 8:
                gameObject.GetComponent<PlayerStats>().stats[2] += 1;
                maxHp += gameObject.GetComponent<PlayerStats>().stats[2] * 10;
                break;
            case 9:
                damageabsorption += 1;
                break;
            case 10:
                defense += 1;
                break;
        }
        return;
    }
}
