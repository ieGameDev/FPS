using UnityEngine;

public class EnemyLaserBulletController : MonoBehaviour
{
    [SerializeField] private float _bulletMoveSpeed;
    [SerializeField] private float _bulletLifeTime;
    [SerializeField] private Rigidbody _bulletRigidbody;
    [SerializeField] private GameObject _impactEffect;

    private EnemyLaserGun _enemyLaserGun;
    private float _damageValue;
    private float _impactForceValue;

    [SerializeField] private LayerMask _enemyLayer;

    void Start()
    {
        _enemyLaserGun = FindObjectOfType<EnemyLaserGun>();
        _damageValue = _enemyLaserGun.Damage;
        _impactForceValue = _enemyLaserGun.ImpactForce;
    }

    void Update()
    {
        _bulletRigidbody.velocity = transform.forward * _bulletMoveSpeed;
        _bulletLifeTime -= Time.deltaTime;

        if (_bulletLifeTime <= 0)
        {
            Destroy(gameObject);
        }

        CheckCollision();
    }

    private void CheckCollision()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _bulletMoveSpeed * Time.deltaTime, ~_enemyLayer))
        {

            if (hit.collider != null)
            {
                Instantiate(_impactEffect, hit.point + (transform.forward * (-_bulletMoveSpeed * Time.deltaTime)), Quaternion.LookRotation(hit.normal));

                PlayerController player = hit.transform.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(_damageValue);
                }

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * _impactForceValue);
                }

                Destroy(gameObject);
            }
        }
    }
}
