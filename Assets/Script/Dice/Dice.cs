using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    Rigidbody rb;
    public bool isRolling = false;
    public int diceNumber = 0;

    void Awake()
    {    
        rb = GetComponent<Rigidbody>();
    }
    public int ThrowDice()
    {
        transform.position = new Vector3(0.5f, 1f, 0);

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;


        float randX = Random.Range(-3.5f, -3f);
        float randY = Random.Range(0f, 6f);       // 위로 던지는 힘
        float rand_2 = Random.Range(3f,13f);
        rb.AddForce(new Vector3(randX, randY, 0), ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * rand_2, ForceMode.Impulse);
        isRolling = true;
        StartCoroutine(rolling());
        return diceNumber;
    }
    IEnumerator rolling()
    {
        diceNumber = 0;
        while (isRolling)
        {
            if (rb.IsSleeping())
            {
                isRolling = false;
                 diceNumber = GetTopFaceNumber();
            }
            yield return null;
        }
    }

    // (6 >6)  (2> 1) (4 > 3) (5 > 2)  (1 >5) (3 >4)
    int GetTopFaceNumber()
    {
        Vector3[] faceNormals = {
            transform.forward,         // 1
            transform.up,        // 2
            -transform.right,      // 3
            transform.right,     // 4
            -transform.up,    // 5
            -transform.forward    //6
        };

        float maxDot = -1f;
        int topFace = 0;

        for (int i = 0; i < 6; i++)
        {
            float dot = Vector3.Dot(faceNormals[i], Vector3.up);
            if (dot > maxDot)
            {
                maxDot = dot;
                topFace = i + 1; // 1~6��
            }
        }
        return topFace;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("wall") && isRolling)
        {
            // ȿ������ ó������ �ٽ� ���
            SoundManager.Instance.PlaySFX("Dice");
        }
    }
    
    }
