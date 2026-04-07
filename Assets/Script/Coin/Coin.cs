using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float resultCheckDelay = 1.2f;

    private Vector3 or_coin = new Vector3(0, -0.5f, 0);
    Rigidbody rb;
    public bool tossed = false;
    public string Result;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void TossCoin()
    {
        if (tossed) return;

        transform.localPosition = or_coin;
        tossed = true;
        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        float throwForce = Random.Range(4f, 6f);
        float spinForce = Random.Range(40f, 60f);
        rb.AddForce(Vector3.up * throwForce, ForceMode.Impulse);
        rb.AddTorque(Vector3.right * spinForce + Random.onUnitSphere * 2f, ForceMode.Impulse);

        StartCoroutine(CheckResultAfterDelay());
    }

    IEnumerator CheckResultAfterDelay()
    {
        yield return new WaitForSeconds(resultCheckDelay);

        yield return new WaitUntil(() => rb.linearVelocity.magnitude < 0.05f && rb.angularVelocity.magnitude < 0.05f);

        Result = JudgeResult();

        tossed = false;
    }

    string JudgeResult()
    {
        float dot = Vector3.Dot(transform.up, Vector3.up);
        if (dot > 0.7f)
            return "Coin_head";
        else if (dot < -0.7f)
            return "Coin_tail";
        else
            return "Coin_edge";
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("wall") && tossed)
        {
            SoundManager.Instance.PlaySFX("Coin");
        }
    }

}
