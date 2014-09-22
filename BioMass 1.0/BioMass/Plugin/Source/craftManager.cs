using System;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using KSP;
using UnityEngine;

namespace BioMass
{
	
	
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
		public Texture bIco;
		public Texture eIco;
		public Texture wIco;
		public Texture lIco;
		public Texture mIco;
		
		public Vector2 resourceScrollPosition ;
		public Rect winRect;
		public bool bioWin = false;
		
		public ConfigNode saveGameNode;
		
		public string btnTxt;
		public Vessel thisActiveVessel;
		public Vector2 terminalScrollPosition;
		public static string consoleText;
		//public Texture btnImg;
		public bool showBtn;
		
		
		
		
		public void Start(){
			
			showBtn = true;
			winRect = new Rect(150, 150, 350, 250);
			bioBtnIcon = loadImage("bioIcon");//mainscreen button. Opens window.
			bioMeterBackground = loadImage("meter");	
			bioCloseBack = loadImage("close");
			bioError = loadImage("error");
			bioPass = loadImage("pass");
			bioBtnBackground = loadImage("box");//gui.button background
			bioWinBackground = loadImage("window");//qui.window background
			bioBtnOnBackground = loadImage("box_on");
			bioOutPutStyleBG = loadImage("outPut");
			bIco = loadImage("BioMass");
			eIco = loadImage("ElectricCharge");
			wIco = loadImage("Water");
			lIco = loadImage("Light");
			mIco = loadImage("MicroFlora");
			
			if(System.IO.File.Exists("GameData/BioMass/Plugin/pluginData/bioSave-" + HighLogic.CurrentGame.Title + ".cfg")){
				
				saveGameNode = ConfigNode.Load("GameData/BioMass/Plugin/pluginData/bioSave-" + HighLogic.CurrentGame.Title + ".cfg");
				new bioMsg("previous save loaded!");
			}else{
				new bioMsg("load cfg failed");
				saveGameNode = new ConfigNode();
				saveGameNode.CopyTo(saveGameNode);
				saveGameNode.Save("GameData/BioMass/Plugin/pluginData/bioSave-" + HighLogic.CurrentGame.Title + ".cfg");
				Debug.Log("new Config created " + saveGameNode);
				
			}
			thisActiveVessel = FlightGlobals.ActiveVessel;
			consoleText = "No errors reporting.";
			
		}

		public Texture2D loadResourceIcon(string imgName){

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
		}
		
		
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
		}
		void OnGUI(){
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
			VscrollBarStyle.fixedWidth = 9;
			
			VThumb = new GUIStyle ();
			VThumb.padding = new RectOffset(0, 0, 0, 0);
			VThumb.normal.background =VThumb.focused.background = VThumb.active.background = VThumb.onHover.background = (Texture2D)bioBtnOnBackground;
			
			HighLogic.Skin.verticalSliderThumb = VThumb;
			
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
			bioLabelStyle.padding = new RectOffset(2, 2, 2, 2);
			bioLabelStyle.richText = true;
			
			bioOutPutStyle = new GUIStyle (); 
			bioOutPutStyle.normal.textColor = Color.blue;
			bioOutPutStyle.fontSize = 9;
			bioOutPutStyle.padding = new RectOffset(0, 0, -2, 0);
			bioOutPutStyle.margin = new RectOffset(0, 0, 0, 0);
			bioOutPutStyle.alignment = TextAnchor.MiddleRight;
			bioOutPutStyle.richText = true;
			
			bioResourceIcon = new GUIStyle (); 
			bioResourceIcon.normal.textColor = Color.blue;
			bioResourceIcon.padding = new RectOffset(0, 0, 0, 0);
			bioResourceIcon.margin = new RectOffset(0, 0, 0, 0);
			bioResourceIcon.alignment = TextAnchor.MiddleRight;
			bioResourceIcon.fixedHeight = 16f;
			bioResourceIcon.fixedWidth = 32f;
			
			if(showBtn){
				
				//Texture btnImg = (Texture)Resources.Load("bioico");
				
				bioWin = GUI.Toggle(new Rect(Screen.width-175, 0, 36,36 ),bioWin, bioBtnIcon, "");
				
				if(bioWin == true){
					winRect = GUILayout.Window (21, winRect, makeCraftWin, "",  bioWinStyle);
				}
			}
			
			
		}
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
					
					
					if (!resources.ContainsKey(resource.info.name)) { resources.Add(resource.info.name,thisList ); }
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
		}
		void makeCraftWin(int winid){
			int barYPos = 1;
			
			if(GUI.Button (new Rect(4,2,12,12), "", bioCloseBtn)){bioWin = false;}
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
//				Texture2D resourceIcon = loadResourceIcon(resource.Key);
//				if(resourceIcon == null){
//					resourceIcon =  bioMeterBackground as Texture2D;
//				}
//				else{
//					resourceIcon.filterMode = FilterMode.Point;
//				}
				
				GUI.BeginGroup(new Rect(0,barYPos, 345, 24));
				GUILayout.BeginHorizontal();
				
				
				GUI.Label(new Rect(0,0,100,16),resource.Key,bioLabelStyle);
				
				GUI.Button(new Rect(75,0,200,16), "<b>" + Math.Round (resource.Value.ToArray()[0] ,4).ToString() + " / " + resource.Value.ToArray()[1] + "</b>" , bioMeterStyle);
				if(qtyBar > (resource.Value.ToArray()[0] / 200) && qtyBar > 10){ 
					GUI.Button(new Rect(75,0,(float)qtyBar,16),"", bioMeterBack);
				}
				//Resource Icon

				GUI.Label(new Rect(280,0,32,16),eIco, bioResourceIcon);
				
				GUILayout.EndHorizontal();
				GUI.EndGroup();
				
				barYPos += 20;	
			}
			//GUILayout.EndArea();
			
			GUILayout.EndVertical();
			GUILayout.EndScrollView();
			
			
			
			GUILayout.EndHorizontal();
			GUILayout.Space (6);
			
			GUILayout.BeginHorizontal();
			GUILayout.BeginVertical(GUILayout.Height(150));
			terminalScrollPosition = GUILayout.BeginScrollView(terminalScrollPosition, false, true, HscrollBarStyle,VscrollBarStyle);
			consoleText = GUILayout.TextArea(consoleText, bioErrStyle, GUILayout.MaxWidth(150));
			GUILayout.EndScrollView();
			GUILayout.EndVertical();
			GUILayout.BeginVertical();
			
			
			foreach(Part bioPart in thisActiveVessel.parts)
			{
				if(bioPart.FindModulesImplementing<BioGen>().Count > 0){
					
					GUILayout.Label(bioPart.partInfo.title, bioOutPutStyle);
					
					foreach(BioGen myBioGenPart in  bioPart.FindModulesImplementing<BioGen>()){
						string bioGenState;
						if(!myBioGenPart.AlwaysActive){
							
							GUILayout.BeginHorizontal();
							Texture indicatorLight = new Texture();
							
							if(myBioGenPart.activated){
								
								bioGenState = "Active";
								indicatorLight = bioPass;
							}
							else{
								
								indicatorLight = bioError;
								bioGenState = "Inactive";
							}
							
							
							GUILayout.Label(indicatorLight, bioIndicatorLight, GUILayout.Width(18));
							myBioGenPart.activated = GUILayout.Toggle(myBioGenPart.activated, myBioGenPart.bioLabel + " " + bioGenState, bioBtnStyle);
							if(GUI.changed){
								if(myBioGenPart.activated == true){myBioGenPart.Activate(); }
								else{myBioGenPart.Deactivate(); }
							}
							
							
							
							GUILayout.EndHorizontal();
						}
					}
				}
			}
			GUILayout.EndVertical();
			
			GUILayout.EndHorizontal();
			
			
			GUI.DragWindow();
		}
		
		
	}
}