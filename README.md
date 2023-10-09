# Youtube Chat Integration - Godot Version

As a part of practicing Godot and testing converting stuff over to this engine and whatnot, I made a quick simple integration for Youtube functionality, similar to the Unity version.

Any instructions here are only how to quickly get it working here. If you want more details, please check out the original [Youtube Livestream Chat](https://github.com/stat-void/Youtube-Livestream-Chat) project.

## How to use

- The scene "Main.tscn" is an example scene on how this was implemented.
- The scene "YoutubeDataAPI.tscn" is basically the only thing that needs to be dragged into another scene. From there, you can use another script referencing "YoutubeDataAPI.cs" to connect them.
- "ConnectToLivestreamChat(string videoID, string apiKey)" is the command used to connect to a youtube chat. It returns a boolean on if it succeeded, and prints any issues in the Godot console.
- The manual API Timer, Quota statuses and other functionality can be gotten through public reference in "YoutubeDataAPI.cs"