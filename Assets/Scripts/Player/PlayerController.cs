using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;

    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float staminaDrainRate = 20f;   // Stamina drain per second when running
    public float staminaRecoveryRate = 15f; // Stamina recovery per second when not running
    private float currentStamina;

    [Header("UI Elements")]
    public GameObject staminaPanel;       // Panel for stamina
    public Slider staminaSlider;          // Slider for stamina

    [Header("Camera Settings")]
    public Transform cameraTransform;
    public float mouseSensitivity = 1f;   // Adjusted sensitivity (lower = less sensitive)

    private CharacterController characterController;
    private Vector3 velocity;
    private float verticalRotation = 0f;

    private bool isCursorLocked = true;   // Tracks whether the cursor is locked

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController component is missing!");
        }

        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform is not assigned in the inspector!");
        }

        currentStamina = maxStamina;
        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = currentStamina;
        staminaPanel.SetActive(false); // Initially hide the stamina panel

        LockCursor(true); // Lock the cursor initially
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
        HandleStamina();
        HandleCursorToggle();
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        // Определяем, может ли игрок бежать
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && currentStamina > 0;

        // Если игрок бежит, уменьшаем стамину
        if (isRunning)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            if (!staminaPanel.activeSelf) staminaPanel.SetActive(true); // Показываем панель стамины
        }
        else
        {
            // Если не бежим, восстанавливаем стамину
            if (currentStamina < maxStamina)
            {
                currentStamina += staminaRecoveryRate * Time.deltaTime;
            }
        }

        // Устанавливаем скорость движения
        float speed = isRunning ? runSpeed : walkSpeed;

        // Движение персонажа
        characterController.Move(move * speed * Time.deltaTime);

        // Обновляем вертикальную скорость (гравитация)
        velocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }


    private void HandleMouseLook()
    {
        if (!isCursorLocked) return; // Skip rotation when the cursor is not locked

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleStamina()
    {
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        staminaSlider.value = currentStamina;

        // Hide stamina panel when fully recovered
        if (currentStamina >= maxStamina && staminaPanel.activeSelf)
        {
            staminaPanel.SetActive(false);
        }
    }

    private void HandleCursorToggle()
    {
        // Press Escape to toggle the cursor lock state
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LockCursor(!isCursorLocked);
        }
    }

    private void LockCursor(bool lockCursor)
    {
        isCursorLocked = lockCursor;

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
