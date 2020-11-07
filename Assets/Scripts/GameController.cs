using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public enum Diffculty { NORMAL, HARD }
    [Header("GameControl")]
    [SerializeField] private StarArrangeController FieldMaker;
    [SerializeField] private BackGround backGround;
    [SerializeField] private SoundManager soundManager;
    [SerializeField] private GameObject destination;
    [Space]

    [Header("UI")]
    [SerializeField] private GameObject Title;
    [SerializeField] private GameObject UI;
    [SerializeField] private GameObject Result;
    [SerializeField] private GameObject Manual;
    [SerializeField] private Text ResultText;

    [Header("Gages")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Text hpText;

    [Space]
    [SerializeField] private Slider fuelSlider;
    [SerializeField] private Text fuelText;
    [SerializeField] private Text fuelDSpeedText;

    [Space]
    [SerializeField] private Text speedText;

    [Space]
    [Header("PROGRESS")]
    [SerializeField] private Slider progressSlider;

    [Space]
    [Header("TIME")]
    [SerializeField] private Text timeText;


    private SpaceShipController spaceShip;
    public Diffculty currentDiff { get; private set; }


    private float playTime;
    private bool isClear = false;

    private void Awake()
    {
        spaceShip = GameObject.FindWithTag("Player").GetComponent<SpaceShipController>();
        playTime = 0.0f;
    }
    void Start()
    {
        Title.SetActive(true);
        Result.SetActive(false);
        UI.SetActive(false);
        Manual.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        //체력 연료 등 게이지
        hpSlider.value = (float)spaceShip.HP / spaceShip.MaxHP;
        fuelSlider.value = (float)spaceShip.Fuel / spaceShip.MaxFuel;

        hpText.text = spaceShip.HP + " / " + spaceShip.MaxHP;
        fuelText.text = spaceShip.Fuel.ToString("N2") + " / " + spaceShip.MaxFuel;
        fuelDSpeedText.text = "-" + spaceShip.FuelDSpeed.ToString("N2") + "/s";

        speedText.text = "SPEED x" + spaceShip.Speed.ToString("N1");

        //진행도
        progressSlider.value = spaceShip.transform.position.x / 150.0f;

        //시간
        timeText.text = playTime.ToString("N2") + "s";
    }

    void FixedUpdate()
    {
        if (isClear == false)
            playTime += Time.deltaTime;

        if (Input.GetButtonDown("SpeedUp"))
        {
            spaceShip.SpeedAdjust(true);
        }
        else if (Input.GetButtonDown("SpeedDown"))
        {
            spaceShip.SpeedAdjust(false);
        }
        else if (Input.GetButtonDown("Stealth"))
        {
            spaceShip.StealthMode();
        }
    }

    public void DifficultySet(bool isHard)
    {
        if (isHard) currentDiff = Diffculty.HARD;
        else currentDiff = Diffculty.NORMAL;
    }

    public void BackGroundOnOff(bool isOn)
    {
        if (isOn) backGround.gameObject.SetActive(true);
        else backGround.gameObject.SetActive(false);
    }
    public void StartGame()
    {
        isClear = false;
        Manual.SetActive(false);
        Result.SetActive(false);
        spaceShip.gameObject.SetActive(true);
        destination.SetActive(true);
        playTime = 0.0f;
        soundManager.PlayMusic("Playing BGM");
        Title.SetActive(false);
        UI.SetActive(true);
        spaceShip.InitializeSpaceShip(100, 100, 1);
        FieldMaker.InitializeField(currentDiff);
        backGround.MeetBlackHole();
        spaceShip.isGame = true;
    }
    public void PlayEffect(string str)
    {
        soundManager.PlayEffect(str);
    }

    public void NewArrange(int pos)
    {
        FieldMaker.InitializeField(currentDiff, pos - 1);
        backGround.MeetBlackHole();
    }
    public void ManualONOFF()
    {
        Manual.SetActive(!Manual.activeSelf);
    }
    public void GameOver()
    {
        spaceShip.isGame = false;
        Result.SetActive(true);
        ResultText.text = "[RESULT]\n" + "<color=#ff0000>FAIL</color>\n" + "TIME : " + playTime.ToString("N2") + "s";
        isClear = true;
    }
    public void Clear()
    {
        spaceShip.isGame = false;
        Result.SetActive(true);
        ResultText.text = "[RESULT]\n" + "<color=#00ff00>" +
        ((currentDiff == Diffculty.HARD) ? "HARD" : "NORMAL") + " CLEAR</color>\n" + "TIME : " + playTime.ToString("N2") + "s";

        spaceShip.Clear();
        backGround.isClear = true;
        isClear = true;
    }

    public void GoTitle()
    {
        spaceShip.ResetSpaceShip();
        soundManager.PlayMusic("Title BGM");
        destination.SetActive(false);
        spaceShip.gameObject.SetActive(true);
        FieldMaker.ClearStars();
        Title.SetActive(true);
        Result.SetActive(false);
        UI.SetActive(false);
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
