using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class MultitouchManager : MonoBehaviour
{
    public TextMeshProUGUI  PositionIndexInfo;
    public TextMeshProUGUI  PositionMidInfo;
    public TextMeshProUGUI  PositionRingInfo;
    public TextMeshProUGUI  PositionLittInfo;
    public TextMeshProUGUI  PositionTumInfo;

    public TextMeshProUGUI  DistanceIndexInfo;
    public TextMeshProUGUI  DistanceMidInfo;
    public TextMeshProUGUI  DistanceRingInfo;
    public TextMeshProUGUI  DistanceLittInfo;
    public TextMeshProUGUI  DistanceTumInfo;
    public Button exit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        PositionIndexInfo.text = "0.0";
        PositionMidInfo.text = "0.0";
        PositionRingInfo.text = "0.0";
        PositionLittInfo.text = "0.0";
        PositionTumInfo.text = "0.0";

        DistanceIndexInfo.text = "0.0";
        DistanceMidInfo.text = "0.0";
        DistanceRingInfo.text = "0.0";
        DistanceLittInfo.text = "0.0";
        DistanceTumInfo.text = "0.0";
        
        foreach (var touch in Touch.activeTouches)
        {
            int fingerIndex = touch.finger.index;
            Vector2 position = touch.screenPosition;

            Debug.Log($"Dedo {fingerIndex}: {position}");

            switch (fingerIndex)
            {
                case 0:
                    PositionIndexInfo.text = position.ToString("F0");
                    break;

                case 1:
                    PositionMidInfo.text = position.ToString("F0");
                    break;

                case 2:
                    PositionRingInfo.text = position.ToString("F0");
                    break;

                case 3:
                    PositionLittInfo.text = position.ToString("F0");
                    break;

                case 4:
                    PositionTumInfo.text = position.ToString("F0");
                    break;
            }
        }

        if (Touch.activeTouches.Count >= 2)
        {
            Vector2 posicionBase = Touch.activeTouches[0].screenPosition;

            for (int i = 1; i < Touch.activeTouches.Count && i < 5; i++)
            {
                Vector2 posicionActual = Touch.activeTouches[i].screenPosition;

                float distancia = Vector2.Distance(posicionBase, posicionActual);

                switch (i)
                {
                    case 1:
                        DistanceMidInfo.text = distancia.ToString("F1");
                        break;

                    case 2:
                        DistanceRingInfo.text = distancia.ToString("F1");
                        break;

                    case 3:
                        DistanceLittInfo.text = distancia.ToString("F1");
                        break;

                    case 4:
                        DistanceTumInfo.text = distancia.ToString("F1");
                        break;
                }
            }
        }
    }
}


