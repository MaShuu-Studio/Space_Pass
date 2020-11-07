using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarArrangeController : MonoBehaviour
{
    private enum Star { COMET = 0, ASTEROID = 1, PLANET = 2, FIXEDSTAR = 3, NOVA = 4, BLACKHOLE = 5 }
    private List<Star> stars = new List<Star>();
    private List<GameObject> created_Stars = new List<GameObject>();

    [SerializeField] private List<GameObject> Star_Bases;
    [SerializeField] private GameObject whiteHole;
    [SerializeField] private Transform StarParent;

    public void ClearStars()
    {
        for (int i = 0; i < created_Stars.Count; i++)
        {
            Destroy(created_Stars[i]);
        }
        stars.Clear();
        created_Stars.Clear();
    }
    public void InitializeField(GameController.Diffculty diff, int SpaceShipPos = -1)
    {
        ClearStars();

        float range = 4;
        if (diff == GameController.Diffculty.HARD)
        {
            range = 2.5f;
        }

        int count = (int)(150 / range);
        if ((int)(150 % range) == 0)
        {
            count--;
        }

        //기본배치
        for (int i = 0; i < count; i++)
        {
            int starEnum = Random.Range(0, (int)Star.PLANET + 1);
            stars.Add((Star)starEnum);
        }

        List<int> specialStarIndex = new List<int>();

        //항성배치
        int FixedStarCount = Random.Range(count / 18, count / 9 + 1);
        for (int i = 0; i < FixedStarCount; i++)
        {
            int pos = Random.Range(0, count);
            bool isOK = true;

            for (int j = 0; j < specialStarIndex.Count; j++)
            {
                if (pos == specialStarIndex[j])
                {
                    isOK = false;
                    i--;
                    break;
                }
            }

            if (isOK)
            {
                stars[pos] = Star.FIXEDSTAR;
                specialStarIndex.Add(pos);
            }
        }

        //노바배치        
        int NovaCount = count / 18;
        for (int i = 0; i < NovaCount; i++)
        {
            int pos = Random.Range(0, count);
            bool isOK = true;

            for (int j = 0; j < specialStarIndex.Count; j++)
            {
                if (pos == specialStarIndex[j])
                {
                    isOK = false;
                    i--;
                    break;
                }
            }

            if (isOK)
            {
                stars[pos] = Star.NOVA;
                specialStarIndex.Add(pos);
            }
        }
        //블랙홀 배치
        int BlackHoleCount = count / 18;
        for (int i = 0; i < BlackHoleCount; i++)
        {
            int pos = Random.Range(0, count);
            bool isOK = true;

            for (int j = 0; j < specialStarIndex.Count; j++)
            {
                if (pos == specialStarIndex[j])
                {
                    isOK = false;
                    i--;
                    break;
                }
            }

            if (isOK)
            {
                stars[pos] = Star.BLACKHOLE;
                specialStarIndex.Add(pos);
            }
        }

        float StarPosX = range;
        for (int i = 0; i < count; i++)
        {
            GameObject new_Star;
            if (SpaceShipPos == i)
            {
                new_Star = Instantiate(whiteHole);
            }
            else
            {
                new_Star = Instantiate(Star_Bases[(int)stars[i]]);

                if (stars[i] == Star.ASTEROID || stars[i] == Star.PLANET)
                {
                    new_Star.SendMessage("SetSprite");
                }
            }

            new_Star.transform.position = new Vector3(StarPosX + Random.Range(-0.2f, 0.2f), 0);
            new_Star.transform.SetParent(StarParent);
            StarPosX += range;

            created_Stars.Add(new_Star);

        }
    }
}
