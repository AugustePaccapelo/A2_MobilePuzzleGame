using NaughtyAttributes;
using UnityEngine;

public class AccordeonMiddle : MonoBehaviour
{

    [HorizontalLine(color: EColor.Violet)]
    [BoxGroup("GP"), SerializeField] private Transform _topPart;
    [BoxGroup("GP"), SerializeField] private Transform _bottomPart;

    private float _baseDistance;
    private float _baseYScale;

    private void OnEnable()
    {
        //_baseDistance = Mathf.Abs(_topPart.position.y - _bottomPart.position.y);

        Vector2 topPos = _topPart.transform.position;
        Vector2 botPos = _bottomPart.transform.position;
        _baseDistance = (topPos - botPos).magnitude;

        _baseYScale = transform.localScale.y;
    }

    private void Update()
    {
        transform.position = Vector2.Lerp(_topPart.position, _bottomPart.position, .5f);

        Vector2 topPos = _topPart.transform.position;
        Vector2 botPos = _bottomPart.transform.position;

        //float currentDistance = Mathf.Abs(_topPart.position.y - _bottomPart.position.y);
        float currentDistance = (topPos - botPos).magnitude;

        float ratio = currentDistance / _baseDistance;
        float yScale = _baseYScale * ratio;
        
        transform.localScale = new Vector3(transform.localScale.x, yScale, transform.localScale.z);
    }

}
