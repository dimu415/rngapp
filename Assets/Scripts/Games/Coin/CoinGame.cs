using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGame : MonoBehaviour
{
    public bool isToss = false;
    public int I_DiceNumber = 0;
    public GameObject ob_Coin;
    public List<Material> Mt_Coin;

    private void OnEnable()
    {
        isToss = false;
    }
    public void Coin_Chang(int v)
    {
        if (!isToss)
        {
            int AfterCount = I_DiceNumber + v;
            if (0 <= AfterCount && AfterCount < Mt_Coin.Count)
            {
                ob_Coin.GetComponent<MeshRenderer>().material = Mt_Coin[AfterCount];
                I_DiceNumber = AfterCount;
            }
            Debug.Log(AfterCount);
        }
    }
    public void Coin_Toss()
    {
        if (!isToss)
        {
            ob_Coin.GetComponent<Coin>().tossed = false;
            isToss = true;
            StartCoroutine(Coin_Toss_co());
        }
    }
    IEnumerator Coin_Toss_co()
    {
        Result("empty");
        ob_Coin.GetComponent<Coin>().TossCoin();
        while (ob_Coin.GetComponent<Coin>().tossed)
            yield return null;
        Result(ob_Coin.GetComponent<Coin>().Result);
        isToss = false;
    }
    public void Result(string T) => UIManager.instace.ResultKey(T);
}
