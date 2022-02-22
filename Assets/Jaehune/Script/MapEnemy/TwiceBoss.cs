using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwiceBoss : BasicEnemyScript
{
    [SerializeField] bool GoToPlayer, IsTurn, Go;
    // Start is called before the first frame update
    public override void Start()
    {
        Go = false;
        IsTurn = false;
        GoToPlayer = false;
        animator = GetComponent<Animator>();
        WarningObj.SetActive(false);
    }
    // Update is called once per frame
    public override void Update()
    {
        StartCoroutine(AnimationP());
        RayCasting();
        if (GoToPlayer == true)
        {
            FindPlayer();
        }
        if (Go == true)
        {
            StartCoroutine(AnimationP2());
        }
    }
    public override void RayCasting()
    {
        Debug.DrawRay(transform.position - new Vector3(0, 1, 0), Vector3.left * (SeeCrossroad * IsPlus), Color.red);
        var rayHit = Physics2D.RaycastAll(transform.position - new Vector3(0, 1, 0), Vector3.left, SeeCrossroad * IsPlus);
        foreach (var hit in rayHit)
        {
            if (hit.collider.gameObject.CompareTag("Player") && GameManager.Instance.isEunsin == false)
            {
                Player = hit.collider.gameObject;
            }
        }
    }
    public override void FindPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.transform.position - new Vector3(0, 5, 0), 10.25f * Time.deltaTime);
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && GameManager.Instance.IsBattleStart == false && GameManager.Instance.BattleEndCount == 0 && GameManager.Instance.isEunsin == false && GameManager.Instance.isDoor == false)
        {
            Instantiate(BattleManager.Instance.Enemy[SpawnMonsterCount], BattleManager.Instance.EnemySpawner.transform.position, Quaternion.Euler(0, 0, 0));
            Invoke("Delete", 2f);
            GameManager.Instance.IsBattleStart = true;
        }
    }
    public override void Delete()
    {
        Destroy(this.gameObject);
    }
    IEnumerator AnimationP()
    {
        if (IsTurn == false)
        {
            IsTurn = true;
            GameManager.Instance.BossSound = true;
            yield return new WaitForSeconds(3);
            GoToPlayer = true;
            yield return new WaitForSeconds(1.5f);
            GoToPlayer = false;
            WarningObj.SetActive(true);
            SeeCrossroad *= -1;
            yield return new WaitForSeconds(0.5f);
            animator.SetBool("IsSkill", true);
            GameObject.Find("SoundManager").GetComponent<SoundManager>().SESound(5);
            GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime3(0.7f);
            yield return new WaitForSeconds(1);
            Go = true;
            yield return null;
        }
    }
    IEnumerator AnimationP2()
    {
        animator.SetBool("IsSkill", false);
        yield return new WaitForSeconds(1);
        animator.SetBool("IsWalk", true);
        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position + new Vector3(0, 1.4f, 0), 5f * Time.deltaTime);
        yield return null;
    }
}
