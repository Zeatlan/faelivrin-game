using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    Physical,
    Magical
}

namespace BattleSystem.Character
{
    public abstract class CharacterBase : MonoBehaviour
    {
        public CharacterStats stats;
        private CharacterAnimation _animation;

        public bool CanAttack
        {
            get => CanAttack == true;
            set
            {
                CanAttack = value;
                SetInactive();
            }
        }
        public bool CanMove
        {
            get => CanMove == true;
            set
            {
                CanAttack = value;
                SetInactive();
            }
        }

        public event Action<int> OnHealthChanged;
        public event Action OnDeath;

        void Start()
        {
            _animation = GetComponent<CharacterAnimation>();
        }

        #region Sprite handlers
        public void SetActive()
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }

        public void SetInactive()
        {
            if (!CanAttack)
            {
                GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.7f);
            }
        }
        #endregion

        #region damage methods
        public virtual void Attack(CharacterBase unit)
        {
            if (!CanAttack) return;

            int damage = CalculateDamage(unit, stats);
            unit.TakeDamage(damage, DamageType.Physical);
        }

        protected abstract int CalculateDamage(CharacterBase unit, CharacterStats stats);

        public void TakeDamage(int damage, DamageType type)
        {
            float armorMultiplicator = 0.06f;

            if (type == DamageType.Physical)
            {
                float damageReduction = 1f - (armorMultiplicator * stats.defense) / (1f + armorMultiplicator * stats.defense);
                int reducedDamage = Mathf.RoundToInt(damage * damageReduction);
                stats.currentHealth = Mathf.Clamp(stats.currentHealth - reducedDamage, 0, stats.maxHealth);
            }
            else if (type == DamageType.Magical)
            {
                float damageReduction = 1f - (armorMultiplicator * stats.resistance) / (1f + armorMultiplicator * stats.resistance);
                int reducedDamage = Mathf.RoundToInt(damage * damageReduction);
                stats.currentHealth = Mathf.Clamp(stats.currentHealth - reducedDamage, 0, stats.maxHealth);
            }

            _animation.TakeDamageAnim(this);

            if (stats.currentHealth <= 0) Die();

            OnHealthChanged?.Invoke(stats.currentHealth);
        }
        #endregion

        public void HealHealth(int amount)
        {
            stats.currentHealth = Mathf.Clamp(stats.currentHealth + amount, 0, stats.maxHealth);
            _animation.ReceiveHeal(this);

            OnHealthChanged?.Invoke(stats.currentHealth);
        }

        private void Die()
        {
            stats.currentHealth = 0;
            _animation.DieAnim(this);

            OnDeath?.Invoke();
        }
    }
}