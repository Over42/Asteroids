using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameMode : MonoBehaviour
{
    public static GameMode Instance { get; private set; }

    public GameModeLogic Logic { get => logic; }
    [SerializeField] GameModeLogic logic;

    [SerializeField] GameUI gameUI;
    [SerializeField] GameOverUI gameOverUI;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        logic.Init(this);
        logic.SetupScene();
        InitUI();
    }

    void Update()
    {
        logic.Update();
    }

    private void InitUI()
    {
        Instantiate(gameUI);
        logic.Player.Logic.OnDeath += () => Instantiate(gameOverUI);
    }
}
