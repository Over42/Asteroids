using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverUI : MonoBehaviour
{
    void OnEnable()
    {
        var gameModeLogic = GameModeLogic.Instance;

        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        var buttonRestartGame = root.Q<Button>("ButtonRestartGame");
        buttonRestartGame.clicked += gameModeLogic.RestartGame;
        buttonRestartGame.clicked += () => Destroy(gameObject);
    }
}
