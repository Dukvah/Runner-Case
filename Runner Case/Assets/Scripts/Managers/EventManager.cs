using Base.PoolSystem.PoolTypes.Abstracts;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public class EventManager : Singleton<EventManager>
    {
        // MAIN EVENTS
        [HideInInspector] public UnityEvent OnGameStart = new(); 
        [HideInInspector] public UnityEvent OnGameRestart = new();
        [HideInInspector] public UnityEvent OnGameEnd = new();
    
        // PLAYER EVENTS
        [HideInInspector] public UnityEvent OnMoneyChange = new();
        [HideInInspector] public UnityEvent OnPlayerHealthChange = new();
        [HideInInspector] public UnityEvent OnScoreChange = new();
        [HideInInspector] public UnityEvent OnHighScoreChange = new();

        //MAP EVENTS
        [HideInInspector] public UnityEvent OnPlayerCheckPoint = new();
        
        // POOL EVENTS
        [HideInInspector] public UnityEvent<PoolObject> ReturnCoinPool = new();
        [HideInInspector] public UnityEvent<PoolObject> ReturnWayPartPool = new();
    }
}
