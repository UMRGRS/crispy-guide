using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] protected EnemyAbilityData defaultAbility;
        protected EnemyAbilityData NextAbility;
        protected Action SetAbilitiesAsUnused;
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
            CharacterStats.SetCurrentHealth(CharacterStats.CurrentHealth);
            CombatManager.OnEnemyActionDeclaration += ShowNextAbility;
            CombatManager.OnEnemyStatusTrigger += CharacterStats.TriggerAllStatus;
        }
        protected override void OnDeath()
        {
            base.OnDeath();
            CombatManager.OnEnemyActionDeclaration -= ShowNextAbility;
            CombatManager.OnEnemyStatusTrigger -= CharacterStats.TriggerAllStatus;
           
            CombatManager.OnEnemyDeath(this);
            AudioManager.PlayOneShot(DeathSoundProfileData.GetRandomClip());
            Destroy(gameObject);
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
                if(EnergyPoolManager.CanPayCosts(ability.Card.CardActionDataList) && !ability.WasUsedLastTurn)
                    availableAbilities.Add(ability);    
            }

            var selectedAbility = availableAbilities.RandomItem() ?? defaultAbility;
            selectedAbility.SetAsUsed();
            SetAbilitiesAsUnused?.Invoke();
            SetAbilitiesAsUnused += selectedAbility.SetAsUnused;
            return selectedAbility;
        }
        #endregion
        
        #region Action Routines
        public virtual IEnumerator ActionRoutine()
        {
            if (CharacterStats.IsStunned)
                yield break;
            
            EnemyCanvas.SetIntentionVisibility(false);
            if (NextAbility.Intention.EnemyIntentionType == EnemyIntentionType.Attack || NextAbility.Intention.EnemyIntentionType == EnemyIntentionType.Debuff)
            {
                yield return StartCoroutine(AttackRoutine(NextAbility));
            }
            else
            {
                yield return StartCoroutine(BuffRoutine(NextAbility));
            }
        }
        
        protected virtual IEnumerator AttackRoutine(EnemyAbilityData targetAbility)
        {
            var waitFrame = new WaitForEndOfFrame();

            if (CombatManager == null) yield break;
            
            var target = CombatManager.CurrentAlliesList.RandomItem();
            
            var startPos = transform.position;
            var endPos = target.transform.position;

            var startRot = transform.localRotation;
            var endRot = Quaternion.Euler(60, 0, 60);
            
            yield return StartCoroutine(MoveToTargetRoutine(waitFrame, startPos, endPos, startRot, endRot, 5));
          
            CardExecutionContext context = new(this, CombatManager.CurrentMainAlly);
            foreach (CardActionData action in targetAbility.Card.CardActionDataList)
            {
                //Insert unable to used ability animation here
                if(!action.CanExecute(context)) continue;
                
                action.Execute(context);

                if (action.ActionDelay > 0)
                    yield return new WaitForSeconds(action.ActionDelay);
            }
            
            yield return StartCoroutine(MoveToTargetRoutine(waitFrame, endPos, startPos, endRot, startRot, 5));
        }
        
        protected virtual IEnumerator BuffRoutine(EnemyAbilityData targetAbility)
        {
            var waitFrame = new WaitForEndOfFrame();
            
            var target = CombatManager.CurrentEnemiesList.RandomItem();
            
            var startPos = transform.position;
            var endPos = startPos+new Vector3(0,0.2f,0);
            
            var startRot = transform.localRotation;
            var endRot = transform.localRotation;
            
            yield return StartCoroutine(MoveToTargetRoutine(waitFrame, startPos, endPos, startRot, endRot, 5));
            
            //Insert execute here
            //targetAbility.ActionList.ForEach(x=>EnemyActionProcessor.GetAction(x.ActionType).DoAction(new EnemyActionParameters(x.ActionValue,target,this)));
            
            yield return StartCoroutine(MoveToTargetRoutine(waitFrame, endPos, startPos, endRot, startRot, 5));
        }
        #endregion
        
        #region Other Routines
        private IEnumerator MoveToTargetRoutine(WaitForEndOfFrame waitFrame,Vector3 startPos, Vector3 endPos, Quaternion startRot, Quaternion endRot, float speed)
        {
            var timer = 0f;
            while (true)
            {
                timer += Time.deltaTime*speed;

                transform.position = Vector3.Lerp(startPos, endPos, timer);
                transform.localRotation = Quaternion.Lerp(startRot,endRot,timer);
                if (timer>=1f)
                {
                    break;
                }

                yield return waitFrame;
            }
        }

        #endregion
    }
}