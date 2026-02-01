
using UnityEngine;
using UnityEngine.UI;
public class PlayerStats : CharacterStats
{
    public Text healthText;

    protected override void Start()
    {
        base.Start();
        UpdateHealthBar();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        if (currentHealth - _damage > 0)
        {
            healthText.text = $"{base.currentHealth.ToString()}" + "/" + $"{base.healthBar.maxValue.ToString()}";
        }

        if (currentHealth - _damage < 0)
        {
            currentHealth -= 0;
        }
    }

    public void UpdateHealthBar()
    {
        currentHealth += Mathf.RoundToInt(health.GetValue() - healthBar.maxValue);
        healthBar.maxValue = health.GetValue();
        healthBar.value = currentHealth;
        healthText.text = $"{base.currentHealth.ToString()}" + "/" + $"{base.healthBar.maxValue.ToString()}";
    }

    public void DestroyPlayer()
    {
        Destroy(gameObject);
        TransitionManager.Instance.TransitionToScene(TransitionManager.Instance.currentScene,TransitionManager.Instance.startScene);
        
    }
}
