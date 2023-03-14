using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameModeLogic
{
    public static GameModeLogic Instance { get => instance; }
    private static GameModeLogic instance;

    public Spaceship Player { get; private set; }
    [SerializeField] Spaceship playerSpaceshipPrefab;
    [SerializeField] Transform playerSpawnPoint;

    public int WinPoins { get; set; }
    public int LevelBoundaryX { get; private set; }
    public int LevelBoundaryY { get; private set; }
    private float tolerance = 0.1f;

    private List<GameObject> asteroids = new List<GameObject>();

    public void Init(GameMode gameMode)
    {
        instance = this;
        SetLevelBoundariesBasedOnCamera();
    }

    public void Update()
    {
        CheckOutOfBoundsAsteroids();
    }

    public void SetupScene()
    {
        Player = SpawnSpaceship();
        WinPoins = 0;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private Spaceship SpawnSpaceship()
    {
        var spaceship = GameObject.Instantiate(playerSpaceshipPrefab, playerSpawnPoint.position, Quaternion.identity);
        return spaceship;
    }

    private void SetLevelBoundariesBasedOnCamera()
    {
        var levelCamera = Camera.main;
        if (levelCamera == null)
        {
            Debug.LogWarning("MainCamera does not exist");
            return;
        }

        LevelBoundaryX = Mathf.CeilToInt(levelCamera.orthographicSize * levelCamera.aspect);
        LevelBoundaryY = Mathf.CeilToInt(levelCamera.orthographicSize);
    }

    public bool IsInsideBoundaries(Vector2 position)
    {
        if (Mathf.Abs(position.x) > LevelBoundaryX + tolerance) return false;
        if (Mathf.Abs(position.y) > LevelBoundaryY + tolerance) return false;

        return true;
    }

    public void AddAsteroid(GameObject asteroid)
    {
        asteroids.Add(asteroid);
    }

    private void CheckOutOfBoundsAsteroids()
    {
        for (int i = 0; i < asteroids.Count; i++)
        {
            var asteroid = asteroids[i];
            if (asteroid.gameObject != null)
            {
                asteroids.RemoveAt(i);
                i--;
                continue;
            }

            if (IsInsideBoundaries(asteroid.transform.position))
                continue;

            GameObject.Destroy(asteroid.gameObject);
            i--;
        }
    }
}
