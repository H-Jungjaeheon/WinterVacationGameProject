using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; set; }
    public GameObject[] Enemy; //���� ���� �� ���� �ʵ忡 ��ȯ�� �� �迭
    public GameObject EnemySpawner; //���� ���� �� ���� �ʵ忡 ��ȯ�� �� ��ġ
    public bool IsPlayerTurn = true, IsEnemyTurn = true; //���� ���� �� �÷��̾� or �� �� ����

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
