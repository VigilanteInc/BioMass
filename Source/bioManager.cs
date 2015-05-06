using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using KSP;
using UnityEngine;

namespace BioMass
{
	
	
//	[KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
//	public class bioManager : MonoBehaviour
//	{
//		
//		public Rect winRect;
//		
//		
//		public GUIStyle bioBtnStyle;
//		public GUIStyle bioMeterStyle;
//		public GUIStyle bioWinStyle;
//		public GUIStyle bioCloseBtn;
//		public GUIStyle bioLabelStyle;
//		public GUIStyle bioOutPutStyle;
//		public Texture2D bioOutPutStyleBG;
//		public Texture2D bioBtnIcon;
//		public Texture2D bioCloseBack;
//		public Texture2D bioBtnBackground;
//		public Texture2D bioBtnOnBackground;
//		public Texture2D bioMeterBackground;
//		public Texture2D bioWinBackground;
//		public Texture2D bioLabelIcon;
//		public string btnTxt;
//		public static Dictionary<string, double> bioResourceIns;
//		public static Dictionary<string, double> bioResourceOuts;
//		public bool isLoading;
//		public bool showBtn;
//		public bool bioWin = false;
//		public List<Vessel> bioCrafts;
//		public List<Vessel> allVessels;
//		public Dictionary<string, double> craftStamps; 
//		public ConfigNode saveGameNode;
//		public ConfigNode.ConfigNodeList nodesList;
//		public bool loadReady;
//		public int loadCount;
//		
//		// Use this for initialization
//		void Start ()
//		{
//			bioBtnIcon = loadImage("bioIcon");//mainscreen button. Opens window.
//			bioMeterBackground = loadImage("meter");	
//			bioCloseBack = loadImage("close");
//			bioBtnBackground = loadImage("box");//gui.button background
//			bioWinBackground = loadImage("window");//qui.window background
//			bioBtnOnBackground = loadImage("box_on");
//			bioOutPutStyleBG = loadImage("outPut");
//			winRect = new Rect(150, 150, 300, 300);
//			showBtn = true;
//			craftStamps = new Dictionary<string, double>();
//			
//			allVessels = FlightGlobals.Vessels;
//			bioCrafts = new List<Vessel>();
//			
//			if(System.IO.File.Exists("GameData/BioMass/Plugin/pluginData/bioSave-" + HighLogic.CurrentGame.Title + ".cfg")){
//				
//				saveGameNode = ConfigNode.Load("GameData/BioMass/Plugin/pluginData/bioSave-" + HighLogic.CurrentGame.Title + ".cfg");
//				new bioMsg("Previous save loaded!");
//				getBioCraft();
//				loadCount = 1;
//				nodesList = saveGameNode.nodes;
//			}else{
//				Debug.LogError("Load cfg failed");
//				saveGameNode = new ConfigNode();
//				saveGameNode.AddNode("BioSave").AddValue("instanceID", HighLogic.CurrentGame.Title);;
//				//saveGameNode.AddValue("instanceID", HighLogic.CurrentGame.Title);
//				//saveGameNode.CopyTo(saveGameNode);
//				saveGameNode.Save("GameData/BioMass/Plugin/pluginData/bioSave-" + HighLogic.CurrentGame.Title + ".cfg");
//				new bioMsg("New savefile created: " + HighLogic.CurrentGame.Title);
//				
//			}
//			
//			
//		}
//		void  FixedUpdate(){
//			//if(Time.timeSinceLevelLoad < 1.0f || !FlightGlobals.ready || loadReady ){return;}
//			if(loadCount >= 2){return;}
//			
//			getBioCraft();
//			
//			
//		}
//		
//		void OnGUI(){
//			GUI.backgroundColor = Color.white;
//			
//			bioBtnStyle = new GUIStyle (GUI.skin.button); 
//			bioBtnStyle.normal.textColor = bioBtnStyle.focused.textColor = bioBtnStyle.hover.textColor = bioBtnStyle.active.textColor = Color.black;
//			bioBtnStyle.onNormal.textColor = bioBtnStyle.onFocused.textColor = bioBtnStyle.onHover.textColor = bioBtnStyle.onActive.textColor = Color.blue;
//			bioBtnStyle.normal.background = bioBtnStyle.focused.background = bioBtnStyle.active.background = bioBtnStyle.onHover.background = bioBtnBackground;
//			bioBtnStyle.onActive.background = bioBtnStyle.onNormal.background = bioBtnStyle.hover.background = bioBtnOnBackground;
//			
//			bioBtnStyle.fontSize = 11;
//			bioBtnStyle.padding = new RectOffset(2, 2, 2, 2);
//			bioBtnStyle.richText = true;
//			
//			bioCloseBtn = new GUIStyle (GUI.skin.button); 		
//			bioCloseBtn.normal.background = bioCloseBtn.focused.background = bioCloseBtn.hover.background = bioCloseBtn.active.background = bioCloseBack;
//			
//			
//			bioMeterStyle = new GUIStyle(GUI.skin.button); 
//			bioMeterStyle.normal.textColor = bioMeterStyle.focused.textColor = bioMeterStyle.hover.textColor = bioMeterStyle.active.textColor = Color.black;
//			bioMeterStyle.onNormal.textColor = bioMeterStyle.onFocused.textColor = bioMeterStyle.onHover.textColor = bioMeterStyle.onActive.textColor = Color.green;
//			bioMeterStyle.normal.background = bioMeterStyle.focused.background = bioMeterStyle.hover.background = bioMeterStyle.active.background = bioMeterBackground;
//			
//			bioMeterStyle.fontSize = 11;
//			bioMeterStyle.padding = new RectOffset(0, 0, 0, 0);
//			bioMeterStyle.richText = true;
//			
//			bioWinStyle = new GUIStyle (GUI.skin.window);
//			bioWinStyle.normal.background = bioWinStyle.focused.background = bioWinStyle.hover.background = bioWinStyle.active.background = bioWinBackground;
//			bioWinStyle.onNormal.background = bioWinStyle.onFocused.background = bioWinStyle.onHover.background = bioWinStyle.onActive.background = bioWinBackground;
//			bioWinStyle.normal.textColor = bioWinStyle.focused.textColor = bioWinStyle.hover.textColor = bioWinStyle.active.textColor = Color.black;
//			bioWinStyle.onNormal.textColor = bioWinStyle.onFocused.textColor = bioWinStyle.onHover.textColor = bioWinStyle.onActive.textColor = Color.black;
//			bioWinStyle.fontSize = 11;
//			bioWinStyle.padding = new RectOffset(2, 2, 2, 2);
//			bioWinStyle.richText = true;
//			
//			bioLabelStyle = new GUIStyle (); 
//			bioLabelStyle.normal.textColor = bioLabelStyle.focused.textColor = bioLabelStyle.hover.textColor = bioLabelStyle.active.textColor = Color.black;
//			bioLabelStyle.onNormal.textColor = bioLabelStyle.onFocused.textColor = bioLabelStyle.onHover.textColor = bioLabelStyle.onActive.textColor = Color.black;
//			bioLabelStyle.fontSize = 11;
//			bioLabelStyle.padding = new RectOffset(2, 2, 2, 2);
//			bioLabelStyle.richText = true;
//			
//			bioOutPutStyle = new GUIStyle (); 
//			bioOutPutStyle.normal.textColor = Color.cyan;
//			bioOutPutStyle.fontSize = 9;
//			bioOutPutStyle.padding = new RectOffset(2, 2, 2, 2);
//			bioOutPutStyle.margin = new RectOffset(0, 0, 0, 0);
//			bioOutPutStyle.fontStyle = FontStyle.Bold;
//			bioOutPutStyle.normal.background = bioOutPutStyleBG;
//			bioOutPutStyle.richText = true;
//			
//			
//			//			if(showBtn){
//			//				
//			//				//Texture btnImg = (Texture)Resources.Load("bioico");
//			//				
//			//				bioWin = GUI.Toggle(new Rect(Screen.width-175, 0, 36,36 ),bioWin, bioBtnIcon, "");
//			//				
//			//				if(bioWin == true){
//			//					winRect = GUILayout.Window (21, winRect, makeWin, "", bioWinStyle);
//			//					
//			//				}	
//			//			}
//			
//		}
//		
//		
//		
//		public ConfigNode loadCFG(string partName){
//			
//			var thisPart =  PartLoader.getPartInfoByName(partName);
//			string partCfg = thisPart.partUrl;
//			ConfigNode thisConfig = new ConfigNode();
//			ConfigNode partNode = new ConfigNode();
//			
//			if(System.IO.File.Exists(partCfg)){
//				
//				thisConfig = ConfigNode.Load(partCfg);
//				partNode = thisConfig.GetNode("PART");
//				ConfigNode.ConfigNodeList moduleList = partNode.nodes;
//				
//				//				
//			}
//			else{
//				Debug.LogError(partCfg  + " not found");
//				
//			}
//			return(partNode);
//		}
//		
//		void pruneSaveFile(){
//			
//		}
//		void getBioCraft(){
//			
//			isLoading = true;
//			bioCrafts.Clear();
//			allVessels = FlightGlobals.Vessels;
//			
//			foreach(Vessel bioCraft in allVessels){
//				bool hasBioGen = false;
//				foreach(ProtoPartSnapshot thisPart in bioCraft.protoVessel.protoPartSnapshots){
//					
//					foreach(var thisModule in thisPart.modules){
//						
//						if(thisModule.moduleName == "BioGen"){ 
//							hasBioGen = true;
//							
//						}
//						
//					}
//				}
//				if(hasBioGen){
//					bioCrafts.Add(bioCraft);
//					ConfigNode.ConfigNodeList nodesList = saveGameNode.nodes;
//					if(nodesList.Contains(bioCraft.id.ToString())){
//						nodesList.RemoveNode(bioCraft.id.ToString());
//					}
//					saveGameNode.AddNode(bioCraft.id.ToString()).AddValue("saveTime", Planetarium.GetUniversalTime().ToString());
//					saveGameNode.Save("GameData/BioMass/Plugin/pluginData/bioSave-" + HighLogic.CurrentGame.Title + ".cfg");
//					new bioMsg("Saved craft.");
//				}
//				
//				
//			}
//			
//			
//			updateCraft();
//		}
//		
//		void updateCraft(){
//			
//			
//			foreach(Vessel thisCraft in bioCrafts){
//				
//				foreach (var thisNode in saveGameNode.GetNode(thisCraft.id.ToString()).values.Cast<ConfigNode.Value>()){
//					
//					new bioMsg("Updating craft..." + bioCrafts.Count);
//					
//					double updateTime = Planetarium.GetUniversalTime() - double.Parse(thisNode.value);
//					
//					
//					foreach(ProtoPartSnapshot thisPart in thisCraft.protoVessel.protoPartSnapshots){
//						foreach(var thisModule in thisPart.modules){
//							if(thisModule.moduleName == "BioGen"){
//								if(thisModule.moduleValues.GetValue("enabled") == "true"){
//									ConfigNode thisBioPartCfg = loadCFG(thisPart.partName) as ConfigNode;
//									
//									ConfigNode[] bioNodeList = thisBioPartCfg.GetNodes("MODULE");
//									
//									foreach(ConfigNode bioGenNode in bioNodeList){
//										if(bioGenNode.GetValue("name") == "BioGen"){ 
//											
//											foreach (var bioInNode in bioGenNode.GetNode("bioIn").values.Cast<ConfigNode.Value>()){
//												
//												foreach(ProtoPartResourceSnapshot thisSnapShot in thisPart.resources){
//													
//													
//													try{
//														if(thisSnapShot.resourceName != null){
//															if(bioInNode.name == thisSnapShot.resourceName ){
//																
//																double moddedAmt =  double.Parse(bioInNode.value) * updateTime;
//																foreach (var resentry in thisSnapShot.resourceValues.values.Cast<ConfigNode.Value>()){
//																	if(resentry.name == "amount"){
//																		resentry.value = (double.Parse(resentry.value) - moddedAmt).ToString();
//																		if(double.Parse(resentry.value) > double.Parse(thisSnapShot.resourceValues.GetValue("maxAmount"))){
//																			resentry.value = thisSnapShot.resourceValues.GetValue("maxAmount");
//																		}
//																		
//																	}
//																}
//																
//															}
//														}
//													}catch(System.NullReferenceException){
//														Debug.Log("Stupid NullReferenceException updateCraft() BioIns");
//													}
//													
//												}
//												
//												
//												
//											}
//											
//											
//											
//											foreach (var bioOutNode in bioGenNode.GetNode("bioOut").values.Cast<ConfigNode.Value>()){
//												
//												
//												foreach(ProtoPartResourceSnapshot thisSnapShot in thisPart.resources){
//													
//													Debug.Log ("checking resource name: " + thisSnapShot.resourceName + " => " + bioOutNode.name);
//													
//													try{
//														if(thisSnapShot.resourceName != null){
//															if(bioOutNode.name == thisSnapShot.resourceName ){
//																
//																double moddedAmt =  double.Parse(bioOutNode.value) * updateTime;
//																Debug.Log ("Producing " + moddedAmt + " => " + bioOutNode.value);
//																
//																foreach (var resentry in thisSnapShot.resourceValues.values.Cast<ConfigNode.Value>()){
//																	if(resentry.name == "amount"){
//																		
//																		Debug.Log (resentry.name + ": +" + resentry.value);
//																		resentry.value = (double.Parse(resentry.value) + moddedAmt).ToString();
//																		if(double.Parse(resentry.value) <= 0){
//																			resentry.value = "0";
//																		}
//																		
//																		
//																	}
//																}
//																
//															}else{
//																Debug.Log (bioOutNode.name + " Not Found.");
//															}
//														}
//													}catch(System.NullReferenceException){
//														Debug.Log("Stupid NullReferenceException updateCraft() BioOuts");
//													}
//													
//												}
//												
//												
//												
//											}
//											
//										}
//										
//									}	
//								}
//								
//							}
//						}
//						
//					}
//					
//					ConfigNode.ConfigNodeList nodesList = saveGameNode.nodes;
//					if(nodesList.Contains(thisCraft.id.ToString())){
//						nodesList.RemoveNode(thisCraft.id.ToString());
//					}
//					saveGameNode.AddNode(thisCraft.id.ToString()).AddValue("saveTime", Planetarium.GetUniversalTime().ToString());
//					saveGameNode.Save("GameData/BioMass/Plugin/pluginData/bioSave-" + HighLogic.CurrentGame.Title + ".cfg");
//					new bioMsg("saved craft");
//					loadCount++;
//					
//				}	
//				
//			}
//			isLoading = false;
//		}
//		
//		
//		public void saveCraft(string resName, double resAmt){
//			
//			
//			
//			foreach(Vessel thisCraft in bioCrafts){
//				
//				//foreach(Vessel thisVessel in allVessels){
//				thisCraft.Unload();
//				foreach(ProtoPartSnapshot thisPart in thisCraft.protoVessel.protoPartSnapshots){
//					
//					foreach(ProtoPartResourceSnapshot thisSnapShot in thisPart.resources){
//						if(thisSnapShot.resourceName == resName){
//							
//							foreach (var entry in thisSnapShot.resourceValues.values.Cast<ConfigNode.Value>()){
//								if(entry.name == "amount"){
//									
//									new bioMsg("setting: " + resName + " : " + entry.value);
//									entry.value = (double.Parse(entry.value) + resAmt).ToString();
//									new bioMsg("setting: " + resName + " : " + entry.value);
//									
//								}
//							}
//							
//						}									
//						
//					}
//				}
//				
//				//}
//			}
//			
//		}
//		public  Dictionary<string, string> getThisCraftResources(Vessel thisbioCraft){
//			Dictionary<string, string> craftResourcelist = new Dictionary<string, string>();
//			
//			foreach(ProtoPartSnapshot thisPart in thisbioCraft.protoVessel.protoPartSnapshots){
//				foreach(ProtoPartResourceSnapshot thisSnapShot in thisPart.resources){
//					
//					if(!craftResourcelist.ContainsKey(thisSnapShot.resourceName)){
//						
//						craftResourcelist.Add(thisSnapShot.resourceName,thisSnapShot.resourceValues.GetValue("amount"));
//						
//						
//					}
//				}
//				
//			}
//			
//			return(craftResourcelist);
//		}
//		
//		public string BIOTime(){
//			string thisTime = "";
//			double currentTime = Planetarium.GetUniversalTime();
//			double bioSeconds = currentTime/1000;
//			double bioMinutes = bioSeconds/60;
//			double bioHours = bioMinutes/60;
//			double bioDays = bioHours/24;
//			
//			thisTime = "D-" + ((int)bioDays).ToString() + " H-" + ((int)bioHours).ToString() + " M-" + ((int)bioMinutes).ToString() + " S-" + ((int)bioSeconds/60).ToString();
//			return(thisTime);
//			
//		}
//		void makeWin(int winid){
//			
//			if(GUI.Button (new Rect(4,2,12,12), "", bioCloseBtn)){bioWin = false;}
//			GUILayout.Space(16);
//			double currentTime = Planetarium.GetUniversalTime();
//			GUILayout.BeginHorizontal();
//			GUILayout.Label(currentTime.ToString (),bioOutPutStyle);
//			GUILayout.Label ("Vessels " + bioCrafts.Count.ToString() + "", bioLabelStyle);
//			GUILayout.EndHorizontal();
//			GUILayout.Space (12);
//			if(isLoading){
//				GUILayout.Label("Loading, please wait...",  bioLabelStyle);
//			}else{
//				foreach(Vessel thisbioCraft in bioCrafts){
//					Vessel bioCraft = thisbioCraft.protoVessel.vesselRef;
//					
//					string lastSave = "unknown";
//					if(nodesList.Contains(bioCraft.id.ToString())){
//						foreach(var entry in nodesList.GetNode(bioCraft.id.ToString()).values.Cast<ConfigNode.Value>()){
//							lastSave = entry.value;
//						}
//						
//					}
//					//if(craftStamps.ContainsKey(bioCraft.id.ToString())){
//					GUILayout.BeginVertical();
//					
//					//if(bioCraft.FindPartModulesImplementing<BioGen>().Count > 0){
//					List<Vessel.ActiveResource> bioResources = bioCraft.GetActiveResources();
//					
//					GUILayout.BeginHorizontal();
//					GUILayout.Label(bioCraft.vesselName,  bioLabelStyle);
//					GUILayout.Label("@" + bioCraft.mainBody.bodyName, bioLabelStyle);
//					GUILayout.Label("last saved: " + lastSave, bioLabelStyle);
//					
//					GUILayout.EndHorizontal();
//					GUILayout.BeginHorizontal();
//					
//					//					if(GUILayout.Button("Update")){
//					//						updateCraft();
//					//					}
//					GUILayout.BeginVertical();
//					GUILayout.Label(" ", bioOutPutStyle);
//					GUILayout.Label("amt.", bioLabelStyle);
//					//GUILayout.Label("max", bioLabelStyle);
//					GUILayout.EndVertical();
//					
//					//foreach(Vessel.ActiveResource thisActiveResource in bioResources){
//					foreach(KeyValuePair<string,string> thisResVal in getThisCraftResources(bioCraft)){
//						GUILayout.BeginVertical();
//						GUILayout.Label(thisResVal.Key.Substring(0,5), bioOutPutStyle);
//						GUILayout.Label(Math.Round (double.Parse (thisResVal.Value),3).ToString(), bioLabelStyle);
//						//GUILayout.Label(thisActiveResource.maxAmount.ToString(), bioLabelStyle);
//						GUILayout.EndVertical();
//					}
//					GUILayout.EndHorizontal();	
//					//}
//					GUILayout.EndVertical();	
//					//}
//					
//				}
//			}
//			GUI.DragWindow();
//		}
//		
//		
//		
//		
//		public Texture2D loadImage(string imgName){
//			string PathPlugin = string.Format("{0}","BioMass");
//			Texture2D thisImg = new Texture2D(16,16);
//			string PathTextures = string.Format("{0}/Textures", PathPlugin);
//			string FolderPath = PathTextures;
//			string imageFilename = String.Format("{0}/{1}", FolderPath, imgName);
//			if (GameDatabase.Instance.ExistsTexture(imageFilename))
//			{
//				//new bioMsg("Load Texture: " + imageFilename);
//				thisImg = GameDatabase.Instance.GetTexture(imageFilename, false);
//				
//			}
//			else
//			{
//				new bioMsg("image not found" + imageFilename);
//			}
//			return(thisImg);
//		}
//		
//	}
//END BIOMANAGER


	/////////////////////////////////////////////////////
	/// ///////////////////////////////////////////////////////
	/// DEPRICATED BIOGEN PROCESS
	/// 
	/// 
	/// ////////////////////////////////////////////////////////
	//	public class BioGen : PartModule
	//	{
	//		
	//		public ConfigNode config;
	//		public ConfigNode saveGameNode;
	//		public static Dictionary<string, double> bioResourceIns;
	//		public static Dictionary<string,double> bioCraftResources;
	//		public bool isRunning;
	//		public static Dictionary<string, double> bioResourceOuts;
	//		public static List<PartResource> connectedResources;
	//		public double connectedTotal = 0;
	//		public static List<string> bioError;
	//		public bool depleted = false;
	//		public bool maxedOut = false;
	//		public double bioTimeStamp = 0.00;
	//		public bool isWarping = false;
	//
	//		[KSPField] // for setting animation name via cfg file
	//		public string AnimationName;
	//		[KSPField] 
	//		public bool AlwaysActive = false;
	//		[KSPField] 
	//		public bool hasMax = true;
	//		[KSPField] 
	//		public bool RequiresAllInputs =  true;
	//		[KSPField]
	//		public bool smartGen = false;
	//		
	//		public bool hasError = false;
	//		
	//		Animation BioAnimate{ get{ return part.FindModelAnimators(AnimationName)[0]; } }
	//		
	//		[KSPField]
	//		public bool activated;
	//		
	//		[KSPField] //for setting the name of the biological process. (respiration)
	//		public string  bioLabel = "Biological Process";
	//		
	//		[KSPEvent (guiActive = true, active = true)]
	//		public void Activate()
	//		{
	//			
	//			try{
	//				
	//				//bioMsg("Activated");
	//				
	//				BioAnimate[AnimationName].speed = 1;
	//				BioAnimate[AnimationName].wrapMode = WrapMode.ClampForever;
	//				BioAnimate[AnimationName].time = 0;
	//				BioAnimate.Blend(AnimationName);
	//				
	//				activated = true;
	//				Events["Activate"].active = false;
	//				Events["Deactivate"].active = true;
	//				//Events["Activate"].guiActive = false;
	//			}catch(System.IndexOutOfRangeException){
	//				activated = true;
	//				Events["Activate"].active = false;
	//				Events["Deactivate"].active = true;
	//			}
	//		}
	//
	//		[KSPEvent(guiActive = true, active = false)]
	//		public void Deactivate()
	//		{
	//			try{
	//				
	//				//bioMsg("Deactivated");
	//				BioAnimate[AnimationName].speed = -1;
	//				BioAnimate[AnimationName].time = BioAnimate[AnimationName].length;
	//				BioAnimate.Blend(AnimationName);
	//				//bioLabel = null;
	//				activated = false;
	//				
	//				//Events["Deactivate"].guiActive = true;
	//				Events["Activate"].active = true;
	//				Events["Deactivate"].active = false;
	//			}catch(System.IndexOutOfRangeException){
	//				activated = false;
	//				Events["Activate"].active = true;
	//				Events["Deactivate"].active = false;
	//			}
	//		}
	//		
	//		public override void OnStart(PartModule.StartState state){
	//			
	//			Events["Activate"].guiName = "Activate " + bioLabel;
	//			Events["Deactivate"].guiName = "Deactivate " + bioLabel;
	//			
	//			
	//			if(AlwaysActive){
	//				Events["Activate"].guiActive = false;
	//				Events["Deactivate"].guiActive = false;
	//				
	//			}
	//			bioResourceIns = new Dictionary<string, double>();
	//			bioResourceOuts = new Dictionary<string, double>();
	//			bioCraftResources = new Dictionary<string, double>();
	//			connectedResources = new List<PartResource>();
	//			//loadBioResources();
	//			bioError = new List<string>();
	//			loadConfig ();
	//		}
	//
	//		public IEnumerator errOutPut(){
	//			List<string> tempErrList = new List<string>();
	//			tempErrList = bioError;
	//			craftManager.consoleText = "";
	//			foreach(string thisErr in tempErrList){
	//				craftManager.consoleText += "<color=red>" + thisErr + "</color> \n";
	//				
	//			}
	//			yield return new WaitForSeconds(0);		
	//			tempErrList.Clear ();
	//		}
	//		
	//		
	//		
	//		public double GetResourceTotal(string resourceKey){
	//			double rTotal = 0;
	//			connectedResources = new List<PartResource>();
	//			connectedResources = GetConnectedResources(this.part, resourceKey);
	//			foreach(PartResource thisResource in connectedResources){
	//				rTotal += thisResource.amount;
	//			}
	//			
	//			return(rTotal);
	//		}
	//		public double GetResourceMaxTotal(string resourceKey){
	//			double rTotal = 0;
	//			connectedResources = new List<PartResource>();
	//			connectedResources = GetConnectedResources(this.part, resourceKey);
	//			foreach(PartResource thisResource in connectedResources){
	//				rTotal += thisResource.maxAmount;
	//			}
	//			
	//			return(rTotal);
	//		}
	//
	//
	//		public bool resourceInsCheck(){
	//			bool thisBool = false;
	//			depleted = false;
	//
	//			loadBioResources();
	//			foreach(var bioResourceIn in bioResourceIns){
	//				//check if the resource is available in connected parts	
	//				try{
	//					GetConnectedResources(this.part, bioResourceIn.Key);
	//				}
	//				catch(NullReferenceException){
	//					if(!AlwaysActive){
	//						if(!bioError.Contains("Resource not available: " + bioResourceIn.Key +  " Check connected parts. "))
	//							bioError.Add("Resource not available: " + bioResourceIn.Key +  " Check connected parts. ");
	//						
	//						Debug.LogWarning("[BioMass] Resource not available: " + bioResourceIn.Key +  " Check connected parts. ");
	//						if(!smartGen){
	//							Deactivate();
	//						}
	//					}
	//				}
	//				
	//				
	//				if(GetConnectedResources(this.part, bioResourceIn.Key).Count > 0){
	//					
	//					
	//					if( GetResourceTotal(bioResourceIn.Key) -  (bioResourceIn.Value * TimeWarp.fixedDeltaTime) < 0 ){
	//						//Debug.Log(GetResourceTotal(bioResourceIn.Key) + " --- " + bioResourceIn.Value + " = " + (GetResourceTotal(bioResourceIn.Key) - bioResourceIn.Value).ToString() );
	//
	//						if(!AlwaysActive || RequiresAllInputs){
	//
	//							
	//							if(!bioError.Contains("Check " + bioResourceIn.Key +  " levels."))
	//								bioError.Add("Check " + bioResourceIn.Key +  " levels.");
	//							
	//							hasError = true;
	//							depleted = true;
	//							if(!smartGen){
	//								Deactivate();
	//							}
	//							//}
	//							//else{
	//							
	//							//}
	//						}
	//						else if(AlwaysActive && !RequiresAllInputs){
	//							//Deactivate();
	//							hasError = false;
	//						}
	//						
	//					}
	//				}
	//				
	//			}
	//			if(!depleted)
	//				thisBool = true;
	//			return(thisBool);
	//		}//END resource ins check
	//
	//
	//		public bool resourceOutCheck(){
	//			bool thisBool = false;
	//			maxedOut = false;
	//			loadBioResources();
	//			foreach( var bioResourceOut in bioResourceOuts){
	//				//get all matching resources in connected parts
	//				try{
	//					GetConnectedResources(this.part, bioResourceOut.Key);
	//
	//				}
	//				catch(NullReferenceException){
	//					if(!bioError.Contains("Resource not available: " + bioResourceOut.Key +  " Check connected parts. "))
	//						bioError.Add("Resource not available: " +  bioResourceOut.Key  +  " Check connected parts. ");
	//					
	//					if(!AlwaysActive){
	//						if(!smartGen){
	//							Deactivate();
	//						}
	//					}
	//				}
	//				foreach(PartResource outResource in GetConnectedResources(this.part,bioResourceOut.Key)){
	//					//if resource we are currently processing == the resource container we are looking at
	//					
	//					if(bioResourceOut.Key == outResource.resourceName){
	//						
	//						//check if the container can hold what we are about to add..	
	//						
	//						//total of what we are adding and what we have
	//						
	//						double checkAmt = (bioResourceOut.Value * TimeWarp.fixedDeltaTime)  + GetResourceTotal(bioResourceOut.Key);
	//						
	//						if(checkAmt < GetResourceMaxTotal(bioResourceOut.Key) || !hasMax){
	//							//get each resource we are about to burn..
	//							if(bioResourceOut.Key == "Light" ){
	//								
	//								outResource.amount = (double)Mathf.Clamp((float)outResource.amount  + (float)(bioResourceOut.Value * TimeWarp.fixedDeltaTime), 0f,(float)outResource.maxAmount);
	//								//outResource.amount = outResource.maxAmount;
	//								
	//							}
	//							maxedOut = false;
	//							
	//							
	//						}
	//						else{
	//							outResource.amount = outResource.maxAmount;
	//							if(!AlwaysActive || hasMax){
	//								if(!bioError.Contains(bioResourceOut.Key +  " @ Maximum capacity."))
	//									bioError.Add(bioResourceOut.Key +  " @ Maximum capacity.");
	//								hasError = true;
	//								maxedOut = true;
	//								if(!smartGen){
	//									Deactivate();
	//								}
	//							}
	//							
	//							
	//						}
	//						
	//						
	//					}// IF bioResourceOut.Key == outResource.resourceName
	//					
	//				}
	//				
	//				
	//			}
	//			if(!maxedOut)
	//				thisBool = true;
	//			return(thisBool);
	//		}//END resource out capacityCheck
	//
	//		
	//		public void FixedUpdate(){
	////			//check our timewarp speed, if higher than X pause bioGen make timeStamp
	////			new bioMsg("TIME SCALE: " + Planetarium.TimeScale);
	////			new bioMsg ("TimeWarTimeWarp.MaxPhysicsRate = " + TimeWarp.MaxPhysicsRate + "AngVel = " + this.vessel.rigidbody.velocity.magnitude );
	////
	////			if(TimeWarp.CurrentRate >= 50){ 
	////				new bioMsg("TransWarp Resource Protection Provided by STRAFE. ");
	////				if(bioTimeStamp <= 0 && !isWarping)
	////					bioTimeStamp = Planetarium.GetUniversalTime();
	////				isWarping = true;
	////				return;
	////
	////			}
	////			else{
	////				if(bioTimeStamp > 0 && isWarping){
	////					isWarping = false;
	////					new bioMsg("Thank you for using STRAFE. Provide by the Society for The Regulation of Anit-Proton and Farad Exploitation");
	////
	////					//pass the timestamp value to the resource level updater, then set bioTimeStamp back to 0
	////					//subtract the time stamp from current time, and multiply by the rae of resource processing
	////					//apply new values to craft, and resume generator
	////
	////
	////				}
	////			}
	//
	//
	//			//UPDATE RESOURCES AMOUNTS
	//			if(FlightGlobals.ready){
	//				if(AlwaysActive){activated = true;}
	//				if(activated){
	//					//loadBioResources();
	//					if(resourceInsCheck() && resourceOutCheck()){ 
	//
	//						//PROCESS RESOURCE INS
	//						foreach(var bioIn in bioResourceIns){
	//
	//							//part.RequestResource(PartResourceLibrary.Instance.GetDefinition(bioIn.Key).id, (bioIn.Value * TimeWarp.fixedDeltaTime));
	//							int i = 0;
	//							//bool complete = false;
	//							foreach(PartResource inResource in GetConnectedResources(this.part,bioIn.Key)){
	//
	//								if(i < GetConnectedResources(this.part,bioIn.Key).Count){
	//									if( inResource.amount > bioIn.Value){
	//
	//										if(bioIn.Key != "Light"){
	//											//Debug.Log ("in amount BEFORE -- " + inResource.amount); 
	//											inResource.amount = inResource.amount - (bioIn.Value * TimeWarp.fixedDeltaTime);
	//											//Debug.Log ("in amount AFTER -- " + inResource.amount);
	//											//break;
	//											//complete = true;
	//										}else{
	//											inResource.amount = inResource.amount - (bioIn.Value);
	//										}
	//
	//									}else{
	//										inResource.amount = 0;
	//									}
	//									i++;
	//							
	//								}
	//							}
	//						}
	//
	//						//PROCESS RESOURCE OUTS
	//						foreach( var bioResourceOut in bioResourceOuts){
	//															
	//							//part.RequestResource(PartResourceLibrary.Instance.GetDefinition(bioResourceOut.Key).id, -(bioResourceOut.Value * TimeWarp.fixedDeltaTime));
	//							int j = 0;
	//							//bool complete = false;
	//							foreach(PartResource outResource in GetConnectedResources(this.part,bioResourceOut.Key)){
	//								if(j < GetConnectedResources(this.part,bioResourceOut.Key).Count){
	//									if(outResource.amount < outResource.maxAmount){
	//									
	//										outResource.amount = outResource.amount + (bioResourceOut.Value * TimeWarp.fixedDeltaTime );	
	//										break;
	//									}
	//								}else{
	//									outResource.amount = outResource.maxAmount;
	//								}
	//								j++;
	//							}
	//						}//END process resource outs
	//					}//END if resource checks completed
	//				}//END if activated	
	//				if(bioError.Count > 0){
	//					StartCoroutine(errOutPut());
	//				}else{
	//					craftManager.consoleText = "<color=cyan>No errors reporting.</color> \n";
	//					StopCoroutine(errOutPut());
	//					return;
	//				}
	//				
	//			}//END if flight globals ready 
	//		}//END fixed update
	//
	//		public void loadConfig(){
	//			new bioMsg ("loading part config...");
	//			foreach (ConfigNode entry in config.nodes) {
	//				
	//				new bioMsg(config.nodes.GetEnumerator().ToString() + "\t" + entry.name);	
	//			}
	//
	//			new bioMsg ("done");
	//		}
	//		
	//		public void loadBioResources(){
	//			
	//			bioResourceIns.Clear();
	//			int i = 0;
	//
	////		try{
	////				
	////				foreach(ConfigNode thisEntry in this.config.GetNodes()){
	////					if(!bioError.Contains("Node: " + thisEntry.GetValues()[0] +  thisEntry.GetNodes()[0]))
	////						bioError.Add("Node: " + thisEntry.GetValues()[0] +  thisEntry.GetNodes()[0]);
	////				}
	////			}catch(NullReferenceException){
	////				if(!bioError.Contains("unable to locate resource input(s)"))
	////					bioError.Add("unable to locate resource input(s)");
	////			}
	//
	//			try{
	//				foreach (var entry in this.config.GetNode("bioIn").values.Cast<ConfigNode.Value>()){
	//					bioResourceIns.Add(entry.name, double.Parse(entry.value));
	//					i++;
	//					}
	//								
	//			}
	//			catch(NullReferenceException){
	//				if(!bioError.Contains("Resource Input(s) " + i + " Not Found!"))
	//					bioError.Add("Resource Input(s) " + i + " Not Found!");
	//			}
	//
	//			bioResourceOuts.Clear();
	//			try{
	//				foreach (var entry in this.config.GetNode("bioOut").values.Cast<ConfigNode.Value>()){
	//					
	//					bioResourceOuts.Add(entry.name, double.Parse(entry.value));
	//				}
	//			}catch(NullReferenceException){
	//				if(!bioError.Contains("Resource Output(s) " + i + " Not Found!"))
	//					bioError.Add("Resource Output(s) " + i + " Not Found!");
	//			}
	//			
	//			foreach( var bioResourceOut in bioResourceOuts){
	//				//get all matching resources in connected parts
	//				
	//				foreach(PartResource outResource in GetConnectedResources(this.part,bioResourceOut.Key)){
	//					//if resource we are currently processing == the resource container we are looking at
	//					if(!bioCraftResources.ContainsKey (bioResourceOut.Key)){
	//						bioCraftResources.Add(outResource.resourceName,outResource.amount);
	//					}
	//				}
	//			}
	//			
	//			foreach(var bioResourceIn in bioResourceIns){	
	//				
	//				foreach(PartResource InResource in GetConnectedResources(this.part,bioResourceIn.Key)){
	//					//if resource we are currently processing == the resource container we are looking at
	//					if(!bioCraftResources.ContainsKey (bioResourceIn.Key)){
	//						bioCraftResources.Add(InResource.resourceName,InResource.amount);
	//					}
	//				}
	//			}
	//			
	//		}//END loadBioResources
	//		
	//		public override void OnLoad(ConfigNode config)
	//		{
	//
	//			if (this.config == null)
	//			{
	//				this.config = new ConfigNode();
	//				config.CopyTo(this.config);
	//
	//				}
	//		}//END OnLoad
	//		
	//		
	//		public static List<PartResource> GetConnectedResources(Part part, String resourceName)
	//		{
	//			var resourceDef = PartResourceLibrary.Instance.GetDefinition(resourceName);
	//			var resources = new List<PartResource>();
	//			part.GetConnectedResources(resourceDef.id, resourceDef.resourceFlowMode, resources);
	//			return resources;
	//		}
	//		
	//		public Texture2D loadImage(string bioImage){
	//			string PathPlugin = string.Format("{0}","BioMass");
	//			Texture2D btnImg = new Texture2D(16,16);
	//			btnImg.filterMode = FilterMode.Point;
	//			string PathTextures = string.Format("{0}/Textures", PathPlugin);
	//			string FolderPath = PathTextures;
	//			string imageFilename = String.Format("{0}/{1}", FolderPath, bioImage);
	//			if (GameDatabase.Instance.ExistsTexture(imageFilename))
	//			{
	//				btnImg = GameDatabase.Instance.GetTexture(imageFilename, false);
	//			}
	//			else
	//			{
	//				new bioMsg("image not found" + imageFilename);
	//			}
	//			return(btnImg);
	//		}//END loadImage
	//	
	//		
	//	}//END BioGen class





	public class bioMsg : MonoBehaviour{
		string thisString;
		//ScreenMessage bioScreenMessage;
		//static ScreenMessage thisScreenMsg;
		//ScreenMessage ScreenMessages.PostScreenMessage();
		
		
		
		public bioMsg(string msg){
			thisString = msg;

			Debug.LogWarning("[BioMass]: " + msg);
			ScreenMessages.PostScreenMessage(thisString, 3.0f, ScreenMessageStyle.UPPER_CENTER);
			
		}
		
		
		
	}
	
	[KSPAddon(KSPAddon.Startup.MainMenu, true)]
	public class bioMassLoadScreen : MonoBehaviour{
		public Texture bioLogo;
		public GameObject logoObj;
		public Color visibleColor;
		public Color invisibleColor;
		
		public bool showLogo;
		
		void Start(){
			visibleColor = Color.white;
			invisibleColor = Color.clear;
			bioLogo = new Texture();
			string PathPlugin = string.Format("{0}","BioMass");
			string PathTextures = string.Format("{0}/Textures", PathPlugin);
			
			string imageFilename = String.Format("{0}/{1}", PathTextures, "logo-1");
			if (GameDatabase.Instance.ExistsTexture(imageFilename))
			{
				//new bioMsg("Load Texture: " + imageFilename);
				bioLogo = GameDatabase.Instance.GetTexture(imageFilename, false);
				
			}
			else
			{
				print("image not found" + imageFilename);
			}
			showLogo = false;
			logoObj = new GameObject();
			logoObj.AddComponent<GUITexture>().texture = bioLogo;
			logoObj.GetComponent<GUITexture>().guiTexture.color = new Color(logoObj.GetComponent<GUITexture>().guiTexture.color.r,logoObj.GetComponent<GUITexture>().guiTexture.color.g, logoObj.GetComponent<GUITexture>().guiTexture.color.b, 0.0f);
			logoObj.transform.localPosition = new Vector3(0.495f,0.6f,1.0f);
			logoObj.transform.localScale = new Vector3(0.12f, 0.1f, 0.1f);
			//logoObj.transform.localRotation = new Quaternion(-30,0,0,0);
			StartCoroutine(logoTimerIn());
		}
		void FixedUpdate(){
			
			if(Input.GetMouseButtonDown(0)){
				StopCoroutine(logoTimerIn());
				StartCoroutine(logoTimerOut());
			}
		}
		
		public IEnumerator logoTimerIn(){
			
			Color newColor = logoObj.GetComponent<GUITexture>().color;
			
			for (int i=0; newColor.a < 0.4; i++) {
				
				newColor.a = Mathf.MoveTowards (newColor.a, 0.4f, 0.02f);
				yield return new WaitForSeconds(0.003f);
				logoObj.GetComponent<GUITexture>().color = newColor;
				if(newColor.a >= 0.4){
					StopCoroutine(logoTimerIn());
				}
			}
			
			
		}
		
		public IEnumerator logoTimerOut(){
			
			Color newColor = logoObj.GetComponent<GUITexture>().color;
			for (int i=0; newColor.a > 0; i++) {
				
				newColor.a = Mathf.MoveTowards (newColor.a, 0.0f, 0.05f);
				yield return new WaitForSeconds(0.003f);
				logoObj.GetComponent<GUITexture>().color = newColor;
				if(newColor.a <= 0 ){
					
					StopCoroutine(logoTimerOut());
					
					//Destroy(logoObj);
				}
			}
			
		}
		
	}
	
}
