using UnityEngine;
using UnityEngine.UI;

public class StoreTabs : MonoBehaviour
{
    public GameObject[] tabs; // ������ ������� ��� �������
    public Button[] tabButtons; // ������ ��� �������
    private int currentTabIndex = 0; // ������ ������� �������

    void Start()
    {
        // ������������� �������
        UpdateTabDisplay();

        // ���������� ������������ ������� ��� ������ �������
        for (int i = 0; i < tabButtons.Length; i++)
        {
            int index = i; // ��������� ���������� ��� �������
            tabButtons[i].onClick.AddListener(() => SwitchTab(index));
        }
    }

    void Update()
    {
        // ����������� ������� � ������� ������� B
        if (Input.GetKeyDown(KeyCode.B))
        {
            currentTabIndex = (currentTabIndex + 1) % tabs.Length; // ����������� �������
            UpdateTabDisplay();
        }
    }

    // ���������� ����������� �������
    void UpdateTabDisplay()
    {
        // �������� ��� �������
        foreach (GameObject tab in tabs)
        {
            tab.SetActive(false);
        }

        // ���������� ������� �������
        tabs[currentTabIndex].SetActive(true);
    }

    // ������������ ������� �� ������
    void SwitchTab(int index)
    {
        currentTabIndex = index;
        UpdateTabDisplay();
    }
}
