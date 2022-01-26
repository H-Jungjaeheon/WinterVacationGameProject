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
    [SerializeField] GameObject menuPanel;
    private Color PanelAlpha;
    private Image PanelImage;
    public bool isPause;
    public static GameManager Instance {get; set;}
    public int Stage = 1; //���� é��(��������)
    [SerializeField] Image FadIn, BattleStartImage; //���� ���۽� ���� ���̵���, ���� Į ���� �̹���
    public bool IsBattleStart = false, IsCamMove = false, AttackOk = false, IsBattlePlace = false; //���� ����, ���� ī�޶� �̵�, ���� ����, ���� ��� ���� ���� �Ǵ�
    [SerializeField] bool IsStart = false; //���� ���� ���� �Ǵ�2
    [SerializeField] private Slider hpBar, surviveBar; //�÷��̾� hp, ������ġ ��
    public Text BattleSkillText; //���� �� ���� or ��ų �̸� ǥ�� �ؽ�Ʈ
    public GameObject BattleButtonUi, BattleSkillBackGround; //������ ��ư, ������ ��ư ��� ������Ʈ
    [SerializeField] float maxHp = 100, maxSurvive = 100; //�ִ� ü��, �ִ� ������ġ
    public float curHp = 100, curSurvive = 0; //ü��, ������ġ
    [SerializeField] GameObject Player; //�÷��̾�

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
