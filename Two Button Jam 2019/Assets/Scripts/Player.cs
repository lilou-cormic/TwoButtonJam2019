using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Player : MonoBehaviour
{
    private static Player _instance = null;

    private SpriteRenderer SpriteRenderer;

    private Invinsibility Invinsibility;

    [SerializeField]
    private SpriteRenderer DamageSpriteRenderer = null;

    [SerializeField]
    private Sprite DamageSprite1 = null;

    [SerializeField]
    private Sprite DamageSprite2 = null;

    [SerializeField]
    private Sprite DamageSprite3 = null;

    [SerializeField]
    private GameObject Shield = null;

    [SerializeField]
    private GameObject ShieldPanel = null;

    [SerializeField]
    private TextMeshProUGUI ShieldCountDownText = null;

    [SerializeField]
    private AudioClip ShieldDownSound = null;

    [SerializeField]
    private GameObject PowerPanel = null;

    [SerializeField]
    private TextMeshProUGUI PowerCountDownText = null;

    [SerializeField]
    private AudioClip PowerDownSound = null;

    [SerializeField]
    private AudioClip GameOverSound = null;

    [SerializeField]
    private GameObject NormalShotPrefab = null;

    [SerializeField]
    private GameObject BigShotPrefab = null;

    public bool IsMoving { get; private set; } = true;
    public bool IsShooting { get; private set; } = false;

    private float _speed = 3f;
    private int _direction = 1;

    private float _button1Timer = 0;
    private float _stopThreshold = 0.1f;

    private float _button2Timer = 0;
    private float _chargingThreshold = 0.1f;
    private float _bigShotThreshold = 0.5f;

    private float _shootCoolDown = 1f;
    private float _coolDownTimer = 0f;

    private int _maxShots = 3;
    private int _shotsLeft = 3;

    private bool _isShieldUp = false;
    private float _shieldTime = 10f;
    private float _shieldTimeLeft = 0f;

    private bool _isPowerUp = false;
    private float _powerTime = 10f;
    private float _powerTimeLeft = 0f;

    private int _maxHitPoints = 4;
    private int HitPoints { get; set; } = 4;

    private bool IsGameOver = false;

    private void Awake()
    {
        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        Invinsibility = GetComponentInChildren<Invinsibility>();
    }

    private void Start()
    {
        IsGameOver = false;

        _instance = this;

        _direction = 1;

        _button1Timer = 0;
        _button2Timer = 0;

        _isShieldUp = false;
        _shieldTimeLeft = 0f;
        Shield.SetActive(false);
        ShieldPanel.SetActive(false);
        ShieldCountDownText.text = "0.0";

        _isPowerUp = false;
        _powerTimeLeft = 0f;
        PowerPanel.SetActive(false);
        PowerCountDownText.text = "0.0";

        HitPoints = _maxHitPoints;
        SetDamageSprite();

        _shotsLeft = _maxShots;
        _coolDownTimer = 0;
    }

    private void ChangeDirection()
    {
        _direction *= -1;
    }

    private void Update()
    {
        if (IsGameOver)
            return;

        ManageButton1();

        ManageButton2();

        ManageEdges();

        ManageShield();

        ManagePower();
    }

    #region ManageButton1
    private void ManageButton1()
    {
        if (Input.GetButtonDown("Button1"))
        {
            _button1Timer = 0;
            IsMoving = false;
        }
        else if (Input.GetButton("Button1"))
        {
            _button1Timer += Time.deltaTime;
        }
        else if (Input.GetButtonUp("Button1"))
        {
            IsMoving = true;

            if (_button1Timer < _stopThreshold)
                ChangeDirection();

            _button1Timer = 0;
        }
    }
    #endregion

    #region ManageButton2
    private void ManageButton2()
    {
        _coolDownTimer -= Time.deltaTime;

        if (_coolDownTimer > 0 && !IsShooting)
            return;

        if (!IsShooting)
            _shotsLeft = _maxShots;

        if (Input.GetButtonDown("Button2"))
        {
            _button2Timer = 0;
        }
        else if (Input.GetButton("Button2"))
        {
            _button2Timer += Time.deltaTime;

            if (_button2Timer > _bigShotThreshold)
                SpriteRenderer.color = Color.green;
            else if (_button2Timer > _chargingThreshold)
                SpriteRenderer.color = Color.yellow;
        }
        else if (Input.GetButtonUp("Button2"))
        {
            SpriteRenderer.color = Color.white;

            IsShooting = true;

            bool isFirstShot = _shotsLeft == _maxShots;

            if (_button2Timer > _bigShotThreshold)
                ShootBigShot();
            else
                ShootNormal();

            _button2Timer = 0;

            if (isFirstShot)
                _coolDownTimer = _shootCoolDown;
        }
    }
    #endregion

    #region ManageEdges
    private void ManageEdges()
    {
        if (transform.position.x <= -2.7)
        {
            _direction = 1;
            return;
        }

        if (transform.position.x >= 2.7)
        {
            _direction = -1;
            return;
        }
    }
    #endregion

    #region ManageShield
    private void ManageShield()
    {
        if (_shieldTimeLeft > 0)
        {
            ShieldCountDownText.text = _shieldTimeLeft.ToString("0.0");

            _shieldTimeLeft -= Time.deltaTime;

            if (_shieldTimeLeft <= 0)
                DropShield();
        }
    }
    #endregion

    #region ManagePower
    private void ManagePower()
    {
        if (_powerTimeLeft > 0)
        {
            PowerCountDownText.text = _powerTimeLeft.ToString("0.0");

            _powerTimeLeft -= Time.deltaTime;

            if (_powerTimeLeft <= 0)
                DropPower();
        }
    }
    #endregion

    private void ShootNormal()
    {
        if (_shotsLeft > 0)
        {
            Shoot(NormalShotPrefab);
            _shotsLeft--;
        }

        IsShooting = _shotsLeft > 0;
    }

    private void ShootBigShot()
    {
        Shoot(BigShotPrefab);

        IsShooting = false;
    }

    private void Shoot(GameObject shotPrefab)
    {
        Instantiate(shotPrefab, transform.position, Quaternion.identity);

        if (_isPowerUp)
        {
            Instantiate(shotPrefab, transform.position, Quaternion.Euler(0, 0, 15));
            Instantiate(shotPrefab, transform.position, Quaternion.Euler(0, 0, -15));
        }
    }

    private void FixedUpdate()
    {
        if (IsMoving)
            transform.position += Vector3.right * _direction * Time.deltaTime * _speed;
    }

    private void Hit(int damage)
    {
        if (IsGameOver)
            return;

        if (_isShieldUp)
        {
            DropShield();
            return;
        }

        HitPoints -= damage;

        if (HitPoints <= 0)
        {
            SoundPlayer.Play(GameOverSound);
            StartCoroutine(GameOver());
            return;
        }

        SetDamageSprite();

        Invinsibility.MakeInvinsible();
    }

    private IEnumerator GameOver()
    {
        ScoreManager.SetHighScore();
        IsGameOver = true;
        IsMoving = false;

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("GameOver");
    }

    private void Heal(int health)
    {
        if (HitPoints >= _maxHitPoints)
            return;

        HitPoints += health;

        SetDamageSprite();
    }

    private void LiftShield()
    {
        _isShieldUp = true;

        _shieldTimeLeft = _shieldTime;

        Shield.SetActive(true);
        ShieldCountDownText.text = _shieldTimeLeft.ToString("0.0");
        ShieldPanel.SetActive(true);
    }

    private void DropShield()
    {
        SoundPlayer.Play(ShieldDownSound);

        _isShieldUp = false;

        _shieldTimeLeft = 0;

        Shield.SetActive(false);
        ShieldCountDownText.text = "0.0";
        ShieldPanel.SetActive(false);
    }

    private void LiftPower()
    {
        _isPowerUp = true;

        _powerTimeLeft = _powerTime;

        PowerCountDownText.text = _powerTimeLeft.ToString("0.0");
        PowerPanel.SetActive(true);
    }

    private void DropPower()
    {
        SoundPlayer.Play(PowerDownSound);

        _isPowerUp = false;

        _powerTimeLeft = 0;

        PowerCountDownText.text = "0.0";
        PowerPanel.SetActive(false);
    }

    private void SetDamageSprite()
    {
        switch (HitPoints)
        {
            case 1:
                DamageSpriteRenderer.sprite = DamageSprite3;
                break;

            case 2:
                DamageSpriteRenderer.sprite = DamageSprite2;
                break;

            case 3:
                DamageSpriteRenderer.sprite = DamageSprite1;
                break;

            default:
                DamageSpriteRenderer.sprite = null;
                break;
        }
    }

    public static void HitPlayer(int damage)
    {
        _instance.Hit(damage);
    }

    public static void HealPlayer(int health)
    {
        _instance.Heal(health);
    }

    public static void ShieldPlayer()
    {
        _instance.LiftShield();
    }

    public static void PowerPlayer()
    {
        _instance.LiftPower();
    }
}
