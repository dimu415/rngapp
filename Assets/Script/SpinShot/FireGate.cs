
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class FireGate : MonoBehaviour
{
    public enum Mode { Bernoulli, OneInN,  Pity }
    public Transform ArrowOb;
    private Vector3 ArrowStart;
    public bool Arrowfire = false;

    [Header("Common")]
    [SerializeField] private Mode mode = Mode.Bernoulli;

    [Header("Bernoulli (매번 독립확률)")]
    [SerializeField, Range(0f, 1f)] private float p = 0.1f;

    [Header("OneInN")]
    [SerializeField, Min(1)] public int N = 3; // 사이클 길이

    [Header("Pity (실패할 때 확률 증가)")]
    [SerializeField, Range(0f, 1f)] private float baseP = 0.1f;
    [SerializeField, Range(0f, 1f)] private float addPOnFail = 0.01f;
    [SerializeField, Range(0f, 1f)] private float maxP = 0.9f;


    int pressInCycle = 0;
    int hitIndex = -1;        // OneInN용: 이 인덱스에서 발사
     public float curP;               // Pity용

    [Header("회전 대상")]
    public Transform cylinder;          // 회전할 원통
    public Camera cam;

    public float sensitivity = 0.2f;
    private float curX;
    private float prevX;
    private float centerY;
    
    bool soundChain=true;
    public bool Trylnn = false;
 
    private void Awake()
    {
        centerY = Camera.main.WorldToScreenPoint(cylinder.position).y;
        ArrowStart = ArrowOb.localPosition;

    }
  
    public void gameReset()
    {
        ArrowOb.localPosition = ArrowStart;

        Arrowfire = false;
        pressInCycle = 0;
        hitIndex = -1;
        curP = baseP;
        Trylnn = false;
    }
    private void OnMouseDown()
    {
        prevX = Input.mousePosition.x;
    }
    private void OnMouseDrag()
    {
        float curY = Input.mousePosition.y; // 현재 Y좌표

       

        curX = Input.mousePosition.x; // 현재 프레임 X
        float delta = curX - prevX;         // 차이
        if (curY > centerY)
        {
            delta = -delta; // 위쪽이면 반대 방향
        }
        if (Mathf.Abs(curX - prevX) > 2f)
        {
            ChainSoundStart();
        }
        cylinder.Rotate(0, 0, delta * sensitivity);
        prevX = curX; // 이번 값을 다음 프레임 prev로 저장
    }
    void ChainSoundStart()
    {
        if (soundChain)
        {
            SoundManager.Instance.PlaySFX("chain");
            soundChain=false;
            Invoke("soundTrue", 0.2f);

        }
    }
    void soundTrue()=>soundChain = true;    
    public void ChangeMode(int idx)
    {
        mode = (Mode)idx;
    }
    public void chang_P(float m)=> p = m;
    public void chang_N(int m) => N = m;
    public void chang_AP(float m) => addPOnFail = m;
    /// <summary>버튼 눌렀을 때 호출: 발사할지 여부</summary>
    public bool TryFire()
    {
        switch (mode)
        {
            case Mode.Bernoulli:
                return Random.value < p;

            case Mode.OneInN:
                if (hitIndex < 0) hitIndex = Random.Range(0, N);
                bool fire = (pressInCycle % N) == hitIndex;
                pressInCycle++;
                if (pressInCycle >= N) { pressInCycle = 0; hitIndex = Random.Range(0, N); }
                return fire;


            case Mode.Pity:
                if (Random.value < curP)
                {
                    return true;
                }
                else
                {
                    curP = Mathf.Min(maxP, curP + addPOnFail);
                    return false;
                }
        }
        return false;
    }

    public bool Fire()
    {
        bool FireIn = TryFire();
        Trylnn = true;
        if (FireIn)
        {
            Arrowfire = true;
            StartCoroutine(FireArrowShot());
        }
        else
        {
            StartCoroutine(NoneArrowShot());

            Trylnn = false;
        }
        return FireIn;
    }
    public string SetT()
    {
        switch (mode)
        {
            case Mode.Bernoulli:
                return "";
            case Mode.OneInN:
                return $"{pressInCycle} / {N}";

            case Mode.Pity:

                return $"{(curP*100).ToString("F2")}%";
        }
        return  "";
    }
    IEnumerator FireArrowShot()
    {
        Vector3 end = new Vector3(0, 10, 0);
        float t = 0;
        SoundManager.Instance.PlaySFX("Arrow");

        while (t < 3f)
        {
            t += Time.deltaTime / 0.3f;
            ArrowOb.localPosition = Vector3.Lerp(ArrowStart, end, t);
            yield return null;
        }
        ArrowOb.position = end;
    }
    IEnumerator NoneArrowShot()
    {
        Vector3 startScale = cylinder.localScale;
        Vector3 minScale = startScale * 0.9f; // 최소 크기 (30%)
        Vector3 maxScale = startScale;        // 원래 크기
        float t = 0;

        SoundManager.Instance.PlaySFX("emp");

        while (t < 0.5f)
        {
            t += Time.deltaTime / 0.3f; // 0.3초 동안
            cylinder.localScale = Vector3.Lerp(startScale, minScale, t);
            yield return null;
        }

        t = 0;
        while (t < 0.5f)
        {
            t += Time.deltaTime / 0.3f; // 다시 0.3초 동안
            cylinder.localScale = Vector3.Lerp(minScale, maxScale, t);
            yield return null;
        }

        cylinder.localScale = maxScale; // 안전하게 보정
    }
}
