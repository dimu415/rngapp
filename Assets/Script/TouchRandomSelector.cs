using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TouchRandomSelector : MonoBehaviour
{
    [System.Serializable]
    public class TouchData
    {
        public int pointerId;
        public GameObject circle;
        public Image fillImage;
        public float progress;
    }

    public GameObject circlePrefab;
    public RectTransform parent;
    public float loadTime = 2f;
    public float selectDelay = 2f;

    public GameObject StartBtn;
    Dictionary<int, TouchData> touchMap = new Dictionary<int, TouchData>();
    bool isRunning = false;
    float allLoadedTime = -1f;
    public GameObject RestartBtn;

    public TextMeshProUGUI timerText;    // 선택: 텍스트 표시
    private void OnEnable()
    {
        Set();
    }
    public void Set()
    {
        // 🔥 남아있는 모든 서클 제거 (winner 포함)
        foreach (var data in touchMap.Values)
        {
            if (data.circle != null)
                Destroy(data.circle);
        }

        touchMap.Clear();

        StartBtn.SetActive(true);
        RestartBtn.SetActive(false);

        isRunning = false;
        allLoadedTime = -1f;

        parent.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    // 🔥 라운드 시작
    public void StartRound()
    {
        if (isRunning) return;
        StartBtn.SetActive(false);
        isRunning = true;
        allLoadedTime = -1f;
        timerText.gameObject.SetActive(true);
        timerText.text = "2:00";
        StartCoroutine(RoundRoutine());
    }

    // 🔥 버튼 위 터치
    public void OnTouchDown(int pointerId, Vector2 screenPos)
    {
        if (!isRunning)
        {
           
            return;
        }
        if (touchMap.ContainsKey(pointerId)) return;

        GameObject circle = Instantiate(circlePrefab, parent);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parent,
            screenPos,
            null,
            out Vector2 localPos
        );

        circle.GetComponent<RectTransform>().anchoredPosition = localPos;

        TouchData data = new TouchData
        {
            pointerId = pointerId,
            circle = circle,
            fillImage = circle.transform.Find("Fill").GetComponent<Image>(),
            progress = 0f
        };

        data.fillImage.fillAmount = 0;
        touchMap.Add(pointerId, data);

        // ❗ 새 터치 들어오면 대기 리셋
        allLoadedTime = -1f;
    }

    // 🔥 손 떼면 제거
    public void OnTouchUp(int pointerId)
    {
        if (!isRunning) return; // 🔒 게임 끝났으면 무시
        if (!touchMap.ContainsKey(pointerId)) return;

        Destroy(touchMap[pointerId].circle);
        touchMap.Remove(pointerId);

        allLoadedTime = -1f;
    }

    // =========================
    // 🔁 메인 코루틴 timerText
    // =========================
    IEnumerator RoundRoutine()
    {
        while (isRunning)
        {
            foreach (var data in touchMap.Values)
            {
                data.progress += Time.deltaTime / loadTime;
                data.fillImage.fillAmount = data.progress;
            }

            // ✅ 모두 게이지 완료?
            if (IsAllLoaded())
            {
                if (allLoadedTime < 0f)
                {
                    // 처음 다 찼을 때 시간 기록
                    allLoadedTime = Time.time;
                }
                else if (Time.time - allLoadedTime >= selectDelay)
                {
                    SelectRandomOne();
                    yield break;
                }

                float timer = selectDelay - (Time.time - allLoadedTime);
                timerText.text = timer.ToString("F1");
            }
            else
            {
                // 하나라도 덜 차면 초기화
                allLoadedTime = -1f;
            }

            yield return null;
        }
    }

    bool IsAllLoaded()
    {
        if (touchMap.Count < 2) return false;

        foreach (var data in touchMap.Values)
        {
            if (data.progress < 1f)
                return false;
        }
        return true;
    }
    public GameObject winnerC=null;
    void SelectRandomOne()
    {
        isRunning = false;
        timerText.text = "0:0";
        timerText.gameObject.SetActive(false);
        List <TouchData> list = new List<TouchData>(touchMap.Values);

        TouchData winner = list[Random.Range(0, list.Count)];
        foreach (var data in list)
        {
            if (data != winner)
                Destroy(data.circle);
        }

        touchMap.Clear();
        touchMap.Add(winner.pointerId, winner);
        RestartBtn.SetActive(true);
        StartCoroutine(ColorFadeRoutine());
    }
    IEnumerator ColorFadeRoutine()
    {
        float time = 0f;
         float duration = 0.5f;
        Image targetImage = parent.GetComponent<Image>();
        Color startColor = new Color32(255, 255, 255, 255);
        Color targetColor = new Color32(150, 150, 150, 255);
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            targetImage.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        // 🔒 최종 색 고정
        targetImage.color = targetColor;
    }
}
