using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Start is called before the first frame update
[System.Serializable]
public class coordinates
{
    public string latitude;
    public string longitude;
}

[System.Serializable]
public class dob
{
    public string date;
    public int age;
}

[System.Serializable]
public class id
{
    public string name;
    public string value;    
}

[System.Serializable]
public class info
{
    public string seed;
    public int results;
    public int page;
    public string version;
}

[System.Serializable]
public class location
{
    public street street;
    public string city;
    public string state;
    public string country;
    public string postcode;
    public coordinates coordinates;
    public timezone timezone;
}

[System.Serializable]
public class login
{
    public string uuid;
    public string username;
}

[System.Serializable]
public class Name
{
    //public string title; 
    public string first;
    public string last;
}

[System.Serializable]
public class Picture
{
    public string large; 
    //public string medium;
}

[System.Serializable]
public class registered
{
    public DateTime date;
    public int age;
}

[System.Serializable]
public class result
{
    //public string gender;
    public Name name;
    //public location location;
    //public string email;
    //public login login;
    //public dob dob;
    //public registered registered;
    //public string phone;
    //public string cell;
    //public id id;
    public Picture picture;
    //public string nat;
}

[System.Serializable]
public class ProfileInfo
{
    public result[] results;
    //public info info;
}
[System.Serializable]
public class street
{
    public int number;
    public string name;
}
[System.Serializable]
public class timezone
{
    public string offset;
    public string description;
}


