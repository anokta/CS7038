using UnityEngine;
using System.Collections;
using Grouping;

public class InGameGUI : MonoBehaviour
{

	#region Hand stuff
	public Texture hand;
	public Texture circle;
	public Texture background;
	public GameObject cleanParticles;
	public GameObject infectionParticles;

	public Texture handEmpty;
	public Texture warning1;
	public Texture warning2;
	public Texture warningSign;

	#endregion

    void Start()
    {
        GroupManager.main.group["Running"].Add(this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AudioManager.PlaySFX("Level Swipe");

            GroupManager.main.activeGroup = GroupManager.main.group["Paused"];
        }
    }

    void OnGUI()
    {
//        GUI.skin = GUIManager.GetSkin();

		if (GUI.Button(new Rect(GUIManager.OffsetX(), GUIManager.OffsetY(), GUIManager.ButtonSize(), GUIManager.ButtonSize()), "Pause", GUIManager.Style.pause))
        {
            AudioManager.PlaySFX("Level Swipe");

            GroupManager.main.activeGroup = GroupManager.main.group["Paused"];
        }

		if (Event.current.type.Equals(EventType.Repaint) && HandController.activeHand != null)
		{
			Material GUIpie = GUIManager.GUIPie;
			Rect drawPos = new Rect(GUIManager.OffsetX(), Screen.height - Screen.height * 0.1f - GUIManager.OffsetY() - Screen.height * 0.1f, Screen.height * 0.2f, Screen.height * 0.2f);
			GUIpie.SetFloat("Value", 1);
			GUIpie.color = new Color(1, 1, 1, 0.6f);
			//GUIpie.color = Color.white;
			Graphics.DrawTexture(drawPos, background, GUIpie);

			GUIpie.SetFloat("Value", HandController.activeHand.Ratio);
			GUIpie.SetFloat("Clockwise", 0);
			GUIpie.color = new Color((1 - HandController.activeHand.Ratio) * 0.75f, 0.75f, 0, 0.5f);

			Graphics.DrawTexture(drawPos, circle, GUIpie);

			int value = HandController.activeHand.value;

			//  GUIpie.SetFloat("Value", 1);
			//  GUIpie.color = Color.white;

			if (value <= HandController.MinValue) {
				if (value == HandController.MinValue) {
					Graphics.DrawTexture(drawPos, handEmpty);
				} else if (value == -1) {
					Graphics.DrawTexture(drawPos, warning1);
				} else if (value >= -2) {
					Graphics.DrawTexture(drawPos, warning2);
				}
				if (HandController.activeHand.showingWarning) {
					Graphics.DrawTexture(drawPos, warningSign);
				}
			} else {
				Graphics.DrawTexture(drawPos, hand);
			}
		}
    }
}
