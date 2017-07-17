using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConfig {
	public const int PLAYER_LEVEL_INDEX = 0;
	public const int PLAYER_EXP_INDEX = 1;

	protected int level = 1;
	protected int exp = 1;

	public PlayerConfig(CsvRecord recorder)
	{
		level = recorder.GetInt(PLAYER_LEVEL_INDEX, level);
		exp = recorder.GetInt(PLAYER_EXP_INDEX, exp);
		Debug.Log(level + "------------"+ exp);
	}
}
