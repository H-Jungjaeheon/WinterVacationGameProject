using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePlayer : MonoBehaviour
{
    [SerializeField] RaycastHit2D hit; //적 인식 레이캐스트
    [SerializeField] GameObject Enemy, PlayerSpawner, EnemySpawner, DmgText, GM, SkillButton, ManaText; //전투시 인식한 적 오브젝트 담는 그릇
    [SerializeField] bool GoToEnemy = false, GoToReturn = false, IsAttackSkill = false; //적의 위치(근접 공격시)로 갈지 판단
    public bool IsHit = false;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        this.transform.position = PlayerSpawner.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        RayCasting();
        if (Input.GetKeyDown(KeyCode.Space) && BattleManager.Instance.IsPlayerTurn == true && GameManager.Instance.AttackOk == true)
        {
            Playerattack();
        }
        if (GoToEnemy == true && GameManager.Instance.IsBattleStart == true && IsAttackSkill == false)
        {
            animator.SetBool("IsWalk", true);
            transform.position = Vector3.MoveTowards(this.transform.position, EnemySpawner.transform.position + new Vector3(-2, 0.77f, 0), 10 * Time.deltaTime);
        }
        else if(GoToEnemy == true && GameManager.Instance.IsBattleStart == true && IsAttackSkill == true)
        {
            animator.SetBool("IsWalk", true);
            transform.position = Vector3.MoveTowards(this.transform.position, EnemySpawner.transform.position + new Vector3(-6, 0.77f, 0), 10 * Time.deltaTime);
        }
        else if(GoToReturn == true)
        {
            animator.SetBool("IsWalk", true);
            transform.position = Vector3.MoveTowards(this.transform.position, PlayerSpawner.transform.position, 10 * Time.deltaTime);
        }
        else if(GoToReturn == false)
        {
            animator.SetBool("IsWalk", false);
        }
        if (IsHit == true)
        {
            StartCoroutine(PlayerHit());
        }
    }

    void RayCasting()
    {
        Debug.DrawRay(this.transform.position, Vector3.right * 50, Color.red);
        var rayHit = Physics2D.RaycastAll(transform.position, Vector3.right, 50);
        foreach (var hit in rayHit)
        {
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                Enemy = hit.collider.gameObject;
            }
        }
    }
    public void Playerattack()
    {
        if (BattleManager.Instance.IsPlayerTurn == true && GameManager.Instance.AttackOk == true)
        {
            StartCoroutine("PlayerAttack", 1f);
        }
    }
    public void Playerskill()
    {     
        SkillButton.SetActive(true);
        GameManager.Instance.BattleButtonUi.SetActive(false);   
    }
    public void PlayerItem()
    {
        if (BattleManager.Instance.IsPlayerTurn == true && GameManager.Instance.AttackOk == true)
        {
            StartCoroutine("PlayerAttack", 1f);
        }
    }
    public void PlayerDeffence()
    {
        if (BattleManager.Instance.IsPlayerTurn == true && GameManager.Instance.AttackOk == true)
        {
            StartCoroutine("PlayerAttack", 1f);
        }
    }
    public void Return()
    {
        SkillButton.SetActive(false);
        GameManager.Instance.BattleButtonUi.SetActive(true);
    }
    public void UseSkill()
    {
        if (BattleManager.Instance.IsPlayerTurn == true && GameManager.Instance.AttackOk == true)
        {
            if (GameManager.Instance.curMana >= 25)
            {
                GameManager.Instance.curMana -= 25;
                IsAttackSkill = true;
                StartCoroutine("PlayerSkill", 1f);
            }
            else
            {
                ManaText.SetActive(true);
                Invoke("ManatextOff", 2f);
            }
        }
    }
    public void UseSkill2()
    {
        if (BattleManager.Instance.IsPlayerTurn == true && GameManager.Instance.AttackOk == true)
        {
            if (GameManager.Instance.curMana >= 40)
            {
                GameManager.Instance.curMana -= 40;
                IsAttackSkill = true;
                StartCoroutine("PlayerSkill", 1f);
            }
            else
            {
                ManaText.SetActive(true);
                Invoke("ManatextOff", 2f);
            }
        }
    }
    public void UseSkill3()
    {
        if (BattleManager.Instance.IsPlayerTurn == true && GameManager.Instance.AttackOk == true)
        {
            if (GameManager.Instance.curMana >= 55)
            {
                GameManager.Instance.curMana -= 55;
                IsAttackSkill = true;
                StartCoroutine("PlayerSkill", 1f);
            }
            else
            {
                ManaText.SetActive(true);
                Invoke("ManatextOff", 2f);
            }
        }
    }
    public IEnumerator PlayerHit()
    {
        IsHit = false;
        animator.SetBool("IsHit", true);
        yield return new WaitForSeconds(1);
        animator.SetBool("IsHit", false);
        yield return null;
    }
    void ManatextOff()
    {
        ManaText.SetActive(false);
    }
    IEnumerator PlayerAttack()
    {
        SkillButton.SetActive(false);
        GameManager.Instance.BattleButtonUi.SetActive(false);
        GameManager.Instance.BattleSkillBackGround.SetActive(true);
        GameManager.Instance.BattleSkillText.text = "실험 : 불꽃 점화";
        BattleManager.Instance.IsPlayerTurn = false;
        GoToEnemy = true;
        yield return new WaitForSeconds(1.5f);
        BattleManager.Instance.CamP = true;
        animator.SetBool("IsAttack", true);
        GameObject DT = Instantiate(DmgText);
        DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
        DT.transform.position = Enemy.transform.position;
        DT.GetComponent<BattleDamageText>().damage = GM.GetComponent<PlayerStats>().stats[1];
        Enemy.GetComponent<BattleBasicEnemy>().Hp -= GM.GetComponent<PlayerStats>().stats[1];
        if (Enemy.GetComponent<BattleBasicEnemy>().IsReflect && GM.GetComponent<PlayerStats>().stats[1] == 1)
        {
            GameObject DTT = Instantiate(DmgText);
            DTT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
            DTT.transform.position = this.transform.position;
            DTT.GetComponent<BattleDamageText>().damage = GM.GetComponent<PlayerStats>().stats[1];
            GameManager.Instance.stackDamage += GM.GetComponent<PlayerStats>().stats[1];
        }
        else if (Enemy.GetComponent<BattleBasicEnemy>().IsReflect && GM.GetComponent<PlayerStats>().stats[1] >= 1)
        {
            GameObject DTT = Instantiate(DmgText);
            DTT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
            DTT.transform.position = this.transform.position;
            DTT.GetComponent<BattleDamageText>().damage = GM.GetComponent<PlayerStats>().stats[1] / 2;
            GameManager.Instance.stackDamage += GM.GetComponent<PlayerStats>().stats[1] / 2;
        }
        GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
        Enemy.GetComponent<BattleBasicEnemy>().IsHit = true;
        yield return new WaitForSeconds(1);
        animator.SetBool("IsAttack", false);
        BattleManager.Instance.CamP = false;
        GameManager.Instance.BattleSkillBackGround.SetActive(false);
        GoToEnemy = false;
        GoToReturn = true;
        yield return new WaitForSeconds(1);
        GoToReturn = false;
        yield return new WaitForSeconds(2);
        if (GameManager.Instance.IsCamMove == true)
        {
            BattleManager.Instance.IsEnemyTurn = true;
        }
        else
        {
            BattleManager.Instance.IsPlayerTurn = true;
        }
        yield return null;
    }
    IEnumerator PlayerSkill()
    {
        GameObject.Find("Main Camera").GetComponent<CameraMove>().IsFarAway = true;
        SkillButton.SetActive(false);
        GameManager.Instance.BattleButtonUi.SetActive(false);
        GameManager.Instance.BattleSkillBackGround.SetActive(true);
        GameManager.Instance.BattleSkillText.text = "실험 : 불꽃 발사";
        BattleManager.Instance.IsPlayerTurn = false;
        GoToEnemy = true;
        yield return new WaitForSeconds(1.5f);
        BattleManager.Instance.CamP = true;
        animator.SetBool("IsAttack", true);
        GameObject DT = Instantiate(DmgText);
        DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
        DT.transform.position = Enemy.transform.position;
        DT.GetComponent<BattleDamageText>().damage = GM.GetComponent<PlayerStats>().stats[1] * 2;
        Enemy.GetComponent<BattleBasicEnemy>().Hp -= GM.GetComponent<PlayerStats>().stats[1] * 2;
        GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
        Enemy.GetComponent<BattleBasicEnemy>().IsHit = true;
        yield return new WaitForSeconds(1);
        BattleManager.Instance.CamP = false;
        animator.SetBool("IsAttack", false);
        GameManager.Instance.BattleSkillBackGround.SetActive(false);
        GoToEnemy = false;
        GoToReturn = true;
        yield return new WaitForSeconds(1);
        GoToReturn = false;
        yield return new WaitForSeconds(2);
        IsAttackSkill = false;
        if (GameManager.Instance.IsCamMove == true)
        {
            BattleManager.Instance.IsEnemyTurn = true;
        }
        else
        {
            BattleManager.Instance.IsPlayerTurn = true;
        }
        yield return null;
    }
    IEnumerator PlayerSkill2()
    {
        GameObject.Find("Main Camera").GetComponent<CameraMove>().IsFarAway = true;
        SkillButton.SetActive(false);
        GameManager.Instance.BattleButtonUi.SetActive(false);
        GameManager.Instance.BattleSkillBackGround.SetActive(true);
        GameManager.Instance.BattleSkillText.text = "실험 : 상처 재생";
        BattleManager.Instance.IsPlayerTurn = false;
        GoToEnemy = true;
        yield return new WaitForSeconds(1.5f);
        BattleManager.Instance.CamP = true;
        animator.SetBool("IsAttack", true);
        GameObject DT = Instantiate(DmgText);
        DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
        DT.transform.position = Enemy.transform.position;
        DT.GetComponent<BattleDamageText>().damage = GM.GetComponent<PlayerStats>().stats[1] * 2;
        Enemy.GetComponent<BattleBasicEnemy>().Hp -= GM.GetComponent<PlayerStats>().stats[1] * 2;
        GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
        Enemy.GetComponent<BattleBasicEnemy>().IsHit = true;
        yield return new WaitForSeconds(1);
        BattleManager.Instance.CamP = false;
        animator.SetBool("IsAttack", false);
        GameManager.Instance.BattleSkillBackGround.SetActive(false);
        GoToEnemy = false;
        GoToReturn = true;
        yield return new WaitForSeconds(1);
        GoToReturn = false;
        yield return new WaitForSeconds(2);
        IsAttackSkill = false;
        if (GameManager.Instance.IsCamMove == true)
        {
            BattleManager.Instance.IsEnemyTurn = true;
        }
        else
        {
            BattleManager.Instance.IsPlayerTurn = true;
        }
        yield return null;
    }
    IEnumerator PlayerSkill3()
    {
        GameObject.Find("Main Camera").GetComponent<CameraMove>().IsFarAway = true;
        SkillButton.SetActive(false);
        GameManager.Instance.BattleButtonUi.SetActive(false);
        GameManager.Instance.BattleSkillBackGround.SetActive(true);
        GameManager.Instance.BattleSkillText.text = "실험 : 피해 무효";
        BattleManager.Instance.IsPlayerTurn = false;
        GoToEnemy = true;
        yield return new WaitForSeconds(1.5f);
        BattleManager.Instance.CamP = true;
        animator.SetBool("IsAttack", true);
        GameObject DT = Instantiate(DmgText);
        DT.GetComponentInChildren<Canvas>().worldCamera = UnityEngine.Camera.main;
        DT.transform.position = Enemy.transform.position;
        DT.GetComponent<BattleDamageText>().damage = GM.GetComponent<PlayerStats>().stats[1] * 2;
        Enemy.GetComponent<BattleBasicEnemy>().Hp -= GM.GetComponent<PlayerStats>().stats[1] * 2;
        GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime(0.5f);
        Enemy.GetComponent<BattleBasicEnemy>().IsHit = true;
        yield return new WaitForSeconds(1);
        BattleManager.Instance.CamP = false;
        animator.SetBool("IsAttack", false);
        GameManager.Instance.BattleSkillBackGround.SetActive(false);
        GoToEnemy = false;
        GoToReturn = true;
        yield return new WaitForSeconds(1);
        GoToReturn = false;
        yield return new WaitForSeconds(2);
        IsAttackSkill = false;
        if (GameManager.Instance.IsCamMove == true)
        {
            BattleManager.Instance.IsEnemyTurn = true;
        }
        else
        {
            BattleManager.Instance.IsPlayerTurn = true;
        }
        yield return null;
    }
}
