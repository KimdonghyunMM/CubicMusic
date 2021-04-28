using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]       //인스펙터창에서 컨트롤하기위함
public class ObjectInfo
{
    public GameObject goPrefab;     //풀링할 프리팹
    public int count;               //프리팹 갯수
    public Transform tfPoolParent;  //프리팹 위치를 설정할 부모객체
}

public class ObjPool : MonoBehaviour
{
    //싱글톤
    public static ObjPool instance;

    public ObjectInfo[] objInfo;

    public Queue<GameObject> noteQueue = new Queue<GameObject>();


    private void Awake()
    {
        //싱글톤
        if (instance != this) instance = this;
    }
    private void Start()
    {
        noteQueue = InsertQueue(objInfo[0]);    
    }

    //GameObject의 타입을 저장하는 큐를 반환하는 함수
    Queue<GameObject> InsertQueue(ObjectInfo p_objinfo)
    {
        //임시 큐
        Queue<GameObject> t_queue = new Queue<GameObject>();
        for (int i = 0; i < p_objinfo.count; i++)
        {
            GameObject t_clone = Instantiate(p_objinfo.goPrefab, transform.position, Quaternion.identity);
            t_clone.SetActive(false);
            //부모객체 설정
            //부모객체가 존재하면 그대로 그 객체를 부모객체로 사용
            if (p_objinfo.tfPoolParent != null) t_clone.transform.SetParent(p_objinfo.tfPoolParent);
            //부모객체가 없으면 이 스크립트를 포함한 객체를 부모객체로 지정
            else t_clone.transform.SetParent(this.transform);

            t_queue.Enqueue(t_clone);
        }

        return t_queue;
    }
}
