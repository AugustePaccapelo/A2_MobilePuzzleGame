using NaughtyAttributes;
using UnityEngine;

/// <summary>
///  Author : Maxence Bernard
///  
/// Core script d'un portail
/// </summary>

public class Portal : MonoBehaviour
{

    [Header("GD -- Sortie du portail")]
    [HorizontalLine(2, EColor.Blue)]
    [SerializeField, Required] private Portal _exitPortal;

    [Header("GP -- Points pour le vecteur")]
    [HorizontalLine(2, EColor.Violet)]
    [SerializeField, Required] private Transform _pointA;
    [SerializeField, Required] private Transform _pointB;
    private Vector2 _normalVector;

    private void Start()
    {
        float angleRad = transform.localRotation.z * Mathf.Deg2Rad;
        _normalVector = Vector2.Perpendicular(new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Ball>() != null)
        {
            

            Debug.Log(Vector2.Dot(_normalVector.normalized, collision.gameObject.transform.position));
            if (Vector2.Dot(_normalVector.normalized, collision.gameObject.transform.position) > 0)
            {
                //Calcul de l'angle d'entrée dans le portail
                Vector2 ballDirection = collision.GetComponent<Rigidbody2D>().linearVelocity.normalized;
                float angle = Mathf.Atan2(ballDirection.y, ballDirection.x);
                Debug.Log(angle);

                //Téléporter la balle à l'autre portail en prenant compte de sa vitesse et de sa position relative au portail
                collision.gameObject.transform.position = _exitPortal.transform.position;
                
                
            }
        }
    }

}
