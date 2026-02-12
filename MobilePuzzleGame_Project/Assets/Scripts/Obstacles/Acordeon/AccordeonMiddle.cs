using NaughtyAttributes;
using UnityEngine;

public class AccordeonMiddle : MonoBehaviour
{

    [HorizontalLine(color: EColor.Violet)]
    [BoxGroup("GP"), SerializeField] private Transform _topPart;
    [BoxGroup("GP"), SerializeField] private Transform _bottomPart;

    private void FixedUpdate()
    {
        transform.position = Vector2.Lerp(_topPart.position, _bottomPart.position, .5f);
        transform.localScale = new Vector2(1, Vector2.Distance(_topPart.position, _bottomPart.position));
    }

}
