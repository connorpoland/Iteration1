Starting Game:
	-run the 'Iteration1.exe' 
		Note: Having tried this on multiple computers I know that
		aspect ratios differing from the one I presented/demo'd
		with make the UI a mess and possibly lose functionality
		(If a button isn't drawn on the screen for example).
		If this is the case you can customize the aspect ratio
		much more in the Unity Editor with the 'Unity Project'
		folder I've included.

Playing the Game:
	-Selected either 'Start Game' or 'Fast Rigged Game'
		-Fast Rigged Game is an game with the decks rigged for
		specific scenarios. This game ends when a player reaches 5
		shields and evolves for the first time.
	Note: Players 0, 1, 2, 3 correspond to Players 1, 2, 3, 4 respectively
		in the scenarios. Each player is represented by their own 
		coat of arms.
	-Follow the text prompts on screen, interact with objects (cards)
	by clicking on them (click the story deck to draw story card). If 
	you see no text prompts on the screen it is likely the aspect ratio
	issue explained above.

	-Sponsoring Quests: You create quests all at once by selecting
	cards from your hand IN ORDER. if you wanted to create the scenario 1
	quest you would press Saxons -> Boar -> Dagger -> Sword. The UI
	will tell you when you've done it right/wrong

	-Playing Weapons: You will be able to play weapons by pressing the weapon
	you want to equip -only before a fight.- Pressing weapons you can't equip 
	will notify you that it is an invalid move

	-Discarding Cards: The same as playing weapons -only when prompted to 
	discard by the UI-

Logs:
	Games you play will be logged in the "MyLogs.txt," "StoryDeckLog.txt," and
	"AdventureDeckLogs.txt" as you play games. Scenario logs (which are included
	in this folder were generated this way.

Strategy Pattern:
	AIs are not implimented in this game but I have some source code for
	them to not lose out on these marks entirely. In the Scripts-SourceCode
	directory you can find my strategy pattern code in this
	.\Scripts-SourceCode\Model\Strategy directory.
		-AIPlayerModel.cs
		-PlayerModel.cs
		-Strategy.cs
		-Strategy1.cs
		-Strategy2.cs

		