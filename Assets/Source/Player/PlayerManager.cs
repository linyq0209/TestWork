using System.Collections.Generic;

public class PlayerManager : PSingleton<PlayerManager> {
	public const string CONFIG_PATH = "PlayerConfig";

	protected override void Init()
	{
		CsvFile file =  FileUtility.LoadCsvFile(CONFIG_PATH);
		if(file != null)
		{
			int head = file.HeaderCount;
			int row = file.RecordCount;
			for (int i = 0; i < row; i++)
			{
				CsvRecord record = file[i];
				PlayerConfig config = new PlayerConfig(record);
			}
		}
	}
}
