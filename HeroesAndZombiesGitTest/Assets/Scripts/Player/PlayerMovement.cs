using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Joysticks")]
    public FloatingJoystick moveJoystick;
    public FloatingJoystick lookJoystick;

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("References")]
    public Animator playerAnimatorMove;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerAnimatorMove = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        // GameManager'dan joystickleri çek
        moveJoystick = GameManager.Instance.moveJoystick;
        lookJoystick = GameManager.Instance.lookJoystick;
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        if (moveJoystick == null) return;

        float h = moveJoystick.Horizontal;
        float v = moveJoystick.Vertical;
        Vector3 moveDir = new Vector3(h, 0, v);

        if (moveDir.magnitude > 0.1f)
        {
            moveDir.Normalize();
            rb.MovePosition(rb.position + moveDir * moveSpeed * Time.fixedDeltaTime);

            // Animasyon
            playerAnimatorMove.SetFloat("Speed_f", 0.5f);
        }
        else
        {
            playerAnimatorMove.SetFloat("Speed_f", 0f);
        }
    }

    private void HandleRotation()
    {
        Vector3 direction = Vector3.zero;

        
        if (lookJoystick != null && (Mathf.Abs(lookJoystick.Horizontal) > 0.2f || Mathf.Abs(lookJoystick.Vertical) > 0.2f))
        {
            direction = new Vector3(lookJoystick.Horizontal, 0, lookJoystick.Vertical);
        }
        else if (moveJoystick != null && (Mathf.Abs(moveJoystick.Horizontal) > 0.2f || Mathf.Abs(moveJoystick.Vertical) > 0.2f))
        {
            direction = new Vector3(moveJoystick.Horizontal, 0, moveJoystick.Vertical);
        }

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
