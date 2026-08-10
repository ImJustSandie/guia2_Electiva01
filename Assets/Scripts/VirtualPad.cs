using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;


public class VirtualPad : MonoBehaviour
{
    public GameObject character;
    public float speed = 5f;
    public TextMeshProUGUI positionText;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private Vector2 moveInput;
    private Animator animator;
    private bool isWalking;
    private float stopCooldown;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        animator = character.GetComponent<Animator>();
    }

    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();

        if (moveInput.sqrMagnitude > 0.01f)
        {
            moveInput = moveInput.normalized;
            isWalking = true;
            stopCooldown = 0.15f;
        }
        else
        {
            moveInput = Vector2.zero;
            stopCooldown -= Time.deltaTime;
            if (stopCooldown <= 0f)
                isWalking = false;
        }

        animator.SetBool("IsWalking", isWalking);

        var sprite = character.GetComponent<SpriteRenderer>();
        if (sprite != null && moveInput != Vector2.zero)
            sprite.flipX = moveInput.x > 0;

        Vector3 movement = new Vector3(moveInput.x, moveInput.y, 0f);
        character.transform.Translate(movement * speed * Time.deltaTime);

        if (positionText != null)
        {
            Vector3 pos = character.transform.position;
            positionText.text = $"X: {pos.x:F2}  Y: {pos.y:F2}";
        }
    }
}
