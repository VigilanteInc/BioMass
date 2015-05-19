using UnityEngine;
using KSP;

namespace BioMass
{

	/// <summary>
	/// ModuleResourceConverter w/ Animations
	/// Adds animation functionality to the ModuleResourceConverter for parts that require an animation as well as converter/generator functionality.
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
}