using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Song
{
    public string name;
    public string composer;
    public int bpm;
    public Sprite sprite;
}

public class StageMenu : MonoBehaviour
{
    public Song[] songList;
    public Text txtSongName;
    public Text txtSongComposer;
    public Text txtSongScore;
    public Image imgDisk;

    public GameObject TitleMenu;
    public Button[] Button;

    DataBaseManager Dm;

    int currentSong = 0;
    private void Start()
    {
 
    }

    private void OnEnable()
    {
        if (Dm == null)
            Dm = FindObjectOfType<DataBaseManager>();
        SettingSong();
    }

    public void BtnNext()
    {
        AudioManager.instance.PlaySFX("Touch");
        if (++currentSong > songList.Length - 1)
            currentSong = 0;
        SettingSong();
    }
    public void BtnPrior()
    {
        AudioManager.instance.PlaySFX("Touch");
        if (--currentSong < 0)
            currentSong = songList.Length - 1;
        SettingSong();
    }
    void SettingSong()
    {
        txtSongName.text = songList[currentSong].name;
        txtSongComposer.text = songList[currentSong].composer;
        txtSongScore.text = string.Format("{0:#,##0}", Dm.score[currentSong]);
        imgDisk.sprite = songList[currentSong].sprite;

        AudioManager.instance.PlayerBGM("BGM" + currentSong);
    }

    public void BtnBack()
    {
        TitleMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void BtnPlay()
    {
        int t_bpm = songList[currentSong].bpm;

        GameManager.instance.GameStart(currentSong, t_bpm);
        this.gameObject.SetActive(false);
        for (int i = 0; i < Button.Length; i++)
        {
            Button[i].GetComponent<ButtonManager>().gameObject.SetActive(true);
        }
    }
}
