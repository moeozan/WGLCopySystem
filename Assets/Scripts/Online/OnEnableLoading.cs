using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnEnableLoading : MonoBehaviour
{
    float timer = 0;
    [SerializeField] private Text lobbyTimer;
    [SerializeField] private Text roomLink;

    private void StartLobbyTimer()
    {
        timer += Time.deltaTime;
        if (timer > 0)
        {
            lobbyTimer.text = timer.ToString("N0");
        }
    }
    private void Update()
    {
        StartLobbyTimer();
    }
    private void OnDisable()
    {
        timer = 0;
    }
}
