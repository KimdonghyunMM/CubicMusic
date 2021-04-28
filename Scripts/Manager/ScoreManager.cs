using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text txtScore;

    public int increaseScore = 10;
    private int currentScore;

    public float[] weight;
    public int comboBonusScore = 10;

    Animator Ani;
    string aniScoreUp = "ScoreUp";

    ComboManager combo;

    void Start()
    {
        combo = FindObjectOfType<ComboManager>();
        Ani = GetComponent<Animator>();
        currentScore = 0;
        txtScore.text = "0";
    }

    public void Initialized()
    {
        currentScore = 0;
        txtScore.text = "0";
    }

    public void IncreaseScore(int p_judgementState)
    {
        //콤보
        combo.IncreaseCombo();

        //콤보 보너스 점수 계산
        int t_currentCombo = combo.GetCurrentCombo();
        int t_bonusComboScore = (t_currentCombo / 10) * comboBonusScore;

        //가중치 계산
        //판정마다 점수배수가 다름 Perfect >> Cool >> Gool >> Bad
        int t_increaseScore = increaseScore + t_bonusComboScore;
        t_increaseScore = (int)(t_increaseScore * weight[p_judgementState]);

        //점수반영
        currentScore += t_increaseScore;
        txtScore.text = string.Format("{0:#,##0}", currentScore);

        //스코어 애니메이션 실행
        Ani.SetTrigger(aniScoreUp);
    }
 
    public int GetCurrentScore()
    {
        return currentScore;
    }
}
