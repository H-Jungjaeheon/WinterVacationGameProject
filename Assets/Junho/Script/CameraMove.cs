using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;
    public Transform Battletarget;
    public Vector3 offset;
    public bool isright;
    [SerializeField] GameObject BEnemy, BPlayer; //���� ������ ���� ������Ʈ
    [SerializeField] UnityEngine.Camera MCamera;

    public float speed;

    public Vector2 Left_center, Left_size, Right_center, Right_size, center, size;
    

    float height;
    float width;
    private void Start()
    {
        height = UnityEngine.Camera.main.orthographicSize;
        width = height*Screen.width/Screen.height;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Left_center, Left_size);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(Right_center, Right_size);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, size);

    }

    void Update()
    {

        float X = target.position.x;
        if (GameManager.Instance.isRoom == true)
        {
            if (GameObject.Find("X-Axis").transform.position.x < target.position.x) isright = true;
        }
        if (GameManager.Instance.IsBattleStart == false && GameManager.Instance.IsCamMove == false)
        {
            transform.position = target.position + offset;
            MCamera.orthographicSize = 5;
            if (GameManager.Instance.isRoom)
            {
                if (isright)
                {
                    transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);

                    float Ix = Right_size.x * 0.5f - width;
                    float clampX = Mathf.Clamp(transform.position.x, -Ix + Right_center.x, Ix + Right_center.x);

                    float Iy = Right_size.y * 0.5f - height;
                    float clampY = Mathf.Clamp(transform.position.y, -Iy + Right_center.y, Iy + Right_center.y);
                    transform.position = new Vector3(clampX, clampY,-10f);
                }
                else
                {
                    transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);

                    float Ix = Left_size.x * 0.5f - width;
                    float clampX = Mathf.Clamp(transform.position.x, -Ix + Left_center.x, Ix + Left_center.x);

                    float Iy = Left_size.y * 0.5f - height;
                    float clampY = Mathf.Clamp(transform.position.y, -Iy + Left_center.y, Iy + Left_center.y);
                    transform.position = new Vector3(clampX, clampY, -10f);
                }
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);

                float Ix = size.x * 0.5f - width;
                float clampX = Mathf.Clamp(transform.position.x, -Ix + center.x, Ix + center.x);

                float Iy = size.y * 0.5f - height;
                float clampY = Mathf.Clamp(transform.position.y, -Iy + center.y, Iy + center.y);
                transform.position = new Vector3(clampX, clampY, -10f);
            }
        }
        else if (GameManager.Instance.IsBattleStart == true && GameManager.Instance.IsCamMove == true && BattleManager.Instance.CamE == false && BattleManager.Instance.CamP == false)
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