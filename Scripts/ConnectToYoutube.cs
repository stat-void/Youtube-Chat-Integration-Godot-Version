using Godot;
using System;
using System.Collections.Generic;
using Void.YoutubeAPI;
using Void.YoutubeAPI.LiveStreamChat.Messages;

public partial class ConnectToYoutube : Node
{
	
	[Export] protected Control YTFieldsBase;
	[Export] protected LineEdit VideoIDField;
	[Export] protected LineEdit APIKeyField;
	[Export] protected Button ConnectionButton;
	
	[Export] protected YoutubeDataAPI YoutubeAPI;
	[Export] protected ScrollContainer Container;
	[Export] protected VBoxContainer ContentBase;
	
	private int _maxMessages = 30;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (!string.IsNullOrWhiteSpace(YoutubeAPI.KeyManager.APIKey))
			APIKeyField.PlaceholderText = "API Key found.";
		
		
		ConnectionButton.Pressed += ButtonPressed;
	}


	public override void _ExitTree()
	{
		base._ExitTree();
		YoutubeLiveChatMessages.OnChatMessages -= Messages;
		ConnectionButton.Pressed -= ButtonPressed;
	}
	
	private async void ButtonPressed()
	{
		
		if (string.IsNullOrWhiteSpace(VideoIDField.Text))
			return;
		
		if (!string.IsNullOrWhiteSpace(APIKeyField.Text))
			YoutubeAPI.KeyManager.SetAPIKey(APIKeyField.Text, true);

		ConnectionButton.Disabled = true;

		bool connected = await YoutubeAPI.ConnectToLivestreamChat(VideoIDField.Text);
		if (connected)
		{
			YoutubeAPI.APITimer.StartTimer();
			YoutubeLiveChatMessages.OnChatMessages += Messages;
			
			YTFieldsBase.Visible = false;
		}
		
		ConnectionButton.Disabled = false;
	}
	
	private void Messages(List<YoutubeChatMessage> messages)
	{
		for (int i = messages.Count-1; i >= 0; i--)
		{
			// Not max messages, create a new Rich Field Text
			if (ContentBase.GetChildCount() < _maxMessages)
			{
				RichTextLabel text = new()
				{
					Text = $"{messages[i].Username} | {messages[i].Message}",
					FitContent = true
				};
				
				ContentBase.AddChild(text);
			}
			
			// Is max messages, move the top message to the bottom and update the text
			else
			{
				(ContentBase.GetChild(0) as RichTextLabel).Text = $"{messages[i].Username} | {messages[i].Message}";
				ContentBase.MoveChild(ContentBase.GetChild(0), ContentBase.GetChildCount()-1);
			}
			
			// Force scroll to the bottom
			//Container.scroll
			//Container.ScrollVertical = Container.VerticalScrollMode;
			
			//GD.Print($"{messages[i].Username} | {messages[i].Message}");
		}
	}


}
