using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterAttack : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayers;
    [SerializeField] private Vector2Int _damageRange;
    [SerializeField] private ParticleSystem _effect;

    [SerializeField] private float _angle;
    [SerializeField] private float _range;
    [SerializeField] private float _cooldown;

    private Collider[] _hits = new Collider[20];
    private float _currentCooldown;
    private bool _queueAttack;
    
    public void Attack()
    {
        if (_currentCooldown > 0)
        {
            _queueAttack = true;
            return;
        }

        if (TargetDummyInput.ControllerType == ControllerType.KeyboardAndMouse)
        {
            transform.LookAt(CursorInputHandler.MousePosition);
        }
        else
        {
            transform.localRotation = Quaternion.identity;
        }

        _currentCooldown = _cooldown;
        
        Vector3 pos = transform.position;
        Vector3 forward = transform.forward;
        float arcAngle = _angle / 2f;
        _effect.Play();

        bool hit = false;

        int hits = Physics.OverlapSphereNonAlloc(pos, _range, _hits, _targetLayers);
        for (int i = 0; i < hits; i++)
        {
            Vector3 otherPos = _hits[i].ClosestPoint(pos);
            Vector3 dir = otherPos - pos;
            if (Vector3.Angle(dir, forward) > arcAngle)
            {
                continue;
            }
            
            int damage = Random.Range(_damageRange.x, _damageRange.y);
            
            Health healthComponent = _hits[i].GetComponent<Health>();
            if(healthComponent == null) continue;

            hit = true;
            healthComponent.Damage(damage);
        }
    }

    private void Update()
    {
        if (_currentCooldown > 0)
        {
            _currentCooldown -= Time.deltaTime;
            return;
        }

        if (_queueAttack)
        {
            _queueAttack = false;
            Attack();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(_range <= 0) return;
        GizmosExtentions.DrawWireArc(transform.position, transform.forward, _angle, _range);
    }
}
