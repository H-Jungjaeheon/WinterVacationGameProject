using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicEnemyScript : MonoBehaviour
{
    [SerializeField] float Speed, MoveCount, MaxMoveCount, SeeCrossroad;
    [SerializeField] bool IsFind = false, IsMove = true;
    [SerializeField] GameObject Player, Warning;
    [SerializeField] RaycastHit2D hit;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RayCasting();
        if(IsMove == true)
        {
            Moving();
        }
        else if(IsMove == false)
        {
            MoveCount = 0;
        }
        if(IsFind == true)
        {
            FindPlayer();
        }
    }

    void Moving()
    {
        MoveCount += Time.deltaTime;
        transform.position += new Vector3(Speed * Time.deltaTime, 0, 0);
        if(MoveCount >= MaxMoveCount)
        {
            MoveCount = 0;
            IsMove = false;
            Invoke("Trun", 4f);
        }
    }
    void Trun()
    {
        Speed *= -1;
        SeeCrossroad *= -1;
        IsMove = true;
    }
    void RayCasting()
    {
        Debug.DrawRay(transform.position, Vector3.left * SeeCrossroad, Color.red);
        var rayHit = Physics2D.RaycastAll(transform.position, Vector3.left, SeeCrossroad);
        foreach(var hit in rayHit)
        {
            if (hit.collider.gameObject.CompareTag("Player")) 
            {
                IsFind = true;
            }
            else
            {
                IsFind = false;
            }
        }
    }
    void FindPlayer()
    {
        MoveCount = 0;
        transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, Speed * 1.3f * Time.deltaTime);
    }
}