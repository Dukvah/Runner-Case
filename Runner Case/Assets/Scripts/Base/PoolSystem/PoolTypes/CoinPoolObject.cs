using Base.PoolSystem.PoolTypes.Abstracts;
using DG.Tweening;
using Managers;
using UnityEngine;

namespace Base.PoolSystem.PoolTypes
{
    public class CoinPoolObject : PoolObject
    {
        [SerializeField] GameObject visual;
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                GameManager.Instance.PlayerMoney++;
                ReturnPool();
            }
        }

        public override void OnEnable()
        {
            DOTween.Restart("TurnTween");
        }

        private void OnDisable()
        {
            ReturnPool();
        }

        private void Start()
        { 
            visual.transform.DOLocalRotate(new Vector3(0, 360, 0), 5f, RotateMode.FastBeyond360).SetRelative(true)
                .SetEase(Ease.Linear).SetLoops(-1,LoopType.Incremental).SetId("TurnTween");
        }

        public override void ReturnPool()
        {
            if (!gameObject.activeSelf) return;

            DOTween.Pause("TurnTween");
            EventManager.Instance?.ReturnCoinPool.Invoke(this);
        }
    
    }
}
