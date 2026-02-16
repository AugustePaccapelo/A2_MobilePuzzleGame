using NaughtyAttributes;
using UnityEngine;

public class AccordeonMiddle : MonoBehaviour
{

    [HorizontalLine(color: EColor.Violet)]
    [BoxGroup("GP"), SerializeField] private Transform _topPart;
    [BoxGroup("GP"), SerializeField] private Transform _bottomPart;

    private void Update()
    {
        transform.position = Vector2.Lerp(_topPart.position, _bottomPart.position, .5f);
        transform.localScale = new Vector2(transform.localScale.x, Vector2.Distance(_topPart.position, _bottomPart.position) / 3f);
    }

}
