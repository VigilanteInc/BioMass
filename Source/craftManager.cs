using System;
using System.Collections.Generic;
using System.Collections;
using KSP;
using UnityEngine;
using BioMass;

	[KSPAddon(KSPAddon.Startup.Flight, false)]
	public class craftManager : MonoBehaviour
	{
		public GUIStyle bioBtnStyle;
		public GUIStyle bioIndicatorLight;
		public GUIStyle bioMeterStyle;
		public GUIStyle bioErrStyle;
		public GUIStyle bioMeterBack;
		public GUIStyle bioWinStyle;
		public GUIStyle bioCloseBtn;
		public GUIStyle bioLabelStyle;
		public GUIStyle bioOutPutStyle;
		public GUIStyle	bioResourceIcon;
		public GUIStyle	VscrollBarStyle;
		public GUIStyle	HscrollBarStyle;
		public GUIStyle VThumb;
		public Texture bioBtnIcon;
		public Texture bioCloseBack;
		public Texture bioBtnBackground;
		public Texture bioBtnOnBackground;
		public Texture bioMeterBackground;
		public Texture bioWinBackground;
		public Texture bioOutPutStyleBG;
		public Texture bioError;
		public Texture bioPass;
		public Texture bioLabelIcon;
		public Texture2D resourceIcon;

		public Dictionary<string, Texture2D> resourceIcons;
		public Vector2 resourceScrollPosition ;
		public Rect winRect;
		public bool isOpen;
		public bool bioWin = false;
		
		public ConfigNode saveGameNode;

		public string btnTxt;
		public Vessel thisActiveVessel;
		public Vector2 terminalScrollPosition;
		public static string consoleText;
		public static List<string> consoleMsgs;
		public bool isUpdating;
		//public Texture btnImg;
		public bool showBtn;
		public Vector2[] winToggleState;
		public bool monitorWinOpen;

		
		public void Start(){
			
			consoleMsgs = new List<string> ();
			resourceIcon = new Texture2D(32,16);
			resourceIcons = new Dictionary<string, Texture2D>();
			resourceIcons = resourceIconDefs();

			winToggleState = new Vector2[2];
			winToggleState[0] = new Vector2(Screen.width/4,Screen.height/4);
			winToggleState[1] = new Vector2(0, 0);
			showBtn = true;
			winRect = new Rect(0, 0, 350, 250);
			bioBtnIcon = loadImage("bioIcon");//flightscreen button. Opens window.
			bioMeterBackground = loadImage("meter");	
			bioCloseBack = loadImage("close");
			bioError = loadImage("error");
			bioPass = loadImage("pass");
			bioBtnBackground = loadImage("box");//gui.button background
			bioWinBackground = loadImage("window");//qui.window background
		
			bioBtnOnBackground = loadImage("box_on");
			bioOutPutStyleBG = loadImage("outPut");

//			if(System.IO.File.Exists("GameData/BioMass/Plugin/pluginData/bioSave-" + HighLogic.CurrentGame.Title + ".cfg")){
//				
//				saveGameNode = ConfigNode.Load("GameData/BioMass/Plugin/pluginData/bioSave-" + HighLogic.CurrentGame.Title + ".cfg");
//				new bioMsg("previous save loaded!");
//			}else{
//				new bioMsg("load cfg failed");
//				saveGameNode = new ConfigNode();
//				saveGameNode.CopyTo(saveGameNode);
//				saveGameNode.Save("GameData/BioMass/Plugin/pluginData/bioSave-" + HighLogic.CurrentGame.Title + ".cfg");
//				Debug.Log("new Config created " + saveGameNode);
//			}
			thisActiveVessel = FlightGlobals.ActiveVessel;
			consoleText = "No errors reporting.";
		}//END Start


		/// <summary>
		/// Resources the icon defs. lol No really! IT DOES!
		/// </summary>
		/// <returns>The icon defs.</returns>
		public Dictionary<string,Texture2D> resourceIconDefs(){

			Dictionary<string,Texture2D> tempDict = new Dictionary<string, Texture2D>();
			foreach(GameDatabase.TextureInfo thisTexture in GameDatabase.Instance.GetAllTexturesInFolderType("Icons")){

				string[] nameString = thisTexture.name.Split('/');
				
				if(nameString[0] == "BioMass"){
					tempDict.Add(nameString[2],thisTexture.texture);
					
				}
				
			}
			return(tempDict);
		}

		public IEnumerator winSlideToggle(){

			float progress = 0.0f;
			float speed = 0.5f;
			bioWin = true;

			Vector2 newPosition = new Vector2();
			if(!isOpen){

				newPosition = winToggleState[0];
				isOpen = true;
			}else{

				newPosition = winToggleState[1];
				isOpen = false;
			}

		 
			while(progress < 1){
				winRect.position = Vector2.Lerp(winRect.position, newPosition, speed);
				progress = progress + TimeWarp.fixedDeltaTime;
				yield return new WaitForFixedUpdate();
				if(progress >= 0.4f){
					
					progress = 1;
				}

			}

			winRect.position = newPosition;

			if(!isOpen){
				bioWin = false;
			}


		}

		/// <summary>
		/// Loads the resource icon. Returns Imagename
		/// </summary>
		/// <returns>The resource icon.</returns>
		/// <param name="imgName">Image name.</param>
		public Texture2D loadResourceIcon(string imgName){

			//GameDatabase.Instance.GetAllTexturesInFolderType("GameData/BioMass/Plugin/plugin");

			string PathPlugin = string.Format("{0}","BioMass");
			Texture2D thisImg = new Texture2D(32,16);

			string PathTextures = string.Format("{0}/Textures", PathPlugin);
			string FolderPath = PathTextures;
			string imageFilename = String.Format("{0}/{1}", FolderPath, imgName);
		
			if (GameDatabase.Instance.ExistsTexture(imageFilename))
			{
				//new bioMsg("Load Texture: " + imageFilename);
				thisImg = GameDatabase.Instance.GetTexture(imageFilename, false);
			}
			else
			{
				//Debug.Log("image not found" + imageFilename);
			}
			return(thisImg);
		}//END loadResourceIcon
		
	
		public Texture loadImage(string imgName){
			string PathPlugin = string.Format("{0}","BioMass");
			Texture thisImg = new Texture();
			//Texture2D thisImg = new Texture2D(16,16);
			string PathTextures = string.Format("{0}/Textures", PathPlugin);
			string FolderPath = PathTextures;
			string imageFilename = String.Format("{0}/{1}", FolderPath, imgName);
			if (GameDatabase.Instance.ExistsTexture(imageFilename))
			{
				//new bioMsg("Load Texture: " + imageFilename);
				thisImg = GameDatabase.Instance.GetTexture(imageFilename, false);
			
			}
			else
			{
				Debug.LogError("image not found" + imageFilename);
			}
			return(thisImg);
		}//END loadImage

		void OnGUI(){
			
			// Window background in flightview
			GUI.backgroundColor = Color.white;

			bioBtnStyle = new GUIStyle (GUI.skin.button); 
			bioBtnStyle.normal.textColor = bioBtnStyle.focused.textColor = bioBtnStyle.hover.textColor = bioBtnStyle.active.textColor = Color.black;
			bioBtnStyle.onNormal.textColor = bioBtnStyle.onFocused.textColor = bioBtnStyle.onHover.textColor = bioBtnStyle.onActive.textColor = Color.blue;
			bioBtnStyle.normal.background = bioBtnStyle.focused.background = bioBtnStyle.active.background = bioBtnStyle.onHover.background = (Texture2D)bioBtnBackground;
			bioBtnStyle.onActive.background = bioBtnStyle.onNormal.background = bioBtnStyle.hover.background = (Texture2D)bioBtnOnBackground;
			
			bioBtnStyle.fontSize = 11;
			bioBtnStyle.padding = new RectOffset(2, 2, 2, 2);
			bioBtnStyle.richText = true;
			
			bioErrStyle  = new GUIStyle (GUI.skin.textArea); 
			bioErrStyle.fontSize = 10;
			bioErrStyle.padding = new RectOffset(2, 2, 2, 2);
			bioErrStyle.richText = true;
			bioErrStyle.fontStyle = FontStyle.Bold;

			bioIndicatorLight = new GUIStyle (GUI.skin.label); 
			bioIndicatorLight.border = new RectOffset(0,0,0,0);
			bioIndicatorLight.padding = new RectOffset(0, 0, 2, 0); 
			bioIndicatorLight.margin =  new RectOffset(0, 0, 0, 0); 
			bioIndicatorLight.normal.background = bioIndicatorLight.focused.background = bioIndicatorLight.active.background = bioIndicatorLight.onHover.background = null;
			bioIndicatorLight.onActive.background = bioIndicatorLight.onNormal.background = bioIndicatorLight.hover.background = null;

			bioCloseBtn = new GUIStyle (GUI.skin.button); 		
			bioCloseBtn.normal.background = bioCloseBtn.focused.background = bioCloseBtn.hover.background = bioCloseBtn.active.background = (Texture2D)bioCloseBack;
			bioCloseBtn.padding = new RectOffset(0, 0, 1, 0); 
			bioCloseBtn.margin =  new RectOffset(0, 0, 0, 0); 

			bioMeterBack = new GUIStyle (); 
			bioMeterBack.normal.textColor = bioMeterBack.focused.textColor = bioMeterBack.hover.textColor = bioMeterBack.active.textColor = Color.black;
			bioMeterBack.onNormal.textColor = bioMeterBack.onFocused.textColor = bioMeterBack.onHover.textColor = bioMeterBack.onActive.textColor = Color.blue;
			bioMeterBack.normal.background = bioMeterBack.focused.background = bioMeterBack.active.background = bioMeterBack.onHover.background = (Texture2D)bioBtnOnBackground;
			//bioMeterBack.onActive.background = bioMeterBack.onNormal.background = bioMeterBack.hover.background = (Texture2D)bioBtnOnBackground;
			bioMeterBack.border = new RectOffset(6,6,6,4);
			bioMeterBack.fontSize = 11;
			bioMeterBack.padding = new RectOffset(0, 0, 0, 0);
			bioMeterBack.richText = true;

			VscrollBarStyle = new GUIStyle (GUI.skin.verticalScrollbar); 
			VscrollBarStyle.fixedWidth = 16;
			VscrollBarStyle.padding = new RectOffset(0, 0, 0, 0);
			VscrollBarStyle.normal.background = (Texture2D)bioBtnOnBackground;

			VThumb = new GUIStyle (GUI.skin.verticalSliderThumb);
			VThumb.padding = new RectOffset(0, 0, 0, 0);
			VThumb.normal.background =VThumb.focused.background = VThumb.active.background = VThumb.onHover.background = (Texture2D)bioBtnOnBackground;

			GUI.skin.verticalSliderThumb = VThumb;
//			GUISkin thisSkin = HighLogic.Skin; 
//			thisSkin.verticalSliderThumb.padding = new RectOffset(0, 0, 0, 0);
//			thisSkin.verticalSliderThumb.normal.background = thisSkin.verticalSliderThumb.focused.background = (Texture2D)bioBtnOnBackground;
//			thisSkin.horizontalSliderThumb.padding = new RectOffset(0, 0, 0, 0);
//			thisSkin.horizontalSliderThumb.normal.background = (Texture2D)bioBtnOnBackground;

			HscrollBarStyle = new GUIStyle (GUI.skin.horizontalScrollbar); 
			HscrollBarStyle.normal.background = HscrollBarStyle.focused.background = HscrollBarStyle.active.background = HscrollBarStyle.onHover.background = (Texture2D)bioBtnOnBackground;
			HscrollBarStyle.fixedWidth = 9;

			bioMeterStyle = new GUIStyle(GUI.skin.button); 
			bioMeterStyle.normal.textColor = bioMeterStyle.focused.textColor = bioMeterStyle.hover.textColor = bioMeterStyle.active.textColor = Color.black;
			bioMeterStyle.onNormal.textColor = bioMeterStyle.onFocused.textColor = bioMeterStyle.onHover.textColor = bioMeterStyle.onActive.textColor = Color.green;
			bioMeterStyle.normal.background = bioMeterStyle.focused.background = bioMeterStyle.hover.background = bioMeterStyle.active.background = (Texture2D)bioMeterBackground;
			bioMeterStyle.fontSize = 11;
			bioMeterStyle.padding = new RectOffset(1, 1, 1 ,1);
			bioMeterStyle.richText = true;
			
			bioWinStyle = new GUIStyle (GUI.skin.window);
			bioWinStyle.normal.background = bioWinStyle.focused.background = bioWinStyle.hover.background = bioWinStyle.active.background = (Texture2D)bioWinBackground;
			bioWinStyle.onNormal.background = bioWinStyle.onFocused.background = bioWinStyle.onHover.background = bioWinStyle.onActive.background = (Texture2D)bioWinBackground;
			bioWinStyle.normal.textColor = bioWinStyle.focused.textColor = bioWinStyle.hover.textColor = bioWinStyle.active.textColor = Color.black;
			bioWinStyle.onNormal.textColor = bioWinStyle.onFocused.textColor = bioWinStyle.onHover.textColor = bioWinStyle.onActive.textColor = Color.black;
			bioWinStyle.fontSize = 11;
			bioWinStyle.padding = new RectOffset(2, 2, 2, 2);
			bioWinStyle.richText = true;
			
			bioLabelStyle = new GUIStyle (); 
			bioLabelStyle.normal.textColor = bioLabelStyle.focused.textColor = bioLabelStyle.hover.textColor = bioLabelStyle.active.textColor = Color.black;
			bioLabelStyle.onNormal.textColor = bioLabelStyle.onFocused.textColor = bioLabelStyle.onHover.textColor = bioLabelStyle.onActive.textColor = Color.black;
			bioLabelStyle.fontSize = 11;
			bioLabelStyle.padding = new RectOffset(0, 2, 2, 2);
			bioLabelStyle.richText = true;
			bioLabelStyle.alignment = TextAnchor.MiddleLeft;
			
			bioOutPutStyle = new GUIStyle (); 
			bioOutPutStyle.normal.textColor = Color.blue;
			bioOutPutStyle.fontSize = 9;
			bioOutPutStyle.padding = new RectOffset(0, 0, -2, 0);
			bioOutPutStyle.margin = new RectOffset(0, 0, 0, 0);
			bioOutPutStyle.alignment = TextAnchor.MiddleRight;
			bioOutPutStyle.richText = true;

			bioResourceIcon = new GUIStyle (); 
			bioResourceIcon.normal.textColor = Color.blue;
			bioResourceIcon.padding = new RectOffset(0, 0, -2, 0);
			bioResourceIcon.margin = new RectOffset(0, 0, 0, 0);
			bioResourceIcon.alignment = TextAnchor.MiddleRight;
			bioResourceIcon.fixedHeight = 16f;
			bioResourceIcon.fixedWidth = 32f;
			
			if(showBtn){
				
				//Texture btnImg = (Texture)Resources.Load("bioico");
				
				if(GUI.Button(new Rect(175, 0, 36,36 ), bioBtnIcon, "")){
					monitorWinOpen = true;
					StartCoroutine(winSlideToggle());

				} 
				if(monitorWinOpen){
					GUILayout.BeginArea(new Rect(1, 50 , 250, 150));
						GUILayout.BeginHorizontal(bioLabelStyle);
							GUILayout.Label("<color=yellow>  <b>BIOMONITOR</b></color>", bioLabelStyle);
							if(GUILayout.Button("<size=9>close</size>",bioCloseBtn, GUILayout.Width(36))){
								monitorWinOpen = false;
							}
							GUILayout.EndHorizontal();
						
					consoleText = GUILayout.TextArea(consoleText, bioErrStyle);
					GUILayout.EndArea();
				}
				if(bioWin == true){
					winRect = GUILayout.Window (21, winRect, makeCraftWin, "",  bioWinStyle);

				}
			}
		//Updates monitor output
		if (!isUpdating)
			StartCoroutine(updateConsole ());
		
		}//END OnGUI

		private Dictionary<string,List<double>> GetResources()
		{
			Dictionary<string,List<double>> resources = new Dictionary<string,List<double>>();
			foreach (Part part in thisActiveVessel.Parts)
			{
				foreach (PartResource resource in part.Resources)
				{		
					List<double> thisList = new List<double>();
					thisList.Add(resource.amount);
					thisList.Add (resource.maxAmount);
					
					
					if (!resources.ContainsKey(resource.info.name)) { 
						resources.Add(resource.info.name,thisList ); 
					}
					else{

						List<double> newValueList = new List<double>();
						int i = 0;
						foreach(double thisDouble in resources[resource.info.name]){
							
							newValueList.Add( (thisDouble + thisList[i]) );
							i++;

						}
						if(resources.Remove(resource.info.name)){
							resources.Add (resource.info.name,newValueList);
						}
						
					}
				}
			}
			return resources;
		}//END GetResources dictionary

		void makeCraftWin(int winid){
			int barYPos = 1;
			
			if(GUI.Button (new Rect(4,2,12,12), "", bioCloseBtn)){

				StartCoroutine(winSlideToggle());
			
			}
			GUILayout.Space (11);
			GUILayout.BeginHorizontal();
			GUILayout.Label("<size=18><color=green><b>BioCraft</b></color></size>",bioLabelStyle);
			GUILayout.EndHorizontal();
			GUILayout.Space (6);
			GUILayout.BeginHorizontal("box",GUILayout.Height(150));

			resourceScrollPosition = GUILayout.BeginScrollView(resourceScrollPosition, false, true, HscrollBarStyle,VscrollBarStyle);
			GUILayout.BeginVertical(GUILayout.Height(255));
			//GUILayout.BeginArea(new Rect(2,2,345,250), "","box");

			foreach(KeyValuePair<string,List<double>> resource in GetResources()) {
				double qtyBar = (resource.Value.ToArray()[0] / resource.Value.ToArray()[1] ) * 200;

				if(resourceIcons.ContainsKey(resource.Key)){
					resourceIcon = resourceIcons[resource.Key];
				}else{
					resourceIcon = resourceIcons["BioMass"];
				}

				GUI.BeginGroup(new Rect(0,barYPos, 325, 24));
				GUILayout.BeginHorizontal();
				
				
				GUI.Label(new Rect(0,0,100,16),resource.Key,bioLabelStyle);
				GUI.Button(new Rect(80,0,200,16), "" , bioMeterStyle);
				if(qtyBar > (resource.Value.ToArray()[0] / 200) && qtyBar > 10){ 
					GUI.Button(new Rect(80,0,(float)qtyBar,16),"", bioMeterBack);
				}
				GUI.Button(new Rect(80,0,200,16), "<b>" + Math.Round (resource.Value.ToArray()[0] ,10).ToString() + " / " + resource.Value.ToArray()[1] + "</b>" ,bioLabelStyle);

				//Resource Icon
				GUI.Label(new Rect(285,0,32,16),resourceIcon, bioResourceIcon);
				
				GUILayout.EndHorizontal();
				GUI.EndGroup();
				
				barYPos += 20;	
			}
			//GUILayout.EndArea();

			GUILayout.EndVertical();
			GUILayout.EndScrollView();

			GUILayout.EndHorizontal();
//			GUILayout.Space (6);
//
 			GUILayout.BeginHorizontal();
//			GUILayout.BeginVertical(GUILayout.Height(150), GUILayout.Width(150));
			//terminalScrollPosition = GUILayout.BeginScrollView(terminalScrollPosition, false, false);
			//GUILayout.VerticalSlider(terminalScrollPosition.y,0, 100, VscrollBarStyle, VThumb);
			///consoleText = GUILayout.TextArea(consoleText, bioErrStyle, GUILayout.MaxWidth(150));
			//GUILayout.EndScrollView();
//			GUILayout.EndVertical();
			GUILayout.BeginVertical();
			
			
			foreach(Part bioPart in thisActiveVessel.parts)
			{
				if(bioPart.FindModulesImplementing<BiologicalProcess>().Count > 0){
					
					GUILayout.Label(bioPart.partInfo.title, bioOutPutStyle);
					foreach (AnimatedGenerator myBioGenPart in bioPart.FindModulesImplementing<AnimatedGenerator>()) {
						string bioGenState;
						if(!myBioGenPart.AlwaysActive){

							GUILayout.BeginHorizontal();
							Texture indicatorLight = new Texture();

							if(myBioGenPart.IsActivated){

								bioGenState = "Active";
								indicatorLight = bioPass;
							}
							else{

								indicatorLight = bioError;
								bioGenState = "Inactive";
							}


							GUILayout.Label(indicatorLight, bioIndicatorLight, GUILayout.Width(18));
							myBioGenPart.IsActivated = GUILayout.Toggle(myBioGenPart.IsActivated, myBioGenPart.status + " " + bioGenState, bioBtnStyle);
							if(GUI.changed){
								if(myBioGenPart.IsActivated == true){myBioGenPart.EnableModule(); }
								else{myBioGenPart.EnableModule(); }
							}

							GUILayout.EndHorizontal();
						}
					}
					foreach(BiologicalProcess myBioGenPart in bioPart.FindModulesImplementing<BiologicalProcess>()){
						string bioGenState;
						if(!myBioGenPart.AlwaysActive){
							
							GUILayout.BeginHorizontal();
							Texture indicatorLight = new Texture();
							
							if(myBioGenPart.IsActivated){
								
								bioGenState = "Active";
								indicatorLight = bioPass;
							}
							else{
								
								indicatorLight = bioError;
								bioGenState = "Inactive";
							}
							
							
							GUILayout.Label(indicatorLight, bioIndicatorLight, GUILayout.Width(18));
							myBioGenPart.IsActivated = GUILayout.Toggle(myBioGenPart.IsActivated, myBioGenPart.status + " " + bioGenState, bioBtnStyle);
							if(GUI.changed){
								if(myBioGenPart.IsActivated == true){myBioGenPart.EnableModule(); }
								else{myBioGenPart.EnableModule(); }
							}
							
							GUILayout.EndHorizontal();
						}
					}
				}
			}
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			GUI.DragWindow();
		}//END makeCraftWin

	//processes messages to be sent to the monitor
	public IEnumerator updateConsole(){
		isUpdating = true;
		string tempText = "System Status:\n";
		int i = 0;
		List<string> tempList = consoleMsgs;

		while (i < tempList.Count) {
			tempText += tempList [i] + "\n";
			i++;
		}
		consoleText = tempText;
		consoleMsgs.Clear ();
		isUpdating = false;

		yield return new WaitForSeconds(0);
	}

	}//END craftManager class
