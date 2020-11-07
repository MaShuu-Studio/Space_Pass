using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    enum KeySeries { UP, DOWN, LEFT, RIGHT }


    [SerializeField] private KeySeries[] BreakKeyType;
    [SerializeField] private GameObject[] InputImage;
    private int breakIndex = 0;
    protected bool isOn = false;

    private GameController gameController;
    protected virtual void Awake()
    {
        breakIndex = 0;
        isOn = false;
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    void Update()
    {
        if (isOn)
            GetType();

    }
    private void GetType()
    {
        if (Input.GetButtonDown("Up"))
        {
            if (BreakKeyType[breakIndex] == KeySeries.UP)
                breakIndex++;
            else if (BreakKeyType[0] == KeySeries.UP)
                breakIndex = 1;
            else breakIndex = 0;
        }
        else if (Input.GetButtonDown("Down"))
        {

            if (BreakKeyType[breakIndex] == KeySeries.DOWN)
                breakIndex++;
            else if (BreakKeyType[0] == KeySeries.DOWN)
                breakIndex = 1;
            else breakIndex = 0;
        }
        else if (Input.GetButtonDown("Left"))
        {
            if (BreakKeyType[breakIndex] == KeySeries.LEFT)
                breakIndex++;
            else if (BreakKeyType[0] == KeySeries.LEFT)
                breakIndex = 1;
            else breakIndex = 0;
        }
        else if (Input.GetButtonDown("Right"))
        {
            if (BreakKeyType[breakIndex] == KeySeries.RIGHT)
                breakIndex++;
            else if (BreakKeyType[0] == KeySeries.RIGHT)
                breakIndex = 1;
            else breakIndex = 0;
        }

        int index;

        for (int i = 0; i < InputImage.Length; i++)
        {
            if (i < breakIndex)
                InputImage[i].SetActive(false);
            else
                InputImage[i].SetActive(true);
        }

        if (breakIndex == BreakKeyType.Length)
        {
            gameController.PlayEffect("ClearStar");
            Destroy(gameObject);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isOn = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isOn = false;
        }
    }

    public void EndStar(){
        isOn = false;
    }
}
