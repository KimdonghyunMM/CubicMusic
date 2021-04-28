using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public int bpm = 0;
    double currentTime = 0.0d;

   

    public Transform tfNoteAppear;
    //public GameObject goNote;

    TimingManager Tm;
    EffectManager Em;
    ComboManager Cm;

    void Start()
    {
        Tm = GetComponent<TimingManager>();
        Em = FindObjectOfType<EffectManager>();
        Cm = FindObjectOfType<ComboManager>();
    }

    void Update()
    {
        if(GameManager.instance.isStartGame)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= 60.0d / bpm)
            {
                //오브젝트 풀링
                GameObject t_note = ObjPool.instance.noteQueue.Dequeue();
                t_note.transform.position = tfNoteAppear.position;
                t_note.SetActive(true);

                /* 가비지컬렉터로 힙에서 제거
                GameObject t_note = Instantiate(goNote, tfNoteAppear.position, Quaternion.identity);
                //NoteManager 스크립트를 포함하고 있는 객체를 부모클래스로 지정
                //이렇게 해야 노트 이미지가 캔버스를 부모객체로 인식하여 UI로 등장한다.
                //안 그러면 하이어라키에서 생성은 되지만 캔버스 밖에 이미지가 나와 안 보임
                //즉 이미지는 캔버스 안에 있어야만 볼 수 있음
                t_note.transform.SetParent(this.transform);
                */

                //노트가 생성되는 순간 노트 리스트에 노트를 담음
                Tm.boxNoteList.Add(t_note);

                //0으로 초기화하지 않고 오차를 줄이기위해 숫자를 빼준다
                //0으로 초기화하면 만약 currentTime이 0.510055101 일 때 0.010055101만큼 오차가 생김
                //그래서 초기화를 위해 currentTime을 0으로 선언하는게 아닌 뺴준다
                currentTime -= 60.0d / bpm;
            }
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Note"))
        {
            //노트의 이미지가 활성화 상태일때만 Miss를 띄움
            //노트의 이미지가 비활성화상태(노트를 판정시켰을 때)에서는 MISS가 안뜸
            if (collision.GetComponent<Note>().GetNoteFlag())
            {
                Tm.MissRecord();
                Em.JudgementEffect(4);
                Cm.ResetCombo();
            }

            //리스트에서 노트 제거
            Tm.boxNoteList.Remove(collision.gameObject);

            ObjPool.instance.noteQueue.Enqueue(collision.gameObject);
            collision.gameObject.SetActive(false);
            //노트 파괴
            //Destroy(collision.gameObject);
        }
            
    }

    public void RemoveNote()
    {
        GameManager.instance.isStartGame = false;

        for (int i = 0; i < Tm.boxNoteList.Count; i++)
        {
            Tm.boxNoteList[i].SetActive(false);
            ObjPool.instance.noteQueue.Enqueue(Tm.boxNoteList[i]);
        }

        Tm.boxNoteList.Clear();
    }
}
