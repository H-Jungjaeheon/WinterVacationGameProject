using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnceBoss : BasicEnemyScript
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
        if(Go == true)
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
        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position + new Vector3(0, 1.7f, 0), 10f * Time.deltaTime);
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && GameManager.Instance.IsBattleStart == false && GameManager.Instance.BattleEndCount == 0 && GameManager.Instance.isEunsin == false && GameManager.Instance.isDoor == false)
        {
            Instantiate(BattleManager.Instance.Enemy[SpawnMonsterCount], BattleManager.Instance.EnemySpawner.transform.position, Quaternion.Euler(0, 0, 0));
            Invoke("Delete", 2f);
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
            yield return new WaitForSeconds(2);
            WarningObj.SetActive(true);
            yield return new WaitForSeconds(1);
            transform.rotation = Quaternion.Euler(0, 180, 0);
            SeeCrossroad *= -1;
            yield return new WaitForSeconds(1);
            animator.SetBool("IsAttack", true);
            Go = true;
            GameObject.Find("Main Camera").GetComponent<CameraMove>().VibrateForTime3(0.7f);
            StartCoroutine(AnimationP2());
            yield return null;
        }
    }
    IEnumerator AnimationP2()
    {
        yield return new WaitForSeconds(1);
        animator.SetBool("IsAttack", false);
        animator.SetBool("IsWalk", true);
        GoToPlayer = true;
        yield return null;
    }
}
