using UnityEngine; 
using System.Collections; 

public class ScreenShot : MonoBehaviour { 

   void Update () { 
        if (Input.GetKeyDown("k")) { 
            string filename = Application.dataPath + "/screenshots/screen" 
                            + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png"; 

			Application.CaptureScreenshot(filename);

            Debug.Log(string.Format("Took screenshot to: {0}", filename)); 
        } 
   } 
} 