using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game_Area
{
    public class Obstacle : MonoBehaviour
    {
        [Header("Obstacle Features")]
        [Tooltip("Area covered by the obstacle on the line")] 
        [SerializeField] int obstacleLenght;
        [Tooltip("The length required for another obstacle to follow this obstacle(Max)")]
        [SerializeField] int obstacleMaxOffsetArea;
        [Tooltip("The length required for another obstacle to follow this obstacle(Min)")]
        [SerializeField] int obstacleMinOffsetArea;
    
        [SerializeField] ObstacleTypes obstacleType;
        [SerializeField] GameObject coinParent;

        public int ObstacleLenght => obstacleLenght;
        public int ObstacleFillArea => GetRandomAfterLenght();
        private int GetRandomAfterLenght()
        {
            return Random.Range(obstacleMinOffsetArea, obstacleMaxOffsetArea);
        }

        public void TryAddGold(int frontSpace, int backSpace)
        {
            if (obstacleType == ObstacleTypes.HighBarrier)
            {
                switch (Random.Range(0, 1))
                {
                    case 0:	
                        FillCoinLowFlat(frontSpace, backSpace);
                        break;									
                    case 1:
                        break;	
                }
            }
            else if(obstacleType == ObstacleTypes.HighFlat)
            {
                switch (Random.Range(0, 1))
                {
                    case 0:	
                        FillCoinHighFlat();
                        break;									
                    case 1:
                        break;	
                }
            }
            else if (obstacleType == ObstacleTypes.LowBarrier)
            {
                switch (Random.Range(0, 4))
                {
                    case 0:	
                        FillCoinLowFlat(frontSpace, backSpace);
                        break;									
                    case 1:
                        FillCoinRainbow(frontSpace, backSpace);
                        break;	
                    case 2:
                        break;	
                    case 3:
                        break;	
                }
            }
        
        }

        #region Fill Coin Functions

        private void FillCoinLowFlat(int frontSpace, int backSpace)
        {
            int availableSpace = Mathf.Abs(frontSpace + backSpace);
            
            if (availableSpace > 5)
            {
                int usedSpace = Random.Range(5, availableSpace);
                int startPoint = backSpace + Random.Range(0, availableSpace - 5);
            
                for (int i = startPoint; i < usedSpace; i++)
                {
                    var coin = GameManager.Instance.CoinPool.GetPooledObject();
                    coin.transform.parent = coinParent.transform;
                    coin.transform.localPosition = new Vector3(0, 0.3f, i);
                    coin.gameObject.SetActive(true);

                    coin = GameManager.Instance.CoinPool.GetPooledObject();
                    coin.transform.parent = coinParent.transform;
                    coin.transform.localPosition = new Vector3(0, 0.3f, i + 0.5f);
                    coin.gameObject.SetActive(true);
                }
            }
        }

        private void FillCoinHighFlat()
        {
            for (int i = 0; i < obstacleLenght; i++)
            {
                var coin = GameManager.Instance.CoinPool.GetPooledObject();
                coin.transform.parent = coinParent.transform;
                coin.transform.localPosition = new Vector3(0, 1.25f, i);
                coin.gameObject.SetActive(true);

                coin = GameManager.Instance.CoinPool.GetPooledObject();
                coin.transform.parent = coinParent.transform;
                coin.transform.localPosition = new Vector3(0, 1.25f, i + 0.5f);
                coin.gameObject.SetActive(true);
            }
        }
    
        private void FillCoinRainbow(int frontSpace, int backSpace)
        {
            int availableSpace = Mathf.Abs(frontSpace + backSpace);
            
            if (availableSpace > 5)
            {
                int useableSpace = Random.Range(5, availableSpace); 
                int startPoint = (backSpace + frontSpace) / 2;

                float height = 0.05f;
            
                for (int i = 0; i < useableSpace; i++)
                {
                    if (i < useableSpace / 2)
                    {
                        height += 0.25f;
                    }
                    else
                    {
                        height -= 0.25f;
                        if (height < 0.3f) height = 0.3f;
                    }
                
                    var coin = GameManager.Instance.CoinPool.GetPooledObject();
                    coin.transform.parent = coinParent.transform;
                    coin.transform.localPosition = new Vector3(0, height, i);
                    coin.gameObject.SetActive(true);

                    if (i < useableSpace / 2)
                    {
                        height += 0.25f;
                    }
                    else
                    {
                        height -= 0.25f;
                        if (height < 0.3f) height = 0.3f;
                    
                    }
                
                    coin = GameManager.Instance.CoinPool.GetPooledObject();
                    coin.transform.parent = coinParent.transform;
                    coin.transform.localPosition = new Vector3(0, height, i + 0.5f);
                    coin.gameObject.SetActive(true);
                
                    startPoint++;
                }
            }
        }

        #endregion
    }

    public enum ObstacleTypes { HighBarrier = 0, LowBarrier = 1, HighFlat = 2, Ramp = 3}
}