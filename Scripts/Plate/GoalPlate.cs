using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPlate : MonoBehaviour
{
    AudioSource As;
    NoteManager Nm;

    Result _result;

    void Start()
    {
        As = GetComponent<AudioSource>();
        Nm = FindObjectOfType<NoteManager>();
        _result = FindObjectOfType<Result>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            As.Play();
            PlayerController.s_canPresskey = false;
            Nm.RemoveNote();
            _result.ShowResult();
        }
    }
}
