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
    public TextMeshProUGUI  Fingers;
    public Button exit;
    private readonly System.Collections.Generic.HashSet<int> activeFingers = new();
    private float movedHoldTimer;
    private float endedHoldTimer;
    private const float holdDuration = 0.1f;
    public AudioSource EasterEgg;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var currentTouches = Touch.activeTouches;

        var currentFingers = new System.Collections.Generic.HashSet<int>();
        foreach (var touch in currentTouches)
            currentFingers.Add(touch.finger.index);

        bool released = false;
        foreach (int finger in activeFingers)
        {
            if (!currentFingers.Contains(finger))
            {
                released = true;
                break;
            }
        }
        activeFingers.Clear();
        activeFingers.UnionWith(currentFingers);

        if (released)
            endedHoldTimer = holdDuration;
        else if (endedHoldTimer > 0f)
            endedHoldTimer -= Time.deltaTime;

        if (currentTouches.Count == 0)
        {
            Fingers.text = "0";
            movedHoldTimer = 0f;
        }
        else
        {
            bool anyMoved = false;
            foreach (var touch in currentTouches)
            {
                if (touch.phase == UnityEngine.InputSystem.TouchPhase.Moved)
                    anyMoved = true;

                int dedos = currentTouches.Count;
                UnityEngine.InputSystem.TouchPhase phase = touch.phase;
                Vector2 position = touch.screenPosition;
                Vector2 delta = touch.delta;
                float pressure = touch.pressure;
                var (direction, magnitude) = calculateDirection(touch.startScreenPosition, touch.screenPosition);

                updateText(dedos, phase, position, delta, pressure, direction, magnitude);

                Debug.Log(
                    $"Finger {dedos}" +
                    $"Phase {phase}" +
                    $"Pos {position}" +
                    $"Delta {delta}" +
                    $"Pressure {pressure}"
                );
            }

            if (anyMoved)
                movedHoldTimer = holdDuration;
            else if (movedHoldTimer > 0f)
                movedHoldTimer -= Time.deltaTime;
        }

        if (endedHoldTimer > 0f)
            Phase.text = "Ended";
        else if (currentTouches.Count > 0)
            Phase.text = movedHoldTimer > 0f ? "Moved" : "Stationary";
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

    private void updateText(int dedos, UnityEngine.InputSystem.TouchPhase phase, Vector2 position, Vector2 delta, float pressure, Vector2 direction, float magnitude)
    {
        Fingers.text = dedos.ToString();
        Phase.text = phase.ToString();
        Position.text = position.ToString();
        Delta.text = delta.ToString();
        Direction.text = direction.ToString();
        Magnitude.text = magnitude.ToString();
        Pressure.text = pressure.ToString();
    }
}
