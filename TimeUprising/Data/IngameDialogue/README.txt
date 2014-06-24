# Empty lines and ines starting with # are ignored
# Dialogues begin with ">>>" followed by the name of the trigger followed by the dialogue type
# Ex. ">>> LevelStart Standard" begins a dialogue mapped to TriggerDialogue ("LevelStart")
# 
# DialogueTypes are triggered at different times
#   Standard - triggered through standard events
#   Realtime - is only updated when the game is paused, ex. pause menu or win/lose screen
#   Chatter  - (Not implemented) these are added to a pool and randomly selected with TriggerChatter ()
#   Warning  - this is high priority dialogue and played immediately when triggered with TriggerRealtimeDialogue ()

# Dialogue Format
# Duration State Speaker Location --- Message
# Duration  - the amount of time the dialogue remains on the screen before the next dialogue is displayed
# State     -  The emotion of the speaker. Used to swap out the unit portrait
# Speaker   - Who is speaking. Used to choose the unit portrait as state the name
# Location  - The location the dialogue is displayed (Currently only Left or Right)
# ---       - Three dashes used to separate dialogue meta from the message
# Message   - the message that is displayed to the user

### Examples

>>> Tutorial Standard # begin tutorial dialogue
4 Normal Swordsman Right --- This is a test of swordsman dialogue!
5 Normal Saemus Right    --- Hey, wanna buy some towers?
6 Normal Swordsman Left  --- I am a mysterious stranger! # no swordsman_left portrait, so the mysterious stranger is used
15 Normal King Left      --- Welcome to my castle.  Click the towers to select them, and a location in the castle to send units there.
15 Normal King Left      --- If you click the 'X' on the ground your units will ignore enemies and go to that point.
15 Normal Advisor Right  --- Hit 'Escape' to pause the game.
15 Normal Advisor Right  --- The white tower can heal your other towers if they become damaged.  Be careful though, the heal tower cannot heal itself.
<<< Tutorial # end tutorial dialogue

>>> TowerDestroyed Warning # begin tower destroyed dialogue
10 Normal Swordsman Right --- Sir, it appears that the peasants have taken over a tower.
10 Normal King      Left  --- Well don't just stand there. Go get some more swordsman and get it back!
20 Normal Advisor   Right --- Send units towards the tower and they will bring it back under your control. Beware of the stronger peasants though!
<<< TowerDestroyed  # end tower destroyed dialogue

>>> TowerRetaken Warning
7 Normal King  Left --- It’s about time we took that tower back.
<<<

>>> FirstKill Standard
15 Normal King  Left --- . . . What was that shimmering blue circle thing?  And why did it eat that peasant?
<<<

>>> ArcherMage Chatter # begin conversation between archer and mage
4.0 Angry   Mage    Right  --- Hey Archer, you almost hit me!
5.0 Nervous Archer  Left   --- It's not my fault you're blocking my view. Stop wearing such a big hat!
3.0 Normal  Mage    Right  --- . . . . .
<<< ArcherMage  # end conversation between archer and mage

>>> Pause Realtime # played when the game is paused
100 Nervous King    Left    --- Why isn’t anyone moving? It’s so quiet...
<<< Pause

>>> GameLost Realtime # played when the user loses a level
10  Dying   King    Left    --- Next time DON’T LET ME DIE!!!
<<< GameLost

>>> GameWon Realtime # played when the user wins a level
10 Happy   King    Left    --- Finally! Victory is ours.  The Uprising is defeated.
20 Normal  Saemus  Right   --- Now that they're gone, how about some nice tower upgrades?
<<< GameWon

>>> KingDamaged Warning
10 Angry King Left        --- Help! Help! I’m being oppressed!
<<< KingDamaged

>>> KingInjured Warning
10 Nervous King Left      --- Guards, where are you? I’M DYING, and I can’t get up...
<<< KingInjured