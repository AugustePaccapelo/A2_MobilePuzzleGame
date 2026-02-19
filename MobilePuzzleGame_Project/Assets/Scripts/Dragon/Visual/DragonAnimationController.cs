using UnityEngine;

public class DragonAnimationController : MonoBehaviour
{
    private Animator _animator;

    private Target _dragon;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _dragon = GetComponentInParent<Target>();

        Target.OnWin += PlayGoodNoteAnimation;
        Target.OnLoose += PlayBadNoteAnimation;
    }

    private void OnDisable()
    {
        Target.OnWin -= PlayGoodNoteAnimation;
        Target.OnLoose -= PlayBadNoteAnimation;
    }


    private void PlayGoodNoteAnimation(int id)
    {
        if (id != _dragon.Id) return;
        Debug.Log("On win target");
        _animator.Play("dragon_bonne_note");
    }

    private void PlayBadNoteAnimation(int id)
    {
        if (id != _dragon.Id) return;
        Debug.Log("On loose target");
        _animator.Play("animation_dragon_erreur");
    }

    private void PlayEatAnimation()
    {
        _animator.Play("animation_dragon_manger");
    }
}
