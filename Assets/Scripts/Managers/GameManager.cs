using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class GameManager : MonoBehaviour
{
    private GameState currentState = GameState.Playing;
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
        if (currentState != GameState.GameOver) return;
        #if UNITY_EDITOR
            EditorApplication.isPaused = true;
        #endif
        Debug.Log("OYUN BİTTİ!");
    }

    private void Pause()
    {
        throw new NotImplementedException();
    }

    private void Play()
    {
        if (currentState != GameState.Playing) return;

        var spawner = BlockSpawner.Instance;
        var board = BoardManager.Instance;
        if (spawner == null || board == null) return;

        var blocks = spawner.CurrentBlocks;
        // Henüz blok yoksa (set spawnlanmadan önceki frame) game over kararı verme
        if (blocks.Count == 0) return;

        // Elimdeki bloklardan en az biri herhangi bir yere sığıyorsa oyun devam eder.
        // Unity'nin fake-null davranışı yüzünden Count > 0 olsa bile referansların hepsi
        // destroy edilmiş olabilir; bu durumda spawner henüz ayıklayıp respawn etmediği
        // için GameOver tetiklememeliyiz.
        bool anyAlive = false;
        foreach (var block in blocks)
        {
            if (block == null) continue;
            anyAlive = true;
            if (board.CanShapeFitAnywhere(block.Shape)) return;
        }

        if (!anyAlive) return;

        // Hiçbiri sığmıyor: oyun biter
        currentState = GameState.GameOver;
    }
}
