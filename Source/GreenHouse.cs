using System;
using System.Collections.Generic;
using UnityEngine;
using KSP;

namespace BioMass
{

	//Greenhouse PartModule based on ModuleDeployableSolarPanel.
	//Detct sunlight and produces the Light resource.
	//Also used for scaling plant models based on the rate of biomass growth. 
	public class GreenHouse : ModuleDeployableSolarPanel{

		[KSPField]
		public float plantMaxScale = 1.0f; //the max scale you want the plants to grow visually. This will not stop the growth of biomas, only the visual size of the model
		[KSPField]
		public string cropType = "Kamboo"; 
		[KSPField]
		public string plantMeshName = "plant"; //this is the transform.name given to the plant prefabs in unity.
		[KSPField]
		public bool growPlants = false;  //should the greenhouse try to scale plants
		[KSPField]
		public string startingScaleString = "0.04,0.04,0.05";  //for setting the starting scale of plants if they need adjusting
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
				//craftManager.mustHarvest = true;
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
			if (growPlants) {
				config.AddValue ("startingScaleString", startingScaleString);
				config.AddValue ("lastBioMassAmt", lastBioMassAmt.ToString ());
			}
			base.OnSave (config);
		}

		public override void OnLoad(ConfigNode config){
			//pull the scale from save file
			if(growPlants){
				startingScaleString = config.GetValue ("startingScaleString");
				lastBioMassAmtString = config.GetValue ("lastBioMassAmt");
				//set the current scale
				currentPlantScale = plantScale(config.GetValue("startingScaleString"));
				Debug.Log("[BioMass] - Previous plant growth loaded.");
				//No. This does not update the scale based on growth while the ship was not in focus.
				//It will load the last scale that they were when saved.
				//While yes, this is pretty easy to calculate and put in place, I have decided not too for the moment.
				//Mostly becuase the scaling stops well before biomass is full anyway.
				//maybe in a later update...
			}
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
				Debug.Log("[BioMass] - Setting starting scale of plants failed. NullRef");
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

}
