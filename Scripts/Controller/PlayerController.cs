using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    public static bool s_canPresskey = true;

    [Header("Move")]
    public float moveSpeed = 3;
    Vector3 dir = new Vector3();
    Vector3 originPos = new Vector3();
    public Vector3 destPos = new Vector3();

    [Header("SpinMove")]
    public float spinSpeed = 270;
    Vector3 rotDir = new Vector3();
    Quaternion destRot = new Quaternion();

    [Header("Recoil")]
    public float recoilPosY = 0.25f;
    public float recoilSpeed = 1.5f;

    [Header("DeadZone")]
    public GameObject deadZone;


    bool canMove = true;
    bool isFalling;

    [Header("CUBE")]
    //가짜 큐브를 먼저 돌리고 그 큐브가 돌아간 만큼의 값을 목표 회전값으로 삼음
    public Transform fakeCube;
    public Transform realCube;

    TimingManager Tm;
    StatusManager Sm;
    CameraController Cam;
    Rigidbody Rb;

    private void Start()
    {
        Tm = FindObjectOfType<TimingManager>();
        Sm = FindObjectOfType<StatusManager>();
        Cam = FindObjectOfType<CameraController>();
        Rb = GetComponentInChildren<Rigidbody>();   //자식객체에 있는 컴포넌트를 찾을 때
        originPos = transform.position;

        

    }

    public void Initialized()
    {
        transform.position = Vector3.zero;
        destPos = Vector3.zero;
        realCube.localPosition = Vector3.zero;
        canMove = true;
        s_canPresskey = true;
        isFalling = false;
        Rb.useGravity = false;
        Rb.isKinematic = true;
    }



    void Update()
    {
        if (GameManager.instance.isStartGame)
        {
            CheckFalling();

            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
            {
                if (canMove && s_canPresskey && !isFalling)
                {
                    //판정을 체크하기전에 움직일 값을 미리 계산해놓는다.
                    Calculate(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"));

                    //판정 체크
                    if (Tm.CheckTiming())
                    {
                        StartAction();
                    }
                }
            }
        }

        deadZone.transform.position = new Vector3(realCube.position.x, -5.0f, realCube.position.z);

    }

    public void BtnUp()
    {
        if (GameManager.instance.isStartGame)
        {
            CheckFalling();

            if (canMove && s_canPresskey && !isFalling)
            {
                //판정을 체크하기전에 움직일 값을 미리 계산해놓는다.
                Calculate(1.0f, 0.0f);

                //판정 체크
                if (Tm.CheckTiming())
                {
                    StartAction();
                }
            }
        }
    }
    public void BtnDown()
    {
        if (GameManager.instance.isStartGame)
        {
            CheckFalling();

            if (canMove && s_canPresskey && !isFalling)
            {
                //판정을 체크하기전에 움직일 값을 미리 계산해놓는다.
                Calculate(-1.0f, 0.0f);

                //판정 체크
                if (Tm.CheckTiming())
                {
                    StartAction();
                }
            }
        }
    }
    public void BtnLeft()
    {
        if (GameManager.instance.isStartGame)
        {
            CheckFalling();

            if (canMove && s_canPresskey && !isFalling)
            {
                //판정을 체크하기전에 움직일 값을 미리 계산해놓는다.
                Calculate(0.0f, -1.0f);

                //판정 체크
                if (Tm.CheckTiming())
                {
                    StartAction();
                }
            }
        }
    }
    public void BtnRight()
    {
        if (GameManager.instance.isStartGame)
        {
            CheckFalling();

            if (canMove && s_canPresskey && !isFalling)
            {
                //판정을 체크하기전에 움직일 값을 미리 계산해놓는다.
                Calculate(0.0f, 1.0f);

                //판정 체크
                if (Tm.CheckTiming())
                {
                    StartAction();
                }
            }
        }
    }


    void Calculate(float v, float h)
    {
        //방향 계산
        dir.Set(v, 0, h);

        //이동 목표값 계산
        destPos = transform.position + new Vector3(-dir.x, 0, dir.z);

        //회전 목표값 계산
        rotDir = new Vector3(-dir.z, 0, -dir.x);
        //공전할떄 사용하는 함수
        fakeCube.RotateAround(transform.position, rotDir, spinSpeed);
        destRot = fakeCube.rotation;
    }

    void CheckFalling()
    {
        if (!isFalling && canMove)
        {
            if (!Physics.Raycast(transform.position, Vector3.down, 1.1f))
            {
                Falling();
            }
        }
    }

    void Falling()
    {
        isFalling = true;
        Rb.useGravity = true;
        Rb.isKinematic = false;
    }

    public void ResetFalling()
    {
        Sm.DecreaseHp(1);
        AudioManager.instance.PlaySFX("Falling");

        if (!Sm.IsDead())
        {
            isFalling = false;
            Rb.useGravity = false;
            Rb.isKinematic = true;

            //Player 원위치
            transform.position = originPos;
            //Cube 원위치
            realCube.localPosition = new Vector3(0, 0, 0);
        }
    }

    void StartAction()
    {
        //코루틴 함수 실행
        StartCoroutine(MoveCo());
        StartCoroutine(SpinCo());
        StartCoroutine(RecoilCo());
        StartCoroutine(Cam.ZoomCam());
    }

    IEnumerator MoveCo()
    {
        canMove = false;
        //좌표간 거리차 반환
        //              제곱근리턴 함수
        while (Vector3.SqrMagnitude(transform.position - destPos) >= 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, destPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = destPos;
        canMove = true;
    }

    IEnumerator SpinCo()
    {
        while (Quaternion.Angle(realCube.rotation, destRot) > 0.5f)
        {
            realCube.rotation = Quaternion.RotateTowards(realCube.rotation, destRot, spinSpeed * Time.deltaTime);
            yield return null;
        }

        realCube.rotation = destRot;
    }

    IEnumerator RecoilCo()
    {
        while (realCube.position.y < recoilPosY)
        {
            realCube.position += new Vector3(0, recoilSpeed * Time.deltaTime, 0);
            yield return null;
        }

        while (realCube.position.y > 0)
        {
            realCube.position -= new Vector3(0, recoilSpeed * Time.deltaTime, 0);
            yield return null;
        }

        realCube.localPosition = new Vector3(0, 0, 0);
    }
}
