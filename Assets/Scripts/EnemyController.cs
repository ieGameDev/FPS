using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _health;

    [SerializeField] float _distanceToChase;
    [SerializeField] float _distanceToLose;
    [SerializeField] float _distanceToStop;
    private bool _chasing;
    private Vector3 _targetPoint;
    private Vector3 _startPoint;

    [SerializeField] private NavMeshAgent _navMeshAgent;

    public NavMeshAgent NavMeshAgent { get { return _navMeshAgent; } set { _navMeshAgent = value; } }

    [SerializeField] private float _keepChasingTime;
    private float _chaseCounter;

    private EnemyLaserGun _enemyLaserGun;

    private void Start()
    {
        _startPoint = transform.position;
        _enemyLaserGun = GameObject.Find("Enemy/Gun Holder/Enemy Energy Gun").GetComponent<EnemyLaserGun>();
    }

    private void Update()
    {
        _targetPoint = PlayerMovement.Instance.transform.position;
        _targetPoint.y = transform.position.y;

        float distanceToTarget = Vector3.Distance(transform.position, _targetPoint);

        if (!_chasing)
        {
            if (distanceToTarget < _distanceToChase)
            {
                _chasing = true;
                _enemyLaserGun.TimeToShoot();
            }

            if (_chaseCounter > 0)
            {
                _chaseCounter -= Time.deltaTime;

                if (_chaseCounter <= 0)
                {
                    _navMeshAgent.destination = _startPoint;
                }
            }
        }
        else
        {
            if (distanceToTarget > _distanceToStop)
            {
                _navMeshAgent.destination = _targetPoint;
            }
            else
            {
                _navMeshAgent.destination = transform.position;
            }

            if (distanceToTarget > _distanceToLose)
            {
                _chasing = false;
                _chaseCounter = _keepChasingTime;
            }

            _enemyLaserGun.EnemyFire();
        }
    }

    public void TakeDamage(float amount)
    {
        _health -= amount;
        if (_health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
