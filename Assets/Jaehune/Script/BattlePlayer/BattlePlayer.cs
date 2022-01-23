using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayer : MonoBehaviour
{
    [SerializeField] int Hp, Damage;
    [SerializeField] RaycastHit2D hit;
    [SerializeField] GameObject Enemy;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RayCasting();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }
    void RayCasting()
    {
        Debug.DrawRay(this.transform.position, Vector3.left * 50, Color.red);
        var rayHit = Physics2D.RaycastAll(transform.position, Vector3.left, 50);
        foreach (var hit in rayHit)
        {
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                Enemy = hit.collider.gameObject; 
            }
        }
    }
    void Attack()
    {
        Enemy.GetComponent<BattleBasicEnemy>().Hp -= Damage;
    }
}
