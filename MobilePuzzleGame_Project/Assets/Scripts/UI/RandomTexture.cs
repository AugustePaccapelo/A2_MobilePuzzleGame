using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomTexture : MonoBehaviour
{
    public enum Mode { Random, SequentialVeertical}

    public Mode mode = Mode.Random;
    public List<Sprite> sprites = new List<Sprite>();

    public int startIndex = 0;
    
    private static int s_globalSequentialVertIndex = 0;

    void Start()
    {
        if (sprites == null || sprites.Count == 0)
            return;

        int index = 0;
        if (mode == Mode.Random)
        {
            index = Random.Range(0, sprites.Count);
        }
        else
        {
            index = s_globalSequentialVertIndex;
            s_globalSequentialVertIndex = (s_globalSequentialVertIndex + 1) % sprites.Count;
        }

        Image uiImage = GetComponent<Image>();
        if (uiImage == null)
            uiImage = GetComponentInChildren<Image>(true);

        if (uiImage != null)
        {
            uiImage.sprite = sprites[index];
            return;
        }

        UnityEngine.UI.RawImage raw = GetComponent<UnityEngine.UI.RawImage>();
        if (raw == null)
            raw = GetComponentInChildren<UnityEngine.UI.RawImage>(true);

        if (raw != null && sprites[index] != null)
        {
            raw.texture = sprites[index].texture;
        }
    }
}
