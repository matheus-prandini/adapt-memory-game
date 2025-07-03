using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CardAudio : MonoBehaviour
{
    [Tooltip("Arraste aqui os clips: index 0 = carta 0, etc.")]
    public AudioClip[] flipClips;

    AudioSource src;
    [HideInInspector] public int cardID;

    void Awake() => src = GetComponent<AudioSource>();

    public void PlayFlip()
    {
        if (cardID >= 0 && cardID < flipClips.Length && flipClips[cardID] != null)
            src.PlayOneShot(flipClips[cardID]);
    }
}
