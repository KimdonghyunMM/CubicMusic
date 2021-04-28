using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    public GameObject comboImg;
    public Text comboText;

    int currentCombo;
    int maxCombo;

    Animator Ani;
    string aniComboUp = "ComboEffect";

    private void Start()
    {
        Ani = GetComponent<Animator>();
        comboText.gameObject.SetActive(false);
        comboImg.SetActive(false);
    }

    public void IncreaseCombo(int p_num = 1)
    {
        currentCombo += p_num;
        comboText.text = string.Format("{0:#,##0}", currentCombo);

        if (currentCombo > maxCombo) maxCombo = currentCombo;

        if (currentCombo > 2)
        {
            comboText.gameObject.SetActive(true);
            comboImg.SetActive(true);

            Ani.SetTrigger(aniComboUp);
        }
    }
    
    public int GetCurrentCombo()
    {
        return currentCombo;
    }
    public int GetMaxCombo()
    {
        return maxCombo;
    }

    public void ResetCombo()
    {
        currentCombo = 0;
        comboText.text = "0";
        comboText.gameObject.SetActive(false);
        comboImg.SetActive(false);
    }
}
