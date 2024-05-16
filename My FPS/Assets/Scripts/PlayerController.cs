using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _health = 100f;

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
