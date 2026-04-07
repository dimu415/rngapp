using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LadderGame : MonoBehaviour
{
    [Header("UI")]
    public GameObject settingPanel;
    public GameObject gamePanel;
    public Text warningText;
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
    public Button drawButton;
    public Button resetButton;

    private readonly List<GameObject> _rows = new();
    private readonly List<string> _leftEntries = new();
    private readonly List<string> _rightEntries = new();

    private int _entryCount = 2;
    private int _drawIndex;

    private void Start()
    {
        countPlusButton.onClick.AddListener(() => SetCount(_entryCount + 1));
        countMinusButton.onClick.AddListener(() => SetCount(_entryCount - 1));

        startButton.onClick.AddListener(StartGame);
        drawButton.onClick.AddListener(DrawNext);
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
        _leftEntries.Clear();
        _rightEntries.Clear();
        warningText.text = string.Empty;
        resultText.text = string.Empty;

        for (var i = 0; i < _rows.Count; i++)
        {
            var input = _rows[i].GetComponentInChildren<InputField>();
            var value = input != null ? input.text.Trim() : string.Empty;

            if (string.IsNullOrWhiteSpace(value))
            {
                warningText.text = "모든 입력칸을 채워주세요.";
                return;
            }

            _leftEntries.Add(value);
        }

        _rightEntries.AddRange(_leftEntries);
        Shuffle(_rightEntries);

        _drawIndex = 0;
        settingPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    private void DrawNext()
    {
        if (_drawIndex >= _leftEntries.Count)
        {
            resultText.text = "모든 결과를 확인했습니다.";
            return;
        }

        resultText.text = $"{_leftEntries[_drawIndex]} → {_rightEntries[_drawIndex]}";
        _drawIndex++;
    }

    public void ResetGame()
    {
        settingPanel.SetActive(true);
        gamePanel.SetActive(false);
        warningText.text = string.Empty;
        resultText.text = string.Empty;
        _drawIndex = 0;
    }

    private static void Shuffle(List<string> list)
    {
        for (var i = list.Count - 1; i > 0; i--)
        {
            var j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
