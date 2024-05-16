using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Shooting Settings")]
    [SerializeField] protected float _damage;
    [SerializeField] protected float _range;
    [SerializeField] protected float _fireRate;
    [SerializeField] protected float _impactForce;

    [Header("Sway Settings")]
    [SerializeField] private float _smooth;
    [SerializeField] private float _swayMultiplayer;    

    protected void GunSway()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * _swayMultiplayer;
        float mouseY = Input.GetAxisRaw("Mouse Y") * _swayMultiplayer;
        float moveZ = Input.GetAxisRaw("Vertical") * _swayMultiplayer;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);
        Quaternion rotationZ = Quaternion.AngleAxis(moveZ, Vector3.forward);

        Quaternion targetRotation = rotationX * rotationY * rotationZ;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, _smooth * Time.deltaTime);
    }
}
