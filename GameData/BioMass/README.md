BioMass WIP: Updated 2015.0818

Creators:
=========

Models and most of the code = Roboto
Science and some of the code = seanth

Major reworking of how BioMass works:
-------------------------------------

1. Brand new custom dlls that allow BioMass parts to continue to work even when ship are not selected, and a means of changing difficulty in-game.
2. New parts, improved animations, etc.

Lots of play balancing left to do!

-------------------------------------
###2015.0919 Fixed the volumes held by the food and seed storage containers. MUCH more storage now. Also fixed the left and right nodes on the food and seed storage containers

###2015.0901 Added "wrapper tanks" to hold water. Useful for protecting crew from radiation. :)

###2015.0818 Pull requests merged (punctuation changes). Additional changes related to puncuation. Rest the hydrogen gas compressor to ~9877:1 MonoProp:H2Gas.

###2015.0805 Updated Alternate Resource Icons. Included a radial oxidizer tank on Medium and Hard. Included gas storage bags (roverdude's landing bags). This allows things to function on rails when not selected by providing places for gasses to actually be. Will only be in medium and hard. LiquidCO2 changed to LqdCO2 to comply with community resource pack. The Atmospheric extractor works on Kerbin and Duna (though there is a rate bug on the KSP side, I think).

###2015.0704 Tweaked aspects of how the compressors work. Added a new part: gas bleed valve for venting excess gas to the atmosphere (or wherever). It's meant to be used on the surface of planets during ISRU stuff. Added Alternate Resource Panel icons back in. Not sure how we lost them.

###2015.0624 Somehow lost edits to BioReactor from 2015.0619. Hard BioReactor is fixed again. Minor testing of the MicroBiome. Set the modules (except the water valve) to be always working.

###2015.0622 Fixed popup window problem. Reported by Lachlan (http://forum.kerbalspaceprogram.com/threads/125880-WIP-BioMass-mod-needs-testers-%28greenhouses-gas-compressors-biofuel-production%29?p=2032459&viewfull=1#post2032459)

###2015.0619 14:18 HARD and MEDIUM: Simplified the BioCake+Kethane gas rxns

###2015.0619 12:28 Increased compressor release rates for Medium and Hard by factor of 10, except for Hydrogen. Left that as-is.

###2015.0619 10:35 Misc tweaks to BioReactor. Fixed animations. HARD: increased rate of Bioreactor. Now yields 0.24L liquidFuel/day. MEDIUM: Fixed Easy BioReactor(wasn't mass balanced), increased rate of Bioreactor. Now yields 0.41L liquidFuel/day. EASY: increased rate of Bioreactor. Now yields 0.72L liquidFuel/day. Medium and Hard need water for everything to work.

###2015.0619 10:06 HARD: removed legacy BioReactor module. Renamed the buttons Start/Stop H2 Rxn, Start/Stop KH4+O2 Rxn

###2015.0618 23:38 EASY: moved tanks from Science to Utility. Changed the name of the gas compressor

###2015.0618 19:10 Fixed problem with starting game and not explicitly choosing a difficulty. 

###2015.0618 18:44 Removed algae greenhouse from Easy

###2015.0618 18:32 This is what happens when you start using automated tools to help. Added light resource back to all greenhouses. Fixed red, hard attachment node sizes. Removed duplicate modules from Easy greenhouses.

###2015.0618 18:07 Updated Hard yellow greenhouse. Removed artificial light. Removed Easy's food production module. Reduced node attachment sizes. Removed duplicate shutter buttons. Removed seed resource. Added light resource. Defaults to half max amount of biomass

###Update 0.2.2.1 (2014.12.01)
	*Expanding greenhouse attachment points fixed.
	*Odd bug with textures files having "-" in the name not being loading on OS X 10.10 fixed.
	*Placed numbers as folder prefixes to control the load order of parts.
	*Fixed a silly error in the AVC version file (need to point to raw vs html formatted pages).
	*Stared flushing out the biological experiments:
		*Built in light works.
		*Light is "consumed"
		*Respiration experiments in place
		*Sketchs of Photosynthesis, Growth, Germination, and Reproduction in place
		*Needs: opened->light accumulation, all actual growth functions.

###Update 0.2.2.0 (2014.10.30)
    *New Koylent maker model. 
    *New greenhouse. Clone of how the blue greenhouse works. Needs to be improved.
    *New science part. Needs experiments defined.

###Update 0.2.1.0 (2014.10.02)
	*Removed 2nd Goo Experiment part appearing in VAB/SPH

###Update 0.2.0.0 (2014.09.29):
	*Added version file for Automatic Version Control (AVC) mod integration (suggested by Bit Fiddler)
	*Added file paths to resources for use by Alternate Resource Panel mod.
	*Fixed purple icon in toolbar at Space Center view (bug reported by Bit Fiddler)
    *In-flight button moved
	*Made "Easy" play mode a mass-balanced version of Classic (mass balance bug reported by KBluey). Seanth is punching himself in the head. Note: there are no plans to alter how Classic works.
	*Most "Easy" play mode parts shifted into Utility vs Classic having them in "Science".
	*Some "Easy" play mode parts have slightly different description compared to "Classic" to make it easier to go from Easy to Hard.
    *Misc spelling and other fixes

    
