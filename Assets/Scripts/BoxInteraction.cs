using UnityEngine;
using UnityEngine.UI;  // ��� ������ � UI ����������

public class BoxInteraction : MonoBehaviour
{
    [Header("Box Settings")]
    public GameObject boxPrefab;         // ������ �������
    public Transform handPosition;       // ����� � ����� ��� �������
    public float interactionTime = 1f;   // ����� ��� �������� ������� (1 �������)
    public float pickupRadius = 5f;     // ������, � ������� ����� ������� �������

    [Header("UI Elements")]
    public GameObject interactionPanel;  // ������ ��� ��������������
    public Image progressBarImage;      // ����������� ��� �������� ��������-����
    private float progressBarFill = 0f; // ������� ��������

    private GameObject currentBox;       // ������� �������, � ������� ���������������
    private bool isCarrying = false;     // ����, �����������, ��� ������� � �����
    private float holdTime = 0f;         // ����� ��������� ������
    private Transform playerTransform;   // ������ �� ��������� ��������� (��� ������)

    void Start()
    {
        // �������� ������ ��� ������ ����
        interactionPanel.SetActive(false);

        // �������� ������ �� ��������� ��������� (��� ������)
        playerTransform = Camera.main.transform;

        // �������� ��������-��� � ������
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

        // ���������, �� ��� �� ������� ������
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Box")) // �������� �� ������ � ����� "Box"
            {
                currentBox = hit.collider.gameObject;

                // ��������� ���������� �� �������
                float distanceToBox = Vector3.Distance(playerTransform.position, currentBox.transform.position);
                if (distanceToBox <= pickupRadius) // ���� ������� � ������� 5 ������
                {
                    // ���������� ������ ��������������
                    if (interactionPanel != null)
                    {
                        interactionPanel.SetActive(true);
                    }

                    if (Input.GetKey(KeyCode.E)) // ���������� E ��� �������� �������
                    {
                        holdTime += Time.deltaTime;

                        // ��������� �������� ���
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
                    // ���� ������� ��� �������, �������� ������
                    if (interactionPanel != null)
                    {
                        interactionPanel.SetActive(false);
                    }

                    currentBox = null;
                }
            }
            else
            {
                // ���� �� ������� �� �������, �������� ������
                if (interactionPanel != null)
                {
                    interactionPanel.SetActive(false);
                }

                currentBox = null;
            }
        }
        else
        {
            // ���� ������ �� �� �������, �������� ������
            if (interactionPanel != null)
            {
                interactionPanel.SetActive(false);
            }

            currentBox = null; // ������ ������, ���� ��� ��������� �� �������
        }
    }

    void PickUpBox()
    {
        if (!isCarrying && currentBox != null)
        {
            isCarrying = true;
            currentBox.transform.SetParent(handPosition);  // ����������� ������� � �����
            currentBox.transform.localPosition = Vector3.zero;  // ��������� ������� � �����
            currentBox.GetComponent<Rigidbody>().isKinematic = true; // ������ � �������������� (�� ���������� ������)
            currentBox.transform.rotation = Quaternion.identity;  // ���������� �������� �������
        }
        else if (isCarrying)
        {
            DropBox(); // ���� ������� ��� � �����, ���������� �
        }
    }

    void HandleBoxHolding()
    {
        if (isCarrying)
        {
            if (Input.GetKeyDown(KeyCode.E)) // ���� ���� ������ ������� E
            {
                DropBox(); // ���������� ������� �� ���
            }
        }
    }

    void DropBox()
    {
        if (isCarrying && currentBox != null) // ��������, ��� ������� ���� � ��� � �����
        {
            isCarrying = false;
            currentBox.transform.SetParent(null);  // ����������� ������� �� ���
            currentBox.GetComponent<Rigidbody>().isKinematic = false; // �������� ������

            // ����������� ��� ������ �������
            Vector3 dropDirection = Camera.main.transform.forward; // ����������� ������
            dropDirection.y = 0f; // ��������� ������� �� ����� (�� ��������� �����)
            dropDirection.Normalize(); // ����������� ������ ��� ���������� ������ ����

            // ��������� ���� ��� ������ ������� � ������ �����������
            currentBox.GetComponent<Rigidbody>().AddForce(dropDirection * 5f, ForceMode.Impulse); // 5f - ���� ������, ����� ���������

            currentBox = null; // �������� ������� �������

            // �������� ��������-���
            if (progressBarImage != null)
            {
                progressBarImage.fillAmount = 0f;
            }
        }
    }
}
