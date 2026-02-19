using UnityEngine;

public class DragonEyeAnimation : MonoBehaviour
{
    private Ball _currentBall;
    private float _distance = 0.05f;

    NoteSpawner _spawner;
    Transform _target;

    private void Awake()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        _spawner = GameObject.Find("Spawner").GetComponent<NoteSpawner>();
        _spawner.OnNoteSpawnInfo += AssignBall;
        _target = GetComponentInParent<Target>().transform;
    }

    private void OnDisable()
    {
        _spawner.OnNoteSpawnInfo -= AssignBall;
    }

    private void AssignBall(Ball ball)
    {
        _currentBall = ball;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentBall == null) return;

        float angle;


        if (_target.localScale.x == -1)
        {
            angle = Mathf.Atan2(_currentBall.transform.position.y - transform.position.y, transform.position.x - _currentBall.transform.position.x);
        }
        else
        {
            angle = Mathf.Atan2(_currentBall.transform.position.y - transform.position.y, _currentBall.transform.position.x - transform.position.x);
        }

        

        Vector2 finalPos = new Vector2
                (
                    _distance * Mathf.Cos(angle - (_target.eulerAngles.z * Mathf.Deg2Rad)),
                    _distance * Mathf.Sin(angle - (_target.eulerAngles.z * Mathf.Deg2Rad))
                );

        transform.localPosition = finalPos;
    }
}
