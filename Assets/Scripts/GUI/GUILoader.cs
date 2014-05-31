using UnityEngine;
using System.Collections;
using Grouping;

public class GUILoader : MonoBehaviour
{
    public Texture pixel;
    public string progressText;

    public float borderSize = 0.015f;
    public float barWidth = 0.5f;

    public int interval = 10;
    int count;

    float progress;
    Vector2 textSize;

    void Start()
    {
        GroupManager.main.group["Loading"].Add(this);

        textSize = GUIManager.Style.loading.CalcSize(new GUIContent(progressText));
    }

    void Update()
    {
        progress = Mathf.Min(1.0f, Mathf.Lerp(progress, ((float)count / GUIManager.StyleList.Count), Time.deltaTime * 4));
    }

    void OnGUI()
    {
        for (int i = count; i < Mathf.Min(count + interval, GUIManager.StyleList.Count); ++i)
        {
            GUI.Label(new Rect(0, Screen.height, Screen.width, Screen.height),
                "Force Preload", 
                GUIManager.StyleList[i].style);
        }

        // Loading text
        GUI.Label(
            new Rect(
                0.5f * (Screen.width - textSize.x), 
                0.5f * (Screen.height - textSize.y) - textSize.y, 
                textSize.x, 
                textSize.y),
            progressText, 
            GUIManager.Style.loading);

        // Progress bar
        Rect bar = new Rect(
            0.5f * (Screen.width - barWidth * Screen.height),
            0.5f * (Screen.height - textSize.y) + textSize.y * 0.5f,
            barWidth * Screen.height,
            0.5f * textSize.y);

        GUI.color = Color.white;
        GUI.DrawTexture(new Rect(
            bar.x - 0.5f * borderSize * Screen.height,
            bar.y - 0.5f * borderSize * Screen.height,            
            bar.width + borderSize * Screen.height,
            bar.height + borderSize * Screen.height),
            pixel);

        GUI.color = Color.black;
        GUI.DrawTexture(bar, pixel);

        GUI.color = new Color(0.1f, 0.6f, 0.2f);
        GUI.DrawTexture(new Rect(bar.x, bar.y, bar.width * progress, bar.height), pixel);
      
        // Check if the loading over
        if (Event.current.type.Equals(EventType.Repaint))
        {
            if (count >= GUIManager.StyleList.Count)
            {
                GroupManager.main.activeGroup = GroupManager.main.group["Main Menu"];
                Destroy(this);
            }
            else
            {
                count += interval;
            }
        }
    }
}

