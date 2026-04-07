using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private int targetFPS = 60;
    [SerializeField] private bool lowPowerMode = false;

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
            return;
        }

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = lowPowerMode ? 30 : targetFPS;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void OnLowMemory()
    {
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }
}
