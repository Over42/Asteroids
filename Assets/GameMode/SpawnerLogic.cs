using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnerLogic
{
	private Spawner spawner;

	[SerializeField] GameObject objectToSpawn;
	[SerializeField] bool useChildSpawnPoints;
	[SerializeField] bool loopSpawn;
	[SerializeField] float minSpawnDelay;
	[SerializeField] float maxSpawnDelay;
	private float spawnDelay;
	private float currentSpawnDelay;

	public void Init(Spawner spawner)
    {
		this.spawner = spawner;
    }

	public void Update(float deltaTime)
	{
		if (loopSpawn)
		{
			currentSpawnDelay += deltaTime;
			if (currentSpawnDelay > spawnDelay)
			{
				Spawn();
				currentSpawnDelay = 0f;
			}
		}
	}

	public void Spawn()
    {
		if (useChildSpawnPoints)
		{
			SpawnInChildSpawnPoint();
		}
        else
        {
			SpawnAsteroid();
        }

		SetSpawnDelay();
	}

	private void SpawnInChildSpawnPoint()
    {
		var spawnPointArray = spawner.GetComponentsInChildren<Transform>();
		var spawnPoints = new List<Transform>(spawnPointArray);
		spawnPoints.Remove(spawner.transform);
		int index = Random.Range(0, spawnPoints.Count);
		var spawnPosition = spawnPoints[index].transform.position;
		GameObject.Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
	}

	private void SpawnAsteroid()
	{
		var gameMode = GameModeLogic.Instance;
		Vector2 direction = Random.insideUnitCircle;
		Vector2 position = Vector2.zero;
		if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
		{
			// Appear on the left/right
			position = new Vector2(Mathf.Sign(direction.x) * gameMode.LevelBoundaryX, direction.y * gameMode.LevelBoundaryY);
		}
		else
		{
			// Appear on the top/bottom
			position = new Vector2(direction.x * gameMode.LevelBoundaryX, Mathf.Sign(direction.y) * gameMode.LevelBoundaryY);
		}

		// Slightly offset so not out of screen at creation time (it would destroy the asteroid)
		position -= position.normalized * 0.1f;
		var rotation = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
		var asteroid = GameObject.Instantiate(objectToSpawn, position, rotation);

		gameMode.AddAsteroid(asteroid);
	}

	private void SetSpawnDelay()
	{
		spawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
	}
}
