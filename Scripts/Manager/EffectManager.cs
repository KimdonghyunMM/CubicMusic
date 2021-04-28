using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour
{
    public Animator hitAni;
    string hit = "Hit";

    public Animator judgeAni;
    public Image    judgeImg;
    public Sprite[] judgeSprite;
                                
                                
    public void JudgementEffect(int p_num)
                                //파라미터에 맞는 판정이미지 스프라이트 교체
    {
        judgeImg.sprite = judgeSprite[p_num];
        judgeAni.SetTrigger(hit);
    }

    public void NoteHitEffect()
    {
        hitAni.SetTrigger(hit);
    }

}
