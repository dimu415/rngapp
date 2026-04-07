using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public enum Suit { Spade, Heart, Diamond, Club }



    [System.Serializable]
    public class CardInfo
    {
        public Sprite sprite;   // 이미지
        public Suit suit;       // 모양
        public int rank;        // 숫자 (2~14; A=14)
    }
    public CardInfo[] CardSet;

    [System.Serializable]
    public class CurrentCard
    {
        public GameObject CardObj;
        public Sprite BackSp;
        public Suit suit;       // 모양
        public int rank;        // 숫자 (2~14; A=14)
    }
    public CurrentCard currentCard;
    public List<CardInfo> ShuffleCard=new List<CardInfo>();
    public int CurrentIndx = 0;
    public bool FirstDrow = true;
    public bool win;
    public GameObject UP, DOWN, DORW;

    public Text CountTxt;
    private void OnEnable()=> CardReset();
    public void CardReset()
    {
        CurrentIndx = 0;
        currentCard.CardObj.GetComponent<SpriteRenderer>().sprite = currentCard.BackSp;
        FirstDrow = true;
        UP.SetActive(false);
        DOWN.SetActive(false);
        DORW.SetActive(true);

        SuffleCard();
        CardCountSet();
    }
    public void SuffleCard()
    {
        ShuffleCard.Clear();

        int n = CardSet.Length;
        if (n == 0) return;

        // 결과 리스트에 재할당 최소화
        if (ShuffleCard.Capacity < n) ShuffleCard.Capacity = n;

        // 0..n-1 인덱스 배열 준비
        var idx = new int[n];
        for (int i = 0; i < n; i++) idx[i] = i;

        // Fisher–Yates 셔플 (O(n))
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1); // int 버전 상한 포함
            (idx[i], idx[j]) = (idx[j], idx[i]);
        }

        // 섞인 순서대로 카드 채우기
        for (int i = 0; i < n; i++)
            ShuffleCard.Add(CardSet[idx[i]]);
    }
    public void DrowCard(bool UnD=false)
    {
        if (CurrentIndx > 51)
        {
            CardReset();
            return;
        }
        VersusCard(ShuffleCard[CurrentIndx], UnD);
        CurrentIndx++;
        SoundManager.Instance.PlaySFX("Card");
        CardCountSet();
    }
    private void CardCountSet() => CountTxt.text = $"{CurrentIndx}/52";
    //카드 경쟁
    public void VersusCard(CardInfo NextCard,bool UnD) //UP=TR DOWN=FAL
    {
        //  : ♠ > ♥ > ♦ > ♣
        if (FirstDrow) FirstDrow = false;
        else
        {
            int cmp = currentCard.rank.CompareTo(NextCard.rank);// 현재 vs 다음 (다음이 크면 -1)
            if (cmp == 0)
            {
                cmp = currentCard.suit.CompareTo(NextCard.suit);

            }

            // UP 선택 시
            bool winUp = (cmp < 0);
            // DOWN 선택 시
            bool winDown = (cmp > 0);

            win= UnD ? winUp : winDown;
        }


        currentCard.CardObj.GetComponent<SpriteRenderer>().sprite = NextCard.sprite;
        currentCard.suit = NextCard.suit;
        currentCard.rank = NextCard.rank;
    }

}
