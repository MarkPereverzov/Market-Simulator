using UnityEngine;

[CreateAssetMenu(fileName = "NewLicenseList", menuName = "Market Simulator/License List")]
public class LicenseList : ScriptableObject
{
    public License[] licenses;
}
