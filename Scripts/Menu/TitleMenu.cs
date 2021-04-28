using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenu : MonoBehaviour
{
    public GameObject goStageUI;

    public void BtnPlay()
    {
        goStageUI.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
