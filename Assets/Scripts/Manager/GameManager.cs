using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	[Range(0, 100)]
	[SerializeField]
	private int volume = 100;

	[Range(0, 100)]
	[SerializeField]
	private int musicVolume = 100;

	[Range(0, 100)]
	[SerializeField]
	private int soundEffectVolume = 100;

	[SerializeField]
	private bool cheatMode = false;

	protected override void Awake()
	{
		base.Awake();
		DontDestroyOnLoad(gameObject);
		volume = Mathf.Clamp(volume, 0, 100);
		musicVolume = Mathf.Clamp(musicVolume, 0, 100);
		soundEffectVolume = Mathf.Clamp(soundEffectVolume, 0, 100);
	}

	public int Volume
	{
		get => volume;
		set => volume = Mathf.Clamp(value, 0, 100);
	}

	public int MusicVolume
	{
		get => musicVolume;
		set => musicVolume = Mathf.Clamp(value, 0, 100);
	}

	public int SoundEffectVolume
	{
		get => soundEffectVolume;
		set => soundEffectVolume = Mathf.Clamp(value, 0, 100);
	}

	public bool CheatMode
	{
		get => cheatMode;
		set => cheatMode = value;
	}
}
