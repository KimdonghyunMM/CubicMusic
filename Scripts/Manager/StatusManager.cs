using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusManager : MonoBehaviour
{
    public float blinkSpeed = 0.1f;
    public int blinkCount = 15;
    int currentBlinkCount = 0;

    bool isBlink;
    bool isDead;

    int maxHp = 3;
    int currentHp = 3;

    int maxShield = 3;
    int currentShield = 0;

    public Image[] hpImg;
    public Image[] shieldImg;

    public int shieldIncreaseCombo = 5;
    int currentShieldCombo = 0;
    public Image shieldGauge;

    public MeshRenderer playerMesh;

    Result _result;
    NoteManager Nm;


    private void Start()
    {
        _result = FindObjectOfType<Result>();
        Nm = FindObjectOfType<NoteManager>();
    }

    public void Initialized()
    {
        currentHp = maxHp;
        currentShield = 0;
        currentShieldCombo = 0;
        shieldGauge.fillAmount = 0;
        isDead = false;
        SettingHpImage();
        SettingShieldImage();
    }

    public void CheckShield()
    {
        currentShieldCombo++;

        if (currentShieldCombo >= shieldIncreaseCombo)
        {
            currentShieldCombo = 0;
            IncreaseShield();
        }

        shieldGauge.fillAmount = (float)currentShieldCombo / shieldIncreaseCombo;
    }

    public void ResetShieldCombo()
    {
        currentShieldCombo = 0;
        shieldGauge.fillAmount = (float)currentShieldCombo / shieldIncreaseCombo;
    }

    public void IncreaseShield()
    {
        currentShield++;

        if (currentShield >= maxShield) currentShield = maxShield;

        SettingShieldImage();
    }

    public void DecreaseShield(int p_num)
    {
        currentShield -= p_num;

        if (currentShield <= 0) currentShield = 0;

        SettingShieldImage();
    }

    public void IncreaseHp(int p_num)
    {
        currentHp += p_num;
        if (currentHp >= maxHp) currentHp = maxHp;

        SettingHpImage();
    }

    public void DecreaseHp(int p_num)
    {
        if (!isBlink)
        {
            if (currentShield >= 1) DecreaseShield(p_num);
            else
            {
                currentHp -= p_num;

                if (currentHp <= 0)
                {
                    isDead = true;
                    _result.ShowResult();
                    Nm.RemoveNote();
                }
                else StartCoroutine(BlinkCo());

                SettingHpImage();
            }
        }
    }

    void SettingHpImage()
    {
        for (int i = 0; i < hpImg.Length; i++)
        {
            if (i < currentHp)
                hpImg[i].gameObject.SetActive(true);
            else
                hpImg[i].gameObject.SetActive(false);
        }
    }

    void SettingShieldImage()
    {
        for (int i = 0; i < shieldImg.Length; i++)
        {
            if (i < currentShield)
                shieldImg[i].gameObject.SetActive(true);
            else
                shieldImg[i].gameObject.SetActive(false);
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    IEnumerator BlinkCo()
    {
        isBlink = true;

        while (currentBlinkCount <= blinkCount)
        {
            playerMesh.enabled = !playerMesh.enabled;
            yield return new WaitForSeconds(blinkSpeed);
            currentBlinkCount++;
        }

        playerMesh.enabled = true;
        currentBlinkCount = 0;
        isBlink = false;
    }
}
