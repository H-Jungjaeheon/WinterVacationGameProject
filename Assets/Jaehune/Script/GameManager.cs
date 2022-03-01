using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    public int Stage = 1, Money;
    [SerializeField] public Image FadIn, BattleStartImage;

    #region Bool Fields 
    [Header("bool fields")]
    [Space(10f)]
    public bool IsBattleStart;
    public bool IsCamMove, AttackOk, IsBattlePlace, isPause, isRoom, LevelUp,
    bosssurvival, isGetKey, isManaBarrier, isEunsin, isTrapBarrier, isBurns, isGameOver, is2F,
    isBossRoom, isDoor, BossRoomStart, BossRoom, BossSound, finalBossSound;
    #endregion

    [Header("변수")]
    [Space(10f)]
    [SerializeField] bool IsStart = false;
    public Text BattleSkillText;
    public Text[] PBattleSkillBackGroundText;
    public GameObject BattleButtonUi, BattleSkillBackGround, StatUp, itemParticle;


    [SerializeField] private Slider hpBar, manaBar;
    public float curHp = 100, curMana = 100, maxHp = 100, maxMana = 100, BattleEndCount = 0, stackDamage = 0, damageabsorption = 0, defense = 0;

    [SerializeField] GameObject Player, menuPanel, soundpanel;
    private Color PanelAlpha;
    private Image PanelImage;
    [SerializeField] UnityEngine.Camera MCamera;
    [SerializeField] public Text manaText, hpText, moneyText, manaOverText;
    private GameObject stop;
    [SerializeField] private int Bossidx;
    [SerializeField] GameObject[] Boss, BattleBackGround;
    public GameObject[] PBattleSkillBackGround; //Player Skill BackGround
    [SerializeField] GameObject GameOverPanel;
    public ParticleSystem speedParticle;
    [SerializeField] private Sprite stopimage;
    [SerializeField] Sprite continue_image;

    private float cnt;

    private void Awake()
    {
        itemParticle.GetComponent<ParticleSystem>().Stop();
        stop = GameObject.Find("stop").gameObject;
        isGameOver = false;
        BattleButtonUi.SetActive(false);
        Instance = this;
        BattleSkillText.text = "";
        BattleSkillBackGround.SetActive(false);
        StatUp.transform.position = GameObject.Find("GameManager").GetComponent<PlayerStats>().statStartPos.transform.position;
        maxHp = GetComponent<PlayerStats>().stats[0];
        maxMana = GetComponent<PlayerStats>().stats[2];
        speedParticle.Stop();
    }

    

    // Update is called once per frame
    void Update()
    {
        HandleSlider();
        moneyText.text = Money + "";
        if (IsBattleStart == true && IsStart == false && BattleEndCount == 0)
        {
            StartCoroutine("BattleStart");
            IsStart = true;
            if (isBossRoom == false)
            {
                switch (Stage)
                {
                    case 1:
                        BattleBackGround[0].SetActive(true);
                        BattleBackGround[1].SetActive(false);
                        BattleBackGround[2].SetActive(false);
                        BattleBackGround[3].SetActive(false);
                        BattleBackGround[4].SetActive(false);
                        break;
                    case 2:
                        BattleBackGround[0].SetActive(false);
                        BattleBackGround[1].SetActive(true);
                        BattleBackGround[2].SetActive(false);
                        BattleBackGround[3].SetActive(false);
                        BattleBackGround[4].SetActive(false);
                        break;
                }
            }
            else
            {
                switch (Stage)
                {
                    case 1:
                        BattleBackGround[0].SetActive(false);
                        BattleBackGround[1].SetActive(false);
                        BattleBackGround[2].SetActive(true);
                        BattleBackGround[3].SetActive(false);
                        BattleBackGround[4].SetActive(false);
                        break;
                    case 2:
                        BattleBackGround[0].SetActive(false);
                        BattleBackGround[1].SetActive(false);
                        BattleBackGround[2].SetActive(false);
                        BattleBackGround[3].SetActive(true);
                        BattleBackGround[4].SetActive(false);
                        break;
                    case 3:
                        BattleBackGround[0].SetActive(false);
                        BattleBackGround[1].SetActive(false);
                        BattleBackGround[2].SetActive(false);
                        BattleBackGround[3].SetActive(false);
                        BattleBackGround[4].SetActive(true);
                        break;
                }
            }
        }
        if (IsBattleStart == false && IsStart == true && LevelUp == false)
        {
            StartCoroutine("BattleEnd");
            IsStart = false;
        }
        if (BattleEndCount <= 0)
        {
            BattleEndCount = 0;
        }
        else
        {
            BattleEndCount -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Time.timeScale *= 2;
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            Time.timeScale /= 2;
        }
        //else if (Input.GetKeyDown(KeyCode.C))
        //{
        //    isGetKey = true;
        //}
        //else if (Input.GetKeyDown(KeyCode.V))
        //{
        //    GameObject.Find("GameManager").GetComponent<PlayerStats>().Stateup = true;
        //    GameObject.Find("GameManager").GetComponent<PlayerStats>().DmgUpgrade();
        //}
        if (BossRoomStart)
        {
            StartCoroutine(BossRooms());
            BossRoomStart = false;
        }
    }

    IEnumerator BattleStart()
    {
        StartCoroutine(BattleStartFaidOut(0.8f));
        if (BossSound)
            GameObject.Find("SoundManager").GetComponent<SoundManager>().musicSound(2);
        else if (finalBossSound == true)
            GameObject.Find("SoundManager").GetComponent<SoundManager>().musicSound(3);
        else
            GameObject.Find("SoundManager").GetComponent<SoundManager>().musicSound(1);
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
        GameObject.Find("SoundManager").GetComponent<SoundManager>().musicSound(0);
        yield return null;
    }


    IEnumerator BattleStartFaidOut(float FaidTime)
    {
        Color color = FadIn.color;
        Color color2 = BattleStartImage.color;
        stop.SetActive(false);
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
        stop.SetActive(true);
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
    IEnumerator StartFaidOut(float FaidTime)
    {
        Color color = FadIn.color;
        while (color.a < 1f)
        {
            color.a += Time.deltaTime / FaidTime;
            FadIn.color = color;
            if (color.a >= 1f)
            {
                color.a = 1f;
            }
            yield return null;
        }
        Debug.Log(1);
        yield return new WaitForSeconds(2f);
        StartCoroutine(StartFaidIn(1f));
    }
    IEnumerator DoorCnt()
    {
        GameManager.Instance.isDoor = true;
        yield return new WaitForSeconds(1f);
        GameManager.Instance.isDoor = false;
    }
    IEnumerator StartFaidIn(float FaidTime)
    {
        Color color = FadIn.color;
        while (color.a > 0f)
        {
            color.a -= Time.deltaTime / FaidTime;
            FadIn.color = color;
            if (color.a <= 0f)
            {
                color.a = 0f;
            }
            yield return null;
        }

    }
    public IEnumerator BossRooms()
    {
        if (bosssurvival == false)
        {
            yield break;
        }
        StartCoroutine(StartFaidOut(0.01f));
        Player.transform.position += Vector3.left * 10;
        Player.transform.localEulerAngles = Vector3.zero;
        MCamera.GetComponent<CameraMove>().BossRoomstartcam();
        MCamera.transform.position += Vector3.down*2;
        yield return new WaitForSeconds(2f);
        Boss[Stage - 1].SetActive(true);
        MCamera.orthographicSize = 7f;
        MCamera.transform.position += new Vector3(0, 2f, 0);
        Player.GetComponent<Player>().anim.SetBool("IsWalk", true);
        while (Boss[Stage - 1].transform.position.x + 4 < MCamera.transform.position.x)
        {
            MCamera.transform.position -= new Vector3(4f, 0, 0) * Time.deltaTime * 1;
            if (Boss[Stage - 1].transform.position.x + 10 <= Player.transform.position.x)
            {
                Player.transform.position -= new Vector3(2f, 0, 0) * Time.deltaTime * 1;
            }
            yield return null;
        }
        Player.GetComponent<Player>().anim.SetBool("IsWalk", false);
        yield return new WaitForSeconds(5f);
        bosssurvival = false;
        BossRoom = false;
        yield return null;
    }


    private void HandleSlider()
    {
        if (stackDamage < 0)
        {
            stackDamage = 0;
        }
        else if(stackDamage >= maxHp)
        {
            stackDamage = maxHp;
        }
        if (curMana >= maxMana)
        {
            curMana = maxMana;
        }

        curHp = maxHp - stackDamage;


        if (IsBattleStart == false && LevelUp == false && isManaBarrier == false&&curMana>=0)
        {
            curMana -= Time.deltaTime * 0.05f;
        }
        hpBar.value = curHp / maxHp;
        float hp = (float)(Math.Truncate(curHp) / 1);
        hpText.text = hp + "/" + maxHp;
        manaBar.value = curMana / maxMana;
        float mana = (float)(Math.Truncate(curMana) / 1);
        manaText.text = mana + "/" + maxMana;
        
        if (curHp <= 0f)
        {
            isGameOver = true;
            StartCoroutine(GameOver());
        }
        else if (curMana <= 0f && IsBattleStart == false)
        {
            manaOverText.DOFade(1, 1);
            cnt += Time.deltaTime;
            float Cnt = (float)(Math.Truncate(cnt) / 1);
            manaOverText.text = 5-Cnt+"초 안에 만나를 흭득하십시오";
            
            if (cnt>=5)
            {
                manaOverText.DOFade(0, 1);
                isGameOver = true;
                StartCoroutine(GameOver());
            }

        }
        if (curMana > 0)
        {
            manaOverText.DOFade(0, 1);
            cnt = 0;
        }
    }
    public void StopButton()
    {
        PanelImage = menuPanel.GetComponent<Image>();
        if (isPause)
        {
            PanelAlpha.a = 0f;
            PanelImage.color = PanelAlpha;
            PanelImage.raycastTarget = false;
            EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = stopimage;
            isPause = false;
            menuPanel.transform.GetChild(1).gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            PanelAlpha.a = 0.7f;
            PanelImage.color = PanelAlpha;
            PanelImage.raycastTarget = true;
            EventSystem.current.currentSelectedGameObject.GetComponent<Image>().sprite = continue_image;
            isPause = true;
            soundpanel.SetActive(false);
            menuPanel.transform.GetChild(1).gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    private IEnumerator GameOver()
    {
        StartCoroutine(StartFaidOut(1));
        yield return new WaitForSeconds(1f);
        GameOverPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        GameObject.Find("GameOver").GetComponent<GameOver>().OnGameOver();
        yield return null;
    }
    public void Exitbutton()
    {
        Application.Quit();
    }
    public void mainbutton()
    {
        SceneManager.LoadScene("Title");
        Time.timeScale = 1f;
    }
    public void Soundbutton()
    {
        soundpanel.SetActive(true);
    }
    public void SoundPanalExit()
    {
        soundpanel.SetActive(false);
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
        speedParticle.Play();
        GameObject.Find("Player").GetComponent<Player>().speed = 10;
        yield return new WaitForSeconds(5.0f);
        speedParticle.Stop();
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
                itemParticle.GetComponent<ParticleSystem>().Play();

                stackDamage -= 10;
                Debug.Log(curHp);
                break;
            case 1:
                itemParticle.GetComponent<ParticleSystem>().Play();

                curMana += 10;
                break;
            case 2:
                itemParticle.GetComponent<ParticleSystem>().Play();

                Debug.Log("speed");
                StartCoroutine(speedPotion());
                break;
            case 3:
                itemParticle.GetComponent<ParticleSystem>().Play();

                Debug.Log("manaBarrier");

                StartCoroutine(ManaBarrier());
                break;
            case 4:
                itemParticle.GetComponent<ParticleSystem>().Play();

                Debug.Log("Eunsin");
                StartCoroutine(Eunsincnt());
                break;
            case 5:
                itemParticle.GetComponent<ParticleSystem>().Play();

                Debug.Log("TrapBarrier");

                StartCoroutine(TrapBarrier());
                break;
            case 6:
                gameObject.GetComponent<PlayerStats>().stats[0] += 20;
                maxHp += 20;
                break;
            case 7:
                gameObject.GetComponent<PlayerStats>().stats[1] += 2;
                break;
            case 8:
                gameObject.GetComponent<PlayerStats>().stats[2] += 10;
                maxMana += 10;
                break;
            case 9:
                damageabsorption += 3;
                break;
            case 10:
                defense += 3;
                break;
            case 11:
                gameObject.GetComponent<PlayerStats>().stats[0] += 20;
                maxHp += 20;
                gameObject.GetComponent<PlayerStats>().stats[1] += 2;
                break;
            case 12:
                GameObject.Find("Player").GetComponent<Player>().speed += 2;
                break;
            case 13:
                GameObject.Find("Player").GetComponent<Player>().gasmasktrue = true;
                break;
        }
        return;
    }
}