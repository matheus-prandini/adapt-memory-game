using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CardAudio : MonoBehaviour
{
    [HideInInspector] public AudioClip[] audioClips;
    [HideInInspector] public int      cardID;

    AudioSource src;

    void Awake()
    {
        src = GetComponent<AudioSource>();
    }

    public void PlayFlip()
    {
        if (audioClips == null) return;
        if (cardID < 0 || cardID >= audioClips.Length) return;
        var clip = audioClips[cardID];
        if (clip != null)
            src.PlayOneShot(clip);
    }
}
