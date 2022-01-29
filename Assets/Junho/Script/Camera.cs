using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;
    public Transform Battletarget;
    public Vector3 offset;
    [SerializeField] GameObject BEnemy, BPlayer; //공격 연출을 위한 오브젝트
    [SerializeField] UnityEngine.Camera MCamera;

    private void Awake()
    {
        //MCamera.GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsBattleStart == false && GameManager.Instance.IsCamMove == false)
        {
            transform.position = target.position + offset;
            MCamera.orthographicSize = 5;
        }
        else if(GameManager.Instance.IsBattleStart == true && GameManager.Instance.IsCamMove == true && BattleManager.Instance.CamE == false && BattleManager.Instance.CamP == false)
        {
            BattleCameraMove();
        }
        else if (BattleManager.Instance.CamE == true)
        {
            transform.position = BPlayer.transform.position + offset + new Vector3(2, -3, 0);
            MCamera.orthographicSize = 3f;
        }
        else if (BattleManager.Instance.CamP == true)
        {
            transform.position = BEnemy.transform.position + offset + new Vector3(-2, -3, 0);
            MCamera.orthographicSize = 3f;
        }
    }
    void BattleCameraMove()
    {
        this.transform.position = Battletarget.position + offset;
        MCamera.orthographicSize = 5;
    }
}
