using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OddEvenGame : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject readyPanel;
    [SerializeField] private GameObject playPanel;

    [Header("Count Setting (1~10)")]
    [SerializeField] private TMP_Text objectCountText;
    [SerializeField] private Button minusButton;
    [SerializeField] private Button plusButton;
    [SerializeField] private int minCount = 1;
    [SerializeField] private int maxCount = 10;

    [Header("Guess Button")]
    [SerializeField] private Button oddButton;
    [SerializeField] private Button evenButton;

    [Header("Streak UI")]
    [SerializeField] private TMP_Text streakText;
    [SerializeField] private TMP_Text statusText;

    [Header("Sphere Object")]
    [SerializeField] private Transform sphereRoot;
    [SerializeField] private GameObject spherePrefab;
    [SerializeField] private Vector3 spawnArea = new Vector3(3f, 0f, 3f);

    private readonly List<GameObject> _spawnedSpheres = new List<GameObject>();

    private int _selectedCount = 1;
    private int _currentStreak = 0;

    private void OnEnable()
    {
        ResetToReady();
    }

    public void StartGame()
    {
        if (readyPanel != null) readyPanel.SetActive(false);
        if (playPanel != null) playPanel.SetActive(true);

        _currentStreak = 0;
        UpdateStreakText();
        UpdateCountText();
        SetStatus("홀/짝을 선택하세요");
        PublishResult("empty");
        SetGuessButtons(true);
    }

    public void BackToReady()
    {
        ResetToReady();
    }

    public void IncreaseCount() => SetObjectCount(_selectedCount + 1);
    public void DecreaseCount() => SetObjectCount(_selectedCount - 1);

    public void SetObjectCount(int count)
    {
        _selectedCount = Mathf.Clamp(count, minCount, maxCount);
        UpdateCountText();
        UpdateCountButtons();
    }

    public void GuessOdd()
    {
        PlayRound(true);
    }

    public void GuessEven()
    {
        PlayRound(false);
    }

    private void PlayRound(bool guessOdd)
    {
        int spawnedCount = Random.Range(minCount, _selectedCount + 1);
        bool resultOdd = spawnedCount % 2 == 1;
        bool isWin = resultOdd == guessOdd;

        SpawnSpheres(spawnedCount);

        if (isWin)
        {
            _currentStreak++;
            PublishResult("RPS_Win");
            SetStatus($"정답! 구체 {spawnedCount}개 ({(resultOdd ? "홀" : "짝")})");
        }
        else
        {
            _currentStreak = 0;
            PublishResult("RPS_Lose");
            SetStatus($"오답! 구체 {spawnedCount}개 ({(resultOdd ? "홀" : "짝")})");
        }

        UpdateStreakText();
    }

    private void SpawnSpheres(int count)
    {
        ClearSpawnedSpheres();

        Transform parent = sphereRoot != null ? sphereRoot : transform;
        for (int i = 0; i < count; i++)
        {
            Vector3 localPos = new Vector3(
                Random.Range(-spawnArea.x, spawnArea.x),
                Random.Range(-spawnArea.y, spawnArea.y),
                Random.Range(-spawnArea.z, spawnArea.z));

            GameObject sphere;
            if (spherePrefab != null)
            {
                sphere = Instantiate(spherePrefab, parent);
                sphere.transform.localPosition = localPos;
            }
            else
            {
                sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.SetParent(parent, false);
                sphere.transform.localPosition = localPos;
                sphere.transform.localScale = Vector3.one * 0.5f;
            }

            _spawnedSpheres.Add(sphere);
        }
    }

    private void ClearSpawnedSpheres()
    {
        for (int i = 0; i < _spawnedSpheres.Count; i++)
        {
            if (_spawnedSpheres[i] != null)
                Destroy(_spawnedSpheres[i]);
        }

        _spawnedSpheres.Clear();
    }

    private void ResetToReady()
    {
        if (readyPanel != null) readyPanel.SetActive(true);
        if (playPanel != null) playPanel.SetActive(false);

        _selectedCount = Mathf.Clamp(_selectedCount, minCount, maxCount);
        _currentStreak = 0;

        ClearSpawnedSpheres();
        UpdateCountText();
        UpdateCountButtons();
        UpdateStreakText();
        SetStatus("시작 버튼을 눌러 준비하세요");
        PublishResult("empty");
        SetGuessButtons(false);
    }

    private void UpdateCountText()
    {
        if (objectCountText != null)
            objectCountText.text = _selectedCount.ToString();
    }

    private void UpdateCountButtons()
    {
        if (minusButton != null)
            minusButton.interactable = _selectedCount > minCount;

        if (plusButton != null)
            plusButton.interactable = _selectedCount < maxCount;
    }

    private void UpdateStreakText()
    {
        if (streakText != null)
            streakText.text = $"연승: {_currentStreak}";
    }

    private void SetGuessButtons(bool enabled)
    {
        if (oddButton != null) oddButton.interactable = enabled;
        if (evenButton != null) evenButton.interactable = enabled;
    }

    private void SetStatus(string text)
    {
        if (statusText != null)
            statusText.text = text;
    }

    private void PublishResult(string key)
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ResultKey(key);
        }
        else if (UIManager.instace != null)
        {
            UIManager.instace.ResultKey(key);
        }
    }
}
