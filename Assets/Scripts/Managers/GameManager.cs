using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameState currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ManageState();
    }

    private void ManageState()
    {
        switch (currentState)
        {
            case GameState.Playing:
                Play();
                break;
            case GameState.Paused:
                Pause();
                break;
            case GameState.GameOver:
                GameOver();
                break;  
            default:
                Debug.LogError("Tanımsız bir GameState ile işlem yapılmaya çalışılıyor.");
                break;      
        }
    }

    private void GameOver()
    {
        throw new NotImplementedException();
    }

    private void Pause()
    {
        throw new NotImplementedException();
    }

    private void Play()
    {
        throw new NotImplementedException();
    }
}