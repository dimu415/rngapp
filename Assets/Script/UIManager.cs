using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instace;
     private void Awake()
    {
        if (instace == null)
            instace = this;
        else
            Destroy(gameObject);
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
        foreach(var gm in gameMode)
        {
            gm.G_UI.SetActive(false);
            gm.G_Object.SetActive(false);
        }
        MainBoard.SetActive(false);
        BackButton.SetActive(true);

        defaultOb.SetActive(true);

        gameMode[mode].G_UI.SetActive(true);
        gameMode[mode].G_Object.SetActive(true);
        ResultKey("empty");
    }
    public void ClickGameBack()
    {
        foreach (var gm in gameMode)
        {
            gm.G_UI.SetActive(false);
            gm.G_Object.SetActive(false);
        }
        MainBoard.SetActive(true);

        defaultOb.SetActive(false);
        adManager.ShowInterstitialAd();
    }

    public void ResultTset(string t)
    {
        resultTx.text = t;
    }
    public void ResultKey(string key)
    {
        StartCoroutine(SetLocalizedText(key));
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
