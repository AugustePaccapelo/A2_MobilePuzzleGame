using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private int _id = 1;
    public int Id
    {
        get => _id;
        set
        {
            if (value < 1)
            {
                Debug.LogWarning(name + ": id cannot be less than 1.");
                _id = 1;
                return;
            }
            _id = value;
        }
    }

    private void OnValidate()
    {
        Id = _id;
    }

    private void Awake()
    {
        Id = _id;
    }
}
