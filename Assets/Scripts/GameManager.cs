using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private List<WallController> walls = new List<WallController>();
    [SerializeField]
    private GameObject pictureprefab;
    private float xMax { get
        {
            float x = 0;
            foreach (WallController wall in walls)
            {
                x += wall.xsize;
            }
            return x;
        } }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            GeneratePicture();
        }
    }

    void GeneratePicture()
    {
        float xposition = Random.value * xMax;
        float calculatedoffset;
        WallController startingwall = OnWhichWall(xposition, out calculatedoffset);
        float yposition = Random.value * startingwall.ysize;
        Vector2 pos = new Vector2(xposition - calculatedoffset, yposition);
        PictureControler picture = pictureprefab.GetComponent<PictureControler>();
        
        WallController wall = startingwall;
        Vector2 newpos = pos = wall.CheckIfBetweenBorder(pos, picture);

        while (!wall.CheckPlaceAvailability(newpos, picture))
        {
            newpos = wall.Relocate(pos, newpos, picture);
            if(Vector2.Distance(newpos, pos) <= 0.2f && wall.CheckIfLoop()) 
            {
                 wall = FindAnotherWall(wall);
                if(wall == startingwall)
                {
                       Debug.LogError("no space available");
                       return;
                }
                pos = new Vector2(Random.value * wall.xsize, Random.value * wall.ysize);
                newpos = pos = wall.CheckIfBetweenBorder(pos, picture);
            } 
        }

        var obj = Instantiate(picture, wall.ConverteToWorld(newpos), wall.transform.localRotation);
        wall.picturesThatAreOnTheWall.Add(obj.gameObject.GetComponent<PictureControler>());
    }

    WallController OnWhichWall(float xposition, out float sizeidx)
    {
        
        sizeidx = 0;
        WallController wallThatBeenChoice = null;
        foreach (WallController wall in walls)
        {
            if (sizeidx <= xposition && xposition <= sizeidx + wall.xsize)
            {
                wallThatBeenChoice = wall;
                break;
            } else
            {
                sizeidx += wall.xsize;
            }
        }
        return wallThatBeenChoice;
 
    }
    
    WallController FindAnotherWall(WallController currentwall)
    {
        for (int i = walls.IndexOf(currentwall) + 1; i != walls.IndexOf(currentwall); i++)
        {
            if (i == walls.Count)
            {
                i = 0;
            }
            if (walls[i].isActiveAndEnabled)
            {
                return walls[i];
            }

        }
        return currentwall;
    }
}

