using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceGame : MonoBehaviour
{
    public bool isrolling = false;
    public int I_DiceCount = 1;
    public int TotalScore;
    public List<GameObject> Ob_Dice;

    private void OnEnable()
    {
        isrolling = false;
    }
    public void Dice_Chang(int v)
    {
        if (!isrolling)
        {
            int AfterCount = I_DiceCount + v;
            if (1 <= AfterCount && AfterCount <= 3)
            {
                I_DiceCount = AfterCount;
                for (int i = 0; i < Ob_Dice.Count; i++)
                {
                    if (i < AfterCount)
                        Ob_Dice[i].SetActive(true);
                    else
                        Ob_Dice[i].SetActive(false);
                }

            }

        }

    }
    public void Dice_Throw()
    {
        if (!isrolling)
        {

            isrolling = true;
            StartCoroutine(Dice_Throw_Co());
        }
    }
    public IEnumerator Dice_Throw_Co()
    {
        Result( "");
        TotalScore = 0;
        List<Dice> activeDices = new List<Dice>();
        foreach (var diceObj in Ob_Dice)
        {
            if (diceObj.activeSelf)
            {
                Dice dice = diceObj.GetComponent<Dice>();
                dice.ThrowDice();
                activeDices.Add(dice);
            }
        }
        // 모든 주사위가 멈출 때까지 대기
        while (activeDices.Exists(d => d.isRolling))
            yield return null;

        foreach (var dice in activeDices)
            TotalScore += dice.diceNumber;


        Result(TotalScore.ToString());
        isrolling = false;
    }
    public void Result(string T) => UIManager.instace.ResultTset(T);
}
