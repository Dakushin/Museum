using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Start is called before the first frame update

[System.Serializable]
public class Name
{
    public string first;
    public string last;
}

[System.Serializable]
public class Picture
{
    public string large; 

}


[System.Serializable]
public class Result
{

    public Name name;
    public Picture picture;

}

[System.Serializable]
public class ProfileInfo
{
    public Result[] results;
}


