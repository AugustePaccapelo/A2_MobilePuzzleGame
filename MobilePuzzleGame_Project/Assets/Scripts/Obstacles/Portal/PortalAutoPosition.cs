using UnityEngine;

// Author = Maxence Bernard

public class PortalAutoPosition : MonoBehaviour
{

    // ----- Variables -----

    // -- Position la plus proche --
    private Vector3 _closestPosition;

    // -- Vectoeur de direction --
    private Vector2 _direction;
    private Vector3 _normal;

    public LayerMask _layer;

    private void Awake()
    {
        _direction = transform.up;
    }

    private void Update()
    {
        
        for (int i = 0; i < 8; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, _direction, 100f, _layer);
            if(hit.collider == null)
            {
                _direction = Portal.RotateVector2(_direction, 45 * Mathf.Deg2Rad);
                continue;
            }

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                
                if (Vector2.Distance(transform.position, hit.point) < Vector2.Distance(transform.position, _closestPosition) || _closestPosition == Vector3.zero)
                {
                    _closestPosition = hit.point;
                    _normal = hit.normal;
                }

            }

            //On tourne la direction à 45 degré pour avoir les 8 raycast autour du portail
            _direction = Portal.RotateVector2(_direction, 45 * Mathf.Deg2Rad);

            Debug.DrawLine(transform.position, transform.position + (Vector3)_direction);
        }

        transform.position = _closestPosition;
        transform.up = _normal;

    }

}
