using System.Collections.Generic;
using Base.PoolSystem.PoolTypes.Abstracts;
using Game_Area;
using Managers;
using UnityEngine;

namespace Base.PoolSystem.PoolTypes
{
    public class WayPartPoolObject : PoolObject
    {
        [SerializeField] List<GameObject> sideTrees = new();
        [SerializeField] List<Obstacle> obstacles = new();
    
        public override void OnEnable()
        {
            ResetObstacles();
            SetSideTrees();
            SetObstacles();
        }

        public override void ReturnPool()
        {
            EventManager.Instance.ReturnWayPartPool.Invoke(this);
        }

        private void SetObstacles()
        {
            float rightCoordinate = -23;
            float leftCoordinate = -23; 
            float midCoordinate = -23;

            rightCoordinate += Random.Range(2,6);
            leftCoordinate += Random.Range(2,6);
            midCoordinate += Random.Range(2,6);

            int lastOffset = 0;
        
            foreach (var obstacle in obstacles)
            {
                if (rightCoordinate <= 23)
                {
                    obstacle.transform.localPosition = new Vector3(2, 0, rightCoordinate);

                    var newOffset = obstacle.ObstacleFillArea;
                
                    obstacle.TryAddGold(newOffset, lastOffset);
                    obstacle.gameObject.SetActive(true);
                
                    rightCoordinate += obstacle.ObstacleLenght;
                    rightCoordinate += newOffset;
                    lastOffset = newOffset;
                }
                else if (leftCoordinate <= 23)
                {
                    obstacle.transform.localPosition = new Vector3(-2, 0, leftCoordinate);
                    var newOffset = obstacle.ObstacleFillArea;
                
                    obstacle.TryAddGold(newOffset, lastOffset);
                    obstacle.gameObject.SetActive(true);
                
                    leftCoordinate += obstacle.ObstacleLenght;
                    leftCoordinate += newOffset;
                    lastOffset = newOffset;
                }
                else if (midCoordinate <= 23)
                {
                    obstacle.transform.localPosition = new Vector3(0, 0, midCoordinate);
                    var newOffset = obstacle.ObstacleFillArea;
                
                    obstacle.TryAddGold(newOffset, lastOffset);
                    obstacle.gameObject.SetActive(true);
                
                    midCoordinate += obstacle.ObstacleLenght;
                    midCoordinate += newOffset;
                    lastOffset = newOffset;
                }
            }
        }
    
        private void SetSideTrees()  // Places trees of random scale and rotation on both sides of the path
        {
            float leftCoordinate = -24; 
            float rightCoordinate = -24;

            const float minScale = 0.8f;
            const float maxScale = 2f;
        
            foreach (var sideTree in sideTrees)
            {
                if (leftCoordinate <= 24)
                {
                    sideTree.transform.localPosition = new Vector3(-4, 0, leftCoordinate);
                    sideTree.transform.localScale = new Vector3(Random.Range(minScale, maxScale),Random.Range(minScale, maxScale),Random.Range(minScale, maxScale));
                    sideTree.transform.rotation = Quaternion.Euler(0,Random.Range(0, 360), 0);
                    leftCoordinate += 1.5f;
                }
                else
                {
                    sideTree.transform.localPosition = new Vector3(4, 0, rightCoordinate);
                    sideTree.transform.localScale = new Vector3(Random.Range(minScale, maxScale),Random.Range(minScale, maxScale),Random.Range(minScale, maxScale));
                    sideTree.transform.rotation = Quaternion.Euler(0,Random.Range(0, 360), 0);
                    rightCoordinate += 1.5f;
                }
            
            }
        }

        private void ResetObstacles()
        {
            foreach (var obstacle in obstacles)
            {
                obstacle.gameObject.SetActive(false);
                obstacle.transform.position = Vector3.zero;
            }
        
            ShuffleObstacleList();
        }
    
        private void ShuffleObstacleList()
        {
            List<Obstacle> temp = new();
            List<Obstacle> shuffled = new();
            temp.AddRange(obstacles);

            for (int i = 0; i < obstacles.Count; i++)
            {
                int index = Random.Range(0, temp.Count - 1);
                shuffled.Add(temp[index]);
                temp.RemoveAt(index);
            }

            obstacles = shuffled;
        }
    
    }
}
