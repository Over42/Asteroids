using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameUI : MonoBehaviour
{
    private GameModeLogic gameModeLogic;

    private Label labelScore;

    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        labelScore = root.Q<Label>("LabelScore");
    }

    void Start()
    {
        gameModeLogic = GameModeLogic.Instance;
    }

    void Update()
    {
        labelScore.text = gameModeLogic.WinPoins.ToString();
    }
}
