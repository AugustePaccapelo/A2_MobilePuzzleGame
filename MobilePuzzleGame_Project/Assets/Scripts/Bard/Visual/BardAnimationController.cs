using UnityEngine;

public class BardAnimationController : MonoBehaviour
{
    private Animator _animator;

    private Target _dragon;
    private NoteSpawner _spawner;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _dragon = GameObject.Find("Target").GetComponent<Target>();
        _spawner = GameObject.Find("Spawner").GetComponent<NoteSpawner>();

        _dragon.OnWin += PlayWinAnimation;
        _dragon.OnLoose += PlayLooseAnimation;
        _spawner.OnNoteSpawn += PlayNoteAnimation;
    }

    private void OnDisable()
    {
        _dragon.OnWin -= PlayWinAnimation;
        _dragon.OnLoose -= PlayLooseAnimation;
        _spawner.OnNoteSpawn -= PlayNoteAnimation;
    }


    private void PlayWinAnimation()
    {
        _animator.Play("animation_bard_win");
    }

    private void PlayLooseAnimation()
    {
        _animator.Play("animation_bard_lose");
    }

    private void PlayNoteAnimation()
    {
        _animator.Play("animation_bard_play_note"); 
    }
}
