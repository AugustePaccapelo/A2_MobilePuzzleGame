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
        _dragon = GameObject.Find("Target").GetComponent<Target>();

        _dragon.OnWin += PlayGoodNoteAnimation;
        _dragon.OnLoose += PlayBadNoteAnimation;
    }

    private void OnDisable()
    {
        _dragon.OnWin -= PlayGoodNoteAnimation;
        _dragon.OnLoose -= PlayBadNoteAnimation;
    }


    private void PlayGoodNoteAnimation()
    {
        _animator.Play("dragon_bonne_note");
    }

    private void PlayBadNoteAnimation()
    {
        _animator.Play("animation_dragon_erreur");
    }

    private void PlayEatAnimation()
    {
        _animator.Play("animation_dragon_manger");
    }
}
