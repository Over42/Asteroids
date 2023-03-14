using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpaceshipHUD : MonoBehaviour
{
    private Spaceship spaceship;

    private Label labelCoordX;
    private Label labelCoordY;
    private Label labelAngle;
    private Label labelSpeed;
    private Label labelLaserCharges;
    private Label labelLaserCooldown;

    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        labelCoordX = root.Q<Label>("LabelCoordX");
        labelCoordY = root.Q<Label>("LabelCoordY");
        labelAngle = root.Q<Label>("LabelAngle");
        labelSpeed = root.Q<Label>("LabelSpeed");
        labelLaserCharges = root.Q<Label>("LabelLaserCharges");
        labelLaserCooldown = root.Q<Label>("LabelLaserCooldown");
    }

    void Start()
    {
        spaceship = GameModeLogic.Instance.Player;
    }

    void Update()
    {
        labelCoordX.text = spaceship.Movement.Position.x.ToString("0");
        labelCoordY.text = spaceship.Movement.Position.y.ToString("0");
        labelAngle.text = spaceship.Movement.Rotation.ToString("0");
        labelSpeed.text = spaceship.Movement.Speed.ToString("0");
        labelLaserCharges.text = spaceship.Weapons.LaserCharges.ToString();
        labelLaserCooldown.text = spaceship.Weapons.CurrentLaserRestoreCooldown.ToString("F1");
    }
}
