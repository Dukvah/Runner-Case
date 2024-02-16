using System.Collections.Generic;
using Base.PoolSystem.PoolTypes.Abstracts;
using Managers;
using UnityEngine;

namespace Controllers
{
    public class WayController : MonoBehaviour
    {
        #region Variables

        private Queue<PoolObject> _activeWays = new(); // Paths currently active on the hierarchy screen
    
        private int _totalSummonedWayCount;
        private readonly float _distanceBetweenWays = 50f;

        #endregion

        #region Unity Functions

        private void Awake()
        {
            WayInitialize(3);
        }

        private void OnEnable()
        {
            EventManager.Instance.OnPlayerCheckPoint.AddListener(OnPlayerCheckPoint);
        }

        private void OnDisable()
        {
            EventManager.Instance?.OnPlayerCheckPoint.RemoveListener(OnPlayerCheckPoint);
        }

        #endregion

        #region Other Functions

        private void WayInitialize(int wayCount)
        {
            float startOffset = 24f;

            for (int i = 0; i < wayCount; i++)
            {
                var way = GameManager.Instance.WayPartPool.GetPooledObject();
                _activeWays.Enqueue(way);
                way.transform.position = Vector3.forward * (startOffset +  _distanceBetweenWays * _totalSummonedWayCount);
                way.gameObject.SetActive(true);
                _totalSummonedWayCount++;
            }
        }

        private void OnPlayerCheckPoint()
        {
            WayInitialize(1);
        
            var removedWay = _activeWays.Dequeue();
            removedWay.ReturnPool();
        }

        #endregion
    }
}
