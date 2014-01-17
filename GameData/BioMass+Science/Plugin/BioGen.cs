using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
using UnityEngine;

namespace KSPBioMass
{
	
		
	public class BioGen : PartModule
		
	{		
		[KSPField]
		public String Label;
		
		[KSPField]
		public bool IsEnabled;
		
		[KSPField(isPersistant = false)]
		public bool ShowInfo = false;

		[KSPField]
		public string AnimationName;

		[KSPField(isPersistant = false)]
		public bool isActivated = false;
				 		
		ModuleGenerator biogen;
			
		Animation BioAnimate{ get{ return part.FindModelAnimators(AnimationName)[0]; } }


		public override void OnStart(PartModule.StartState state){
				
			BioAnimate[AnimationName].layer = 2;
			biogen = part.Modules["ModuleGenerator"] as ModuleGenerator;
				base.OnStart(state);
			
		}

		public override string GetInfo ()
		{
			if(ShowInfo == true){
				var myString = String.Format(Label + "\n") + base.GetInfo ();
				return myString;
			}
			else { return "Biological Activity";}
				
		
		}

		public override void OnUpdate()
			
        {

			base.OnUpdate ();
			if(biogen){

					if(biogen.generatorIsActive == true){
						if(isActivated == false){
						BioAnimate[AnimationName].normalizedTime = 0;
						BioAnimate[AnimationName].speed = 1;
						BioAnimate[AnimationName].enabled = true;
						BioAnimate[AnimationName].wrapMode = WrapMode.ClampForever;
						BioAnimate.Play(AnimationName);
							
							isActivated = true;
							print("Attempted to play " + AnimationName);
						}
				
				
					}else{
						if(biogen.generatorIsActive == false){
						if(isActivated == true){
							BioAnimate[AnimationName].normalizedTime = 1;
							BioAnimate[AnimationName].speed = -1;
							BioAnimate[AnimationName].wrapMode = WrapMode.ClampForever;
							BioAnimate.Play(AnimationName);
								
								isActivated = false;
								print("attempted to play " + AnimationName);
						}
							
				}
				
						
			}
							
				
						
		}
		
	
	}
	
	
	
}
	public class GermSeeds : PartModule
	{
		[KSPField(guiActive = true, guiName = "Germinate")]
		public bool activated;

		[KSPField]
		public string AnimationName;

		protected Animation[] anims;

		[KSPEvent(guiActive = true, guiName = "Germinate")]
		public void Activate()
		{
			print("attempted to play " + AnimationName);

			anims[0].Play(AnimationName);

			activated = false;
			Events["Activate"].active = true;
			Events["Deactivate"].active = false;
		}
		
		[KSPEvent(guiActive = true, guiName = "Deactivate", active = false)]
		public void Deactivate()
		{
			print("attempted to play " + AnimationName);

			animation.Stop(AnimationName);

			activated = false;
			Events["Activate"].active = true;
			Events["Deactivate"].active = false;
		}

		
		public override void OnStart(PartModule.StartState state){
			//animation[AnimationName].layer = 2;
			anims = part.FindModelAnimators(AnimationName);
			if ((anims == null) || (anims.Length == 0))
			{
				print("BioMass - animation not found: " + AnimationName);
			}
			base.OnStart(state);
			
		}
		
				
		public override void OnUpdate()
			
		{
			
			base.OnUpdate ();
					
			
		}


	}

 
}

