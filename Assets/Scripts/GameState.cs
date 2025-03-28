
using UnityEngine;

public class GameState
{
	public static float gameTime24 { get; set; } = 12.0f;

	#region Skyboxes
	private static Material _daySkybox;
	public static Material daySkybox
	{
		get => _daySkybox;
		set
		{
			if (value != _daySkybox)
			{
				_daySkybox = value;
				GameEventController.EmitEvent(nameof(GameState), nameof(daySkybox));
			}
		}
	}

	private static Material _nightSkybox;
	public static Material nightSkybox
	{
		get => _nightSkybox;
		set
		{
			if (value != _nightSkybox)
			{
				_nightSkybox = value;
				GameEventController.EmitEvent(nameof(GameState), nameof(nightSkybox));
			}
		}
	}
	#endregion

	#region Coins

	public const int minCoinsOnScene = 10;
	public static float coinSpawnProbability { get; set; } = 0.5f;
	public static float _coinSpawnProbability 
	{ 
		get => _coinSpawnProbability;
		set
		{
			_coinSpawnProbability = value;
			GameEventController.EmitEvent(nameof(GameState), nameof(coinSpawnProbability));
		} 
	}

	#endregion

	#region Stamina
	public const float maxPossibleStamina = 20.0f;
	private static float _maxStamina = maxPossibleStamina / 2;
	public static float maxStamina
	{
		get => _maxStamina;
		set
		{
			_maxStamina = value;
			GameEventController.EmitEvent(nameof(GameState), nameof(maxStamina));
		}
	}
	public static float stamina { get; set; } = maxStamina;
	#endregion

	#region coinSpawnDistance
	public const float coinSpawnDistanceMin = 10.0f;
	public const float coinSpawnDistanceMax = 80.0f;
	public const float coinSpawnZoneRatio = 1.5f;

	private static float _coinSpawnDistance = 30.0f;

	public static float coinSpawnDistance
	{
		get => _coinSpawnDistance;
		set
		{
			if (_coinSpawnDistance != value)
			{
				_coinSpawnDistance = value;
				GameEventController.EmitEvent(nameof(GameState), nameof(coinSpawnDistance));
			}
		}
	}

	#endregion

	#region isClockVisible
	private static bool _isClockVisible = false;
	public static bool isClockVisible
	{
		get => _isClockVisible;
		set
		{
			if (_isClockVisible != value)
			{
				_isClockVisible = value;
				GameEventController.EmitEvent(nameof(GameState), nameof(isClockVisible));
			}
		}
	}
	#endregion

	#region isCompassVisible
	private static bool _isCompassVisible = false;
	public static bool isCompassVisible
	{
		get => _isCompassVisible;
		set
		{
			if (_isCompassVisible != value)
			{
				_isCompassVisible = value;
				GameEventController.EmitEvent(nameof(GameState), nameof(isCompassVisible));
			}
		}
	}
	#endregion

	#region isRadarVisible
	private static bool _isRadarVisible = false;
	public static bool isRadarVisible
	{
		get => _isRadarVisible;
		set
		{
			if (_isRadarVisible != value)
			{
				_isRadarVisible = value;
				GameEventController.EmitEvent(nameof(GameState), nameof(isRadarVisible));
			}
		}
	}
	#endregion

	#region isHintsVisible
	private static bool _isHintsVisible = false;
	public static bool isHintsVisible
	{
		get => _isHintsVisible;
		set
		{
			if (_isHintsVisible != value)
			{
				_isHintsVisible = value;
				GameEventController.EmitEvent(nameof(GameState), nameof(isHintsVisible));
			}
		}
	}

	#endregion
	#region isHintsVisible
	private static int _activeSceneIndex = 0;
	public static int activeSceneIndex
	{
		get => _activeSceneIndex;
		set
		{
			if (_activeSceneIndex != value)
			{
				_activeSceneIndex = value;
				GameEventController.EmitEvent(nameof(GameState), nameof(activeSceneIndex));
			}
		}
	}
	#endregion

	#region radarVisibleRadius
	private static float _radarVisibleRadius = 30.0f;
	public const float radarVisibleRadiusMin = 20.0f;
	public const float radarVisibleRadiusMax= 60.0f;
	public static float radarVisibleRadius
	{
		get => _radarVisibleRadius;
		set
		{
			if (_radarVisibleRadius != value)
			{
				_radarVisibleRadius = value;
				GameEventController.EmitEvent(nameof(GameState), nameof(radarVisibleRadius));
			}
		}
	}
	#endregion

}
