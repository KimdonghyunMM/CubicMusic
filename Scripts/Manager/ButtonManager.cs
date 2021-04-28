using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    private void Start()
    {
        this.gameObject.SetActive(false);
    }
    private void Update()
    {
        if(!GameManager.instance.isStartGame)
        {
            this.gameObject.SetActive(false);

        }
    }
}
