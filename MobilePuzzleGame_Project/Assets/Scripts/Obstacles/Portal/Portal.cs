using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
///  Author : Maxence Bernard
///  
/// Core script d'un portail
/// </summary>

public class Portal : MonoBehaviour
{

    [HorizontalLine(color: EColor.Blue)]
    [BoxGroup("GD -- Portail de Sortie")]
    [Label("Portail de sortie") ,SerializeField] private Portal _exitPortal;

    [HorizontalLine(color: EColor.Blue)]
    [BoxGroup("GD -- Events")]
    [SerializeField] private UnityEvent _onTeleportation;



    private Vector3 _portalToBall;

    private void Awake()
    {
        _exitPortal = this;
    }

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


            //Vérification de la position pour la téléportation
            if (Vector2.Dot(transform.up, _portalToBall) < -0.1f)
            {
                Vector2 ballDirection = collision.GetComponent<Rigidbody2D>().linearVelocity;

                float DegToRad = (_exitPortal.transform.eulerAngles.z % 360 - transform.eulerAngles.z % 360 - 180) * Mathf.Deg2Rad;

                // On tourne la direction de la balle de sorte à ce qu'elle s'oriente relativement au portail de sortie
                Vector2 ballDirectionRotated = RotateVector2(ballDirection, DegToRad);

                //Téléportation
                collision.gameObject.transform.position = _exitPortal.transform.position;
                _onTeleportation?.Invoke();

                if (ballDirectionRotated.magnitude < 3f)
                {
                    
                    ballDirectionRotated *= 2f;
                }
                collision.GetComponent<Rigidbody2D>().linearVelocity = ballDirectionRotated;


            }
        }
    }

    public void SetExitPortal(Portal exitPortal)
    {
        _exitPortal = exitPortal;
    }


    public static Vector2 RotateVector2(Vector2 vector, float angleInRad)
    {
        vector = new Vector2(
                    vector.x * Mathf.Cos(angleInRad) - vector.y * Mathf.Sin(angleInRad),
                    vector.x * Mathf.Sin(angleInRad) + vector.y * Mathf.Cos(angleInRad)
                    );

        return vector;
    }

}
