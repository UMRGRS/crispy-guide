using System;
using System.Collections;
using System.Collections.Generic;
using NueGames.NueDeck.Scripts.Data.Characters;
using NueGames.NueDeck.Scripts.Data.Collection;
using NueGames.NueDeck.Scripts.Data.Containers;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Interfaces;
using NueGames.NueDeck.Scripts.Managers;
using NueGames.NueDeck.Scripts.NueExtentions;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Characters
{
    public class EnemyBase : CharacterBase, IEnemy
    {
        [Header("Enemy Base References")]
        [SerializeField] protected EnemyCharacterData enemyCharacterData;
        [SerializeField] protected EnemyCanvas enemyCanvas;
        [SerializeField] protected SoundProfileData deathSoundProfileData;
        protected EnemyAbilityData NextAbility;
        protected string lastAbilityID = "";
        public EnemyCharacterData EnemyCharacterData => enemyCharacterData;
        public EnemyCanvas EnemyCanvas => enemyCanvas;
        public SoundProfileData DeathSoundProfileData => deathSoundProfileData;

        #region Setup
        public override void BuildCharacter()
        {
            
            base.BuildCharacter();
            EnemyCanvas.InitCanvas();
            CharacterStats = new CharacterStats(EnemyCharacterData.MaxHealth,EnemyCanvas);
            CharacterStats.OnDeath += OnDeath;
            CharacterStats.OnTakeDamageAction += RunDamageAnimation;
            CharacterStats.SetCurrentHealth(CharacterStats.CurrentHealth);
            CombatManager.OnEnemyActionDeclaration += ShowNextAbility;
            CombatManager.OnEnemyStatusTrigger += CharacterStats.TriggerAllStatus;
        }
        protected override void OnDeath()
        {
            base.OnDeath();
            CombatManager.OnEnemyActionDeclaration -= ShowNextAbility;
            CombatManager.OnEnemyStatusTrigger -= CharacterStats.TriggerAllStatus;
            EnemyCanvas.gameObject.SetActive(false);
           
            CombatManager.OnEnemyDeath(this);
            AudioManager.PlayOneShot(DeathSoundProfileData.GetRandomClip());
        }
        #endregion
        
        #region Private Methods
        private void ShowNextAbility()
        {
            NextAbility = SelectUsableCard();
            EnemyCanvas.SetEnemyIntention(NextAbility);
        }

        private EnemyAbilityData SelectUsableCard()
        {
            List<EnemyAbilityData> availableAbilities = new();

            foreach(EnemyAbilityData ability in EnemyCharacterData.EnemyDeck.CardList)
            {
                if(EnergyPoolManager.CanPayCosts(ability.Card.CardActionDataList) && lastAbilityID != ability.Card.Id)
                    availableAbilities.Add(ability);    
            }

            var selectedAbility = EnemyCharacterData.EnemyDeck.DefaultAbility;
            if(availableAbilities.Count > 0)
                selectedAbility = availableAbilities.RandomItem();

            lastAbilityID = selectedAbility.Card.Id;
            return selectedAbility;
        }
        #endregion
        
        #region Action Routines
        public virtual IEnumerator ActionRoutine()
        {
            if (CharacterStats.IsStunned)
                yield break;
            
            EnemyCanvas.SetIntentionVisibility(false);
            yield return StartCoroutine(RunAbilityRoutine(NextAbility));
        }

        protected virtual IEnumerator RunAbilityRoutine(EnemyAbilityData targetAbility)
        {
            if (CombatManager == null) yield break;
            
            CardExecutionContext context = new(this, CombatManager.CurrentMainAlly);
            foreach (CardActionData action in targetAbility.Card.CardActionDataList)
            {
                if (!action.CanExecute(context))
                {
                    yield return WaitForAnimationEnd(ActionAnimationType.Interruption);
                    yield break;
                }                
            }

            yield return WaitForAnimationEnd(targetAbility.Card.AnimationType);

            foreach (CardActionData action in targetAbility.Card.CardActionDataList)
            {
                action.Execute(context);

                if (action.ActionDelay > 0)
                    yield return new WaitForSeconds(action.ActionDelay);              
            }
        }
        #endregion
    }
}