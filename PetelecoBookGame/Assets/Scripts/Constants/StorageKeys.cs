
namespace Assets.Constants
{
	// Keys for PlayerPrefs storge
	public struct PlayerPrefsKeys
	{
		// Flow control
		public const string PlayerLevel = "player_level";
		public const string STARTED = "started";

		// Sound
		public const string NormalizedSoundVolume = "sound_volume";
		public const string NormalizedMusicVolume = "music_volume";

		// Transitions
		public const string AudioSceneSource = "audio_scene_source";

		// Game Records
		public const string MemoryGameRecord = "memory_game_record";
		public const string FindObjectsRecord = "find_objects_record";
		public const string FindShadowsRecord = "find_shadows_record";
		public const string ConnectPointsRecord = "connect_points_record";
		public const string SeparateTrashRecord = "separate_trash_record";
		public const string CardsGameRecord = "cards_game_record";

		public const string AlreadySawCardsGameTutorial = "already_saw_cards_game_tutorial";
	}
}
