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
    public AudioSource BackgroundMusic;
    public AudioSource DialogueBleep;
    private float easterEggTimer = 0f;
    private bool easterEggPlaying = false;
    private const int easterEggFingers = 7;
    private const float easterEggDuration = 7f;
    private float bleepCooldown = 0f;
    private const float bleepCooldownTime = 0.08f;
    private float prevPressure;
    private float prevMagnitude;
    private Vector2 prevDelta;
    private bool hasPrevValues = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BackgroundMusic.loop = true;
        BackgroundMusic.Play();
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

        if (!easterEggPlaying && activeFingers.Count == easterEggFingers)
        {
            easterEggTimer += Time.deltaTime;
            if (easterEggTimer >= easterEggDuration)
            {
                EasterEgg.Play();
                easterEggPlaying = true;
                BackgroundMusic.Stop();
            }
        }
        else if (activeFingers.Count != easterEggFingers)
        {
            easterEggTimer = 0f;
        }

        if (easterEggPlaying && !EasterEgg.isPlaying)
        {
            easterEggPlaying = false;
            BackgroundMusic.Play();
        }

        if (currentTouches.Count > 0 && !easterEggPlaying)
        {
            foreach (var touch in currentTouches)
            {
                bool changed = false;

                if (touch.phase == UnityEngine.InputSystem.TouchPhase.Moved ||
                    touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
                    changed = true;

                if (hasPrevValues)
                {
                    if (Mathf.Abs(touch.pressure - prevPressure) > 0.005f)
                        changed = true;
                    if (touch.delta.sqrMagnitude > 0.001f)
                        changed = true;
                    var (_, mag) = calculateDirection(touch.startScreenPosition, touch.screenPosition);
                    if (Mathf.Abs(mag - prevMagnitude) > 0.05f)
                        changed = true;
                }

                if (changed && bleepCooldown <= 0f)
                {
                    DialogueBleep.PlayOneShot(DialogueBleep.clip);
                    bleepCooldown = bleepCooldownTime;
                }

                prevPressure = touch.pressure;
                prevDelta = touch.delta;
                var (_, magnitude) = calculateDirection(touch.startScreenPosition, touch.screenPosition);
                prevMagnitude = magnitude;
            }
            hasPrevValues = true;

            bleepCooldown -= Time.deltaTime;
        }
        else
        {
            hasPrevValues = false;
            bleepCooldown = 0f;
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
