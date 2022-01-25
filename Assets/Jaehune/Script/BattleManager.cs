using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; set; }
    public GameObject[] Enemy;
    public GameObject EnemySpawner;
    public bool IsPlayerTurn = true, IsEnemyTurn = false;

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
