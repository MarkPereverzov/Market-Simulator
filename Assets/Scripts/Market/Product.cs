using UnityEngine;

[System.Serializable]
public class Product
{
    public string productName;      // ��� ��������
    public float price;             // ���� ��������
    public Sprite productIcon;      // ������ ��������
    public License requiredLicense; // ��������, ����������� ��� ������� ������
}
