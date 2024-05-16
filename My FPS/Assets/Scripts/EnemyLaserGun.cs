using UnityEngine;

public class EnemyLaserGun : MonoBehaviour
{
    [SerializeField] private GameObject _bullet;
    [SerializeField] private Transform _firePoint;

    [SerializeField] private float _damage;
    [SerializeField] private float _impactForce;
    [SerializeField] private float _fireRate;

    [SerializeField] private float _waitBetweenShots;
    [SerializeField] private float _timeToShoot = 1f;

    private float _shotWaitCounter;
    private float _shootTimeCounter;

    public float ShotWaitCounter { get { return _shotWaitCounter; } }
    public float ShootTimeCounter { get { return _shootTimeCounter; } }
    public float Damage { get { return _damage; } }
    public float ImpactForce { get { return _impactForce; } }
    public float FireCount { get; set; }

    [Header("Shooting Effects")]
    [SerializeField] private ParticleSystem _muzzleFlash;

    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private GameObject _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player == null)
        {
            Debug.LogError("Player object not found!");
        }

        TimeToShoot();
    }

    public void TimeToShoot()
    {
        _shootTimeCounter = _timeToShoot;
        _shotWaitCounter = _waitBetweenShots;
    }

    public void EnemyFire()
    {
        if (_shotWaitCounter > 0)
        {
            _shotWaitCounter -= Time.deltaTime;

            if (_shotWaitCounter <= 0)
            {
                _shootTimeCounter = _timeToShoot;
            }
        }
        else
        {
            _shootTimeCounter -= Time.deltaTime;

            if (_shootTimeCounter > 0)
            {
                FireCount -= Time.deltaTime;

                if (FireCount <= 0)
                {
                    FireCount = _fireRate;

                    Vector3 playerPosition = _player.transform.position;
                    _firePoint.LookAt(playerPosition);

                    RaycastHit hit;
                    if (Physics.Raycast(_firePoint.position, _firePoint.forward, out hit, Mathf.Infinity, ~_enemyLayer))
                    {
                        if (hit.collider.CompareTag("Player"))
                        {
                            Instantiate(_bullet, _firePoint.position, _firePoint.rotation);
                        }
                    }
                }
            }
            else
            {
                _shotWaitCounter = _waitBetweenShots;
            }
        }
    }
}
