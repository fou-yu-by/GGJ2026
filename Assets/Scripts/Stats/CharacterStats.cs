using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
   [Header("Major Stats")]
   public Stats damage;
   public Stats health;
   public Stats moveSpeed;
   public Stats attackDistance;
   
   protected Slider healthBar;
   public int currentHealth;

   private void Awake()
   {
      healthBar = GetComponentInChildren<Slider>();
   }

   protected virtual void Start()
   {
      currentHealth = health.GetValue();
      healthBar.maxValue = currentHealth;
      healthBar.value = currentHealth;
      
   }
   
   //buff增益
   public virtual void IncreaseStatsByBuff(int _modifier, float _duration, Stats _stats)
   {
      StartCoroutine(IncreaseStatsByBuffCoroutine(_modifier, _duration, _stats));
   }
   private IEnumerator IncreaseStatsByBuffCoroutine(int _modifier, float _duration, Stats _stats)
   {
      _stats.AddModifier(_modifier);
      this.TriggerEvent("ChangeModifierEvent", new PlayerArgs{ _stats = _stats});
      yield return new WaitForSeconds(_duration);
      _stats.RemoveModifier(_modifier);
      this.TriggerEvent("ChangeModifierEvent", new PlayerArgs{ _stats = _stats});
   }

   //对其他单位造成伤害
   public virtual void Dodamage(CharacterStats _targetStats)
   {
      int totalDamage = damage.GetValue();
      _targetStats.TakeDamage(totalDamage);
      
   }
   
   public virtual void TakeDamage(int _damage)
   {
      if (currentHealth - _damage > 0)
      {
         currentHealth -= _damage;
         healthBar.value = currentHealth;
      }
      else
      {
         //TODO:单位死亡
      }
   }
   
}
