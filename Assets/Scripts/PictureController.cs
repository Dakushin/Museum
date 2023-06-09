using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PictureController : MonoBehaviour
{
    //Private Variables
    private RawImage image;
    private TMP_Text textMeshPro;

    //Public Variables
    public float Height {
        get { 
            return GetComponent<RectTransform>().rect.height* this.transform.localScale.y;
    } private set { Height = value; } }
    public float Width { get { return GetComponent<RectTransform>().rect.width * this.transform.localScale.x; } private set { Width = value; } }


    void Start()
    {
        //Initialisation
        image = GetComponentInChildren<RawImage>();
        textMeshPro = GetComponentInChildren<TMP_Text>();
        if (!image || !textMeshPro)
        {
            Debug.LogError("Can't get components");
        }
        StartCoroutine(GetInfoFromAPI("https://randomuser.me/api/", OnInfoReceive));
    }


    /// <summary>
    /// Coroutine that downloads a texture from a link.
    /// </summary>
    /// <param name="uri">URI of the texture.</param>
    /// <param name="Callback">Callback function.</param>
    /// <returns></returns>
    IEnumerator GetTexturesFromAPI(string uri, Action<Texture2D> Callback)
    {
        UnityWebRequest unityWebRequest = UnityWebRequestTexture.GetTexture(uri);
        yield return unityWebRequest.SendWebRequest();
        switch (unityWebRequest.result)
        {
            case UnityWebRequest.Result.Success:
                Callback(((DownloadHandlerTexture)unityWebRequest.downloadHandler).texture);
                break;
            default: Debug.LogError("Error on connection"); break;
        }
    }

    /// <summary>
    /// Coroutine that receives info from a REST API in JSON.
    /// </summary>
    /// <param name="uri">URI of the texture.</param>
    /// <param name="Callback">Callback function.</param>
    /// <returns></returns>
    IEnumerator GetInfoFromAPI(string uri, Action<ProfileInfo> Callback)
    {
        using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(uri))
        { 
            yield return unityWebRequest.SendWebRequest();
            switch (unityWebRequest.result)
            {
                case UnityWebRequest.Result.Success: Callback(JsonUtility.FromJson<ProfileInfo>(unityWebRequest.downloadHandler.text));
                break;
                default: Debug.LogError("Error on connection"); break;
            }
        }
    }

    /// <summary>
    /// Callback function when we receive JSON.
    /// </summary>
    /// <param name="profileInfo">Serialized Data.</param>
    void OnInfoReceive(ProfileInfo profileInfo)
    {
        if (profileInfo.results[0] == null)
        {
            Debug.LogWarning("Error in data parse");
            return;
        }
        ChangeTextFromReceive(profileInfo);
        StartCoroutine(GetTexturesFromAPI(profileInfo.results[0].picture.large, OnTextureDownloaded));
        
    }

    /// <summary>
    /// Callback function when the texture is downloaded.
    /// </summary>
    /// <param name="textureDownloaded">Downloaded texture.</param>
    void OnTextureDownloaded(Texture2D texturedownloaded)
    {
        image.texture = texturedownloaded;
    }

    /// <summary>
    /// Change the text with the JSON data.
    /// </summary>
    /// <param name="profileInfo">Serialized Data.</param>
    void ChangeTextFromReceive(ProfileInfo profileInfo)
    {
        textMeshPro.text = $"{profileInfo.results[0].name.first} {profileInfo.results[0].name.last}";
    }
}

