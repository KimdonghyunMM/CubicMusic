using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    public GameObject goUI;

    public Text[] txtCount;
    public Text txtCoin;
    public Text txtScore;
    public Text txtMaxCombo;

    int currentSong = 0;

    ScoreManager Sm;
    ComboManager Cm;
    TimingManager Tm;
    DataBaseManager Dm;

    // Start is called before the first frame update
    void Start()
    {
        Sm = FindObjectOfType<ScoreManager>();
        Cm = FindObjectOfType<ComboManager>();
        Tm = FindObjectOfType<TimingManager>();
        Dm = FindObjectOfType<DataBaseManager>();
    }

    public void ShowResult()
    {
        FindObjectOfType<CenterFrame>().ResetMusic();
        AudioManager.instance.StopBGM();

        goUI.SetActive(true);

        for (int i = 0; i < txtCount.Length; i++) txtCount[i].text = "0";

        txtCoin.text = "0";
        txtScore.text = "0";
        txtMaxCombo.text = "0";

        int[] t_judgement = Tm.GetJudgementRecord();
        int t_currentScore = Sm.GetCurrentScore();
        int t_maxCombo = Cm.GetMaxCombo();
        int t_coin = t_currentScore / 50;

        for (int i = 0; i < txtCount.Length; i++)
        {
            txtCount[i].text = string.Format("{0:#,##0}", t_judgement[i]);
        }
        txtScore.text = string.Format("{0:#,##0}", t_currentScore);
        txtMaxCombo.text = string.Format("{0:#,##0}", t_maxCombo);
        txtCoin.text = string.Format("{0:#,##0}", t_coin);

        if (t_currentScore > Dm.score[currentSong])
        {
            Dm.SaveScore();
            Dm.score[currentSong] = t_currentScore;
        }
    }

    public void SetCurrentSong(int p_songNum)
    {
        currentSong = p_songNum;
    }

    public void BtnMainMenu()
    {
        goUI.SetActive(false);
        GameManager.instance.MainMenu();
        Cm.ResetCombo();
    }
}
