using UnityEngine;

public class EnergyLockRotation : MonoBehaviour
{
    void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }
}