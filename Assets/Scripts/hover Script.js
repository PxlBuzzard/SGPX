// script from https://www.youtube.com/watch?feature=player_embedded&v=Z8YGKQvEz1Y
var levelToLoad : String;
var soundhover : AudioClip;
var beep : AudioClip;
var QuitButton : boolean = false;
function OnMouseEnter(){
audio.PlayOneShot(soundhover);
}
function ButtonSelected(){
audio.PlayOneShot(beep);
//yield new WaitForSeconds(0.35);
if(QuitButton){
Application.Quit();
}
else{
Application.LoadLevel(levelToLoad);
}
}
@script RequireComponent(AudioSource)