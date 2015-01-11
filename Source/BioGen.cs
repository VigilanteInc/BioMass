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

	public class BioGen : PartModule
	{
		
		public ConfigNode config;
		public ConfigNode saveGameNode;
		public static Dictionary<string, double> bioResourceIns;
		public static Dictionary<string,double> bioCraftResources;
		public bool isRunning;
		public static Dictionary<string, double> bioResourceOuts;
		public static List<PartResource> connectedResources;
		public double connectedTotal = 0;
		public static List<string> bioError;
		public bool depleted = false;
		public bool maxedOut = false;
		public double bioTimeStamp = 0.00;
		public bool isWarping = false;

		[KSPField] // for setting animation name via cfg file
		public string AnimationName;
		[KSPField] 
		public bool AlwaysActive = false;
		[KSPField] 
		public bool hasMax = true;
		[KSPField] 
		public bool RequiresAllInputs =  true;
		[KSPField]
		public bool smartGen = false;
		
		public bool hasError = false;
		
		Animation BioAnimate{ get{ return part.FindModelAnimators(AnimationName)[0]; } }
		
		[KSPField]
		public bool activated;
		
		[KSPField] //for setting the name of the biological process. (respiration)
		public string  bioLabel = "Biological Process";
		
		[KSPEvent (guiActive = true, active = true)]
		public void Activate()
		{
			
			try{
				
				//bioMsg("Activated");
				
				BioAnimate[AnimationName].speed = 1;
				BioAnimate[AnimationName].wrapMode = WrapMode.ClampForever;
				BioAnimate[AnimationName].time = 0;
				BioAnimate.Blend(AnimationName);
				
				activated = true;
				Events["Activate"].active = false;
				Events["Deactivate"].active = true;
				//Events["Activate"].guiActive = false;
			}catch(System.IndexOutOfRangeException){
				activated = true;
				Events["Activate"].active = false;
				Events["Deactivate"].active = true;
			}
		}

		[KSPEvent(guiActive = true, active = false)]
		public void Deactivate()
		{
			try{
				
				//bioMsg("Deactivated");
				BioAnimate[AnimationName].speed = -1;
				BioAnimate[AnimationName].time = BioAnimate[AnimationName].length;
				BioAnimate.Blend(AnimationName);
				//bioLabel = null;
				activated = false;
				
				//Events["Deactivate"].guiActive = true;
				Events["Activate"].active = true;
				Events["Deactivate"].active = false;
			}catch(System.IndexOutOfRangeException){
				activated = false;
				Events["Activate"].active = true;
				Events["Deactivate"].active = false;
			}
		}
		
		public override void OnStart(PartModule.StartState state){
			
			Events["Activate"].guiName = "Activate " + bioLabel;
			Events["Deactivate"].guiName = "Deactivate " + bioLabel;
			
			
			if(AlwaysActive){
				Events["Activate"].guiActive = false;
				Events["Deactivate"].guiActive = false;
				
			}
			bioResourceIns = new Dictionary<string, double>();
			bioResourceOuts = new Dictionary<string, double>();
			bioCraftResources = new Dictionary<string, double>();
			connectedResources = new List<PartResource>();
			//loadBioResources();
			bioError = new List<string>();
			
		}

		public IEnumerator errOutPut(){
			List<string> tempErrList = new List<string>();
			tempErrList = bioError;
			craftManager.consoleText = "";
			foreach(string thisErr in tempErrList){
				craftManager.consoleText += "<color=red>" + thisErr + "</color> \n";
				
			}
			yield return new WaitForSeconds(0);		
			tempErrList.Clear ();
		}
		
		
		
		public double GetResourceTotal(string resourceKey){
			double rTotal = 0;
			connectedResources = new List<PartResource>();
			connectedResources = GetConnectedResources(this.part, resourceKey);
			foreach(PartResource thisResource in connectedResources){
				rTotal += thisResource.amount;
			}
			
			return(rTotal);
		}
		public double GetResourceMaxTotal(string resourceKey){
			double rTotal = 0;
			connectedResources = new List<PartResource>();
			connectedResources = GetConnectedResources(this.part, resourceKey);
			foreach(PartResource thisResource in connectedResources){
				rTotal += thisResource.maxAmount;
			}
			
			return(rTotal);
		}


		public bool resourceInsCheck(){
			bool thisBool = false;
			depleted = false;
			loadBioResources();
			foreach(var bioResourceIn in bioResourceIns){
				//check if the resource is available in connected parts	
				try{
					GetConnectedResources(this.part, bioResourceIn.Key);
				}
				catch(NullReferenceException){
					if(!AlwaysActive){
						if(!bioError.Contains("Resource not available: " + bioResourceIn.Key +  " Check connected parts. "))
							bioError.Add("Resource not available: " + bioResourceIn.Key +  " Check connected parts. ");
						
						Debug.LogWarning("[BioMass] Resource not available: " + bioResourceIn.Key +  " Check connected parts. ");
						if(!smartGen){
							Deactivate();
						}
					}
				}
				
				
				if(GetConnectedResources(this.part, bioResourceIn.Key).Count > 0){
					
					
					if( GetResourceTotal(bioResourceIn.Key) -  (bioResourceIn.Value * TimeWarp.fixedDeltaTime) < 0 ){
						//Debug.Log(GetResourceTotal(bioResourceIn.Key) + " --- " + bioResourceIn.Value + " = " + (GetResourceTotal(bioResourceIn.Key) - bioResourceIn.Value).ToString() );

						if(!AlwaysActive || RequiresAllInputs){

							
							if(!bioError.Contains("Check " + bioResourceIn.Key +  " levels."))
								bioError.Add("Check " + bioResourceIn.Key +  " levels.");
							
							hasError = true;
							depleted = true;
							if(!smartGen){
								Deactivate();
							}
							//}
							//else{
							
							//}
						}
						else if(AlwaysActive && !RequiresAllInputs){
							//Deactivate();
							hasError = false;
						}
						
					}
				}
				
			}
			if(!depleted)
				thisBool = true;
			return(thisBool);
		}//END resource ins check


		public bool resourceOutCheck(){
			bool thisBool = false;
			maxedOut = false;
			loadBioResources();
			foreach( var bioResourceOut in bioResourceOuts){
				//get all matching resources in connected parts
				try{
					GetConnectedResources(this.part, bioResourceOut.Key);

				}
				catch(NullReferenceException){
					if(!bioError.Contains("Resource not available: " + bioResourceOut.Key +  " Check connected parts. "))
						bioError.Add("Resource not available: " +  bioResourceOut.Key  +  " Check connected parts. ");
					
					if(!AlwaysActive){
						if(!smartGen){
							Deactivate();
						}
					}
				}
				foreach(PartResource outResource in GetConnectedResources(this.part,bioResourceOut.Key)){
					//if resource we are currently processing == the resource container we are looking at
					
					if(bioResourceOut.Key == outResource.resourceName){
						
						//check if the container can hold what we are about to add..	
						
						//total of what we are adding and what we have
						
						double checkAmt = (bioResourceOut.Value * TimeWarp.fixedDeltaTime)  + GetResourceTotal(bioResourceOut.Key);
						
						if(checkAmt < GetResourceMaxTotal(bioResourceOut.Key) || !hasMax){
							//get each resource we are about to burn..
							if(bioResourceOut.Key == "Light" ){
								
								outResource.amount = (double)Mathf.Clamp((float)outResource.amount  + (float)(bioResourceOut.Value * TimeWarp.fixedDeltaTime), 0f,(float)outResource.maxAmount);
								//outResource.amount = outResource.maxAmount;
								
							}
							maxedOut = false;
							
							
						}
						else{
							outResource.amount = outResource.maxAmount;
							if(!AlwaysActive || hasMax){
								if(!bioError.Contains(bioResourceOut.Key +  " @ Maximum capacity."))
									bioError.Add(bioResourceOut.Key +  " @ Maximum capacity.");
								hasError = true;
								maxedOut = true;
								if(!smartGen){
									Deactivate();
								}
							}
							
							
						}
						
						
					}// IF bioResourceOut.Key == outResource.resourceName
					
				}
				
				
			}
			if(!maxedOut)
				thisBool = true;
			return(thisBool);
		}//END resource out capacityCheck

		
		public void FixedUpdate(){
//			//check our timewarp speed, if higher than X pause bioGen make timeStamp
//			new bioMsg("TIME SCALE: " + Planetarium.TimeScale);
//			new bioMsg ("TimeWarTimeWarp.MaxPhysicsRate = " + TimeWarp.MaxPhysicsRate + "AngVel = " + this.vessel.rigidbody.velocity.magnitude );
//
//			if(TimeWarp.CurrentRate >= 50){ 
//				new bioMsg("TransWarp Resource Protection Provided by STRAFE. ");
//				if(bioTimeStamp <= 0 && !isWarping)
//					bioTimeStamp = Planetarium.GetUniversalTime();
//				isWarping = true;
//				return;
//
//			}
//			else{
//				if(bioTimeStamp > 0 && isWarping){
//					isWarping = false;
//					new bioMsg("Thank you for using STRAFE. Provide by the Society for The Regulation of Anit-Proton and Farad Exploitation");
//
//					//pass the timestamp value to the resource level updater, then set bioTimeStamp back to 0
//					//subtract the time stamp from current time, and multiply by the rae of resource processing
//					//apply new values to craft, and resume generator
//
//
//				}
//			}


			//UPDATE RESOURCES AMOUNTS
			if(FlightGlobals.ready){
				if(AlwaysActive){activated = true;}
				if(activated){
					//loadBioResources();
					if(resourceInsCheck() && resourceOutCheck()){ //I REVERSED THESE ON 9.17.2014*****************************************

						//PROCESS RESOURCE INS
						foreach(var bioIn in bioResourceIns){

							//part.RequestResource(PartResourceLibrary.Instance.GetDefinition(bioIn.Key).id, (bioIn.Value * TimeWarp.fixedDeltaTime));
							int i = 0;
							//bool complete = false;
							foreach(PartResource inResource in GetConnectedResources(this.part,bioIn.Key)){

								if(i < GetConnectedResources(this.part,bioIn.Key).Count){
									if( inResource.amount > bioIn.Value){

										if(bioIn.Key != "Light"){
											//Debug.Log ("in amount BEFORE -- " + inResource.amount); 
											inResource.amount = inResource.amount - (bioIn.Value * TimeWarp.fixedDeltaTime);
											//Debug.Log ("in amount AFTER -- " + inResource.amount);
											//break;
											//complete = true;
										}else{
											inResource.amount = inResource.amount - (bioIn.Value);
										}

									}else{
										inResource.amount = 0;
									}
									i++;
							
								}
							}
						}

						//PROCESS RESOURCE OUTS
						foreach( var bioResourceOut in bioResourceOuts){
															
							//part.RequestResource(PartResourceLibrary.Instance.GetDefinition(bioResourceOut.Key).id, -(bioResourceOut.Value * TimeWarp.fixedDeltaTime));
							int j = 0;
							//bool complete = false;
							foreach(PartResource outResource in GetConnectedResources(this.part,bioResourceOut.Key)){
								if(j < GetConnectedResources(this.part,bioResourceOut.Key).Count){
									if(outResource.amount < outResource.maxAmount){
									
										outResource.amount = outResource.amount + (bioResourceOut.Value * TimeWarp.fixedDeltaTime );	
										break;
									}
								}else{
									outResource.amount = outResource.maxAmount;
								}
								j++;
							}
						}//END process resource outs
					}//END if resource checks completed
				}//END if activated	
				if(bioError.Count > 0){
					StartCoroutine(errOutPut());
				}else{
					craftManager.consoleText = "<color=cyan>No errors reporting.</color> \n";
					StopCoroutine(errOutPut());
					return;
				}
				
			}//END if flight globals ready 
		}//END fixed update
		
		
		
		public void loadBioResources(){
			bioResourceIns.Clear();
			int i = 0;
			
			foreach (var entry in config.GetNode("bioIn").values.Cast<ConfigNode.Value>()){
				
				bioResourceIns.Add(entry.name, double.Parse(entry.value));
				i++;
				
			}
			
			bioResourceOuts.Clear();
			foreach (var entry in config.GetNode("bioOut").values.Cast<ConfigNode.Value>()){
				
				bioResourceOuts.Add(entry.name, double.Parse(entry.value));
			}
			
			
			foreach( var bioResourceOut in bioResourceOuts){
				//get all matching resources in connected parts
				
				foreach(PartResource outResource in GetConnectedResources(this.part,bioResourceOut.Key)){
					//if resource we are currently processing == the resource container we are looking at
					if(!bioCraftResources.ContainsKey (bioResourceOut.Key)){
						bioCraftResources.Add(outResource.resourceName,outResource.amount);
					}
				}
			}
			
			foreach(var bioResourceIn in bioResourceIns){	
				
				foreach(PartResource InResource in GetConnectedResources(this.part,bioResourceIn.Key)){
					//if resource we are currently processing == the resource container we are looking at
					if(!bioCraftResources.ContainsKey (bioResourceIn.Key)){
						bioCraftResources.Add(InResource.resourceName,InResource.amount);
					}
				}
			}
			
		}//END loadBioResources
		
		public override void OnLoad(ConfigNode config)
		{
			if (this.config == null)
			{
				this.config = new ConfigNode();
				config.CopyTo(this.config);
				
			}
		}//END OnLoad
		
		
		public static List<PartResource> GetConnectedResources(Part part, String resourceName)
		{
			var resourceDef = PartResourceLibrary.Instance.GetDefinition(resourceName);
			var resources = new List<PartResource>();
			part.GetConnectedResources(resourceDef.id, resourceDef.resourceFlowMode, resources);
			return resources;
		}
		
		public Texture2D loadImage(string bioImage){
			string PathPlugin = string.Format("{0}","BioMass");
			Texture2D btnImg = new Texture2D(16,16);
			btnImg.filterMode = FilterMode.Point;
			string PathTextures = string.Format("{0}/Textures", PathPlugin);
			string FolderPath = PathTextures;
			string imageFilename = String.Format("{0}/{1}", FolderPath, bioImage);
			if (GameDatabase.Instance.ExistsTexture(imageFilename))
			{
				btnImg = GameDatabase.Instance.GetTexture(imageFilename, false);
			}
			else
			{
				new bioMsg("image not found" + imageFilename);
			}
			return(btnImg);
		}//END loadImage
	
		
	}//END BioGen class
}//END BioMass namespace