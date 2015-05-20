using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Experience;
using KSP;

namespace BioMass
{
	//ModuleResourceConverter for Bilogical Processes such as photosynthesis
	//Includes error reporting to the biomonitor 
	[KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
	public class BioMassTechTree : MonoBehaviour{
		public bool isFirstLoad;
		public Rect modalRect = new Rect( (Screen.width - Screen.width*0.25f) - 48 , 48, (Screen.width*0.25f), (Screen.height*0.33f));
		public ConfigNode stockTechTree;
		public ConfigNode bioTechTree;
		public ConfigNode BioMassSaveData;
		public string saveDataUrl;
		public Game currentGame;
		//public RectTransform thisRect;
		//public Canvas thisCanvas;

		void Start(){

			currentGame = HighLogic.CurrentGame;
			saveDataUrl = "GameData/BioMass/SaveData/saveGame-" + currentGame.Title.Substring(0,4) + ".cfg";
			var tempSaveData = ConfigNode.Load(saveDataUrl);
			try{
				if (tempSaveData.HasData){
					if (tempSaveData.HasValue ("BioTechCheck")) {
						BioMassSaveData = tempSaveData;
						Debug.LogWarning ("[BioMass]: Current TechTree Loaded.");
					} 
				}
			}
			catch(Exception){
				isFirstLoad = true;
				Debug.LogWarning ("[BioMass]: First time loading prompt; Include BioTech in TechTree?");
				BioMassSaveData = new ConfigNode ();
				var game = BioMassSaveData.AddNode ("GAME");
				game.AddValue ("BioTechCheck", true);
				BioMassSaveData.Save (saveDataUrl);
			}

		}
		void setTechTree(){

			try{
				stockTechTree = ConfigNode.Load (HighLogic.CurrentGame.Parameters.Career.TechTreeUrl);
				bioTechTree = ConfigNode.Load ("GameData/BioMass/Resources/BioTech.cfg");
				//
				stockTechTree.GetNode ("TechTree").AddData (bioTechTree);
				stockTechTree.Save ("GameData/BioMass/Resources/BioMass_TechTree.cfg");
				HighLogic.CurrentGame.Parameters.Career.TechTreeUrl = "GameData/BioMass/Resources/BioMass_TechTree.cfg";
				//
				new bioMsg ("The Biosciences Path Has Been Added To The TechTree!\n Enjoy Playing With BioMass!!");
			}catch(Exception){
				new bioMsg ("Failed to load Biosciences into the TechTree configuration!");
			}


		}
		void OnGUI(){
			if (isFirstLoad)
				GUI.Window (42, modalRect, makeModal, "<color=lime>BioMass - v1.0</color>");
		}
		void makeModal(int winId){
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUI.skin.label.fontSize = 18;
			GUILayout.BeginVertical ();
			Texture logo = loadImage("logo-1");
			GUILayout.Label (logo, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
			GUILayout.Label ("Would you like to include the Biosciences path in the TechTree?" );
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Yes")) {
				

				//apply the chnages to the techtree
				setTechTree ();
				isFirstLoad = false;
			}
			if (GUILayout.Button ("No")) {
				
				isFirstLoad = false;
			}
			GUILayout.EndHorizontal ();
			GUILayout.Label ("<b><color=lime>Thanks For Playing!</color></b>");
			GUILayout.EndVertical ();

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
	}

	public class BiologicalProcess : ModuleResourceConverter{

		[KSPField]
		public string bioName = "";   //optional name of crop/organisim for error reporting. 
	
		public Dictionary<int,string> bioSysMsgs;

		public override void OnStart(PartModule.StartState state){

			if (bioName == "")
				bioName = ConverterName;
			bioSysMsgs = new Dictionary<int,string> ();
			base.OnStart (state);
		}
		/// outputs the status of the part to the console.
		public override void OnFixedUpdate(){

			craftManager.consoleMsgs.Add( "<b>" + bioName + ":</b>\t" + status);
			base.OnFixedUpdate ();

		}

	}

	//prints a large yellow string/msg to the center of the screen
	public class bioMsg : MonoBehaviour{
		public string thisString;

		public bioMsg(string msg){
			thisString = msg;

			Debug.LogWarning("[BioMass]: " + msg);
			ScreenMessages.PostScreenMessage(thisString, 3.0f, ScreenMessageStyle.UPPER_CENTER);

		}

	}



}//END BioMass namespace