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

    [Header("Config")]
    public string defaultItemPrefix = "Item";
    public int minCount = 2;
    public int maxCount = 100;

    private List<string> _items = new List<string>();

    public void BuildRoulette()
    {
        int count = ParseCount();
        _items = CustomInputParser.BuildItems(valuesInput != null ? valuesInput.text : string.Empty, count, defaultItemPrefix);

        if (warningText != null)
        {
            warningText.text = string.Empty;
        }

        if (resultText != null)
        {
            resultText.text = $"룰렛 준비 완료 ({_items.Count}개)";
        }
    }

    public void Spin()
    {
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
            return;
        }

        string selected = _items[Random.Range(0, _items.Count)];

        if (resultText != null)
        {
            resultText.text = selected;
        }

        if (UIManager.instace != null)
        {
            UIManager.instace.ResultTset(selected);
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
}
