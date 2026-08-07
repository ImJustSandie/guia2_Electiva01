using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;



public class TouchManager : MonoBehaviour
{
    public TextMeshProUGUI  Phase;
    public TextMeshProUGUI  Position;
    public TextMeshProUGUI  Delta;
    public TextMeshProUGUI  Direction;
    public TextMeshProUGUI  Magnitude;
    public TextMeshProUGUI  Pressure;
    public Button exit;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var currentTouches = Touch.activeTouches;

        foreach (var touch in currentTouches)
        {
            int dedos = touch.finger.index +1;
            UnityEngine.InputSystem.TouchPhase phase = touch.phase;
            Vector2 position = touch.screenPosition;
            Vector2 delta = touch.delta;
            float pressure = touch.pressure;
            var (direction, magnitude) = calculateDirection(touch.startScreenPosition, touch.screenPosition);

           updateText(phase, position, delta, pressure, direction, magnitude);
            
            Debug.Log(
                $"Finger {dedos}" +
                $"Phase {phase}" +
                $"Pos {position}" +
                $"Delta {delta}"+
                $"Pressure {pressure}"
            );
            
        }
    }

    private void OnEnable()
    {
        //Activar el enhanced touch
        EnhancedTouchSupport.Enable(); 
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }


    private (Vector2 direction, float magnitude) calculateDirection(UnityEngine.Vector2 startPos, UnityEngine.Vector2 currentPos)
    {
        UnityEngine.Vector2 delta = currentPos - startPos;
        UnityEngine.Vector2 direction = delta.normalized;
        float magnitude = delta.magnitude;
        return (direction, magnitude);
    }

    private void updateText(UnityEngine.InputSystem.TouchPhase phase, Vector2 position, Vector2 delta, float pressure, Vector2 direction, float magnitude)
    {
        Phase.text = phase.ToString();
        Position.text = position.ToString();
        Delta.text = delta.ToString();
        Direction.text = direction.ToString();
        Magnitude.text = magnitude.ToString();
        Pressure.text = pressure.ToString();
    }
}
