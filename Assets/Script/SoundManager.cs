using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Effect Sources")]
    public AudioSource sfxSource;

    [Header("Clips")]
    public AudioClip[] sfxClips;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(int index)
    {
        if (sfxSource == null || sfxClips == null) return;
        if (index < 0 || index >= sfxClips.Length) return;

        AudioClip clip = sfxClips[index];
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlaySFX(string clipName)
    {
        if (sfxSource == null || sfxClips == null || string.IsNullOrEmpty(clipName)) return;

        AudioClip clip = System.Array.Find(sfxClips, c => c != null && c.name == clipName);
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
