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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Ball>() != null)
        {
            Vector3 portalToBall = collision.transform.position - transform.position;

            if (Vector2.Distance(transform.position, collision.transform.position) <= collision.transform.localScale.x / 3)
            {
                collision.GetComponentInChildren<BallVisual>().ActivateMask();
            }
            else
            {
                collision.GetComponentInChildren<BallVisual>().DeactivateMask();
            }

            //Gestion de la balle fantôme
            _ghostBall.position = _exitPortal.transform.position - portalToBall;

            //Vérification de la position pour la téléportation
            if (Vector2.Dot(transform.up, portalToBall) < -0.1f)
            {
                Vector2 ballDirection = collision.GetComponent<Rigidbody2D>().linearVelocity;

                float DegToRad = (_exitPortal.transform.eulerAngles.z %360 - transform.eulerAngles.z % 360 - 180) * Mathf.Deg2Rad;

                // On tourne la direction de la balle de sorte à ce qu'elle s'oriente relativement au portail de sortie
                Vector2 ballDirectionRotated = new Vector2
                    (ballDirection.x * Mathf.Cos(DegToRad) - ballDirection.y * Mathf.Sin(DegToRad),
                    ballDirection.x * Mathf.Sin(DegToRad) + ballDirection.y * Mathf.Cos(DegToRad))
                    ;

                //Téléporter la balle à l'autre portail en prenant compte de sa vitesse et de sa position relative au portail
                collision.gameObject.transform.position = _exitPortal.transform.position;

                collision.GetComponent<Rigidbody2D>().linearVelocity = ballDirectionRotated;


            }
        }
    }

}
