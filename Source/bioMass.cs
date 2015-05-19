using System.Collections.Generic;
using UnityEngine;
using KSP;

namespace BioMass
{
	//ModuleResourceConverter for Bilogical Processes such as photosynthesis
	//Includes error reporting to the biomonitor 
	public class BiologicalProcess : ModuleResourceConverter{

		[KSPField]
		public string bioName = "";   //optional name of crop/organisim to help determine error sources. 
	
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