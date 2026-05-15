using System;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class GameManager : MonoBehaviour
{
    private GameState currentState;
    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        TouchSimulation.Enable();
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
        TouchSimulation.Disable();
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