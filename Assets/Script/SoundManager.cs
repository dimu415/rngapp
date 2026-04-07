using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Effect Sources")]
    public AudioSource sfxSource;     // 효과음용 오디오소스

    [Header("Clips")]
    public AudioClip[] sfxClips;      // 효과음들 (인덱스별/이름별)

    void Awake()
    {
        // 싱글톤
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
    // 효과음(One Shot)
    public void PlaySFX(int index)
    {
        if (sfxClips.Length > index && sfxClips[index] != null)
            sfxSource.PlayOneShot(sfxClips[index]);
    }
    // 효과음(이름으로)
    public void PlaySFX(string name)
    {
        Debug.Log(name);
        var clip = System.Array.Find(sfxClips, c => c != null && c.name == name);
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
