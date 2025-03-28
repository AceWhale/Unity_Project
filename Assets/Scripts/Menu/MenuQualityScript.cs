using Unity.VisualScripting;
using UnityEngine;

public class MenuQualityScript : MonoBehaviour
{
	[SerializeField]
	private Material[] daySkyboxes = new Material[0];
	[SerializeField]
	private Material[] nightSkyboxes = new Material[0];
	private Material defaultSkybox;

	private TMPro.TMP_Dropdown graphicsDropdown;
	private TMPro.TMP_Dropdown fogDropdown;
	private TMPro.TMP_Dropdown daySkyDropdown;
	private TMPro.TMP_Dropdown nightSkyDropdown;

	void Start()
	{
		Transform layout = transform
			.Find("Content/Quality/Layout");
		graphicsDropdown = layout
			.Find("Graphics/Dropdown")
			.GetComponent<TMPro.TMP_Dropdown>();
		InitQualityDropdown();

		fogDropdown = layout
			.Find("Fog/Dropdown")
			.GetComponent<TMPro.TMP_Dropdown>();
		InitFogDropdown();

		daySkyDropdown = layout
			.Find("DaySky/Dropdown")
			.GetComponent<TMPro.TMP_Dropdown>();
		InitDayDropdown();

		nightSkyDropdown = layout
			.Find("NightSky/Dropdown")
			.GetComponent<TMPro.TMP_Dropdown>();
		InitNightDropdown();

		GameEventController.AddListener(nameof(GameState), OnGameStateEvent);
	}

	private void InitNightDropdown()
	{
		nightSkyDropdown.ClearOptions();
		foreach (Material m in nightSkyboxes)
		{
			nightSkyDropdown.options.Add(new(m.name));
		}
		defaultSkybox = RenderSettings.skybox;
		if (defaultSkybox != null)
		{
			nightSkyDropdown.options.Add(new(defaultSkybox.name));
			nightSkyDropdown.value = nightSkyboxes.Length;
		}
		else
		{
			nightSkyDropdown.value = -1;
		}
	}

	private void InitDayDropdown()
	{
		daySkyDropdown.ClearOptions();
		foreach (Material m in daySkyboxes)
		{
			daySkyDropdown.options.Add(new(m.name));
		}
		defaultSkybox = RenderSettings.skybox;
		if (defaultSkybox != null)
		{
			daySkyDropdown.options.Add(new(defaultSkybox.name));
			daySkyDropdown.value = daySkyboxes.Length;
		}
		else
		{
			daySkyDropdown.value = -1;
		}
	}
	public void OnNightSkyDropdownChanged(int index)
	{
		if (index < nightSkyboxes.Length)
		{
			GameState.nightSkybox = nightSkyboxes[index];
		}
		else
		{
			GameState.nightSkybox = defaultSkybox;
		}
	}
	public void OnDaySkyDropdownChanged(int index)
	{
		if (index < daySkyboxes.Length)
		{
			GameState.daySkybox = daySkyboxes[index];
		}
		else
		{
			GameState.daySkybox = defaultSkybox;
		}
	}

	private void InitFogDropdown()
	{
		fogDropdown.ClearOptions();
		fogDropdown.options.Add(new("Off"));
		foreach (string name in System.Enum.GetNames(typeof(FogMode)))
		{
			fogDropdown.options.Add(new(name));
		}
		if (RenderSettings.fog)
		{
			fogDropdown.value = (int)RenderSettings.fogMode;
		}
		else
		{
			fogDropdown.value = 0;
		}
	}
	public void OnFogDropdownChanged(int index)
	{
		if (index == 0)
		{
			RenderSettings.fog = false;
		}
		else
		{
			RenderSettings.fog = true;
			RenderSettings.fogMode = (FogMode)index;
		}
	}

	private void InitQualityDropdown()
	{
		graphicsDropdown.ClearOptions();
		foreach (string name in QualitySettings.names)
		{
			graphicsDropdown.options.Add(new(name));
		}
		int currentQualityLevel = QualitySettings.GetQualityLevel();
		graphicsDropdown.value = currentQualityLevel;
	}

	public void OnGraphicsDropdownChanged(int index)
	{
		QualitySettings.SetQualityLevel(index, true);
	}

	private void OnGameStateEvent(string type, object payload)
	{
		if (nameof(GameState.activeSceneIndex).Equals(payload))
		{
			InitFogDropdown();
			InitDayDropdown();
			InitNightDropdown();
		}
	}

	void OnDestroy()
	{
		GameEventController.RemoveListener(nameof(GameState), OnGameStateEvent);
	}
}
