# IWantMovement

## What is "I Want Movement?"
The idea behind this plugin is to allow manually controlled combat routines (Routines without movement) to be used with 'afk' botbases such as for questing/dungeonbuddy/gatherbuddy/grinding.

## How to get started
- Install the plugin.
- Select 'Plugins' -> Enable 'I Want Movement'.
- Click 'Settings' to open IWM's options.
- Enable which parts you want the plugin to take control of. 
- Setup your 'Pull' spell. I've added some default abilities, but you probably want to change these. Change the spell related to your class to the ability you wish to use as the opener. Ensure it's in the correct format (i.e. "Frostbolt", not frostbolt/FROSTBOLT/FrostBolt).

## Things to be aware of ..
- Movement/Targeting/Facing should be controlled by the combat routine, as it 'knows' when it wants to move. This plugin has to 'guess' when it needs to be done, and as such will never be 'perfect'.
- If the bot base's POI (point of interest) is something other than combat - for instance it wants to gather an object but we're in combat, the plugin will keep attempting to prepare for combat, so the behavior may look a bit odd.
- Movement isn't ideal at the moment. I'll improve it over the coming days.


## Credits and Developers
Originally created and developed by Millz.
Continued development by Lbniese.
Thanks to Chinajade for demonstrating a method of hooking into a combat routine.
