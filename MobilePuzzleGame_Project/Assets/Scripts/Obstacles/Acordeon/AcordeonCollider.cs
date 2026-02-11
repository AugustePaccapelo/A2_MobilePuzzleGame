using UnityEngine;
using static Acordeon;

public class AcordeonCollider : MonoBehaviour
{

    private Acordeon _parent;

    private void Awake()
    {
        _parent = GetComponentInParent<Acordeon>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (_parent.State == AcordeonState.Extend && _parent.ActionDone)
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * _parent.Power, ForceMode2D.Impulse);
            _parent.ActionDone = false;
        }
    }

}
