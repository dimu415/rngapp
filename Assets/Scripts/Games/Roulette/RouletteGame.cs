using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouletteGame : MonoBehaviour
{
    [Header("Inputs")]
    public InputField countInput;
    public InputField valuesInput;

    [Header("UI")]
    public Text resultText;
    public Text warningText;

    [Header("Roulette Render")]
    public Transform rouletteRoot;
    public GameObject rouletteItemPrefab;
    public Transform rouletteObject;

    [Header("Spin")]
    public float spinDuration = 3.2f;
    public float minSpinRounds = 4f;
    public float maxSpinRounds = 8f;

    [Header("Config")]
    public string defaultItemPrefix = "Item";
    public int minCount = 2;
    public int maxCount = 100;

    private List<string> _items = new List<string>();
    private bool _isSpinning;

    public void BuildRoulette()
    {
        int count = ParseCount();
        _items = CustomInputParser.BuildItems(valuesInput != null ? valuesInput.text : string.Empty, count, defaultItemPrefix);

        if (warningText != null)
        {
            warningText.text = string.Empty;
        }

        BuildRouletteObjects();

        if (resultText != null)
        {
            resultText.text = $"룰렛 준비 완료 ({_items.Count}개)";
        }
        PublishToUIManager($"룰렛 준비 완료 ({_items.Count}개)");
    }

    public void Spin()
    {
        if (_isSpinning)
        {
            return;
        }

        if (_items.Count < 2)
        {
            BuildRoulette();
        }

        if (_items.Count < 2)
        {
            if (warningText != null)
            {
                warningText.text = "최소 2개 이상 필요합니다.";
            }
            PublishToUIManager("최소 2개 이상 필요합니다.");
            return;
        }

        StartCoroutine(SpinRoutine());
    }

    private IEnumerator SpinRoutine()
    {
        _isSpinning = true;

        int selectedIndex = Random.Range(0, _items.Count);
        float startAngle = rouletteObject != null ? rouletteObject.localEulerAngles.z : 0f;

        float segmentAngle = 360f / _items.Count;
        float selectedAngle = selectedIndex * segmentAngle + segmentAngle * 0.5f;
        float spinRounds = Random.Range(minSpinRounds, maxSpinRounds);
        float endAngle = startAngle - (spinRounds * 360f) - selectedAngle;

        float elapsed = 0f;
        while (elapsed < spinDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / spinDuration);
            float eased = 1f - Mathf.Pow(1f - t, 3f);
            float currentAngle = Mathf.Lerp(startAngle, endAngle, eased);

            if (rouletteObject != null)
            {
                rouletteObject.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
            }

            yield return null;
        }

        if (rouletteObject != null)
        {
            rouletteObject.localRotation = Quaternion.Euler(0f, 0f, endAngle);
        }

        string selected = _items[selectedIndex];

        if (resultText != null)
        {
            resultText.text = selected;
        }

        PublishToUIManager(selected);

        _isSpinning = false;
    }

    private void BuildRouletteObjects()
    {
        if (rouletteRoot == null || rouletteItemPrefab == null)
        {
            return;
        }

        for (int i = rouletteRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(rouletteRoot.GetChild(i).gameObject);
        }

        for (int i = 0; i < _items.Count; i++)
        {
            GameObject obj = Instantiate(rouletteItemPrefab, rouletteRoot);
            Text label = obj.GetComponentInChildren<Text>();
            if (label != null)
            {
                label.text = _items[i];
            }
        }
    }

    private int ParseCount()
    {
        int parsed = minCount;

        if (countInput != null && int.TryParse(countInput.text, out int value))
        {
            parsed = value;
        }

        parsed = Mathf.Clamp(parsed, minCount, maxCount);

        if (countInput != null)
        {
            countInput.text = parsed.ToString();
        }

        return parsed;
    }

    private void PublishToUIManager(string text)
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ResultTset(text);
            return;
        }

        if (UIManager.instace != null)
        {
            UIManager.instace.ResultTset(text);
        }
    }
}
