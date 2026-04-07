using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class BoxCell : MonoBehaviour
{
    public bool isBoom = false;
    public BoomGame BoomGame;

    private Button _btn;
    private Image _img;

    int bcount=0;
    public Sprite defultsp, boomsp;
    private void Awake()
    {
        _btn = GetComponent<Button>();
        _img = GetComponent<Image>();

        _btn.onClick.RemoveListener(Onclickme);
        _btn.onClick.AddListener(Onclickme);
    }

    private void OnEnable()
    {
        if (_img == null) _img = GetComponent<Image>();
        _img.color = new Color32(255, 255, 255, 255);
        _img.sprite = defultsp;
        bcount = 0;
        _btn.interactable = true;
    }

    public void Onclickme()
    {
        if (!BoomGame.isboomset)
        {
            isBoom = true;
            BoomGame.BoomSetting();
            bcount++;
            return;
        }
        else
        {
            if (isBoom)
            {
                _img.sprite = boomsp;

                SoundManager.Instance.PlaySFX("boom");
            }
            else
            {
                _img.color = new Color32(255, 255, 255, 0);
                SoundManager.Instance.PlaySFX("emptybomb");
            }
            BoomGame.boxClick(isBoom, bcount);

            _btn.interactable = false; // 한 번 누르면 다시 못 누르게
        }
    }
}
