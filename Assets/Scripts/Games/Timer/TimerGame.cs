using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
public class TimerGame : MonoBehaviour
{
    [Header("timerUI")]

    public Slider MainSlider;
    public TMP_Text timerText;

    public Slider Slider_1p;
    public TMP_Text timerText_1p;

    public Slider Slider_2p;
    public TMP_Text timerText_2p;

    [Header("SETTINGUI")]
    public Toggle GameModeToggle;
    public bool GameType;

    public GameObject SettingUI;
    public GameObject GameUI;

    public GameObject SingelGameui;
    public GameObject MultGameui;

    public GameObject StartBtn;

    private bool isRunning = false;
    private bool isRunning_1p = false;
    private bool isRunning_2p = false;

    private Coroutine timerCoroutine_single;
    private Coroutine timerCoroutine_1p;
    private Coroutine timerCoroutine_2p;


    private float timer_single;
    private float timer_1p;
    private float timer_2p;
    [Header("Result UI")]
    public GameObject endbtn_s, endbtn_1, endbtn_2;
    public TMP_Text endTimer_Single;
    public TMP_Text endTimer_1p;
    public TMP_Text endTimer_2p;

    public TMP_Text ResultT_1p, ResultT_2p;
    public void GameStart()
    {
        GameType = GameModeToggle.isOn; // true = sin false = muly


        SettingUI.SetActive(false);
        GameUI.SetActive(true);

        if(GameType)
        {
            SingelGameui.SetActive(true);
            MultGameui.SetActive(false);
        }
        else
        {
            SingelGameui.SetActive(false);
            MultGameui.SetActive(true);
        }
    }
    public void onStartBtn()
    {
        if (GameType)
        {
            StartTimer_Single();
        }
        else
        {
            StartTimer_1p();
            StartTimer_2p();
        }
        StartBtn.SetActive(false);
    }
    public void onEndBtn()
    {
        SettingGame();
    }
    private void OnEnable()
    {
        SettingGame();
    }
    public void SettingGame()
    {
        SettingUI.SetActive(true);
        GameUI.SetActive(false);

        StartBtn.SetActive(true);
        endbtn_s.SetActive(false);
        endbtn_1.SetActive(false);
        endbtn_2.SetActive(false);
        StopAllCoroutines();
        isRunning = false;
        isRunning_1p = false;
        isRunning_2p = false;
    }

    public void StartTimer_Single()
    {
        if (isRunning) return;

        isRunning = true;
        timerCoroutine_single = StartCoroutine(TimerRoutine_Single());
    }
    public void StopTimer_Single()
    {
        if (!isRunning) return;

        isRunning = false;

        if (timerCoroutine_single != null)
        {
            StopCoroutine(timerCoroutine_single);
            timerCoroutine_single = null;
            gameEnd();
        }
    }
    private IEnumerator TimerRoutine_Single()
    {
        timer_single = 10f;
        while (timer_single > -5f)
        {
            timer_single -= Time.deltaTime;

            MainSlider.value = timer_single;
            timerText.text = timer_single.ToString("F2");
            yield return null;
        }

        MainSlider.value = timer_single;
        timerText.text = timer_single.ToString("F2");

        isRunning = false;
        timerCoroutine_single = null;
        gameEnd();
    }
    private void MultGameEnd()
    {
        if (!isRunning_1p && !isRunning_2p) gameEnd();
    }
    public void StartTimer_1p()
    {
        if (isRunning_1p) return;

        isRunning_1p = true;
        timerCoroutine_1p = StartCoroutine(TimerRoutine_1p());
    }
    public void StopTimer_1p()
    {
        if (!isRunning_1p) return;

        isRunning_1p = false;

        if (timerCoroutine_1p != null)
        {
            StopCoroutine(timerCoroutine_1p);
            timerCoroutine_1p = null;
            MultGameEnd();
        }
    }
    private IEnumerator TimerRoutine_1p()
    {
        timer_1p = 10f;
        while (timer_1p > -5f)
        {
            timer_1p -= Time.deltaTime;

            Slider_1p.value = timer_1p;
            timerText_1p.text = timer_1p.ToString("F2");
            yield return null;
        }


        Slider_1p.value = timer_1p;
        timerText_1p.text = timer_1p.ToString("F2");

        isRunning_1p = false;
        timerCoroutine_1p = null; MultGameEnd();
    }

    public void StartTimer_2p()
    {
        if (isRunning_2p) return;

        isRunning_2p = true;
        timerCoroutine_2p = StartCoroutine(TimerRoutine_2p());
    }
    public void StopTimer_2p()
    {
        if (!isRunning_2p) return;

        isRunning_2p = false;

        if (timerCoroutine_2p != null)
        {
            StopCoroutine(timerCoroutine_2p);
            timerCoroutine_2p = null;
            MultGameEnd();
        }
    }
    private IEnumerator TimerRoutine_2p()
    {
        timer_2p = 10f;
        while (timer_2p > -5f)
        {
            timer_2p -= Time.deltaTime;


            Slider_2p.value = timer_2p;
            timerText_2p.text = timer_2p.ToString("F2");
            yield return null;
        }

        Slider_2p.value = timer_2p;
        timerText_2p.text = timer_2p.ToString("F2");

        isRunning_2p = false;
        timerCoroutine_2p = null;
        MultGameEnd();
    }
    public void gameEnd()
    {
        if (GameType)
        {
            endbtn_s.SetActive(true);
            endTimer_Single.text = timer_single.ToString("F2");
        }

        else
        {
            endbtn_1.SetActive(true);
            endTimer_1p.text = timer_1p.ToString("F2");

            endbtn_2.SetActive(true);
            endTimer_2p.text = timer_2p.ToString("F2");

            // 둘 다 플러스(0 포함)
            if (timer_1p >= 0f && timer_2p >= 0f)
            {
                if (timer_1p < timer_2p)
                {
                    ResultT_1p.text = "Win";
                    ResultT_2p.text = "Lose";
                    ResultT_1p.color = new Color32(0, 0, 255, 255);
                    ResultT_2p.color = new Color32(255, 0, 0, 255);
                }
                else if (timer_1p > timer_2p)
                {
                    ResultT_1p.text = "Lose";
                    ResultT_2p.text = "Win";
                    ResultT_1p.color = new Color32(255, 0, 0, 255);
                    ResultT_2p.color = new Color32(0, 0, 255, 255);
                }
                else
                {
                    ResultT_1p.text = "Draw";
                    ResultT_2p.text = "Draw";
                    ResultT_1p.color = new Color32(255, 255, 255, 255);
                    ResultT_2p.color = new Color32(255, 255, 255, 255);
                }
            }
            // 1P만 마이너스 -> 1P 패배
            else if (timer_1p < 0f && timer_2p >= 0f)
            {
                ResultT_1p.text = "Lose";
                ResultT_2p.text = "Win";
                ResultT_1p.color = new Color32(255, 0, 0, 255);
                ResultT_2p.color = new Color32(0, 0, 255, 255);
            }
            // 2P만 마이너스 -> 2P 패배
            else if (timer_1p >= 0f && timer_2p < 0f)
            {
                ResultT_1p.text = "Win";
                ResultT_2p.text = "Lose";
                ResultT_1p.color = new Color32(0, 0, 255, 255);
                ResultT_2p.color = new Color32(255, 0, 0, 255);
            }
            // 둘 다 마이너스
            else
            {
                // 더 큰 수가 승리 (-0.01 > -0.20)
                if (timer_1p > timer_2p)
                {
                    ResultT_1p.text = "Win";
                    ResultT_2p.text = "Lose";
                    ResultT_1p.color = new Color32(0, 0, 255, 255);
                    ResultT_2p.color = new Color32(255, 0, 0, 255);
                }
                else if (timer_1p < timer_2p)
                {
                    ResultT_1p.text = "Lose";
                    ResultT_2p.text = "Win";
                    ResultT_1p.color = new Color32(255, 0, 0, 255);
                    ResultT_2p.color = new Color32(0, 0, 255, 255);
                }
                else
                {
                    ResultT_1p.text = "Draw";
                    ResultT_2p.text = "Draw";
                    ResultT_1p.color = new Color32(255, 255, 255, 255);
                    ResultT_2p.color = new Color32(255, 255, 255, 255);
                }
            }
        }
    }
}
