using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyRotate : MonoBehaviour
{
    public float rotSpeed;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isStartGame)
            RenderSettings.skybox.SetFloat("_Rotation", rotSpeed * Time.time);
    }
}
