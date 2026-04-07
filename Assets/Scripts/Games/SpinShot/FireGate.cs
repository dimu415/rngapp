
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

    [Header("Bernoulli (Ź Ȯ)")]
    [SerializeField, Range(0f, 1f)] private float p = 0.1f;

    [Header("OneInN")]
    [SerializeField, Min(1)] public int N = 3; // Ŭ

    [Header("Pity (  Ȯ )")]
    [SerializeField, Range(0f, 1f)] private float baseP = 0.1f;
    [SerializeField, Range(0f, 1f)] private float addPOnFail = 0.01f;
    [SerializeField, Range(0f, 1f)] private float maxP = 0.9f;


    int pressInCycle = 0;
    int hitIndex = -1;        // OneInN:  ε ߻
     public float curP;               // Pity

    [Header("ȸ ")]
    public Transform cylinder;          // ȸ
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
        float curY = Input.mousePosition.y; //  Yǥ



        curX = Input.mousePosition.x; //   X
        float delta = curX - prevX;         //
        if (curY > centerY)
        {
            delta = -delta; // ̸ ݴ
        }
        if (Mathf.Abs(curX - prevX) > 2f)
        {
            ChainSoundStart();
        }
        cylinder.Rotate(0, 0, delta * sensitivity);
        prevX = curX; // ̹    prev
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
    /// <summary>ư   ȣ: ߻ </summary>
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
        Vector3 minScale = startScale * 0.9f; // ּ ũ (30%)
        Vector3 maxScale = startScale;        //  ũ
        float t = 0;

        SoundManager.Instance.PlaySFX("emp");

        while (t < 0.5f)
        {
            t += Time.deltaTime / 0.3f; // 0.3
            cylinder.localScale = Vector3.Lerp(startScale, minScale, t);
            yield return null;
        }

        t = 0;
        while (t < 0.5f)
        {
            t += Time.deltaTime / 0.3f; // ٽ 0.3
            cylinder.localScale = Vector3.Lerp(minScale, maxScale, t);
            yield return null;
        }

        cylinder.localScale = maxScale; // ϰ
    }
}
