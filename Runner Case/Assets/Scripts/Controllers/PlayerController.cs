using System.Collections;
using Managers;
using UnityEngine;

namespace Controllers
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        #region Variables

        [Header("Movement Values")]
        [SerializeField] float forwardSpeed;
        [SerializeField] float sideSpeed;
        [SerializeField] float jumpPower;
    
        [Header("Side Points")] // Position points on the x axis
        [SerializeField] float centerPos;
        [SerializeField] float leftPos;
        [SerializeField] float rightPos;

        [Header("Colliders")] 
        [SerializeField] CapsuleCollider fullCollider;
        [SerializeField] CapsuleCollider halfCollider;

        [Header("Visuals")] 
        [SerializeField] GameObject characterVisual;
    
        private Animator _animator;
        private Rigidbody _rigidbody;
        
        private int _currentPos = 0;   // Current pos 0 = center, 1 = left, 2 = right.
        private bool _isJumping;  //Checks if the player is currently jumping.
        private bool _isSliding;  //Checks if the player is currently sliding.

        #endregion

        #region Unity Functions

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            EventManager.Instance.OnGameStart.AddListener(OnGameStart);
        }
        private void OnDisable()
        {
            EventManager.Instance?.OnGameStart.RemoveListener(OnGameStart);
        }

        private void Update()
        {
            CharacterMovementUpdate();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Danger"))
            {
                GameManager.Instance.PlayerHealth--;
                if (GameManager.Instance.PlayerHealth < 1)
                {
                    EventManager.Instance.OnGameEnd.Invoke();
                }
                else
                {
                    StartCoroutine(MakeImmortal(3f));
                }
            }

            if (other.CompareTag("CheckPoint"))
            {
                EventManager.Instance.OnPlayerCheckPoint.Invoke();
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                _isJumping = false;
            }
        }

        #endregion

        #region Gameplay Functions

        private void OnGameStart()
        {
            _animator.SetTrigger("isStart");
            StartCoroutine(GameSpeed());
            StartCoroutine(ScoreMaker());
        }
        
        private IEnumerator GameSpeed()  // Game speed increases over time
        {
            yield return new WaitForSeconds(0.5f); // Delay
            
            while (GameManager.Instance.HasGameStart)
            {
                yield return new WaitForSeconds(10f);
                Time.timeScale += 0.1f;
            }
        }
        private IEnumerator ScoreMaker() // Game score increases over time
        {
            yield return new WaitForSeconds(0.5f); // Delay
            
            while (GameManager.Instance.HasGameStart)
            {
                yield return new WaitForSeconds(0.25f);
                GameManager.Instance.PlayerScore += 10;
            }
        }
        private IEnumerator MakeImmortal(float immortalityTime) // When a player loses a life, they are given some time to recover.
        {
            Physics.IgnoreLayerCollision(3,6);
            float elapsedTime = 0f;

            while (elapsedTime < immortalityTime)
            {
                characterVisual.SetActive(!characterVisual.activeSelf);
                yield return new WaitForSeconds(0.1f);
                elapsedTime += 0.1f;
            }
            
            characterVisual.SetActive(true);
            Physics.IgnoreLayerCollision(3,6, false);
        }

        #endregion
        
        #region Movement Functions

        private void CharacterMovementUpdate()
        {
            if (!GameManager.Instance.HasGameStart) return;
        
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + forwardSpeed * Time.deltaTime);
        
            if (_currentPos == 0)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(centerPos, transform.position.y, transform.position.z), sideSpeed * Time.deltaTime);
            }
            else if (_currentPos == 1)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(leftPos, transform.position.y, transform.position.z), sideSpeed * Time.deltaTime);
            }
            else if (_currentPos == 2)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(rightPos, transform.position.y, transform.position.z), sideSpeed * Time.deltaTime);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (_currentPos == 0)
                {
                    _currentPos = 1;
                    StartCoroutine(SideJump());
                }
                else if (_currentPos == 2)
                {
                    _currentPos = 0;
                    StartCoroutine(SideJump());
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (_currentPos == 0)
                {
                    _currentPos = 2;
                    StartCoroutine(SideJump());
                }
                else if (_currentPos == 1)
                {
                    _currentPos = 0;
                    StartCoroutine(SideJump());
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) && !_isJumping && !_isSliding)
            {
                _isJumping = true;
                _rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                StartCoroutine(FlipForwardJump());
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) && !_isSliding && !_isJumping)
            {
                _isSliding = true;
                StartCoroutine(Slide());
            }
        }

        #endregion
        
        #region Animation Functions

        private IEnumerator FlipForwardJump()
        {
            _animator.SetFloat("isFlipJump", 1);
            yield return new WaitForSeconds(0.75f);
            _animator.SetFloat("isFlipJump", 0);
        }
    
        private IEnumerator Slide()
        {
            fullCollider.enabled = false;
            halfCollider.enabled = true;
            _animator.SetFloat("isSlide", 1);
        
            yield return new WaitForSeconds(0.75f);
        
            _animator.SetFloat("isSlide", 0);
            fullCollider.enabled = true;
            halfCollider.enabled = false;
            _isSliding = false;
        
        }
    
        private IEnumerator SideJump()
        {
            _animator.SetFloat("isSideJump", 1);
            yield return new WaitForSeconds(0.5f);
            _animator.SetFloat("isSideJump", 0);
        }

        #endregion

    }
}
