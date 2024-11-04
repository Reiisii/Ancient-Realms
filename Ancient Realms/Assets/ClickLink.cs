using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickLink : MonoBehaviour
{
    // Start is called before the first frame update
    public void OpenUrl(string link)
    {
        Application.OpenURL(link);
    }


}
