using UnityEngine;
using UnityEngine.UI;
using TMPro; // Added for TextMeshPro support

public class BoxInteraction : MonoBehaviour
{
    [Header("Box Settings")]
    public GameObject boxPrefab;
    public Transform handPosition;
    public float interactionTime = 1f;
    public float pickupRadius = 5f;

    [Header("UI Elements")]
    public GameObject interactionPanel;
    public Image progressBarImage;
    public TMP_Text interactionText; // Changed to TMP_Text for TextMeshPro
    private float progressBarFill = 0f;

    private GameObject currentBox;
    private GameObject currentShelf;
    private bool isCarrying = false;
    private float holdTime = 0f;
    private Transform playerTransform;

    void Start()
    {
        interactionPanel.SetActive(false);
        playerTransform = Camera.main.transform;

        if (progressBarImage != null)
        {
            progressBarImage.fillAmount = 0f;
        }

        if (interactionText != null)
        {
            interactionText.text = "E"; // Default interaction key
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

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Box"))
            {
                HandleBoxInteraction(hit);
            }
            else if (hit.collider.CompareTag("Shelf") && isCarrying)
            {
                HandleShelfInteraction(hit);
            }
            else
            {
                ResetInteraction();
            }
        }
        else
        {
            ResetInteraction();
        }
    }

    void HandleBoxInteraction(RaycastHit hit)
    {
        currentBox = hit.collider.gameObject;
        float distanceToBox = Vector3.Distance(playerTransform.position, currentBox.transform.position);

        if (distanceToBox <= pickupRadius && !isCarrying)
        {
            interactionPanel.SetActive(true);

            if (Input.GetKey(KeyCode.E))
            {
                holdTime += Time.deltaTime;

                if (progressBarImage != null)
                {
                    progressBarFill = holdTime / interactionTime;
                    progressBarImage.fillAmount = progressBarFill;
                }

                if (holdTime >= interactionTime)
                {
                    PickUpBox();
                }
            }
            else
            {
                ResetProgressBar();
            }
        }
        else
        {
            interactionPanel.SetActive(false);
        }
    }

    void HandleShelfInteraction(RaycastHit hit)
    {
        currentShelf = hit.collider.gameObject;
        float distanceToShelf = Vector3.Distance(playerTransform.position, currentShelf.transform.position);

        if (distanceToShelf <= pickupRadius)
        {
            interactionPanel.SetActive(true);

            if (interactionText != null)
            {
                interactionText.text = "F"; // Change interaction key to F
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                PlaceBoxOnShelf();
            }
        }
        else
        {
            interactionPanel.SetActive(false);

            if (interactionText != null)
            {
                interactionText.text = "E"; // Reset interaction key
            }
        }
    }

    void ResetInteraction()
    {
        interactionPanel.SetActive(false);
        currentBox = null;
        currentShelf = null;

        if (interactionText != null)
        {
            interactionText.text = "E"; // Reset interaction key
        }
    }

    void PickUpBox()
    {
        isCarrying = true;
        currentBox.transform.SetParent(handPosition);
        currentBox.transform.localPosition = Vector3.zero;
        currentBox.GetComponent<Rigidbody>().isKinematic = true;
        currentBox.transform.rotation = Quaternion.identity;
        ResetProgressBar();
    }

    void PlaceBoxOnShelf()
    {
        if (isCarrying && currentBox != null && currentShelf != null)
        {
            isCarrying = false;
            currentBox.transform.SetParent(currentShelf.transform);
            currentBox.transform.localPosition = Vector3.zero;
            currentBox.GetComponent<Rigidbody>().isKinematic = false;
            currentBox = null;
            interactionPanel.SetActive(false);

            if (interactionText != null)
            {
                interactionText.text = "E"; // Reset interaction key
            }
        }
    }

    void HandleBoxHolding()
    {
        if (isCarrying && Input.GetKeyDown(KeyCode.E))
        {
            DropBox();
        }
    }

    void DropBox()
    {
        if (isCarrying && currentBox != null)
        {
            isCarrying = false;
            currentBox.transform.SetParent(null);
            currentBox.GetComponent<Rigidbody>().isKinematic = false;

            Vector3 dropDirection = Camera.main.transform.forward;
            dropDirection.y = 0f;
            dropDirection.Normalize();

            currentBox.GetComponent<Rigidbody>().AddForce(dropDirection * 5f, ForceMode.Impulse);
            currentBox = null;
            ResetProgressBar();
        }
    }

    void ResetProgressBar()
    {
        holdTime = 0f;
        progressBarFill = 0f;

        if (progressBarImage != null)
        {
            progressBarImage.fillAmount = 0f;
        }
    }
}
