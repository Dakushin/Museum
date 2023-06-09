using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

[RequireComponent(typeof(BoxCollider))]
public class WallController : MonoBehaviour
{
   //Public variables
    public float SizeY { get; private set; }
    public float SizeX { get; private set; }
    public List<PictureController> PicturesThatAreOnTheWall { get; private set; } = new List<PictureController>();
    public bool LoopY { get; private set; } = false;
    public bool LoopX { get; private set; }= false;

    void Start()
    {
        //Initialisation
        BoxCollider collider = GetComponent<BoxCollider>();
        SizeY = collider.size.y;
        SizeX = collider.size.x;
        
    }

    /// <summary>
    /// Return the variable LoopY cause if it's true, the function Relocate already checked all the position
    /// </summary>
    /// <returns>the variable LoopY</returns>
    public bool CheckIfLoop()
    {
        return LoopY;
    }

    /// <summary>
    /// Converts the local position of the picture relative to the wall into world position.
    /// </summary>
    /// <param name="pos">The Vector2 position of the picture.</param>
    /// <returns>The Vector3 position.</returns>
    public Vector3 ConvertToWorld(Vector2 pos)
    {
        return transform.TransformPoint(pos.x - SizeX / 2, pos.y - SizeY / 2, 0);
    }

    /// <summary>
    /// Checks if the picture is within the borders of the wall.
    /// </summary>
    /// <param name="pos">The current position.</param>
    /// <param name="picture">The picture we want to instantiate.</param>
    /// <returns>The new position if it has changed.</returns>
    public Vector2 CheckIfBetweenBorder(Vector2 pos, PictureController picture)
    {
        LoopX = false;
        LoopY = false;
        if (pos.y > SizeY - picture.Height / 2)
        {
           pos.y = SizeY - picture.Height / 2;
        }
        if (pos.y < 0 + picture.Height / 2)
        {                     
            pos.y = 0 + picture.Height / 2;
        }                     
        if (pos.x > SizeX - picture.Width / 2)
        {                     
            pos.x = SizeX - picture.Width / 2;
        }                     
        if (pos.x < 0 + picture.Width / 2)
        {             
            pos.x = 0 + picture.Width / 2;
        }
        return pos;
    }

    /// <summary>
    /// Relocates the position to find an available space.
    /// </summary>
    /// <param name="startPos">The initial position.</param>
    /// <param name="pos">The current position.</param>
    /// <param name="pictureToPlace">The picture we want to place.</param>
    /// <returns>The new position.</returns>
    public Vector2 Relocate(Vector2 startPos, Vector2 pos, PictureController picturetoplace)
    {
        if (pos.x < SizeX - picturetoplace.Width / 2)
        {
            pos.x += 0.1f;
        } else
        {
            LoopX = true;
            pos.x = 0 + picturetoplace.Width / 2;
        }
        if (startPos.x - 1f < pos.x && pos.x < startPos.x + 1f && LoopX)
        {

             if (pos.y < SizeY - picturetoplace.Height / 2)
             {
                 pos.y += 0.1f;
                 LoopX = false;
             }
             else
             {
                 pos.y = 0 + picturetoplace.Height / 2;
                 LoopY = true;
             }
        }

        return pos;
    }

    /// <summary>
    /// Checks if a place is available for the picture.
    /// </summary>
    /// <param name="pos">The position of the picture.</param>
    /// <param name="pictureToPlace">The picture we want to place on the wall.</param>
    /// <returns>True if a place is available, false otherwise.</returns>
    public bool CheckPlaceAvailability(Vector2 pos, PictureController picturetoplace)
    {
        foreach (PictureController picture in PicturesThatAreOnTheWall)
        {
            Vector3 picturepos = transform.InverseTransformPoint(picture.transform.position);
            picturepos = new Vector3(picturepos.x + SizeX / 2, picturepos.y + SizeY / 2, picturepos.z);
            if (Mathf.Abs(pos.x - picturepos.x) < picture.Width / 2 + picturetoplace.Width / 2 && Mathf.Abs(pos.y - picturepos.y) < picture.Height / 2 + picturetoplace.Height / 2)
            {
                return false;
            }
        }
        return true;
    }
}
