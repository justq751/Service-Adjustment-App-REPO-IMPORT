using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class LocationPanel : MonoBehaviour, IPanel
{
    public RawImage mapImg;
    public InputField mapNotes;
    public Text caseNumberTitle;

    public string apiKey;
    public float xCord, yCord, zoom;
    public int imgSize;
    public string url = "https://www.mapquestapi.com/staticmap/v5/map?key=";

    public void OnEnable()
    {
        caseNumberTitle.text = "CASE NUMBER " + UIManager.Instance.activeCase.caseID;
    }

    public IEnumerator Start()
    {
        Input.location.Start();

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait < 1)
        {
            Debug.Log("Timed Out");
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine location");
            yield break;
        }
        else if (Input.location.status == LocationServiceStatus.Running)
        {
            xCord = Input.location.lastData.longitude;
            yCord = Input.location.lastData.latitude;
        }
        Input.location.Stop();
        StartCoroutine(GetLocationRoutine());
    }

    IEnumerator GetLocationRoutine()
    {
        url = url + apiKey + "&" + "center=" + yCord + "," + xCord + "&" + "zoom=" + zoom;

        using (UnityWebRequest map = UnityWebRequestTexture.GetTexture(url))
        {
            yield return map.SendWebRequest();
        
            if (map.error != null)
            {
                Debug.LogError("Map error" + map.error);
            }

            mapImg.texture = ((DownloadHandlerTexture)map.downloadHandler).texture;

        }
    }

    public void ProcessInfo()
    {
        if (string.IsNullOrEmpty(mapNotes.text) == false)
        {
            UIManager.Instance.activeCase.locationNotes = mapNotes.text;
        }
    }
}