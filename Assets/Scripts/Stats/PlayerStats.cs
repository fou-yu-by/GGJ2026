using System;
using System.Collections;
using System.Collections;
using UnityEngine.UI;
public class PlayerStats : CharacterStats
{
    public Text healthText;

    protected override void Start()
    {
        base.Start();
        healthText.text = $"{base.currentHealth.ToString()}" + "/" +$"{base.currentHealth.ToString()}";
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        if (currentHealth - _damage > 0)
        {
            healthText.text = $"{base.currentHealth.ToString()}" + "/" + $"{base.healthBar.maxValue.ToString()}";
        }
    }
}
