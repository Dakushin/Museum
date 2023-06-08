using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

[RequireComponent(typeof(BoxCollider))]
public class WallController : MonoBehaviour
{
    // Start is called before the first frame update
    public float ysize { get; private set; }
    public float xsize { get; private set; }
    public List<PictureControler> picturesThatAreOnTheWall { get; private set; } = new List<PictureControler>();
    private bool loopy = false;
    private bool loopx = false;

    void Start()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        ysize = collider.size.y;
        xsize = collider.size.x;
        
    }

    public bool CheckIfLoop()
    {
        return loopy;
    }
    public Vector3 ConverteToWorld(Vector2 pos)
    {
        return transform.TransformPoint(pos.x - xsize / 2, pos.y - ysize / 2, 0);
    }

    public Vector2 CheckIfBetweenBorder(Vector2 pos, PictureControler picture)
    {
        loopx = false;
        loopy = false;
        if (pos.y > ysize - picture.height / 2)
        {
           pos.y = ysize - picture.height / 2;
        }
        if (pos.y < 0 + picture.height / 2)
        {                     
            pos.y = 0 + picture.height / 2;
        }                     
        if (pos.x > xsize - picture.width / 2)
        {                     
            pos.x = xsize - picture.width / 2;
        }                     
        if (pos.x < 0 + picture.width / 2)
        {             
            pos.x = 0 + picture.width / 2;
        }
        return pos;
    }

    public Vector2 Relocate(Vector2 startPos, Vector2 pos, PictureControler picturetoplace)
    {
        if (pos.x < xsize - picturetoplace.width / 2)
        {
            pos.x += 0.1f;
        } else
        {
            loopx = true;
            pos.x = 0 + picturetoplace.width / 2;
        }
        if (startPos.x - 1f < pos.x && pos.x < startPos.x + 1f && loopx)
        {

             if (pos.y < ysize - picturetoplace.height / 2)
             {
                 pos.y += 0.1f;
                 loopx = false;
             }
             else
             {
                 pos.y = 0 + picturetoplace.height / 2;
                 loopy = true;
             }
        }

        return pos;
    }

    public bool CheckPlaceAvailability(Vector2 pos, PictureControler picturetoplace)
    {
        foreach (PictureControler picture in picturesThatAreOnTheWall)
        {
            Vector3 picturepos = transform.InverseTransformPoint(picture.transform.position);
            picturepos = new Vector3(picturepos.x + xsize / 2, picturepos.y + ysize / 2, picturepos.z);
            if (Mathf.Abs(pos.x - picturepos.x) < picture.width / 2 + picturetoplace.width / 2 && Mathf.Abs(pos.y - picturepos.y) < picture.height / 2 + picturetoplace.height / 2)
            {
                return false;
            }
        }
        return true;
    }
}
