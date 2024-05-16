using UnityEngine;

public class Gun : Weapon
{
    [Header("Shooting Effects")]
    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private GameObject _impactEffect;

    [Header("Raycast Settings")]
    [SerializeField] private Camera _fpsCamera;
    [SerializeField] private LayerMask _playerLayer;

    private float _nextTimeToFire = 0f;
    private CameraRecoil _cameraRecoil;

    private void Start()
    {
        _cameraRecoil = GameObject.Find("Camera Point/Camera Recoil").GetComponent<CameraRecoil>();
    }

    void Update()
    {
        GunSway();

        if (Input.GetButton("Fire1") && Time.time >= _nextTimeToFire)
        {
            _nextTimeToFire = Time.time + 1f / _fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        _muzzleFlash.Play();
        _cameraRecoil.RecoilFire();

        RaycastHit hit;
        if (Physics.Raycast(_fpsCamera.transform.position, _fpsCamera.transform.forward, out hit, _range, ~_playerLayer))
        {
            EnemyController enemy = hit.transform.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(_damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * _impactForce);
            }

            GameObject impactGameObject = Instantiate(_impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGameObject, 2f);
        }
    }
}
