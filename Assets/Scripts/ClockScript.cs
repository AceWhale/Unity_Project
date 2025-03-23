using UnityEngine;

public class ClockScript : MonoBehaviour
{
    private TMPro.TextMeshProUGUI clock;

    void Start()
    {
        clock = GetComponent<TMPro.TextMeshProUGUI>();
        GameEventController.AddListener(nameof(GameState), OnGameStateChangedEvent);
        clock.enabled = GameState.isClockVisible;
    }

    void Update()
    {
        if (clock.isActiveAndEnabled)
        {
            int hour = (int)GameState.gameTime24;
            int minute = Mathf.FloorToInt((GameState.gameTime24 - hour) * 60);

            clock.text = $"{hour:D2}:{minute:D2}";
        }
    }

    private void OnGameStateChangedEvent(string type, object payload)
    {
        if (payload.Equals(nameof(GameState.isClockVisible)))
        {
            clock.enabled = GameState.isClockVisible;
        }
    }
    private void OnDestroy()
    {
        GameEventController.RemoveListener(nameof(GameState), OnGameStateChangedEvent);
    }
}
