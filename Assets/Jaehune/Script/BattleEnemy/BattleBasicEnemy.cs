using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleBasicEnemy : MonoBehaviour
{
    public int Hp, MaxHp, Damage;
    [SerializeField] RaycastHit2D hit;
    [SerializeField] GameObject Player;
    public GameObject Enemy;
    [SerializeField] Image HpBar;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RayCasting();
        if (Hp <= 0)
        {
            Dead();
        }
        HpBar.transform.position = this.transform.position + new Vector3(0, 0.4f, 0);
        HpBar.fillAmount = Hp / MaxHp;
    }
    void RayCasting()
    {
        Debug.DrawRay(transform.position, Vector3.right * 50, Color.red);
        var rayHit = Physics2D.RaycastAll(transform.position, Vector3.right, 50);
        foreach (var hit in rayHit)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                Player = hit.collider.gameObject;
            }
        }
    }
    void Dead()
    {
        GameManager.Instance.IsBattleStart = false;
        //GameManager.Instance.IsMove = true;
        Destroy(this.gameObject);
    }
}
