using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LadderGame : MonoBehaviour
{
    [Header("Inputs")]
    public InputField countInput;
    public InputField valuesInput;

    [Header("UI")]
    public Text resultText;
    public Text warningText;

    [Header("Config")]
    public string defaultItemPrefix = "Player";
    public int minCount = 2;
    public int maxCount = 30;

    private List<string> _items = new List<string>();

    public void BuildLadder()
    {
        int count = ParseCount();
        _items = CustomInputParser.BuildItems(valuesInput != null ? valuesInput.text : string.Empty, count, defaultItemPrefix);

        if (warningText != null)
        {
            warningText.text = string.Empty;
        }

        if (resultText != null)
        {
            resultText.text = $"사다리 준비 완료 ({_items.Count}명)";
        }
    }

    public void RunLadder()
    {
        if (_items.Count < 2)
        {
            BuildLadder();
        }

        if (_items.Count < 2)
        {
            if (warningText != null)
            {
                warningText.text = "최소 2명 이상 필요합니다.";
            }
            return;
        }

        List<string> shuffled = new List<string>(_items);
        Shuffle(shuffled);

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < _items.Count; i++)
        {
            sb.AppendLine($"{_items[i]} → {shuffled[i]}");
        }

        string output = sb.ToString().TrimEnd();

        if (resultText != null)
        {
            resultText.text = output;
        }

        if (UIManager.instace != null)
        {
            UIManager.instace.ResultTset(output);
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

    private void Shuffle(List<string> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
