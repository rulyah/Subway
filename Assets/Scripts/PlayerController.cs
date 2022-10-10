using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;

    [SerializeField] private InputManager _input;
    
    public float _runSpeed;
    private float _strafeSpeed;
    private float currentPosX = 0.0f;
    private float _animatorRunSpeedValue = 1.0f;
    private bool isJumping;
    private bool isRoll;
    private bool isLive = true;

    

    public static int coinsCount { get; private set; }
    
    private static readonly int _lossTrigger = Animator.StringToHash("ObstacleTrigger");
    private static readonly int _jumpTrigger = Animator.StringToHash("JumpTrigger");
    private static readonly int _animatorRunSpeed = Animator.StringToHash("RunSpeed");
    private static readonly int _lyingTrigger = Animator.StringToHash("LyingObstacleTrigger");
    private static readonly int _animatorJumpSpeed = Animator.StringToHash("JumpSpeed");
    private static readonly int _turnTrigger = Animator.StringToHash("TurnTrigger");
    private static readonly int _rollSpeed = Animator.StringToHash("RollSpeed");
    private static readonly int _rollTrigger = Animator.StringToHash("RollTrigger");

    private void Start()
    {
        coinsCount = 0;
        _collider = GetComponent<CapsuleCollider>();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _input.onRightSwipe += OnRightSwipe;
        _input.onLeftSwipe += OnLeftSwipe;
        _input.onUpSwipe += OnUpSwipe;
        _input.onDownSwipe += OnDownSwipe;
        GameController.onGameStart += OnGameStart;
        GameController.onGameStop += StopGame;
        GameController.onGameRestart += UnSubscribe;
    }

    public void StopGame()
    {
        UnSubscribe();
    }

    public void UnSubscribe()
    {
        _input.onRightSwipe -= OnRightSwipe;
        _input.onLeftSwipe -= OnLeftSwipe;
        _input.onUpSwipe -= OnUpSwipe;
        _input.onDownSwipe -= OnDownSwipe;
        GameController.onGameStart -= OnGameStart;
        GameController.onGameStop -= StopGame;
        GameController.onGameRestart -= UnSubscribe;
        StopAllCoroutines();
    }

    
    private void OnGameStart()
    {
        StartRun(() => SetSpeed(2.0f));
    }
    
    private void OnDownSwipe()
    {
        PlayRoll();
    }

    private void OnUpSwipe()
    {
        PlayJump();
    }

    private void OnLeftSwipe()
    {
        Strafe(-1.0f);
    }

    private void OnRightSwipe()
    {
        Strafe(1.0f);
    }

    public void StartRun(Action action)
    {
        StartCoroutine(Turn(action));
        StartCoroutine(SpeedUp());
    }
    
    public void Strafe(float speed)
    {
        if (!CanStrafe(speed)) return;
        _strafeSpeed = speed;
        currentPosX += speed;
    }

    public void PlayJump()
    {
        if (!isJumping && isLive && !isRoll) StartCoroutine(Jumping(() => _rigidbody.AddForce(0.0f, 250.0f, 0.0f)));
    }

    public void PlayRoll()
    {
        if (!isJumping && isLive && !isRoll) StartCoroutine(Roll());
    }
    private bool CanStrafe(float speed)
    {
        if (isJumping || !isLive || isRoll)
        {
            return false;
        }
        return currentPosX + speed >= -1.0f && currentPosX + speed <= 1.0f;
    }
    
    public void SetSpeed(float speed)
    {
        _runSpeed = speed;
        _animator.SetFloat(_animatorRunSpeed, _animatorRunSpeedValue);
    }
    private void Update()
    {
        if (!isLive) return;
        var moveVector = new Vector3(_strafeSpeed, 0.0f, _runSpeed);
        if (moveVector != Vector3.zero)
        {
            transform.forward = moveVector.normalized;
        }
        
        transform.position += moveVector * Time.deltaTime;
        if (_strafeSpeed > 0.0f && transform.position.x >= currentPosX)
        {
            _strafeSpeed = 0.0f;
        }
        else if (_strafeSpeed < 0.0f && transform.position.x <= currentPosX)
        {
            _strafeSpeed = 0.0f;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Obstacle") && !collision.gameObject.CompareTag("LyingObstacle")) return;
        StartCoroutine(Death(collision.gameObject.CompareTag("Obstacle") ? _lossTrigger : _lyingTrigger));
        StopCoroutine(SpeedUp());
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (!trigger.gameObject.CompareTag("Coin")) return;
        coinsCount++;
        GameController.instance.PickUpCoin(trigger.gameObject);
    }

    private IEnumerator Death(int idTrigger)
    {       
        isLive = false;
        _animator.SetTrigger(idTrigger);
        yield return new WaitForSeconds(1.0f);
        GameController.instance.StopGame();
    }
    private IEnumerator Turn(Action action)
    {
        _animator.SetTrigger(_turnTrigger);
        yield return new WaitForSeconds(1.38f);
        transform.localRotation = Quaternion.identity;
        action?.Invoke();
    }

    private IEnumerator Jumping(Action action)
    {
        isJumping = true;
        _animator.SetTrigger(_jumpTrigger);
        action?.Invoke();
        yield return new WaitForSeconds(0.867f);
        isJumping = false;
    }

    private IEnumerator Roll()
    {
        isRoll = true;
        _collider.height = 0.75f;
        _collider.center = new Vector3(0.0f, 0.4f, 0.2f);
        _animator.SetFloat(_rollSpeed, 1.0f);
        _animator.SetTrigger(_rollTrigger);
        yield return new WaitForSeconds(2.0f);
        _collider.height = 1.7f;
        _collider.center = new Vector3(0.0f, 0.85f, 0.2f);
        isRoll = false;
    }
    private IEnumerator SpeedUp()
    {
        yield return new WaitForSeconds(2.0f);
        _runSpeed += 0.1f;
        _animatorRunSpeedValue += 0.05f;
        _animator.SetFloat(_animatorJumpSpeed, _animatorRunSpeedValue);
        
        StartCoroutine(SpeedUp());
    }
}
