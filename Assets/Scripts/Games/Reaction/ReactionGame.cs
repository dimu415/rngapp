using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReactionGame : MonoBehaviour
{
    [Header("UI")]
    public GameObject SettingUI;
    public GameObject GameUI;
    public Button reactionButton;
    public Image reactionButtonImage;
    public TMP_Text statusText;
    public TMP_Text resultText;

    [Header("Reaction Settings")]
    public float minDelaySeconds = 1.2f;
    public float maxDelaySeconds = 4f;
    public Color waitColor = new Color32(220, 30, 30, 255);
    public Color goColor = new Color32(30, 200, 70, 255);
    public Color idleColor = Color.white;

    private Coroutine waitCoroutine;
    private float readyTime;

    private enum ReactionState
    {
        Idle,
        Waiting,
        Ready,
        Result
    }

    private ReactionState currentState = ReactionState.Idle;

    private void OnEnable()
    {
        ResetToIdle();
    }

    public void GameStart()
    {
        if (SettingUI != null) SettingUI.SetActive(false);
        if (GameUI != null) GameUI.SetActive(true);

        ResetToIdle();
        SetStatus("버튼을 눌러 시작");
        ClearMainResult();
    }

    public void BackToSetting()
    {
        if (SettingUI != null) SettingUI.SetActive(true);
        if (GameUI != null) GameUI.SetActive(false);

        ResetToIdle();
        ClearMainResult();
    }

    public void OnReactionButtonPressed()
    {
        switch (currentState)
        {
            case ReactionState.Idle:
            case ReactionState.Result:
                StartRound();
                break;

            case ReactionState.Waiting:
                TooEarly();
                break;

            case ReactionState.Ready:
                SaveReactionResult();
                break;
        }
    }

    private void StartRound()
    {
        ResetToIdle();
        currentState = ReactionState.Waiting;

        SetButtonColor(waitColor);
        SetStatus("기다리세요...");
        SetResult("-");

        waitCoroutine = StartCoroutine(WaitAndTurnGreen());
    }

    private IEnumerator WaitAndTurnGreen()
    {
        float delay = Random.Range(minDelaySeconds, maxDelaySeconds);
        yield return new WaitForSeconds(delay);

        readyTime = Time.realtimeSinceStartup;
        currentState = ReactionState.Ready;
        waitCoroutine = null;

        SetButtonColor(goColor);
        SetStatus("지금 터치!");
    }

    private void TooEarly()
    {
        ResetToIdle();
        currentState = ReactionState.Result;

        SetStatus("너무 빨라요! 다시 도전");
        SetResult("FAIL");
        PublishToUIManager("FAIL");
    }

    private void SaveReactionResult()
    {
        float reactionMs = (Time.realtimeSinceStartup - readyTime) * 1000f;
        currentState = ReactionState.Result;

        SetButtonColor(idleColor);
        SetStatus("측정 완료");
        string result = $"{reactionMs:F0} ms";
        SetResult(result);
        PublishToUIManager(result);
    }

    private void ResetToIdle()
    {
        if (waitCoroutine != null)
        {
            StopCoroutine(waitCoroutine);
            waitCoroutine = null;
        }

        currentState = ReactionState.Idle;
        readyTime = 0f;

        SetButtonColor(idleColor);
    }

    private void SetButtonColor(Color targetColor)
    {
        if (reactionButtonImage != null)
        {
            reactionButtonImage.color = targetColor;
            return;
        }

        if (reactionButton == null) return;

        Image[] images = reactionButton.GetComponentsInChildren<Image>(true);
        foreach (Image image in images)
        {
            image.color = targetColor;
        }
    }

    private void SetStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }

    private void SetResult(string message)
    {
        if (resultText != null)
        {
            resultText.text = message;
        }
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

    private void ClearMainResult()
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ResultKey("empty");
            return;
        }

        if (UIManager.instace != null)
        {
            UIManager.instace.ResultKey("empty");
        }
    }
}
