using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Private variables
    [SerializeField]
    private List<WallController> walls = new List<WallController>();
    [SerializeField]
    private GameObject picturePrefab;
    private float xMax
    {
        get
        {
            float x = 0;
            foreach (WallController wall in walls)
            {
                x += wall.SizeX;
            }
            return x;
        }
    }
    // Public Variable
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance already, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GeneratePicture();
        }
    }

    /// <summary>
    /// Function that generates a picture on a wall.
    /// </summary>
    void GeneratePicture()
    {
        // Local variables of the function
        // Generate a random value between the sizes of all the walls
        float xPosition = UnityEngine.Random.value * xMax;
        float calculatedOffset;
        WallController startingWall = OnWhichWall(xPosition, out calculatedOffset);
        // Once we know on which wall the picture will be, we take the ySize and generate a random position
        float yPosition = UnityEngine.Random.value * startingWall.SizeY;
        // Create a Vector2 position without the offset for the x position
        Vector2 pos = new Vector2(xPosition - calculatedOffset, yPosition);
        PictureController picture = picturePrefab.GetComponent<PictureController>();

        // Save the wall and the loop we had before the loop
        WallController wall = startingWall;
        // Check if the picture position is within the borders of the wall
        Vector2 newPos = pos = wall.CheckIfBetweenBorder(pos, picture);

        // Loop that checks if there is space on the wall
        while (!wall.CheckPlaceAvailability(newPos, picture))
        {
            // Until the loop finds another available space
            newPos = wall.Relocate(pos, newPos, picture);
            if (Vector2.Distance(newPos, pos) <= 0.2f && wall.CheckIfLoop())
            {
                // If the loop didn't find space on the wall, change to another wall
                wall = FindAnotherWall(wall);
                if (wall == startingWall)
                {
                    // If the FindAnotherWall function finds the same wall, it means there is no available space on any wall
                    Debug.LogError("No available space.");
                    return;
                }
                // If the function finds another wall, generate new position for the wall
                pos = new Vector2(UnityEngine.Random.value * wall.SizeX, UnityEngine.Random.value * wall.SizeY);
                newPos = pos = wall.CheckIfBetweenBorder(pos, picture);
            }
        }

        // If we find some space, instantiate a new picture on the wall
        var obj = Instantiate(picture, wall.ConvertToWorld(newPos), wall.transform.localRotation);
        wall.PicturesThatAreOnTheWall.Add(obj.gameObject.GetComponent<PictureController>());
    }

    /// <summary>
    /// Check on which wall, out of all the walls in the walls list, the position is pointing. In the eyes of the function, all the walls are adjacent.
    /// </summary>
    /// <param name="xPosition">The randomly generated position.</param>
    /// <param name="sizeIdx">Output the size value of all the walls that have passed through the loop.</param>
    /// <returns>The wall that has been chosen.</returns>
    WallController OnWhichWall(float xPosition, out float sizeIdx)
    {
        sizeIdx = 0;
        WallController chosenWall = null;
        foreach (WallController wall in walls)
        {
            if (sizeIdx <= xPosition && xPosition <= sizeIdx + wall.SizeX)
            {
                chosenWall = wall;
                break;
            }
            else
            {
                sizeIdx += wall.SizeX;
            }
        }
        return chosenWall;
    }

    /// <summary>
    /// Function that finds another wall that is not the current wall.
    /// </summary>
    /// <param name="currentWall">The current wall we found.</param>
    /// <returns>The next wall that is found.</returns>
    WallController FindAnotherWall(WallController currentWall)
    {
        for (int i = walls.IndexOf(currentWall) + 1; i != walls.IndexOf(currentWall); i++)
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
        return currentWall;
    }
}