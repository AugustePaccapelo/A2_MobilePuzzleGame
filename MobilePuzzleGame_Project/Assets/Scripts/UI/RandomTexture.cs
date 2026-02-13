using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomTexture : MonoBehaviour
{
    public enum Mode { Random, Sequential }

    public Mode mode = Mode.Random;
    public List<Sprite> sprites = new List<Sprite>();

    public int startIndex = 0;

    static private int s_globalSequentialIndex = 0;

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
            if (startIndex >= 0 && startIndex < sprites.Count)
            {
                index = startIndex;
                s_globalSequentialIndex = (startIndex + 1) % sprites.Count;
            }
            else
            {
                index = s_globalSequentialIndex % sprites.Count;
                s_globalSequentialIndex = (s_globalSequentialIndex + 1) % sprites.Count;
            }
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
