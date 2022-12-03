step.1
Drag and drop a prefaps Console out the "Prefabs" folder in you´r hierarchy 
and add a EventSystem its found under right click Ui/EventSystem

step .2
create a a new empty Gameobject and add a new script for you´r commands
 (you can add command methods to any script that you whant)

to make a command use [addCommand("commandName")] over a method that you whant to call with this name
  

step.3
add in the CommandListe (its the component in the ConC Gameobject) all Gameobjects with a 
script thas have a command. You can change the activate key for toggle the console in game 

step.4
in the Playmode press the activate key and use a command (//Commands true || false) to actiavte commands
a custom command you call with //commandName if you command have some parameterslike integer than 
//commandName 1


 
