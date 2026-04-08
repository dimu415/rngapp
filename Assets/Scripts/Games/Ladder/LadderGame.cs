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

    [Header("Ladder Render")]
    public RectTransform ladderRoot;
    public Image ladderCellPrefab;
    public Sprite straightSprite;
    public Sprite leftSprite;
    public Sprite rightSprite;
    public int ladderRows = 8;
    public float cellWidth = 70f;
    public float cellHeight = 50f;
    [Range(0f, 1f)] public float horizontalLineChance = 0.35f;

    [Header("Config")]
    public string defaultItemPrefix = "Player";
    public int minCount = 2;
    public int maxCount = 30;

    private List<string> _items = new List<string>();
    private int[,] _ladderMap;

    public void BuildLadder()
    {
        int count = ParseCount();
        _items = CustomInputParser.BuildItems(valuesInput != null ? valuesInput.text : string.Empty, count, defaultItemPrefix);

        if (warningText != null)
        {
            warningText.text = string.Empty;
        }

        GenerateLadderMap(_items.Count, ladderRows);
        RenderLadder();

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

    private void GenerateLadderMap(int columnCount, int rowCount)
    {
        if (columnCount < 2 || rowCount < 1)
        {
            _ladderMap = null;
            return;
        }

        _ladderMap = new int[rowCount, columnCount];

        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < columnCount - 1; col++)
            {
                if (_ladderMap[row, col] != 0 || _ladderMap[row, col + 1] != 0)
                {
                    continue;
                }

                bool canPlaceHorizontal = Random.value < horizontalLineChance;
                if (!canPlaceHorizontal)
                {
                    continue;
                }

                // 2 = right, 1 = left
                _ladderMap[row, col] = 2;
                _ladderMap[row, col + 1] = 1;
            }
        }
    }

    private void RenderLadder()
    {
        if (ladderRoot == null || ladderCellPrefab == null || _ladderMap == null)
        {
            return;
        }

        for (int i = ladderRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(ladderRoot.GetChild(i).gameObject);
        }

        int rows = _ladderMap.GetLength(0);
        int cols = _ladderMap.GetLength(1);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Image cell = Instantiate(ladderCellPrefab, ladderRoot);
                RectTransform rt = cell.rectTransform;
                rt.anchoredPosition = new Vector2(col * cellWidth, -row * cellHeight);

                int type = _ladderMap[row, col];
                if (type == 1)
                {
                    cell.sprite = leftSprite;
                }
                else if (type == 2)
                {
                    cell.sprite = rightSprite;
                }
                else
                {
                    cell.sprite = straightSprite;
                }
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

    private void Shuffle(List<string> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
