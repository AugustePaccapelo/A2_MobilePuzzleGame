using NaughtyAttributes;
using UnityEngine;

/// <summary>
///  Author : Maxence Bernard
///  
/// Core script d'un portail
/// </summary>

public class Portal : MonoBehaviour
{

    [HorizontalLine(2, EColor.Blue)]
    [BoxGroup("GD -- Sortie du portail")]
    [Label("Portail de sortie") ,SerializeField, Required] private Portal _exitPortal;

    [HorizontalLine(color: EColor.Violet), BoxGroup("GP -- Balle fantôme")]
    [Label("Balle Fantôme") ,SerializeField, Required] private Transform _ghostBall;
    private Vector3 _portalToBall;


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Ball>() != null)
        {
             _portalToBall = collision.transform.position - transform.position;

            if (Vector2.Distance(transform.position, collision.transform.position) <= collision.transform.localScale.x / 3)
            {
                collision.GetComponentInChildren<BallVisual>().ActivateMask();
            }
            else
            {
                collision.GetComponentInChildren<BallVisual>().DeactivateMask();
            }

            //Gestion de la balle fantôme
            

            //Vérification de la position pour la téléportation
            if (Vector2.Dot(transform.up, _portalToBall) < -0.1f)
            {
                Vector2 ballDirection = collision.GetComponent<Rigidbody2D>().linearVelocity;

                float DegToRad = (_exitPortal.transform.eulerAngles.z %360 - transform.eulerAngles.z % 360 - 180) * Mathf.Deg2Rad;

                // On tourne la direction de la balle de sorte à ce qu'elle s'oriente relativement au portail de sortie
                Vector2 ballDirectionRotated = RotateVector2(ballDirection, DegToRad);

                //Téléporter la balle à l'autre portail en prenant compte de sa vitesse et de sa position relative au portail
                Vector2 finalPosition;

                DegToRad = -(transform.eulerAngles.z % 360) * Mathf.Deg2Rad;

                // On tourne la position pour un repère à 0 degré
                finalPosition = RotateVector2(_portalToBall, DegToRad);

                finalPosition = new Vector2(-finalPosition.x, finalPosition.y); // On inverse le X

                //Et on retourne pour la différence
                DegToRad = _exitPortal.transform.eulerAngles.z * Mathf.Deg2Rad;

                finalPosition = RotateVector2(finalPosition, DegToRad);

                collision.gameObject.transform.position = _exitPortal.transform.position + (Vector3)finalPosition;

                if (ballDirectionRotated.magnitude < 3f)
                {
                    Debug.Log("Boost");
                    ballDirectionRotated *= 2f;
                }
                collision.GetComponent<Rigidbody2D>().linearVelocity = ballDirectionRotated;

                
            }

            _ghostBall.position = _exitPortal.transform.position - _portalToBall;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _ghostBall.position = _exitPortal.transform.position - _portalToBall;
    }


    private Vector2 RotateVector2(Vector2 vector, float angleInRad)
    {
        vector = new Vector2(
                    vector.x * Mathf.Cos(angleInRad) - vector.y * Mathf.Sin(angleInRad),
                    vector.x * Mathf.Sin(angleInRad) + vector.y * Mathf.Cos(angleInRad)
                    );

        return vector;
    }

}
