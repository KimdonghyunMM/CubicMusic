using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    public float noteSpeed = 400;

    Image noteImg;

    private void OnEnable()
    {
        if (noteImg == null) noteImg = GetComponent<Image>();
        noteImg.enabled = true;
    }

    private void Update()
    {
        //월드 스페이스가 아닌 캔버스 내에서 움직이도록 localPosition 사용
        transform.localPosition += Vector3.right * noteSpeed * Time.deltaTime;
    }

    public void HideNote()
    {
        noteImg.enabled = false;
    }

    public bool GetNoteFlag()
    {

        return noteImg.enabled;
    }
}
