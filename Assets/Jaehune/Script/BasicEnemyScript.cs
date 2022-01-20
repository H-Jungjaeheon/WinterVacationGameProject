using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicEnemyScript : MonoBehaviour
{
    [SerializeField] float Speed, MoveCount, MaxMoveCount;
    [SerializeField] bool IsFind = false, IsTurnAround = false, IsMove = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoveCount += Time.deltaTime;
        if (IsMove == true)
        {
            Moving();
        }
        else if (IsMove == false)
        {
            MoveCount = 0;
        }
    }

    void Moving()
    {
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
        IsMove = true;
    }
}
