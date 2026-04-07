using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomBox : MonoBehaviour
{
    [Header(" MODE")]
    public Toggle toggleRange;
    public Toggle toggleCustom;

    public GameObject RangeWindow, CustomWindow,GAMEUI,SETTINGUI;
    public GameObject Box;

    [Header(" RANGE UI ")]
    public InputField minInput;
    public InputField maxInput;
    public Button minPlusBtn;
    public Button minMinusBtn;
    public Button maxPlusBtn;
    public Button maxMinusBtn;

    int minValue = 2;
    int maxValue = 99;

    [Header("CUSTOM UI ")]
    public Transform customContent;
    public GameObject customItemPrefab; // InputField + DeleteBtn
    public Button addCustomBtn;

    List<GameObject> customItems = new();

    [Header("  RESULT  ")]
    public Button startBtn;
    public Button boxBtn;
    public Text resultText;
    public GameObject Paper;
    public bool Openln = false;
    List<string> boxItems = new();

    public Text WarnningTxet;

    public Sprite BoxIdle, BoxOpen;
    private Image _boxImage;

    // ===============================
    void Start()
    {
        _boxImage = Box.GetComponent<Image>();

        // Range 버튼 연결
        minPlusBtn.onClick.AddListener(() => SetMin(minValue + 1));
        minMinusBtn.onClick.AddListener(() => SetMin(minValue - 1));
        maxPlusBtn.onClick.AddListener(() => SetMax(maxValue + 1));
        maxMinusBtn.onClick.AddListener(() => SetMax(maxValue - 1));

        // 기본 커스텀 2개
        AddCustomItem();
        AddCustomItem();

        addCustomBtn.onClick.AddListener(AddCustomItem);
        startBtn.onClick.AddListener(OnStart);

        UpdateRangeUI();
    }
    private void OnEnable()
    {

        GameSet();
    }
    public void GameSet()
    {
        GAMEUI.SetActive(false);
        SETTINGUI.SetActive(true);
        Paper.SetActive(false);
        WarnningTxet.text = "";
        Openln = false;
    }
    public void OnMethod()
    {
        if (toggleRange.isOn)
        {
            RangeWindow.SetActive(true);
            CustomWindow.SetActive(false);
        }
        else
        {
            RangeWindow.SetActive(false);
            CustomWindow.SetActive(true);
        }
    }
    // ===============================
    // RANGE LOGIC
    void SetMin(int value)
    {
        minValue = Mathf.Clamp(value,1 , maxValue - 1);
        UpdateRangeUI();
    }

    void SetMax(int value)
    {
        maxValue = Mathf.Clamp(value, minValue + 1, 99);
        UpdateRangeUI();
    }

    public void OnMinInput()
    {
        string value = minInput.text;
        if (int.TryParse(value, out int v))
            SetMin(v);
    }

    public void OnMaxInput()
    {
        string value = maxInput.text;
        if (int.TryParse(value, out int v))
            SetMax(v);
    }

    void UpdateRangeUI()
    {
        minInput.text = minValue.ToString();
        maxInput.text = maxValue.ToString();
    }

    // ===============================
    // CUSTOM LOGIC
    void AddCustomItem()
    {
        GameObject item = Instantiate(customItemPrefab, customContent);
        Button delBtn = item.transform.Find("x").GetComponent<Button>();
        delBtn.onClick.AddListener(() => RemoveCustomItem(item));

        customItems.Add(item);
    }

    void RemoveCustomItem(GameObject item)
    {

        customItems.Remove(item);
        Destroy(item);
    }

    List<string> GetCustomValues()
    {
        List<string> list = new();

        foreach (var item in customItems)
        {
            InputField input = item.GetComponentInChildren<InputField>();
            if (!string.IsNullOrWhiteSpace(input.text))
                list.Add(input.text);
        }

        return list;
    }

    // ===============================
    // START → 종이 넣기
    void OnStart()
    {
        boxItems.Clear();
        resultText.text = "";
        _boxImage.sprite = BoxIdle;
        if (toggleRange.isOn)
        {
            for (int i = minValue; i <= maxValue; i++)
                boxItems.Add(i.ToString());
        }
        else
        {
            boxItems = GetCustomValues();
        }

        Debug.Log($"박스에 {boxItems.Count}개 들어감");
        if (boxItems.Count <= 1)
        {

            WarnningTxet.text = "⚠️ Not enough items.\r\nPlease add at least 2 items to proceed.";
            return;
        }

        Box.SetActive(true);
        GAMEUI.SetActive(true);
        SETTINGUI.SetActive(false);
    }

    // ===============================
    // BOX CLICK → 랜덤 뽑기
    public void OnBoxClick()
    {
        if (Openln) return;
        Openln = true;
        _boxImage.sprite = BoxOpen;
        DrawRandomItem();
    }

    public void OpenPaper(GameObject obj)
    {
        if (Openln) return;

        // 1. 투명하게
        var img = obj.GetComponent<Image>();
        if (img != null)
            img.color = new Color(1, 1, 1, 0);

        // 2. 클릭 막기
        var btn = obj.GetComponent<Button>();
        if (btn != null)
            btn.interactable = false;

        Paper.SetActive(true);
        DrawRandomItem();

        Openln = true;
    }
    public void PaperClick()
    {
        if (boxItems.Count == 0)
        {
            GameSet();

        }
        _boxImage.sprite = BoxIdle;
        Paper.SetActive(false);
        Openln= false;

    }

    private void DrawRandomItem()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFX("Paper");
        }

        if (boxItems.Count == 0)
        {
            resultText.text = "끝!";
            return;
        }

        int index = Random.Range(0, boxItems.Count);
        string result = boxItems[index];
        boxItems.RemoveAt(index);
        resultText.text = result;
    }

}
