using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingManager : MonoBehaviour
{

    //판정범위에 있는지 모든 노트를 비교함
    public List<GameObject> boxNoteList = new List<GameObject>();

    int[] judgementRecord = new int[5];

    //판정될 중앙
    public Transform Center;
    //판정범위에 따라 점수를 다르게함(Perfect, Cool, Good, Bad)
    public RectTransform[] timingRect;
    //판정 범위
    public Vector2[] timingBox;

    EffectManager Em;
    ScoreManager Scm;
    ComboManager Cm;
    StageManager Stm;
    PlayerController Pc;
    StatusManager Sm;
    AudioManager Am;

    private void Start()
    {
        //이펙트매니저 참조
        Em = FindObjectOfType<EffectManager>();
        //스코어매니저 참조
        Scm = FindObjectOfType<ScoreManager>();
        //콤보매니저 참조
        Cm = FindObjectOfType<ComboManager>();
        //스테이지매니저 참조
        Stm = FindObjectOfType<StageManager>();
        //플레이어컨트롤러 참조
        Pc = FindObjectOfType<PlayerController>();
        //스테이터스매니저 참조
        Sm = FindObjectOfType<StatusManager>();
        //오디오매니저 참조
        Am = AudioManager.instance;

        //타이밍 박스
        timingBox = new Vector2[timingRect.Length];

        for (int i = 0; i < timingRect.Length; i++)
        {
            timingBox[i].Set(Center.localPosition.x - timingRect[i].rect.width / 2,   //판정 최소값(x)
                               Center.localPosition.x + timingRect[i].rect.width / 2);  //판정 최대값(y)
        }
    }

    public bool CheckTiming()
    {
        for (int i = 0; i < boxNoteList.Count; i++)
        {
            //노트가 판정 범위에 들어왔는지 체크할 X좌표
            float t_notePosX = boxNoteList[i].transform.localPosition.x;
            //판정 범위 감지
            for (int j = 0; j < timingBox.Length; j++)
            {
                //판정처리 될 범위
                if (timingBox[j].x <= t_notePosX && t_notePosX <= timingBox[j].y)
                {
                    //Hit 판정시 파괴하지 않고 이미지를 비활성화 시킨다.
                    boxNoteList[i].GetComponent<Note>().HideNote();
                    //Hit 판정시 리스트에서 판정처리된 노트는 삭제한다.
                    boxNoteList.RemoveAt(i);

                    //Hit 판정(perfect, cool, good)시 Hit이펙트 애니메이션 작동
                    if (j < timingBox.Length - 1) Em.NoteHitEffect();




                    //Raycast를 이용하여 다음에 나올 발판을 감지함
                    if (CheckCanNextPlate())
                    {
                        //점수 증가
                        Scm.IncreaseScore(j);
                        //바닥 등장
                        Stm.ShowNextPlate();
                        //인덱스를 통해 판정 이펙트를 다르게 등장시킴(판정 연출)
                        Em.JudgementEffect(j);
                        //판정 기록
                        judgementRecord[j]++;
                        //실드체크
                        Sm.CheckShield();
                    }
                    else
                    {
                        //이미 밟은 발판을 밟으면 점수가 안올라가고 normal 이펙트를 띄움
                        Em.JudgementEffect(5);
                    }

                    Am.PlaySFX("Clap");

                    return true;
                }
            }
        }
        Cm.ResetCombo();
        //timingBox의 배열 갯수가 4이므로 4를 넣어도댐
        Em.JudgementEffect(timingBox.Length);
        MissRecord();
        return false;
    }

    bool CheckCanNextPlate()
    {
        if (Physics.Raycast(Pc.destPos, Vector3.down, out RaycastHit t_hitInfo, 1.3f))
        {
            if (t_hitInfo.transform.CompareTag("BasicPlate"))
            {
                BasicPlate t_plate = t_hitInfo.transform.GetComponent<BasicPlate>();
                if (t_plate.flag)
                {
                    t_plate.flag = false;
                    return true;
                }
            }
        }
        return false;
    }

    public int[] GetJudgementRecord()
    {
        return judgementRecord;
    }

    public void MissRecord()
    {
        judgementRecord[4]++;
        Sm.ResetShieldCombo();
        Sm.DecreaseHp(1);
    }

    public void Initialized()
    {
        for (int i = 0; i < judgementRecord.Length; i++)
        {
            judgementRecord[i] = 0;
        }

    }
}
