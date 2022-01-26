using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; set; }
    public GameObject[] Enemy; //전투 시작 시 전투 필드에 소환할 적 배열
    public GameObject EnemySpawner; //전투 시작 시 전투 필드에 소환될 적 위치
    public bool IsPlayerTurn = true, IsEnemyTurn = true; //전투 시작 시 플레이어 or 적 턴 구별

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
