using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public static UIManager instace; // backward compatibility

    private Coroutine _resultCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            instace = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public GameObject MainBoard;
    public GameObject BackButton;

    public GameObject defaultOb;
    public TextMeshProUGUI resultTx;

    public AdManager adManager;
    [System.Serializable]
    public enum GameType
    {
        Dice,
        Coin,
        Card,
        SpinShot,
        randomDrow,
        TouchOne,
        Bomb,
        Timer
    }
    [System.Serializable]
    public class GameMode
    {
        public GameType gameType;
        public GameObject G_UI;
        public GameObject G_Object;
    }

    public List<GameMode> gameMode;

    public void ClickGameMode(int mode)
    {
        if (mode < 0 || mode >= gameMode.Count) return;

        SetAllGameModesActive(false);
        MainBoard.SetActive(false);
        BackButton.SetActive(true);

        defaultOb.SetActive(true);

        gameMode[mode].G_UI.SetActive(true);
        gameMode[mode].G_Object.SetActive(true);
        ResultKey("empty");
    }
    public void ClickGameBack()
    {
        SetAllGameModesActive(false);
        MainBoard.SetActive(true);

        defaultOb.SetActive(false);
        adManager.ShowInterstitialAd();
    }

    private void SetAllGameModesActive(bool active)
    {
        foreach (var gm in gameMode)
        {
            gm.G_UI.SetActive(active);
            gm.G_Object.SetActive(active);
        }
    }

    public void ResultTset(string t)
    {
        resultTx.text = t;
    }
    public void ResultKey(string key)
    {
        if (_resultCoroutine != null)
        {
            StopCoroutine(_resultCoroutine);
        }

        _resultCoroutine = StartCoroutine(SetLocalizedText(key));
    }

    IEnumerator SetLocalizedText(string key)
    {
        yield return LocalizationSettings.InitializationOperation;

        LocalizedString ls = new LocalizedString("UI_Table", key);
        var handle = ls.GetLocalizedStringAsync();
        yield return handle;

        resultTx.text = handle.Result;
    }
}
