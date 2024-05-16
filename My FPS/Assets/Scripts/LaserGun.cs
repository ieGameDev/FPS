using UnityEngine;

public class LaserGun : Weapon
{
    [Header("Bullet Settings")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firePoint;

    [Header("Shooting Effects")]
    [SerializeField] private ParticleSystem _muzzleFlash;

    [Header("Raycast Settings")]
    [SerializeField] private Camera _fpsCamera;
    [SerializeField] private LayerMask _playerLayer;

    public float Damage { get { return _damage; } }
    public float ImpactForce { get { return _impactForce; } }

    private CameraRecoil _cameraRecoil;
    private float _nextTimeToFire = 0f;

    private void Start()
    {
        _cameraRecoil = GameObject.Find("Camera Point/Camera Recoil").GetComponent<CameraRecoil>();
    }

    private void Update()
    {
        GunSway();

        if (Input.GetButton("Fire1") && Time.time >= _nextTimeToFire)
        {
            _nextTimeToFire = Time.time + 1f / _fireRate;
            Shoot();
        }
    }

    private void Shoot()
    {
        _muzzleFlash.Play();
        _cameraRecoil.RecoilFire();

        RaycastHit hit;
        if (Physics.Raycast(_fpsCamera.transform.position, _fpsCamera.transform.forward, out hit, _range, ~_playerLayer))
        {
            _firePoint.LookAt(hit.point);
        }
        else
        {
            _firePoint.LookAt(_fpsCamera.transform.position + (_fpsCamera.transform.forward * 30f));
        }

        InstantiateBullet(_firePoint.position, _firePoint.rotation, hit.point);
    }

    private void InstantiateBullet(Vector3 position, Quaternion rotation, Vector3 hitPoint)
    {
        GameObject bulletObject = Instantiate(_bulletPrefab, position, rotation);
        LaserBulletController bulletController = bulletObject.GetComponent<LaserBulletController>();
        if (bulletController != null)
        {
            bulletController.SetDamageAndForce(_damage, _impactForce);
            bulletController.SetHitPoint(hitPoint);
        }
    }
}