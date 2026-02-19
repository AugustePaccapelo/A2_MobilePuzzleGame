using UnityEngine;

public class BardAnimationController : MonoBehaviour
{
    private Animator _animator;

    private NoteSpawner _spawner;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _spawner = GetComponentInParent<NoteSpawner>();
        
        Target.OnWin += PlayWinAnimation;
        Target.OnLoose += PlayLooseAnimation;
        _spawner.OnNoteSpawn += PlayNoteAnimation;
    }

    private void OnDisable()
    {
        Target.OnWin -= PlayWinAnimation;
        Target.OnLoose -= PlayLooseAnimation;
        _spawner.OnNoteSpawn -= PlayNoteAnimation;
    }


    private void PlayWinAnimation(int id)
    {
        if (id != _spawner.Id) return;
        
        _animator.Play("animation_bard_win");
    }

    private void PlayLooseAnimation(int id)
    {
        if (id != _spawner.Id) return;

        _animator.Play("animation_bard_lose");
    }

    private void PlayNoteAnimation()
    {
        _animator.Play("animation_bard_play_note"); 
    }
}
