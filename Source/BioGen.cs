using System;
using System.Collections.Generic;
using System.Collections;
using KSP;
using UnityEngine;
using System.Text;

namespace BioMass
{

//For producing Light resource and scaling plant models based on the rate of biomass growth

	public class GreenHouse : ModuleDeployableSolarPanel{

		[KSPField]
		public float plantMaxScale = 1.0f; //the max scale you want the plants to grow visually. This will not stop the growth of biomas, only the visual size of the model
		[KSPField]
		public string cropType = "Kamboo"; 
		[KSPField]
		public string plantMeshName = "plant"; //this is the transform.name given to the plant prefabs in unity.
		[KSPField]
		public bool growPlants = false;
		[KSPField]
		public string startingScaleString = "0.04,0.04,0.05";
		[KSPField]
		public string lastBioMassAmtString = "0.00";

	
		//public ConfigNode config;
		private Vector3 currentPlantScale;
		private Transform[] plants;
		private double lastBioMassAmt = 0.00;
		private double growthValue;

		public bool mustHarvest;

		public override void OnStart(PartModule.StartState state){
			
			if(growPlants) {
				try{
					lastBioMassAmt = double.Parse (lastBioMassAmtString);
				}catch(ArgumentNullException){
					lastBioMassAmt = 0.00;
				}
				plants = getPlants ();
				setStartingScale ();
			}
			base.OnStart (state);

		}

		//override the extension of panels, check for biomass and step in the way...
		public override void OnInitialize ()
		{
			if ( lastBioMassAmt > 0 ) {
				this.retractable = false;
				craftManager.mustHarvest = true;
			} else {
				this.retractable = true;
			}
			base.OnInitialize ();
		}
			

		public override void OnFixedUpdate(){
			
			//TODO:stop retraction when plants are grown and prompt for harvest.

			if (growPlants) {
				try {
					if (plants.Length > 0) {
						growthValue = getChangeInBioMass ();

						if (growthValue > 0.00 || growthValue < 0.00)
							scalePlants ();
					}

				} catch (NullReferenceException) {
					craftManager.consoleMsgs.Add ("There are no plants to grow.");
				}

			}

			base.OnFixedUpdate ();
		}//END ONFIXEDUPDATE

		public override void OnSave(ConfigNode config){
			//save the current scale to the startingscale for next load
			config.AddValue ("startingScaleString", startingScaleString);
			config.AddValue ("lastBioMassAmt", lastBioMassAmt.ToString());
			base.OnSave (config);
		}

		public override void OnLoad(ConfigNode config){
			//pull the scale from save file
			startingScaleString = config.GetValue ("startingScaleString");
			lastBioMassAmtString = config.GetValue ("lastBioMassAmt");
			//set the current scale
			currentPlantScale = plantScale(config.GetValue("startingScaleString"));
			Debug.Log("[BioMass] - Previous plant growth loaded.");
			//No. This does not update the scale based on growth while the ship was loaded.
			//It will load the last scale that they were when saved.
			//While yes, this is pretty easy to calculate and put in place, I have decided not too for the moment.
			//Mostly becuase the scaling stops well before biomass is full anyway.
			//maybe in a later update...
			base.OnLoad (config);
		}
		public Vector3 plantScale(string V3String){
			
			var Vect3Pos = V3String.Split (',');
			var tempV3 = new Vector3 ( float.Parse(Vect3Pos[0]) ,float.Parse(Vect3Pos[1]),float.Parse(Vect3Pos[2]));
			return(tempV3);
		}
			
		public void setStartingScale(){
			try{
				var TempScale = plantScale(startingScaleString);
				foreach (Transform plant in plants) {
						plant.localScale = TempScale;
						currentPlantScale = TempScale;
				}
			}
			catch(NullReferenceException){
				Debug.Log("[BioMass] - Setting startign scale of plants failed. NullRef");
			}
		}
		public void scalePlants(){
			
			foreach(Transform plant in plants){
				if (plant.localScale.x > 0f && plant.localScale.x < plantMaxScale)
					
					plant.localScale = new Vector3 (plant.localScale.x + (float)growthValue, plant.localScale.y + (float)growthValue, plant.localScale.z + (float)growthValue );
					currentPlantScale = plant.localScale;
					startingScaleString = currentPlantScale.x + "," + currentPlantScale.y + "," + currentPlantScale.z;
				
			}
		}

		//get the change in total BioMass
		public double getChangeInBioMass (){
			double thisGrowthValue = ((GetResourceTotal ("BioMass")/plants.Length) - (lastBioMassAmt/plants.Length));
			lastBioMassAmt = (float)(GetResourceTotal ("BioMass"));
			return(thisGrowthValue);
		}

		//get all plants in greenhouse
		public Transform[] getPlants(){
			
			var tempTransformList = new List<Transform> (); 
			foreach (Transform entry in this.gameObject.GetComponentsInChildren<Transform> ()) {
				if (entry.name == plantMeshName)
					tempTransformList.Add (entry);

				}
			return(tempTransformList.ToArray ());
		}//END GETPLANTS

		public double GetResourceTotal(string resourceKey){
			double rTotal = 0;
			var connectedResources = new List<PartResource>();
			connectedResources = GetConnectedResources(this.part, resourceKey);
			foreach(PartResource thisResource in connectedResources){
				rTotal += thisResource.amount;
			}

			return(rTotal);
		}// GETRESOURCETOTAL

		public static List<PartResource> GetConnectedResources(Part part, String resourceName)
		{
			var resourceDef = PartResourceLibrary.Instance.GetDefinition(resourceName);
			var resources = new List<PartResource>();
			part.GetConnectedResources(resourceDef.id, resourceDef.resourceFlowMode, resources);
			return resources;
		}//GetConnectedResources


	}// END PlantGrowth

//ModuleResourceConverter for Bilogical Processes such as photosynthesis
//Includes error reporting to the biomonitor 
public class BiologicalProcess : ModuleResourceConverter{

		public Dictionary<int,string> bioSysMsgs;
	

		public override void OnStart(PartModule.StartState state){

			bioSysMsgs = new Dictionary<int,string> ();
			base.OnStart (state);
		}
		/// <summary>
		/// outputs the status of the part to the console.
		/// </summary>
		public override void OnFixedUpdate(){
			
			craftManager.consoleMsgs.Add(ConverterName + " " + status);
			base.OnFixedUpdate ();

		}

	}

/// <summary>
/// Generates the light resource based on ModuleDeployableSolarPanel
/// </summary>
public class NaturalLightSource: ModuleDeployableSolarPanel{
		public override void OnStart(PartModule.StartState state){

			base.OnStart (state);
		}
		public override void OnFixedUpdate(){
			base.OnFixedUpdate ();

		}

	}

/// <summary>
/// Adds animation functionality to the ModuleResourceConverter for parts that require an animation as well as converter functionality
/// </summary>
public class AnimatedGenerator : ModuleResourceConverter{
 
	public bool isPlaying = false;
	/// The name of the animation.
	[KSPField] // for setting animation name via cfg file
	public string AnimationName;
	/// Gets the bio animate.
	Animation BioAnimate{ get{ return part.FindModelAnimators(AnimationName)[0]; } }
	
	public override void OnStart(PartModule.StartState state){

			base.OnStart (state);
		}
	
	public override void OnFixedUpdate(){

			if (IsActivated && !isPlaying) {
				new bioMsg ("Artificial Lights Activated");
				isPlaying = true;
				try{

					BioAnimate[AnimationName].speed = 1;
					BioAnimate[AnimationName].wrapMode = WrapMode.ClampForever;
					BioAnimate[AnimationName].time = 0;
					BioAnimate.Blend(AnimationName);

				}catch(System.IndexOutOfRangeException){

				}
			}
			if (!IsActivated && isPlaying) {
				new bioMsg ("Artificial Lights Deactivated");
				isPlaying = !isPlaying;
				try{

					//bioMsg("Deactivated");
					BioAnimate[AnimationName].speed = -1;
					BioAnimate[AnimationName].time = BioAnimate[AnimationName].length;
					BioAnimate.Blend(AnimationName);

				}catch(System.IndexOutOfRangeException){
			
				}

			}
			base.OnFixedUpdate ();
		}


}//END AnimatedGenerator

	public class bioMsg : MonoBehaviour{
		public string thisString;
	
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
			
}//END BioMass namespace