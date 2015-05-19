using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using KSP;

namespace BioMass {

	[KSPAddon(KSPAddon.Startup.Flight, false)]
	public class craftManager : MonoBehaviour
	{
		public GUIStyle bioBtnStyle;
		public GUIStyle bioErrStyle;
		public GUIStyle bioCloseBtn;
		public GUIStyle bioLabelStyle;
		public Texture bioBtnIcon;
		public Texture bioCloseBack;

		public static List<string> consoleMsgs;
		public static bool mustHarvest;
		public bool isUpdating;
		public bool monitorWinOpen;
		public static string consoleText = "No errors reporting.";
		
		public void Start(){

			consoleMsgs = new List<string> ();
			bioBtnIcon = loadImage("bioIcon");//flightscreen button. Opens window.
			bioCloseBack = loadImage("close");
			StartCoroutine(updateConsole());

		}//END Start

		void FixedUpdate(){
			if (!isUpdating) {
				isUpdating = true;
				StartCoroutine (updateConsole ());
			}
		}

		//processes messages to be sent to the monitor
		public IEnumerator updateConsole(){

			if (consoleMsgs.Count > 0) {
				string tempText = "System Status:\n";
				int i = 0;
				List<string> tempList = consoleMsgs;

				while (i < tempList.Count) {
					tempText += tempList [i] + "\n";
					i++;
				}
				consoleText = tempText;
				consoleMsgs.Clear ();
			}
			isUpdating = false;
			yield return new WaitForSeconds(0);
		}


		/// Loads the image.
		public Texture loadImage(string imgName){
			string PathPlugin = string.Format("{0}","BioMass");
			var thisImg = new Texture();
			string PathTextures = string.Format("{0}/Textures", PathPlugin);
			string FolderPath = PathTextures;
			string imageFilename = String.Format("{0}/{1}", FolderPath, imgName);
			//
			if (GameDatabase.Instance.ExistsTexture(imageFilename)) { thisImg = GameDatabase.Instance.GetTexture(imageFilename, false); }
			else{ Debug.LogError("Image not found: " + imageFilename);	}
			//
			return(thisImg);
		}//END loadImage

		void OnGUI(){
			
			bioErrStyle  = new GUIStyle (GUI.skin.textArea); 
			bioErrStyle.fontSize = 10;
			bioErrStyle.padding = new RectOffset(2, 2, 2, 2);
			bioErrStyle.richText = true;
			bioErrStyle.fontStyle = FontStyle.Bold;
			//
			bioCloseBtn = new GUIStyle (GUI.skin.button); 		
			bioCloseBtn.normal.background = bioCloseBtn.focused.background = bioCloseBtn.hover.background = bioCloseBtn.active.background = (Texture2D)bioCloseBack;
			bioCloseBtn.padding = new RectOffset(0, 0, 1, 0); 
			bioCloseBtn.margin =  new RectOffset(0, 0, 0, 0); 
			//
			bioLabelStyle = new GUIStyle (); 
			bioLabelStyle.normal.textColor = bioLabelStyle.focused.textColor = bioLabelStyle.hover.textColor = bioLabelStyle.active.textColor = Color.black;
			bioLabelStyle.onNormal.textColor = bioLabelStyle.onFocused.textColor = bioLabelStyle.onHover.textColor = bioLabelStyle.onActive.textColor = Color.black;
			bioLabelStyle.fontSize = 11;
			bioLabelStyle.padding = new RectOffset(0, 2, 2, 2);
			bioLabelStyle.richText = true;
			bioLabelStyle.alignment = TextAnchor.MiddleLeft;
			//				
			if(GUI.Button(new Rect(175, 0, 36,36 ), bioBtnIcon, "")) monitorWinOpen = true;
				 
			if(monitorWinOpen){
				GUILayout.BeginArea(new Rect(1, 50 , 350, 600));
					GUILayout.BeginHorizontal(bioLabelStyle);
						GUILayout.Label("<color=yellow>  <b>BIOMONITOR</b></color>", bioLabelStyle);
						if(GUILayout.Button("<size=9><color=black>close</color></size>",bioCloseBtn, GUILayout.Width(36))){
							monitorWinOpen = false;
						}
						GUILayout.EndHorizontal();
					
				consoleText = GUILayout.TextArea(consoleText, bioErrStyle);
				GUILayout.EndArea();
			}
			//If the greenhouse must be emptied before it an close pop a warning...
			if(mustHarvest) GUI.ModalWindow (42,new Rect (Screen.width / 4, Screen.height / 4, Screen.width / 8, Screen.height / 8), makeHarvestModal, "BioMass Must Be Harvested!");
		}//END OnGUI
		//The warning msg about harvesting biomass before closing
		void makeHarvestModal(int modalId){
			GUILayout.Space (32);
			GUILayout.Label ("BioMass must be harvested before the greenhouse can be re-packed");
			GUILayout.Space (12);
			if (GUILayout.Button ("OK")) mustHarvest = false;
		}				
	}//END craftManager class
}//END BioMass namespace