using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : BasicEnemyScript
{
    [SerializeField] bool GoToPlayer, IsTurn, Go;
    // Start is called before the first frame update
    public override void Start()
    {
        IsTurn = false;
        GoToPlayer = false;
        animator = GetComponent<Animator>();
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
    }
    public override void RayCasting()
    {
        Debug.DrawRay(transform.position - new Vector3(0, 2, 0), Vector3.left * (SeeCrossroad * IsPlus), Color.red);
        var rayHit = Physics2D.RaycastAll(transform.position - new Vector3(0, 2, 0), Vector3.left, SeeCrossroad * IsPlus);
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
        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position + new Vector3(-5, 2.935f, 0), 4f * Time.deltaTime);
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
            GameManager.Instance.finalBossSound = true;
            IsTurn = true;
            yield return new WaitForSeconds(1);
            GoToPlayer = true;
            yield return new WaitForSeconds(4);
            GoToPlayer = false;
            this.GetComponent<BoxCollider2D>().size = new Vector3(9, 3);
            yield return new WaitForSeconds(2);
            yield return null;
        }
    }
}
