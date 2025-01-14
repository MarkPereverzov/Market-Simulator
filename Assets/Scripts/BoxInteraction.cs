using UnityEngine;
using UnityEngine.UI;  // Для работы с UI элементами

public class BoxInteraction : MonoBehaviour
{
    [Header("Box Settings")]
    public GameObject boxPrefab;         // Префаб коробки
    public Transform handPosition;       // Место в руках для коробки
    public float interactionTime = 1f;   // Время для поднятия коробки (1 секунда)
    public float pickupRadius = 5f;     // Радиус, в котором можно поднять коробку

    [Header("UI Elements")]
    public GameObject interactionPanel;  // Панель для взаимодействия
    public Image progressBarImage;      // Изображение для круглого прогресс-бара
    private float progressBarFill = 0f; // Текущий прогресс

    private GameObject currentBox;       // Текущая коробка, с которой взаимодействуем
    private bool isCarrying = false;     // Флаг, указывающий, что коробка в руках
    private float holdTime = 0f;         // Время удержания кнопки
    private Transform playerTransform;   // Ссылка на трансформ персонажа (или камеры)

    void Start()
    {
        // Скрываем панель при старте игры
        interactionPanel.SetActive(false);

        // Получаем ссылку на трансформ персонажа (или камеры)
        playerTransform = Camera.main.transform;

        // Скрываем прогресс-бар в начале
        if (progressBarImage != null)
        {
            progressBarImage.fillAmount = 0f;
        }
    }

    void Update()
    {
        HandleRaycast();
        HandleBoxHolding();
    }

    void HandleRaycast()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Проверяем, на что мы наводим курсор
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Box")) // Проверка на объект с тегом "Box"
            {
                currentBox = hit.collider.gameObject;

                // Проверяем расстояние до коробки
                float distanceToBox = Vector3.Distance(playerTransform.position, currentBox.transform.position);
                if (distanceToBox <= pickupRadius) // Если коробка в радиусе 5 метров
                {
                    // Показываем панель взаимодействия
                    if (interactionPanel != null)
                    {
                        interactionPanel.SetActive(true);
                    }

                    if (Input.GetKey(KeyCode.E)) // Удерживаем E для поднятия коробки
                    {
                        holdTime += Time.deltaTime;

                        // Обновляем прогресс бар
                        if (progressBarImage != null)
                        {
                            progressBarFill = holdTime / interactionTime;
                            progressBarImage.fillAmount = progressBarFill;
                        }

                        if (holdTime >= interactionTime && !isCarrying)
                        {
                            PickUpBox();
                        }
                    }
                    else
                    {
                        holdTime = 0f;
                        progressBarFill = 0f;
                        if (progressBarImage != null)
                        {
                            progressBarImage.fillAmount = 0f;
                        }
                    }
                }
                else
                {
                    // Если коробка вне радиуса, скрываем панель
                    if (interactionPanel != null)
                    {
                        interactionPanel.SetActive(false);
                    }

                    currentBox = null;
                }
            }
            else
            {
                // Если не наводим на коробку, скрываем панель
                if (interactionPanel != null)
                {
                    interactionPanel.SetActive(false);
                }

                currentBox = null;
            }
        }
        else
        {
            // Если курсор не на коробке, скрываем панель
            if (interactionPanel != null)
            {
                interactionPanel.SetActive(false);
            }

            currentBox = null; // Прячем панель, если нет попадания по объекту
        }
    }

    void PickUpBox()
    {
        if (!isCarrying && currentBox != null)
        {
            isCarrying = true;
            currentBox.transform.SetParent(handPosition);  // Прикрепляем коробку к рукам
            currentBox.transform.localPosition = Vector3.zero;  // Размещаем коробку в руках
            currentBox.GetComponent<Rigidbody>().isKinematic = true; // Делаем её кинематической (не подвержена физике)
            currentBox.transform.rotation = Quaternion.identity;  // Сбрасываем вращение коробки
        }
        else if (isCarrying)
        {
            DropBox(); // Если коробка уже в руках, сбрасываем её
        }
    }

    void HandleBoxHolding()
    {
        if (isCarrying)
        {
            if (Input.GetKeyDown(KeyCode.E)) // Если была нажата клавиша E
            {
                DropBox(); // Сбрасываем коробку на пол
            }
        }
    }

    void DropBox()
    {
        if (isCarrying && currentBox != null) // Проверка, что коробка есть и она в руках
        {
            isCarrying = false;
            currentBox.transform.SetParent(null);  // Отсоединяем коробку от рук
            currentBox.GetComponent<Rigidbody>().isKinematic = false; // Включаем физику

            // Направление для сброса коробки
            Vector3 dropDirection = Camera.main.transform.forward; // Направление вперед
            dropDirection.y = 0f; // Оставляем коробку на земле (не поднимаем вверх)
            dropDirection.Normalize(); // Нормализуем вектор для корректной работы силы

            // Применяем силу для сброса коробки в нужном направлении
            currentBox.GetComponent<Rigidbody>().AddForce(dropDirection * 5f, ForceMode.Impulse); // 5f - сила сброса, можно настроить

            currentBox = null; // Обнуляем текущую коробку

            // Скрываем прогресс-бар
            if (progressBarImage != null)
            {
                progressBarImage.fillAmount = 0f;
            }
        }
    }
}
