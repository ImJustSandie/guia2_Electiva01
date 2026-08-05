using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;



public class TouchManager : MonoBehaviour
{
    public GameObject Phase;
    public GameObject Position;
    public GameObject Delta;
    public GameObject Direction;
    public GameObject Magnitude;
    public GameObject Pressure;
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
            Vector2 direction = calculateDirection(touch.startScreenPosition, touch.screenPosition);

           
            
            Debug.Log(
                $"Finger {touch.finger.index}" +
                $"Phase {touch.phase}" +
                $"Pos {touch.screenPosition}" +
                $"Delta {touch.delta}"+
                $"Pressure {touch.pressure}"
            );
            calculateDirection(touch.startScreenPosition, touch.screenPosition);

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


    private Vector2 calculateDirection(UnityEngine.Vector2 startPos, UnityEngine.Vector2 currentPos)
    {
        UnityEngine.Vector2 delta = currentPos - startPos;
        UnityEngine.Vector2 direction = delta.normalized;
        float magnitude = delta.magnitude;

        return direction;
    }

    private void updateText( UnityEngine.InputSystem.TouchPhase phase, Vector2 position, Vector2 delta, float pressure, Vector2 direction)
    {
        
    }
}
