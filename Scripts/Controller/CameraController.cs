using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //플레이어
    public Transform Player;
    //카메라가 따라가는 속도
    public float Speed = 15;
    //카메라와 플레이어 간 거리
    Vector3 Distance = new Vector3();

    float hitDistance;
    public float zoomDistance = -1.25f;

    void Start()
    {
        Distance = transform.position - Player.position;
    }


    void Update()
    {
        Vector3 t_destPos = Player.position + Distance + (transform.forward * hitDistance);
        //                           변수 A와 B 사이의 값에서 C 비율의 값을 추출
        transform.position = Vector3.Lerp(transform.position, t_destPos, Speed * Time.deltaTime);
    }

    public IEnumerator ZoomCam()
    {
        hitDistance = zoomDistance;

        yield return new WaitForSeconds(0.15f);

        hitDistance = 0;
    }
}
