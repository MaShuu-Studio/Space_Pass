using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    [SerializeField] private GameObject bg_Obj;
    private List<GameObject> bgs = new List<GameObject>();
    private SpaceShipController SpaceShip;
    int posX = -7;
    public bool isClear = false;
    void Start()
    {
        SpaceShip = GameObject.FindWithTag("Player").GetComponent<SpaceShipController>();

        for (int i = 0; i < 3; i++)
        {
            GameObject newObj = Instantiate(bg_Obj);
            newObj.transform.SetParent(transform);
            newObj.transform.localPosition = new Vector3(posX, 0);
            bgs.Add(newObj);

            posX += 7;
        }
    }
    void Update()
    {
        if (SpaceShip.transform.position.x > bgs[1].transform.position.x)
        {
            Destroy(bgs[0]);
            bgs.RemoveAt(0);

            GameObject newObj = Instantiate(bg_Obj);
            newObj.transform.SetParent(transform);
            newObj.transform.localPosition = new Vector3(posX, 0);
            bgs.Add(newObj);

            posX += 7;
        }

        if (isClear == false)
            transform.position -= new Vector3(SpaceShip.Speed * 1.5f * Time.deltaTime, 0);
        else
            transform.position -= new Vector3(1.5f * Time.deltaTime, 0);
    }

    public void MeetBlackHole()
    {
        transform.position = SpaceShip.transform.position;
        posX = -7;
        for (int i = 0; i < 3; i++)
        {
            bgs[i].transform.localPosition = new Vector3(posX, 0);

            posX += 7;
        }
    }
}
