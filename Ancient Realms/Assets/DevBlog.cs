using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class DevBlog : MonoBehaviour
{
    private string itchRSSUrl = "http://23.88.54.33:26900/proxy";
    private string devlogURL = "https://sureiyaaa.itch.io/eagles-shadow/devlog";
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI dateText;
    string link;
    void Awake()
    {
        StartCoroutine(GetDevlog());
    }

    IEnumerator GetDevlog()
    {
        UnityWebRequest request = UnityWebRequest.Get(itchRSSUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            string rssContent = request.downloadHandler.text;
            ProcessRSS(rssContent);
        }
    }
    public void OpenURL(){
        Application.OpenURL(link);
    }
    public void OpenDevURL(){
        Application.OpenURL(devlogURL);
    }
    void ProcessRSS(string rssContent)
    {
        Debug.Log(rssContent);
        XmlDocument rssDoc = new XmlDocument();
        rssDoc.LoadXml(rssContent);

        XmlNodeList items = rssDoc.GetElementsByTagName("item");
        XmlNode item = items[0];
        titleText.SetText(item["title"].InnerText);
        dateText.SetText(item["pubDate"].InnerText);
        link = item["guid"].InnerText;
        string description = item["description"].InnerText;

        // Remove HTML tags using Regex
        string noHtmlDescription = Regex.Replace(description, "<.*?>", string.Empty);

        // Replace "- " with new lines for proper formatting
        string formattedDescription = noHtmlDescription.Replace("- ", "\n- ");

        // Set the formatted and cleaned description
        descriptionText.SetText(formattedDescription);

        
    }

}
