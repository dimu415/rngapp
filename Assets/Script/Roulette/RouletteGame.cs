using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteGame : MonoBehaviour
{
    [Header("UI")]
    public GameObject settingPanel;
    public GameObject gamePanel;
    public Text warningText;
    public Text rouletteText;
    public Text resultText;

    [Header("Count")]
    public InputField countInput;
    public Button countPlusButton;
    public Button countMinusButton;
    public int minCount = 2;
    public int maxCount = 20;

    [Header("Entries")]
    public Transform inputRoot;
    public GameObject inputItemPrefab;

    [Header("Actions")]
    public Button startButton;
    public Button spinButton;
    public Button resetButton;
    public float spinDuration = 2.5f;
    public float tickInterval = 0.08f;

    private readonly List<GameObject> _rows = new();
    private readonly List<string> _entries = new();

    private int _entryCount = 2;
    private bool _isSpinning;
    private Coroutine _spinRoutine;

    private void Start()
    {
        countPlusButton.onClick.AddListener(() => SetCount(_entryCount + 1));
        countMinusButton.onClick.AddListener(() => SetCount(_entryCount - 1));

        startButton.onClick.AddListener(StartGame);
        spinButton.onClick.AddListener(Spin);
        resetButton.onClick.AddListener(ResetGame);

        SetCount(_entryCount);
        ResetGame();
    }

    private void OnEnable()
    {
        ResetGame();
    }

    public void OnCountInputChanged()
    {
        if (int.TryParse(countInput.text, out var value))
        {
            SetCount(value);
        }
    }

    private void SetCount(int value)
    {
        _entryCount = Mathf.Clamp(value, minCount, maxCount);
        countInput.text = _entryCount.ToString();

        while (_rows.Count < _entryCount)
        {
            var row = Instantiate(inputItemPrefab, inputRoot);
            _rows.Add(row);
        }

        while (_rows.Count > _entryCount)
        {
            var last = _rows[^1];
            _rows.RemoveAt(_rows.Count - 1);
            Destroy(last);
        }
    }

    private void StartGame()
    {
        _entries.Clear();
        warningText.text = string.Empty;
        resultText.text = string.Empty;
        rouletteText.text = "-";

        foreach (var row in _rows)
        {
            var input = row.GetComponentInChildren<InputField>();
            var value = input != null ? input.text.Trim() : string.Empty;

            if (string.IsNullOrWhiteSpace(value))
            {
                warningText.text = "모든 입력칸을 채워주세요.";
                return;
            }

            _entries.Add(value);
        }

        settingPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    private void Spin()
    {
        if (_isSpinning || _entries.Count < minCount)
        {
            return;
        }

        _spinRoutine = StartCoroutine(SpinRoutine());
    }

    private IEnumerator SpinRoutine()
    {
        _isSpinning = true;
        resultText.text = string.Empty;

        var elapsed = 0f;
        string current = string.Empty;

        while (elapsed < spinDuration)
        {
            current = _entries[Random.Range(0, _entries.Count)];
            rouletteText.text = current;

            yield return new WaitForSeconds(tickInterval);
            elapsed += tickInterval;
        }

        resultText.text = $"결과: {current}";
        _isSpinning = false;
        _spinRoutine = null;
    }

    public void ResetGame()
    {
        if (_spinRoutine != null)
        {
            StopCoroutine(_spinRoutine);
            _spinRoutine = null;
        }

        _isSpinning = false;
        settingPanel.SetActive(true);
        gamePanel.SetActive(false);
        warningText.text = string.Empty;
        rouletteText.text = "-";
        resultText.text = string.Empty;
    }
}
