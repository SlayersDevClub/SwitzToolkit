using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Coffee.UIEffects;
using Coffee.UIParticleInternal;
using Coffee.UIExtensions;
using DG.Tweening;

public class LaggingHealthBar : MonoBehaviour
{
    public Image healthBar;  // The main health bar (instant update)
    public Image delayedBar; // The lagging health bar (delayed update)
    public float delayBeforeLerp = 0.5f; // Delay before the lagging bar updates
    public float lerpDuration = 0.75f; // How long the delayed bar takes to catch up

    private float lastHealthValue; // Tracks the previous health value
    public UIParticle particles;

    void Update()
    {
        float currentHealth = healthBar.fillAmount;

        // Detect if the health bar has changed
        if (Mathf.Abs(currentHealth - lastHealthValue) > Mathf.Epsilon)
        {
            OnHealthChanged(currentHealth);
            lastHealthValue = currentHealth; // Update the stored value
        }
    }

    private void OnHealthChanged(float newHealth)
    {
        // If the delayed bar is higher, start the delayed lerp
        if (delayedBar.fillAmount > newHealth)
        {
            delayedBar.DOFillAmount(newHealth, lerpDuration)
                      .SetDelay(delayBeforeLerp)
                      .SetEase(Ease.OutQuad)
                      .OnStart(() => FadeInParticles(true))
                      .OnComplete(() => { FadeInParticles(false);});
        }
        else
        {
            // If health increased, snap the lagging bar immediately
            delayedBar.fillAmount = newHealth;
        }
    }

    void FadeInParticles(bool b)
    {
        if (b)
        {
            particles.DOFade(1, .25f);
        } else
        {
            particles.DOFade(0, .25f);
        }
    }
}
