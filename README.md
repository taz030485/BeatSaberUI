# BeatSaberUI
Plugin so other plugins can add UI to Beat Saber  
Currently only add options to the Settings screen is supported.  
This plugin does nothing on its own!  
  
# Changes  
1.0  
Initial release  

# Developers  
Add a reference to BeatSaberUI to you project, just like IllusionPlugin  

# Exmaple Usage  
Remember to add  
```csharp
using BeatSaberUI;
```
To create a sub menu  
```csharp
var subMenu = SettingsUI.CreateSubMenu("Interface Tweaks"); // Passing in the sub menu label
```
To add a On/Off toggle
```csharp
var energyBar = subMenu1.AddBool("Move Energy Bar"); // Passing in the option label
energyBar.GetValue += delegate { return Settings.MoveEnergyBar; }; // Delegate returning the bool for display
energyBar.SetValue += delegate (bool value) { Settings.MoveEnergyBar = value; }; // Delegate to set the bool when Apply/Ok is pressed
```
To add a list 
```csharp
var noteHit = subMenu.AddList("Note Hit Volume", volumeValues()); // Passing in the option label, and a float[] of possible values
noteHit.GetValue += delegate { return Settings.NoteHitVolume; }; // Delegate returning the current value for display
noteHit.SetValue += delegate (float value) { Settings.NoteHitVolume = value; }; // Delegate to set the float when Apply/Ok is pressed
noteHit.FormatValue += delegate (float value) { return string.Format("{0:0.0}", value); }; // Delegate for formatting the value for display
```
  
Or a custom viewController can be used by extending the following classed and overriding the following methods:  
  
For Toggles
```csharp
class YourToggleViewController : SwitchSettingsController
override bool GetInitValue()
override void ApplyValue(bool value)
override string TextForValue(bool value)
```
Then add it using  
```csharp
subMenu.AddToggleSetting<YourToggleViewController>("Your Toggle");
```
  
For Lists
```csharp
class YourListViewController : SwitchSettingsController
override void GetInitValues(out int idx, out int numberOfElements)
override void ApplyValue(int idx)
override string TextForValue(int idx)
```
Then add it using  
```csharp
subMenu.AddListSetting<YourListViewController>("Your List");
```

You can also get the transform of the UI screen and add your own elements  
(I have not tested if adding buttons etc will cause the MainSettingsMenuViewController to get confused)
```csharp
subMenu.transform
```

List Exmaple
```csharp
subMenu3.AddListSetting<SongSpeedSettingsController>("Song Speed");
```
```csharp
public class SongSpeedSettingsController : ListSettingsController
{
	protected float[] speeds;

	protected override void GetInitValues(out int idx, out int numberOfElements)
	{
		float minValue = 0.25f;
		float increments = 0.05f;
		numberOfElements = 56;
		speeds = new float[numberOfElements];
		for (int i = 0; i < speeds.Length; i++)
		{
			speeds[i] = minValue + increments * i;
		}
		float volume = SongSpeed.TimeScale;
		idx = numberOfElements - 1;
		for (int j = 0; j < speeds.Length; j++)
		{
			if (volume == speeds[j])
			{
				idx = j;
				return;
			}
		}
	}

	protected override void ApplyValue(int idx)
	{
		SongSpeed.TimeScale = speeds[idx];
	}

	protected override string TextForValue(int idx)
	{
		SongSpeed.TimeScale = speeds[idx];
		return string.Format("{0:0}%", speeds[idx]*100);
	}
}
```