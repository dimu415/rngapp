using UnityEngine;

public class CardGame : MonoBehaviour
{
    public Card CardScript;
    public bool wins;
    public void Card_Drow(bool UnD = false)
    {
        bool FirstDrow = CardScript.FirstDrow;
        CardScript.DrowCard(UnD);
        int lastN = CardScript.CurrentIndx;
        Debug.Log(lastN);
        if (FirstDrow || lastN == 0)
        {
            Result("empty");
            return;
        }
        Result(CardScript.win ? "Card_win" : "Card_lose");
    }
    public void Result(string T) => UIManager.instace.ResultKey(T);
}
