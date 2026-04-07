using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoomGame : MonoBehaviour
{
    [Header("Grid")]
    public GridLayoutGroup grid;
    public List<GameObject> cells;

    [Header("Size Toggles")]
    public Toggle toggle3;
    public Toggle toggle4;
    public Toggle toggle5;

    [Header("Bomb Count")]
    public Slider bombSlider;
    public Text slidert;
    public int BoomCount = 1;
    public int currentBoomCount = 0;
    public bool isboomset = false;
    public List<GameObject> bombImage;


    [Header("ui")]
    public GameObject bombListob;
    public GameObject settingboard;
    public GameObject playboard;
    public GameObject startBtn;
    public GameObject endBtn;

    int boardSize = 3;
    int maxCellCount = 0;
    public void SetStart()
    {
        bombListob.SetActive(true);
        settingboard.SetActive(false);
        playboard.SetActive(true);
        currentBoomCount = 0;
        isboomset = false;
        BoomCount = (int)bombSlider.value;
        ApplyBoardSize();
    }
    public void ResetGame()
    {
        startBtn.SetActive(false);
        endBtn.SetActive(false);
        settingboard.SetActive(true);
        playboard.SetActive(false);
        bombListob.SetActive(false);
    }
    private void OnEnable()
    {
        ResetGame();
    }
    public void GameStart()
    {
        startBtn.SetActive(false);
    }
    public void Sliderset() => slidert.text = bombSlider.value.ToString();
    public void ApplyBoardSize()
    {

        if (toggle3.isOn)
        {
            boardSize = 3;
            grid.cellSize = new Vector2(200, 200);
            grid.spacing = new Vector2(50, 50);
            grid.constraintCount = 3;
        }
        else if (toggle4.isOn)
        {
            boardSize = 4;
            grid.cellSize = new Vector2(180, 180);
            grid.spacing = new Vector2(50, 50);
            grid.constraintCount = 4;
        }
        else if (toggle5.isOn)
        {
            boardSize = 5;
            grid.cellSize = new Vector2(150, 150);
            grid.spacing = new Vector2(40, 40);
            grid.constraintCount = 5;
        }

        maxCellCount = boardSize * boardSize;
        SetCell();
    }
    void SetCell()
    {
        for(int i=0;i< bombImage.Count; i++)
        {
            bombImage[i].SetActive(i < BoomCount);
            bombImage[i].GetComponent<Image>().color = new Color32(255, 255, 255, 50);
        }
        for(int i=0; i<cells.Count; i++)
        {
            cells[i].SetActive(i < maxCellCount);
            cells[i].GetComponent<BoxCell>().isBoom=false;
        }
    }
    public void BoomSetting()
    {
        bombImage[currentBoomCount].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        currentBoomCount++;
        if (currentBoomCount >= BoomCount)
        {
            isboomset = true;
            startBtn.SetActive(true);
        }
    }
    public void boxClick(bool isboom,int bc)
    {
        if (isboom)
        {
            Debug.Log("boom");

            currentBoomCount-=bc;
            for(int i = BoomCount; i >= currentBoomCount; i--)
            {

                bombImage[i].GetComponent<Image>().color = new Color32(255, 255, 255, 50);
            }
            if (currentBoomCount <= 0)
            {
                endBtn.SetActive(true);
                Debug.Log("end");
            }
        }
        else
        {

            Debug.Log("safe");
        }
    }
    public void EndBtn()
    {
        ResetGame();
    }
}
