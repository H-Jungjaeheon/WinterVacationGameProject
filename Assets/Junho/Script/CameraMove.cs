using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraMove : MonoBehaviour
{
    public Transform target, Battletarget;
    public Vector3 offset, initialPosition;
    public bool isright, IsFarAway = false, IsGrab = false, IsBossMeet = false, BossBattleStart = false, IsBossCamMove, IsLastBoss;
    [SerializeField] GameObject BEnemy, BPlayer; //공격 연출을 위한 오브젝트
    public GameObject Player;
    [SerializeField] UnityEngine.Camera MCamera;
    [SerializeField] float ShakeAmount;
    private float ShakeTime;
    public float speed;
    public Vector2 Left_center, Left_size, Right_center, Right_size, center, size;
    float height, width;
    public PostProcessVolume volume;
    private Vignette vognette;

    private void Start()
    {
        IsLastBoss = false;
        BossBattleStart = false;
        IsBossCamMove = false;
        volume.profile.TryGetSettings(out vognette);
        vognette.intensity.value = 0;
        IsFarAway = false;
        height = UnityEngine.Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;
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
    private void Update()
    {
        if (IsGrab == true)
        {
            vognette.color.value = new Color(1, 0, 0, 1);
            vognette.intensity.value = 0.3f; 
        }
        else
        {
            vognette.color.value = new Color(0, 0, 0, 1);
            vognette.intensity.value = 0.2f;
        }
        if(GameManager.Instance.IsBattleStart == true)
        {
            IsGrab = false;
        }
    }
    void FixedUpdate()
    {

        float X = target.position.x;

        if (GameManager.Instance.IsBattleStart == false && GameManager.Instance.IsCamMove == false)
        {
            MCamera.orthographicSize = 4.6f;          
            if (GameManager.Instance.isRoom)
            {
                if (GameObject.Find("X-Axis").transform.position.x < target.position.x) isright = true;
                else isright = false;

                if (isright)
                {
                    transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);

                    float Ix = Right_size.x * 0.5f - width;
                    float clampX = Mathf.Clamp(transform.position.x, -Ix + Right_center.x, Ix + Right_center.x);


                    transform.position = new Vector3(clampX, target.transform.position.y + offset.y, -10f);
                }
                else
                {
                    //MCamera.orthographicSize = 5;

                    transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);

                    float Ix = Left_size.x * 0.5f - width;
                    float clampX = Mathf.Clamp(transform.position.x, -Ix + Left_center.x, Ix + Left_center.x);


                    transform.position = new Vector3(clampX, target.transform.position.y + offset.y, -10f);
                }
            }
            else if (BossBattleStart == true)
            {
                StartCoroutine(BossBattleStartCam(0.5f));
            }
            else
            {
                if (IsGrab == true)
                {
                    transform.position = Random.insideUnitSphere * (ShakeAmount - 0.4f) + target.position;
                }
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
            if (IsBossCamMove == false)
            {
                if (IsFarAway == false)
                {
                    transform.position = BPlayer.transform.position + offset + new Vector3(2, 2, 0);
                    MCamera.orthographicSize = 3f;
                }
                else if(IsFarAway == true)
                {
                    transform.position = BPlayer.transform.position + offset + new Vector3(2, 2, 0);
                    MCamera.orthographicSize = 3.7f;
                }
            }
            else
            {
                if (IsFarAway == false)
                {
                    transform.position = BPlayer.transform.position + offset + new Vector3(2, 2, 0);
                    MCamera.orthographicSize = 5f;
                }
                else if(IsFarAway == true)
                {
                    transform.position = BPlayer.transform.position + offset + new Vector3(2, 2, 0);
                    MCamera.orthographicSize = 7f;
                }
            }
        }
        else if (BattleManager.Instance.CamP == true)
        {
            if (IsBossCamMove == false)
            {
                if (IsFarAway == false)
                {
                    transform.position = BEnemy.transform.position + offset + new Vector3(-2, 2, 0);
                    MCamera.orthographicSize = 3f;
                }
                else if (IsFarAway == true)
                {
                    transform.position = BEnemy.transform.position + offset + new Vector3(-2, 2, 0);
                    MCamera.orthographicSize = 3.7f;
                }
            }
            else
            {
                if (IsFarAway == false)
                {
                    transform.position = BEnemy.transform.position + offset + new Vector3(-2, 2, 0);
                    MCamera.orthographicSize = 5f;
                }
                else if (IsFarAway == true)
                {
                    transform.position = BEnemy.transform.position + offset + new Vector3(-2, 2, 0);
                    MCamera.orthographicSize = 7f;
                }
            }
        }
        if (GameManager.Instance.IsBattleStart == true && GameManager.Instance.IsCamMove == true)
        {
            if (BattleManager.Instance.CamE == true)
            {
                initialPosition = BPlayer.transform.position + offset + new Vector3(2, -1.7f, 0);
                if (ShakeTime > 0)
                {
                    transform.position = Random.insideUnitSphere * ShakeAmount + initialPosition;
                    ShakeTime -= Time.deltaTime;
                }
                else
                {
                    ShakeTime = 0.0f;
                    transform.position = initialPosition;
                }
            }
            else if (BattleManager.Instance.CamP == true)
            {
                initialPosition = BEnemy.transform.position + offset + new Vector3(-2, -1.7f, 0);
                if (ShakeTime > 0)
                {
                    transform.position = Random.insideUnitSphere * ShakeAmount + initialPosition;
                    ShakeTime -= Time.deltaTime;
                }
                else
                {
                    ShakeTime = 0.0f;
                    transform.position = initialPosition;
                }
            }
        }
        if(IsBossMeet == true)
        {
            initialPosition = Player.transform.position + offset;
            if (ShakeTime > 0)
            {
                transform.position = Random.insideUnitSphere * ShakeAmount + initialPosition;
                ShakeTime -= Time.deltaTime;
            }
            else
            {
                ShakeTime = 0.0f;
                transform.position = initialPosition;
                IsBossMeet = false;
            }
        }
    }
    void BattleCameraMove()
    {
        if (BossBattleStart == false)
        {
            this.transform.position = Battletarget.position + offset + new Vector3(0, 0.4f, 0);
            MCamera.orthographicSize = 4.5f;
        }
        else
        {
            this.transform.position = Battletarget.position + offset + new Vector3(0, 0.4f, 0);
            StartCoroutine(BossBattleStartCam(0.5f));
        }
    }
    public void VibrateForTime(float time)
    {
        ShakeTime = time;
    }
    public void VibrateForTime2(float time)
    {
        IsBossMeet = true;
        ShakeTime = time;
    }
    IEnumerator BossBattleStartCam(float time)
    {
        if(IsBossCamMove == false)
        {
            float a = MCamera.orthographicSize;
            if(IsLastBoss == false)
            {
                while (a < 10)
                {
                    a += Time.deltaTime / time;
                    MCamera.orthographicSize = a;
                    if (a >= 10)
                    {
                        a = 10;
                        IsBossCamMove = true;
                    }
                    yield return null;
                }
            }
            else
            {
                while (a < 14)
                {
                    a += Time.deltaTime / time;
                    MCamera.orthographicSize = a;
                    if (a >= 14)
                    {
                        a = 14;
                        IsBossCamMove = true;
                    }
                    yield return null;
                }
            }
        }
        else
        {
            if (IsLastBoss == false)
            {
                MCamera.orthographicSize = 10;
            }
            else
            {
                MCamera.orthographicSize = 14;
            }
        }
    }
}
