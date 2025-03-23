using UnityEngine;
using UnityEngine.UI;

public class StaminaScript : MonoBehaviour
{
    private GameObject content;
    private Image indicator;

    void Start()
    {
        Transform t = transform.Find("Content");
        indicator = t.Find("Indicator").GetComponent<Image>();
        content = t.gameObject;
    }

    void Update()
    {
        if(GameState.stamina > 0)
            indicator.fillAmount = Mathf.Clamp01(GameState.stamina / GameState.maxStamina);
    }
}
