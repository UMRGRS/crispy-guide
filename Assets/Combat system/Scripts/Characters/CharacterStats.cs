using System;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Characters.Status;
using NueGames.NueDeck.Scripts.Enums;

namespace NueGames.NueDeck.Scripts.Characters
{
    public class StatusStats
    {
        public StatusStats(){}
        private bool isActive = false;
        private int statusValue = 1;
        private int activeTurns = 0;
        private StatusBase statusBaseData = new();
        public bool IsActive { get => isActive; set => isActive = value; }
        public int StatusValue { get => statusValue; set => statusValue = value; }
        public int ActiveTurns { get => activeTurns; set => activeTurns = value; }
        public StatusBase StatusBaseData { get => statusBaseData; set => statusBaseData = value; }
    }

    public class CharacterStats
    { 
        public int MaxHealth { get; set; }
        public int CurrentHealth { get; set; }
        public bool IsStunned { get;  set; }
        public bool IsDeath { get; private set; }
       
        public Action OnDeath;
        public Action<int, int> OnHealthChanged;
        private readonly Action<StatusType,int> OnStatusChanged;
        private readonly Action<StatusType, int> OnStatusApplied;
        private readonly Action<StatusType> OnStatusCleared;
        public Action OnHealAction;
        public Action OnTakeDamageAction;
        public Action OnShieldGained;
        
        public readonly Dictionary<StatusType, StatusStats> StatusDict = new();
        
        #region Setup
        public CharacterStats(int maxHealth, CharacterCanvas characterCanvas)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
            SetAllStatus();
            
            OnHealthChanged += characterCanvas.UpdateHealthText;
            OnStatusChanged += characterCanvas.UpdateStatusText;
            OnStatusApplied += characterCanvas.ApplyStatus;
            OnStatusCleared += characterCanvas.ClearStatus;
        }
        private void SetAllStatus()
        {
            for (int i = 0; i < Enum.GetNames(typeof(StatusType)).Length; i++) 
                StatusDict.Add((StatusType) i, new StatusStats());
            
            StatusDict[StatusType.PlainPermanentDamageBoost].StatusBaseData.IsPermanent = true;
            StatusDict[StatusType.PlainTemporalDamageBoost].StatusBaseData.DecreaseOverTurn = true;
            StatusDict[StatusType.PlainNextCardDamageBoost].StatusBaseData.IsSingleUse = true;

            StatusDict[StatusType.MultiplierPermanentDamageBoost].StatusBaseData.IsPermanent = true;
            StatusDict[StatusType.MultiplierTemporalDamageBoost].StatusBaseData.DecreaseOverTurn = true;
            StatusDict[StatusType.MultiplierNextCardDamageBoost].StatusBaseData.IsSingleUse = true;
        }
        #endregion
        
        #region Public Methods
        public void ApplyStatus(StatusType targetStatus, int value, int turns = 1)
        {
            if (StatusDict[targetStatus].IsActive)
            {
                StatusDict[targetStatus].StatusValue += value;
                StatusDict[targetStatus].ActiveTurns += turns;
                //Change to also indicate turns if necessary
                OnStatusChanged?.Invoke(targetStatus, StatusDict[targetStatus].StatusValue);
            }
            else
            {
                StatusDict[targetStatus].StatusValue = value;
                StatusDict[targetStatus].ActiveTurns = turns;
                StatusDict[targetStatus].IsActive = true;
                //Change to also indicate turns
                OnStatusApplied?.Invoke(targetStatus, StatusDict[targetStatus].StatusValue);
            }
        }
        public void TriggerAllStatus()
        {
            for (int i = 0; i < Enum.GetNames(typeof(StatusType)).Length; i++)
                TriggerStatus((StatusType) i);
        }
        
        public void SetCurrentHealth(int targetCurrentHealth)
        {
            CurrentHealth = targetCurrentHealth <=0 ? 1 : targetCurrentHealth;
            OnHealthChanged?.Invoke(CurrentHealth,MaxHealth);
        } 
        
        public void Heal(int value)
        {
            CurrentHealth += value;
            if (CurrentHealth>MaxHealth)  CurrentHealth = MaxHealth;
            OnHealthChanged?.Invoke(CurrentHealth,MaxHealth);
        }
        
        public void Damage(int value)
        {
            if (IsDeath) return;
            OnTakeDamageAction?.Invoke();

            CurrentHealth -= value;
            
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                OnDeath?.Invoke();
                IsDeath = true;
            }
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }
        public void ClearAllStatus()
        {
            foreach (var status in StatusDict)
                ClearStatus(status.Key);
        }
        public void ClearStatus(StatusType targetStatus)
        {
            StatusDict[targetStatus].IsActive = false;
            StatusDict[targetStatus].StatusValue = 1;
            StatusDict[targetStatus].ActiveTurns = 0;
            OnStatusCleared?.Invoke(targetStatus);
        }
        #endregion

        #region Private Methods
        private void TriggerStatus(StatusType targetStatus)
        {
            //Check status
            if (StatusDict[targetStatus].StatusValue <= 0)
            {
                if (StatusDict[targetStatus].StatusBaseData.CanNegativeStack)
                {
                    if (StatusDict[targetStatus].StatusValue == 0 && !StatusDict[targetStatus].StatusBaseData.IsPermanent)
                        ClearStatus(targetStatus);
                }
                else
                {
                    if (!StatusDict[targetStatus].StatusBaseData.IsPermanent)
                        ClearStatus(targetStatus);
                }
            }
            
            if (StatusDict[targetStatus].StatusBaseData.DecreaseOverTurn) 
                StatusDict[targetStatus].ActiveTurns--;
            
            if (StatusDict[targetStatus].ActiveTurns <= 0)
                if (!StatusDict[targetStatus].StatusBaseData.IsPermanent)
                    ClearStatus(targetStatus);
            
            OnStatusChanged?.Invoke(targetStatus, StatusDict[targetStatus].StatusValue);
        }
        #endregion
    }
}