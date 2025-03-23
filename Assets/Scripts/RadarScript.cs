using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RadarScript : MonoBehaviour
{
    private Transform character;
    private Image screen;
    private Image samplePoint;
    private List<CoinPoint> coinPoints = new();
    [SerializeField]
    private float activeZoneRatio = 0.85f;

    private float maxVisibleDistance;
	private readonly string[] listenableEvents = new string[] { "SpawnCoin", "CoinDisappear", nameof(GameState) };


	void Start()
    {
        character = GameObject.Find("Character").transform;
        screen = transform.Find("Screen").GetComponent<Image>();
        samplePoint = transform.Find("Screen/Point").GetComponent<Image>();
        samplePoint.gameObject.SetActive(false);

        foreach (GameObject coin in GameObject.FindGameObjectsWithTag("Coin"))
        {
            coinPoints.Add(new CoinPoint
            {
                coin = coin.transform,
                point = GameObject.Instantiate(samplePoint, screen.transform)
            });
        }
		GameEventController.AddListener(listenableEvents, OnGameEvent);
        OnGameEvent(nameof(GameState), null);
	}

    void Update()
    {
        float screenWidth = screen.rectTransform.rect.width;
        float maxRadius = screenWidth * activeZoneRatio * 0.5f;

        foreach (CoinPoint cp in coinPoints)
        {
            Vector3 d = cp.coin.position - character.position;
            if (d.magnitude > maxVisibleDistance)
            {
                cp.point.gameObject.SetActive(false);
            }
            else
            {
                if (!cp.point.gameObject.activeInHierarchy)
                {
                    cp.point.gameObject.SetActive(true);
                }
                Vector3 f = Camera.main.transform.forward;
                d.y = 0f;
                f.y = 0f;
                float angle = Vector3.SignedAngle(f, d, Vector3.down);

                float r = maxRadius * d.magnitude / maxVisibleDistance;

                cp.point.rectTransform.localPosition = new Vector3(
                    -r * Mathf.Sin(angle * Mathf.Deg2Rad),
                    r * Mathf.Cos(angle * Mathf.Deg2Rad)
                );
            }
        }
            
    }

    private void OnDisappearEvent(string type, object payload)
    {
        if (payload is GameObject oldCoin)
        {
            var coinPoint = coinPoints.FirstOrDefault(cp => cp.coin == oldCoin.transform);
            if(coinPoint != null)
            {
                GameObject.Destroy(coinPoint.point.gameObject);
                coinPoints.Remove(coinPoint);
            }
        }
    }
    private void OnCoinSpawnEvent(string type, object payload)
    {
        if (payload is GameObject newCoin)
        {
            coinPoints.Add(new CoinPoint
            {
                coin = newCoin.transform,
                point = GameObject.Instantiate(samplePoint, screen.transform)
            });
        }
    }
	private void OnGameEvent(string type, object payload)
	{
		switch (type)
		{
			case "SpawnCoin":
                OnCoinSpawnEvent(type, payload);
				break;
			case "CoinDisappear":
				OnDisappearEvent(type, payload);
				break;
			case nameof(GameState):
				screen.gameObject.SetActive(GameState.isRadarVisible);
                maxVisibleDistance = GameState.radarVisibleRadius;
				break;

		}
	}

	private void OnDestroy()
	{
		GameEventController.RemoveListener(listenableEvents, OnGameEvent);
	}

	private class CoinPoint
    {
        public Transform coin { get; set; }
        public Image point { get; set; }
    }
}
