using UnityEngine;
using UnityEngine.UI;

public class MenuDifficultyScript : MonoBehaviour
{
	private Toggle clockToggle;
	private Toggle compassToggle;
	private Toggle hintsToggle;
	private Toggle radarToggle;
	private Slider radarSlider;
	private Slider coinSpawnZoneSlider;
	private Slider coinProbabilitySlider;
	private Slider staminaSlider;

	void Start()
	{
		#region Toggles
		Transform layout1 = transform.Find("Content/UISettings/Layout1");

		clockToggle = layout1.Find("ClockToggle").GetComponent<Toggle>();
		clockToggle.isOn = GameState.isCompassVisible;

		compassToggle = layout1.Find("CompassToggle").GetComponent<Toggle>();
		compassToggle.isOn = GameState.isCompassVisible;

		hintsToggle = layout1.Find("HintsToggle").GetComponent<Toggle>();
		hintsToggle.isOn = GameState.isHintsVisible;

		radarToggle = layout1.Find("RadarToggle").GetComponent<Toggle>();
		radarToggle.isOn = GameState.isRadarVisible;
		#endregion

		Transform layout2 = transform.Find("Content/GameDifficulty/Layout2");
		radarSlider = layout2.Find("Radar/Slider").GetComponent<Slider>();
		radarSlider.value = Mathf.Sqrt(
			(GameState.radarVisibleRadius - GameState.radarVisibleRadiusMin) /
				(GameState.radarVisibleRadiusMax - GameState.radarVisibleRadiusMin));

		coinSpawnZoneSlider = layout2.Find("CoinSpawnZone/Slider").GetComponent<Slider>();
		coinSpawnZoneSlider.value = 1 - Mathf.Sqrt(
			(GameState.coinSpawnDistance - GameState.coinSpawnDistanceMin) /
				(GameState.coinSpawnDistanceMax - GameState.coinSpawnDistanceMin));

		coinProbabilitySlider = layout2.Find("CoinProbability/Slider").GetComponent<Slider>();
		coinProbabilitySlider.value = GameState.coinSpawnProbability;

		staminaSlider = layout2.Find("Stamina/Slider").GetComponent<Slider>();
		coinProbabilitySlider.value = GameState.maxStamina;
	}

	void Update()
	{

	}

	public void OnCoinProbabilitySlider(float value)
	{
		GameState.coinSpawnProbability = value;
	}

	public void onStaminaSlider(float value)
	{
		GameState.maxStamina = value * GameState.maxPossibleStamina;
		if(GameState.stamina > GameState.maxStamina)
		{
			GameState.stamina = GameState.maxStamina;
		}
	}


	public void OnCoinZoneSlider(float value)
	{
		float x = 1 - value;
		GameState.coinSpawnDistance = GameState.coinSpawnDistanceMin +
			(GameState.coinSpawnDistanceMax - GameState.coinSpawnDistanceMin) * x * x;
		//Debug.Log("CoinSpawnDistance: " + GameState.coinSpawnDistance.ToString());
	}
	public void OnRadarSlider(float value)
	{
		GameState.radarVisibleRadius = GameState.radarVisibleRadiusMin +
			(GameState.radarVisibleRadiusMax - GameState.radarVisibleRadiusMin) * value * value;
	}
	public void OnClockToggle(bool value)
	{
		GameState.isClockVisible = value;
	}

	public void OnCompassToggle(bool value)
	{
		GameState.isCompassVisible = value;
	}
	public void OnHintsToggle(bool value)
	{
		GameState.isHintsVisible = value;
	}
	public void OnRadarToggle(bool value)
	{
		GameState.isRadarVisible = value;
	}
}
