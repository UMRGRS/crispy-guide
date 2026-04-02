using System.Collections;
using NueGames.NueDeck.Scripts.Enums;
using NueGames.NueDeck.Scripts.Interfaces;
using NueGames.NueDeck.Scripts.Managers;
using UnityEngine;

namespace NueGames.NueDeck.Scripts.Characters
{
    public abstract class CharacterBase : MonoBehaviour, ICharacter
    {
        [Header("Base settings")]
        [SerializeField] private CharacterType characterType;
        [SerializeField] private Transform textSpawnRoot;
        [SerializeField] private Animator characterAnimator;

        #region Cache
        public CharacterStats CharacterStats { get; protected set; }
        public CharacterType CharacterType => characterType;
        public Transform TextSpawnRoot => textSpawnRoot;
        public Animator CharacterAnimator => characterAnimator;
        protected FxManager FxManager => FxManager.Instance;
        protected AudioManager AudioManager => AudioManager.Instance;
        protected GameManager GameManager => GameManager.Instance;
        protected CombatManager CombatManager => CombatManager.Instance;
        protected CollectionManager CollectionManager => CollectionManager.Instance;
        protected UIManager UIManager => UIManager.Instance;
        protected EnergyPoolManager EnergyPoolManager => EnergyPoolManager.Instance; 

        #endregion
        public virtual void Awake()
        {
        }
        
        public virtual void BuildCharacter()
        {
            
        }
        
        protected virtual void OnDeath()
        {
            TriggerAnimation(ActionAnimationType.Death);
        }
        public virtual void TriggerAnimation(ActionAnimationType animationType)
        {
            characterAnimator.SetTrigger(animationType.ToString());
        }
        
        public CharacterBase GetCharacterBase()
        {
            return this;
        }

        public CharacterType GetCharacterType()
        {
            return CharacterType;
        }
        protected virtual IEnumerator WaitForAnimationEnd(ActionAnimationType type)
        {
            TriggerAnimation(type);

            yield return null;

            AnimatorStateInfo stateInfo = CharacterAnimator.GetCurrentAnimatorStateInfo(0);

            while (stateInfo.normalizedTime < 1f || CharacterAnimator.IsInTransition(0))
            {
                yield return null;
                stateInfo = CharacterAnimator.GetCurrentAnimatorStateInfo(0);
            }
        }

        public virtual IEnumerator RunAnimation(ActionAnimationType type)
        {
            yield return WaitForAnimationEnd(type);
        }

        protected void RunDamageAnimation()
        {
            StartCoroutine(RunAnimation(ActionAnimationType.Hurt));
        }
    }
}