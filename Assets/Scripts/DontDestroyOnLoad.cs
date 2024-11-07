using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    [SerializeField] private float _score;
    [SerializeField] private float _time;
    private void Awake()
    {
        DontDestroyOnLoad(this);
        _score = 0;
        _time = 0;
    }

    private void OnEnable()
    {
        GameManager.OnGameOver += HandleOnGameOver;
    }

    private void OnDisable()
    {
        GameManager.OnGameOver -= HandleOnGameOver;
    }

    private void HandleOnGameOver(int score, float elapsedTime)
    {
        _score = score;
        _time = elapsedTime;
    }
}
