using UnityEngine;

public class Object_Chest : MonoBehaviour, IDamageable
{
    private Rigidbody2D _rb => GetComponentInChildren<Rigidbody2D>();
    private Animator _anim => GetComponentInChildren<Animator>();
    private Entity_VFX _fx => GetComponent<Entity_VFX>();

    [Header("Open details")] 
    [SerializeField] private Vector2 knockBack;
        
    public bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        _fx.PlayOnDamageVfx();
        _anim.SetBool("chestOpen", true);
        _rb.linearVelocity = knockBack;
        _rb.angularVelocity = Random.Range(-200f, 200f);

        return true;
    }
}
