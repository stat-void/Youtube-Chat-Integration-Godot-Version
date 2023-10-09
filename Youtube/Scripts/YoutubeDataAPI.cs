using Godot;
using System;
using System.Threading.Tasks;
using Void.YoutubeAPI.LiveStreamChat.Messages;

namespace Void.YoutubeAPI
{
	public partial class YoutubeDataAPI : Node
	{

		private YoutubeDataAPI Instance;

		// Quota and APIKey handler
		public YoutubeKeyManager KeyManager { get; private set; }
		
		[Export] public YoutubeAPITimer APITimer;

		private YoutubeLiveChatMessages _ytLiveChatMessages;

		/// <summary>
		/// If the API Timer is used or this event is manually invoked, 
		/// all currently active API features would be called to do their tasks.
		/// </summary>
		public static event Action OnRequest;

		/// <summary>
		/// Event used to synchronously close all YT API components when this class is destroyed.
		/// </summary>
		public static event Action OnQuit;

		public override void _EnterTree()
		{
			base._EnterTree();

			if (Instance == null)
				Instance = this;
			else
			{
				GD.PrintErr("There should only be one instance of YoutubeDataAPI!");
				return;
			}
			
			APITimer = GetChild(0) as YoutubeAPITimer;
			KeyManager = new YoutubeKeyManager();
			
			YoutubeAPITimer.OnRequest += MakeCallRequest;
		}

		public override void _ExitTree()
		{
			base._ExitTree();
			QuitCalled();
		}

		/// <summary>
		/// Create an event invoke that causes all active API components to activate.
		/// </summary>
		public void MakeCallRequest()
			=> OnRequest?.Invoke();


		/// <summary> Connect to a livestream chat using the provided video ID and a stored Youtube API key. </summary>
		/// <param name="videoID"></param>
		public async Task<bool> ConnectToLivestreamChat(string videoID)
		{
			if (string.IsNullOrWhiteSpace(KeyManager.APIKey))
			{
				GD.PrintErr("No API Key has been previously saved or currently provided.");
				return false;
			}

			return await ConnectToLivestreamChat(videoID, KeyManager.APIKey);

		}

		/// <summary>
		/// Connect to a livestream chat using the provided video ID and Youtube API key.
		/// </summary>
		/// <param name="videoID"></param>
		/// <param name="apiKey"></param>
		public async Task<bool> ConnectToLivestreamChat(string videoID, string apiKey)
		{
			_ytLiveChatMessages ??= new();
			(bool successfulConnect, string reason) = await _ytLiveChatMessages.Connect(videoID, apiKey);
			GD.Print($"Connection success - {successfulConnect}; Reason - {reason}");

			return successfulConnect;
		}

		private void QuitCalled()
		{
			YoutubeAPITimer.OnRequest -= MakeCallRequest;

			KeyManager.QuitCalled();

			if (APITimer != null)
				APITimer.QuitCalled();

			OnQuit?.Invoke();

			YoutubeSaveData.SaveData();
		}
	}
}