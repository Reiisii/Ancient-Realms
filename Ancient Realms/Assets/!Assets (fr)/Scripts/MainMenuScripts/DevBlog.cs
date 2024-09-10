using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using TMPro;
using Unisave.Facets;
using UnityEngine;
using UnityEngine.Networking;

public class DevBlog : MonoBehaviour
{
    private string devlogURL = "https://sureiyaaa.itch.io/eagles-shadow/devlog";
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] GameObject loadingText;
    [SerializeField] TextMeshProUGUI dateText;
    string link;
    // Custom certificate handler to bypass SSL certificate validation (for local testing only)

    public void OpenURL(){
        Application.OpenURL(link);
    }
    public void OpenDevURL(){
        Application.OpenURL(devlogURL);
    }
    public void ProcessRSS(string rssContent)
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
        loadingText.SetActive(false);
        // Set the formatted and cleaned description
        descriptionText.SetText(formattedDescription);
    }
}
