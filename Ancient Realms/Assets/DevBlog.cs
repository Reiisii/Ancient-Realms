using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class DevBlog : MonoBehaviour
{
    private string rssURL = "https://23.88.54.33:3443/rss";
    private string devlogURL = "https://sureiyaaa.itch.io/eagles-shadow/devlog";
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] GameObject loadingText;
    [SerializeField] TextMeshProUGUI dateText;
    string link;
    void Awake()
    {
        StartCoroutine(GetRSSFeed());
    }

    IEnumerator GetRSSFeed()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(rssURL))
        {
            // Add certificate handler to accept self-signed certificates (for local testing)
            request.certificateHandler = new BypassCertificateHandler();
            loadingText.SetActive(true);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                loadingText.SetActive(false);
                string responseText = request.downloadHandler.text;
                ProcessRSS(responseText);
            }
        }
    }

    // Custom certificate handler to bypass SSL certificate validation (for local testing only)
    private class BypassCertificateHandler : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData) { return true; }
    }

    public void OpenURL(){
        Application.OpenURL(link);
    }
    public void OpenDevURL(){
        Application.OpenURL(devlogURL);
    }
    void ProcessRSS(string rssContent)
    {
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
