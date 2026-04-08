using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RPSGame : MonoBehaviour
{
    private enum Hand
    {
        Rock = 0,
        Paper = 1,
        Scissors = 2
    }

    [Header("UI")]
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject playPanel;
    [SerializeField] private Button startButton;
    [SerializeField] private Button rockButton;
    [SerializeField] private Button paperButton;
    [SerializeField] private Button scissorsButton;
    [SerializeField] private TMP_Text streakText;
    [SerializeField] private TMP_Text roundStateText;

    [Header("Hand Objects")]
    [SerializeField] private GameObject[] playerHandObjects; // 0: rock, 1: paper, 2: scissors
    [SerializeField] private GameObject[] cpuHandObjects; // 0: rock, 1: paper, 2: scissors

    [Header("Animation")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator cpuAnimator;
    [SerializeField] private string shakeTriggerName = "Shake";
    [SerializeField] private float revealDelaySeconds = 0.75f;

    private bool isRoundPlaying;
    private int currentStreak;

    private void OnEnable()
    {
        ResetGameUI();
    }

    public void GameStart()
    {
        currentStreak = 0;
        isRoundPlaying = false;

        if (startPanel != null) startPanel.SetActive(false);
        if (playPanel != null) playPanel.SetActive(true);

        SetButtonsInteractable(true);
        SetHandObjects(-1, -1);
        SetStreakText();
        SetRoundStateText("선택하세요");
        ShowResult("empty");
    }

    public void BackToReady()
    {
        ResetGameUI();
    }

    public void PlayRock() => TryPlayRound(Hand.Rock);
    public void PlayPaper() => TryPlayRound(Hand.Paper);
    public void PlayScissors() => TryPlayRound(Hand.Scissors);

    public void PlayRound(int playerHandIndex)
    {
        if (playerHandIndex < 0 || playerHandIndex > 2)
            return;

        TryPlayRound((Hand)playerHandIndex);
    }

    private void TryPlayRound(Hand playerHand)
    {
        if (isRoundPlaying)
            return;

        StartCoroutine(PlayRoundCoroutine(playerHand));
    }

    private IEnumerator PlayRoundCoroutine(Hand playerHand)
    {
        isRoundPlaying = true;
        SetButtonsInteractable(false);
        SetRoundStateText("가위... 바위... 보!");

        PlayShakeAnimation();
        yield return new WaitForSeconds(revealDelaySeconds);

        Hand cpuHand = (Hand)Random.Range(0, 3);
        SetHandObjects((int)playerHand, (int)cpuHand);

        bool isDraw = playerHand == cpuHand;
        bool playerWins =
            (playerHand == Hand.Rock && cpuHand == Hand.Scissors) ||
            (playerHand == Hand.Paper && cpuHand == Hand.Rock) ||
            (playerHand == Hand.Scissors && cpuHand == Hand.Paper);

        if (isDraw)
        {
            ShowResult("RPS_Draw");
            SetRoundStateText("무승부! 다시 도전");
        }
        else if (playerWins)
        {
            currentStreak++;
            ShowResult("RPS_Win");
            SetRoundStateText("승리!");
        }
        else
        {
            currentStreak = 0;
            ShowResult("RPS_Lose");
            SetRoundStateText("패배!");
        }

        SetStreakText();
        SetButtonsInteractable(true);
        isRoundPlaying = false;
    }

    private void ResetGameUI()
    {
        if (startPanel != null) startPanel.SetActive(true);
        if (playPanel != null) playPanel.SetActive(false);

        currentStreak = 0;
        isRoundPlaying = false;

        SetButtonsInteractable(false);
        SetHandObjects(-1, -1);
        SetStreakText();
        SetRoundStateText("시작 버튼을 눌러주세요");
        ShowResult("empty");

        if (startButton != null)
            startButton.interactable = true;
    }

    private void PlayShakeAnimation()
    {
        if (playerAnimator != null)
            playerAnimator.SetTrigger(shakeTriggerName);

        if (cpuAnimator != null)
            cpuAnimator.SetTrigger(shakeTriggerName);
    }

    private void SetButtonsInteractable(bool interactable)
    {
        if (rockButton != null) rockButton.interactable = interactable;
        if (paperButton != null) paperButton.interactable = interactable;
        if (scissorsButton != null) scissorsButton.interactable = interactable;
    }

    private void SetHandObjects(int playerIndex, int cpuIndex)
    {
        ToggleHandSet(playerHandObjects, playerIndex);
        ToggleHandSet(cpuHandObjects, cpuIndex);
    }

    private static void ToggleHandSet(GameObject[] handObjects, int activeIndex)
    {
        if (handObjects == null) return;

        for (int i = 0; i < handObjects.Length; i++)
        {
            if (handObjects[i] != null)
                handObjects[i].SetActive(i == activeIndex);
        }
    }

    private void SetStreakText()
    {
        if (streakText != null)
            streakText.text = $"연승: {currentStreak}";
    }

    private void SetRoundStateText(string message)
    {
        if (roundStateText != null)
            roundStateText.text = message;
    }

    private void ShowResult(string key)
    {
        if (UIManager.Instance != null)
        {
            UIManager.Instance.ResultKey(key);
            return;
        }

        if (UIManager.instace != null)
        {
            UIManager.instace.ResultKey(key);
        }
    }
}
