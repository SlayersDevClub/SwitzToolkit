using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class FillBar : MonoBehaviour
{
    Image fillBar;
    TextMeshProUGUI dmgText;
    public float currentFill
    {
        get { return _currentFill; }
        set
        {
            Flash(_currentFill - value * 1000);
            _currentFill = value;
            DOTween.To(() => fillBar.fillAmount, x => fillBar.fillAmount = x, _currentFill, .25f)
            .SetEase(Ease.OutQuad); // Optional: Smoother transition
            }
    }
    float _currentFill;

    private void Start()
    {
        fillBar = transform.Find("fillbar-parent/group/MainFill").GetComponent<Image>();
        dmgText = transform.Find("DamageText").GetComponent<TextMeshProUGUI>();
    }
    Tween fadeTween;
    public void Flash(float amount)
    {
        fadeTween?.Kill();
        dmgText.SetText(Mathf.Abs(amount).ToString("F0"));

        dmgText.alpha = 0; // Ensure it starts invisible

        // Fade In, Hold, Then Fade Out
        fadeTween = dmgText.DOFade(1f, .25f) // Fade in
            .OnComplete(() =>
                fadeTween = DOVirtual.DelayedCall(1.5f, () => // Hold
                    fadeTween = dmgText.DOFade(0f, .25f) // Fade out
                )
            );
    }

    private void OnGUI()
    {
        if(GUILayout.Button("hit for 25 hp"))
        {
            currentFill -= Random.Range(.18f, .06f);
        }
        if (GUILayout.Button("reset"))
        {
            currentFill = 1;
        }
    }
}
