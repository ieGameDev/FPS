using UnityEngine;

public class LaserBulletController : MonoBehaviour
{
    [SerializeField] private float _bulletMoveSpeed;
    [SerializeField] private float _bulletLifeTime;
    [SerializeField] private Rigidbody _bulletRigidbody;
    [SerializeField] private GameObject _impactEffect;

    private float _damageValue;
    private float _impactForceValue;
    private Vector3 _hitPoint;

    void Start()
    {
        Destroy(gameObject, _bulletLifeTime);
    }

    void Update()
    {
        _bulletRigidbody.velocity = transform.forward * _bulletMoveSpeed;

        CheckCollision();
    }

    private void CheckCollision()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _bulletMoveSpeed * Time.deltaTime))
        {
            if (hit.collider != null)
            {
                Instantiate(_impactEffect, hit.point, Quaternion.LookRotation(hit.normal));

                EnemyController enemy = hit.transform.GetComponent<EnemyController>();

                if (enemy != null)
                {
                    enemy.TakeDamage(_damageValue);
                }

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * _impactForceValue);
                }

                Destroy(gameObject);
            }
        }
    }

    
    public void SetDamageAndForce(float damage, float impactForce)
    {
        _damageValue = damage;
        _impactForceValue = impactForce;
    }

    public void SetHitPoint(Vector3 hitPoint)
    {
        _hitPoint = hitPoint;
    }
}