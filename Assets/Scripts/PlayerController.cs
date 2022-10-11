using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody _rigidbody;

    [SerializeField] private InputManager _input;
    
    public float _runSpeed;
    private float _strafeSpeed;
    private float currentPosX;
    private float _animatorRunSpeedValue = 1.2f;
    private bool isJumping;
    private bool isRoll;
    private bool isLive = true;

    

    public static int coinsCount { get; private set; }
    
    private static readonly int _lossTrigger = Animator.StringToHash("ObstacleTrigger");
    private static readonly int _jumpTrigger = Animator.StringToHash("JumpTrigger");
    private static readonly int _animatorRunSpeed = Animator.StringToHash("RunSpeed");
    private static readonly int _lyingTrigger = Animator.StringToHash("LyingObstacleTrigger");
    private static readonly int _turnTrigger = Animator.StringToHash("TurnTrigger");
    private static readonly int _rollSpeed = Animator.StringToHash("RollSpeed");
    private static readonly int _turnSpeed = Animator.StringToHash("TurnSpeed");
    private static readonly int _rollTrigger = Animator.StringToHash("RollTrigger");

    private void Start()
    {
        coinsCount = 0;
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
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
        _input.onRightSwipe += OnRightSwipe;
        _input.onLeftSwipe += OnLeftSwipe;
        _input.onUpSwipe += OnUpSwipe;
        _input.onDownSwipe += OnDownSwipe;
        StartRun(() => SetSpeed(3.0f));
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
        Strafe(-2.0f);
    }

    private void OnRightSwipe()
    {
        Strafe(2.0f);
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
        currentPosX += speed / 2.0f;
    }

    public void PlayJump()
    {
        if (!isJumping && isLive && !isRoll) StartCoroutine(Jumping(() => _rigidbody.AddForce(0.0f, 175.0f, 0.0f)));
    }

    public void PlayRoll()
    {
        if (!isJumping && isLive && !isRoll) StartCoroutine(Roll());
    }
    private bool CanStrafe(float speed)
    {
        if (isJumping || !isLive || isRoll)
        {
            //return false;
        }
        return currentPosX + speed / 2.0f >= -1.0f && currentPosX + speed / 2.0f <= 1.0f;

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
        _animator.SetFloat(_turnSpeed, 1.36f);
        _animator.SetTrigger(_turnTrigger);
        yield return new WaitForSeconds(1.0f);//1.38f
        transform.localRotation = Quaternion.identity;
        action?.Invoke();
    }

    private IEnumerator Jumping(Action action)
    {
        isJumping = true;
        _animator.SetTrigger(_jumpTrigger);
        action?.Invoke();
        yield return new WaitForSeconds(1.0f);//0.867f
        isJumping = false;
    }

    private IEnumerator Roll()
    {
        isRoll = true;
        _animator.SetFloat(_rollSpeed, 1.75f);
        _animator.SetTrigger(_rollTrigger);
        yield return new WaitForSeconds(1.0f);//2
        isRoll = false;
    }
    private IEnumerator SpeedUp()
    {
        yield return new WaitForSeconds(1.0f);
        _runSpeed += 0.1f;
        _animatorRunSpeedValue += 0.01f;
        _animator.SetFloat(_animatorRunSpeed, _animatorRunSpeedValue);
        
        StartCoroutine(SpeedUp());
    }
}
