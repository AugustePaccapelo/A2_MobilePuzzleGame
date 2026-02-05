using UnityEngine;

public class BallVisual : MonoBehaviour
{
    private SpriteRenderer _sprite;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    public void ActivateMask()
    {
        _sprite.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
    }

    public void DeactivateMask()
    {
        _sprite.maskInteraction = SpriteMaskInteraction.None;
    }

}
