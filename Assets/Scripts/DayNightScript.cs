using UnityEngine;

public class DayNightScript : MonoBehaviour
{
	private float dayDuration = 3000.0f;   // real seconds per game day
	private float rotateAngle;
	private float dayHour;
	private float dayPhase;
	private Light sun;
	private Light moon;


	void Start()
	{
		sun = transform.Find("Sun").gameObject.GetComponent<Light>();
		moon = transform.Find("Moon").gameObject.GetComponent<Light>();
		rotateAngle = -360.0f / dayDuration;
		dayHour = 12;
		GameEventController.AddListener(nameof(GameState), OnGameEvent);
	}

	void Update()
	{
		DayPhase prevDayPhase = PhaseFromHour(dayHour);
		dayHour += 24 * Time.deltaTime / dayDuration;
		if (dayHour >= 24)
		{
			dayHour -= 24;
		}
		GameState.gameTime24 = dayHour;
		DayPhase dayPhase = PhaseFromHour(dayHour);

		if (prevDayPhase != dayPhase)
		{
			DayPhaseChanged();
		}

		float coef;
		if (dayPhase == DayPhase.Night)
		{
			float cosArg;
			if (dayHour < 4)
			{
				cosArg = (dayHour - 0) * Mathf.PI / (2 * 4);
			}
			else
			{
				cosArg = (dayHour - 24) * Mathf.PI / (2 * 4);

			}
			coef = Mathf.Cos(cosArg) / 3;
			RenderSettings.sun = moon;
			moon.intensity = coef;
			coef *= 2.0f;
		}
		else
		{
			float sinArg = (dayHour - 4) * Mathf.PI / 16;
			coef = Mathf.Sin(sinArg);
			RenderSettings.sun = sun;
			sun.intensity = coef;
		}
		RenderSettings.ambientIntensity = coef;
		RenderSettings.skybox.SetFloat("_Exposure", coef);

		this.transform.Rotate(0, 0, rotateAngle * Time.deltaTime);
	}

	private void DayPhaseChanged()
	{
		DayPhase dayPhase = PhaseFromHour(dayHour);
		if (dayPhase == DayPhase.Night)
		{
			if (GameState.nightSkybox != null)
			{
				RenderSettings.skybox = GameState.nightSkybox;
			}
		}
		else
		{
			if (GameState.daySkybox != null)
			{
				RenderSettings.skybox = GameState.daySkybox;
			}
		}
	}

	private DayPhase PhaseFromHour(float hour)
	{
		if (hour > 20 || hour < 4) return DayPhase.Night;
		if (hour < 7) return DayPhase.Dawn;
		if (hour > 17) return DayPhase.Dusk;
		return DayPhase.Day;
	}

	private void OnGameEvent(string type, object payload)
	{
		if (nameof(GameState.daySkybox).Equals(payload))
		{
			if (PhaseFromHour(dayHour) != DayPhase.Night && GameState.daySkybox != null)
			{
				RenderSettings.skybox = GameState.daySkybox;
			}
		}
		else if (nameof(GameState.nightSkybox).Equals(payload))
		{
			if (PhaseFromHour(dayHour) == DayPhase.Night && GameState.nightSkybox != null)
			{
				RenderSettings.skybox = GameState.nightSkybox;
			}
		}
	}

	private void OnDestroy()
	{
		GameEventController.RemoveListener(nameof(GameState), OnGameEvent);
	}
}

enum DayPhase
{
	Night,
	Dawn,
	Day,
	Dusk,
}