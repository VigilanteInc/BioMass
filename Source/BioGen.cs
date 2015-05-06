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

			craftManager.consoleText = status;
			base.OnFixedUpdate ();

		}
	}

/// <summary>
/// Generates the light resource based on solarpanels
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
/// Adds animation functionality to the ModuleResourceConverter
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


}

}//END BioMass namespace