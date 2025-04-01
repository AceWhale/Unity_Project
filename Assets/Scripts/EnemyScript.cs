using UnityEngine;

public class EnemyScript : MonoBehaviour
{
	private CharacterController characterController;
	private Transform character;
	private float lastHitTime = 0f;

	// Змінні для обмеження спавну
	public float minEnemyCharacterDistance = 10f;
	public float maxEnemyCharacterDistance = 30f;
	public float minEnemyMapOffset = 50f;
	public float minEnemyHeight = 1f;
	public float maxEnemyHeight = 2.5f;
	public float seaLevel = 0f;
	void Start()
	{
		characterController = GetComponent<CharacterController>();
		character = GameObject.Find("Character").transform;
	}

	void Update()
	{
		if (Time.time - lastHitTime > 1.0)
		{
			Vector3 v = character.position - transform.position;
			v.Normalize();
			characterController.SimpleMove(v);
			v.y = 0;
			v.Normalize();
			transform.forward = v;
		}
	}
	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hit.gameObject.name == "Character")
		{
			if (Time.time - lastHitTime > 1.0)
			{
				lastHitTime = Time.time;
				Debug.Log("Catch you!");
				Vector3 newPosition = RandomPosition(from: transform.position, distance: 0f);
				newPosition.y = 1.1f + Terrain.activeTerrain.SampleHeight(newPosition);
				transform.position = newPosition;
			}
			else
			{
				Debug.Log("Skipped Hit");
			}
		}
	}

	private Vector3 RandomPosition(Vector3 from, float distance)
	{
		Vector3 delta;
		Vector3 coinPosition;
		int lim = 0;

		do
		{
			delta = new Vector3(
				Random.Range(-maxEnemyCharacterDistance, maxEnemyCharacterDistance),
				0,
				Random.Range(-maxEnemyCharacterDistance, maxEnemyCharacterDistance)
			);
			coinPosition = from + delta;
			lim += 1;
		} while (lim < 100 && (
			delta.magnitude > maxEnemyCharacterDistance ||
			delta.magnitude < minEnemyCharacterDistance ||
			coinPosition.x < minEnemyMapOffset ||
			coinPosition.z < minEnemyMapOffset ||
			coinPosition.x > 1000 - minEnemyMapOffset ||
			coinPosition.z > 1000 - minEnemyMapOffset ||
			coinPosition.y - seaLevel < 2
		));

		return coinPosition;
	}
}

