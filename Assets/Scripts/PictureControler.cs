using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PictureControler : MonoBehaviour
{

    private Image image;
    private TMP_Text textMeshPro;
    public float height {
        get { 
            return GetComponent<RectTransform>().rect.height* this.transform.localScale.y;
    } private set { height = value; } }
    public float width { get { return GetComponent<RectTransform>().rect.width * this.transform.localScale.x; } private set { width = value; } }
    // Start is called before the first frame update

    private void Awake()
    {

    }
    void Start()
    {
        
        image = GetComponentInChildren<Image>();
        textMeshPro = GetComponentInChildren<TMP_Text>();
        if (!image || !textMeshPro)
        {
            Debug.LogError("Can't get components");
        }
        StartCoroutine(GetInfoFromAPI("https://randomuser.me/api/", OnInfoReceive));
    }


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

    void OnTextureDownloaded(Texture2D texturedownloaded)
    {
        image.sprite = Sprite.Create(texturedownloaded, new Rect(0, 0, texturedownloaded.width, texturedownloaded.height), new Vector2(0.5f, 0.5f));
    }
    void ChangeTextFromReceive(ProfileInfo profileInfo)
    {
        textMeshPro.text = $"{profileInfo.results[0].name.first} {profileInfo.results[0].name.last}";
    }
}

