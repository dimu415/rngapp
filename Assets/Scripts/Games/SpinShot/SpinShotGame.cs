using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpinShotGame : MonoBehaviour
{

    public GameObject ShotBtn, ResetBtn;
    public FireGate ob_SpinShot;
    public bool Shot;
    [System.Serializable]
    public class GameModeUi
    {
        public GameObject SettingUI;
        public Slider Slider;
        public Text Text;
    }
    public List<GameModeUi> GMU;

    public Text GameT;
    int m = 0;
    private void OnEnable()
    {
        GameStart();
    }
    public void GameStart()
    {
        ShotBtn.SetActive(false);
        ResetBtn.SetActive(true);
        Result("empty");
    }

    public void Fire_Arrow()
    {
        if (ob_SpinShot.Trylnn) return;

        bool FireIn = ob_SpinShot.Fire();
        if (FireIn)
        {
            Result("Spin_fire");
            Invoke("GameStart", 1f);
        }
        else
        {

            Result("Spin_misfire");
        }
        SetT();
    }
    public void Spin_Reset()
    {
        ShotBtn.SetActive(true);
        ResetBtn.SetActive(false);
        Result("empty");
        ob_SpinShot.gameReset();
        SetT();
    }
    public void ChangeMode(int idx)
    {
        foreach (var item in GMU)
        {
            item.SettingUI.SetActive(false);
        }
        ob_SpinShot.ChangeMode(idx);
        m = idx;
        GMU[idx].SettingUI.SetActive(true);

    }
    public void changSetting()
    {
        switch (m)
        {
            case 0:
                GMU[m].Text.text = (GMU[m].Slider.value * 100).ToString("F2") + "%";

                ob_SpinShot.chang_P(GMU[m].Slider.value);
                break;
            case 1:
                GMU[m].Text.text = GMU[m].Slider.value.ToString();

                ob_SpinShot.chang_N((int)GMU[m].Slider.value);
                break;
            case 2:
                GMU[m].Text.text = (GMU[m].Slider.value * 100).ToString("F2") + "%";

                ob_SpinShot.chang_AP(GMU[m].Slider.value);
                break;
        }
    }
    public void SetT()
    {
        GameT.text = ob_SpinShot.SetT();
    }
    public void Result(string T) =>UIManager.instace.ResultKey( T);
}
