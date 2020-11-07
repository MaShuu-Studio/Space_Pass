using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SpaceShipController : MonoBehaviour
{
    [SerializeField] GameObject Cam;
    [SerializeField] GameController gameController;
    [SerializeField] private Collider2D damaged_collider;
    public int HP { get; private set; }
    public int MaxHP { get; private set; }
    public float Fuel { get; private set; }
    public float MaxFuel { get; private set; }
    public float FuelDSpeed { get; private set; }
    public float Speed { get; private set; }
    public float currentSpeed { get; private set; }

    private bool isBlackHole = false;
    private bool isStealth = false;
    private float damagedTime = 0.0f;
    private Rigidbody2D m_rigidBody;
    private SpriteRenderer m_renderer;
    private Animator m_animator;

    private IEnumerator FuelD;


    private float ShakeDuration = 0.4f;
    private float ShakeAmplitude = 2.5f;
    private float ShakeFrequency = 2.0f;

    private float ShakeElapsedTime = 0f;

    public bool isGame;

    public CinemachineVirtualCamera VirtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    void Awake()
    {
        FuelD = FuelDecrease();
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_renderer = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();

        currentSpeed = Speed = 1.0f;
    }

    void Start()
    {
        if (VirtualCamera != null)
        {
            virtualCameraNoise = VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (isGame)
        {
            if (HP == 0 || Fuel == 0)
            {
                if (HP == 0)
                {
                    gameController.PlayEffect("Destroy");
                    ShakeElapsedTime = 0;
                    gameObject.SetActive(false);
                }
                if (Fuel == 0)
                {
                    StartCoroutine(SpeedZero());
                }
                FuelDSpeed = 0;
                currentSpeed = 0;
                gameController.GameOver();
                isGame = false;
            }
        }
        if (damagedTime > 0)
        {
            damagedTime -= Time.deltaTime;
        }

        if (VirtualCamera != null || virtualCameraNoise != null)
        {
            if (ShakeElapsedTime > 0)
            {
                virtualCameraNoise.m_AmplitudeGain = ShakeAmplitude;
                virtualCameraNoise.m_FrequencyGain = ShakeFrequency;
                ShakeElapsedTime -= Time.deltaTime;
            }
            else
            {
                virtualCameraNoise.m_AmplitudeGain = 0f;
                Cam.transform.rotation = Quaternion.identity;
            }
        }
        // 우주선을 앞으로 이동시킴
        if (isBlackHole == false)
            transform.position += new Vector3(currentSpeed * Time.deltaTime * 2.0f, 0);

    }

    private IEnumerator FuelDecrease()
    {
        float time = 0.0f;
        while (time < 1.0f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        Fuel -= FuelDSpeed;
        if (Fuel <= 0)
        {
            Fuel = 0;
        }
        else
        {
            FuelD = FuelDecrease();
            StartCoroutine(FuelD);
        }
    }

    private IEnumerator Damaged()
    {
        Color c = m_renderer.color;

        if ((int)(damagedTime * 10) % 2 == 0) c.a = 0f;
        else c.a = 1;
        yield return null;

        if (damagedTime > 0) StartCoroutine(Damaged());
        else
        {
            currentSpeed = Speed;
            c.a = 1;
        }

        m_renderer.color = c;
    }

    public void InitializeSpaceShip(int hp, float fl, float spd)
    {
        damaged_collider.enabled = true;

        Color c = m_renderer.color;
        c.a = 1;
        m_renderer.color = c;

        transform.position = new Vector3(0, 0);
        MaxHP = HP = hp;
        MaxFuel = Fuel = fl;
        FuelDSpeed = 1.0f;
        currentSpeed = Speed = spd;
        StartCoroutine(FuelD);
    }

    public void ResetSpaceShip(){
        
        MaxHP = HP = 100;
        MaxFuel = Fuel = 100;
        FuelDSpeed = 0f;
        currentSpeed = Speed = 1;
    }

    public void SpeedAdjust(bool isUp)
    {
        if (isBlackHole) return;

        if (isUp)
        {
            if (Speed >= 2.9f) return;
            Speed += 0.1f;
            FuelDSpeed += 0.05f;
        }
        else
        {
            if (Speed <= 0.5f) return;
            Speed -= 0.1f;
            FuelDSpeed -= 0.05f;
        }

        if (damagedTime <= 0)
            currentSpeed = Speed;
    }

    public void Damage(int dmg)
    {
        int realdmg = (int)(dmg * Speed);
        gameController.PlayEffect("Damage");
        HP -= dmg;
        if (HP <= 0)
        {
            HP = 0;
        }
        damagedTime = 0.8f;
        ShakeElapsedTime = ShakeDuration;
        currentSpeed = 0.5f;
        StartCoroutine(Damaged());
    }

    public void MeetBlackHole()
    {
        isBlackHole = true;
        currentSpeed = 0;
        gameController.PlayEffect("BlackHole");
        m_animator.SetTrigger("BlackHole");
        StartCoroutine(OutWhiteHole());
    }

    private IEnumerator OutWhiteHole()
    {
        float time = 0.0f;
        while (time < 1.0f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        gameController.PlayEffect("WhiteHole");
        m_animator.SetTrigger("WhiteHole");
        float range = 4;
        if (gameController.currentDiff == GameController.Diffculty.HARD) range = 2.5f;
        int pos = Random.Range(1, (int)(transform.position.x / range));
        transform.position = new Vector3(pos * range, 0);

        gameController.NewArrange(pos);
    }

    private void WhiteHoleEnd()
    {
        isBlackHole = false;
        currentSpeed = Speed;
    }

    public void StealthMode()
    {
        if (isBlackHole) return;
        if (Fuel > 10f && isStealth == false)
        {
            Fuel -= 15f;
            StartCoroutine(Stealth());
        }
    }

    private IEnumerator Stealth()
    {
        gameController.PlayEffect("Stealth");
        isStealth = true;
        Color c = m_renderer.color;
        c.a = 0.5f;
        m_renderer.color = c;
        damaged_collider.enabled = false;
        float time = 0.0f;
        while (time < 3.0f)
        {
            time += Time.deltaTime;
            yield return null;
        }
        damaged_collider.enabled = true;
        c.a = 1.0f;
        m_renderer.color = c;
        isStealth = false;
    }

    public void Clear()
    {
        StartCoroutine(SpeedZero());
        FuelDSpeed = 0;
    }

    private IEnumerator SpeedZero()
    {
        damaged_collider.enabled = false;
        for (int i = 0; i < 15; i++)
        {
            currentSpeed /= 1.25f;
            yield return null;
        }
        currentSpeed = 0;
    }
}
