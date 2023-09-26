using game_2048;
using game_2048.DataLayer;

PlayerRecordDTO firstOne = new PlayerRecordDTO("Arseni");
List<PlayerRecordDTO> records = new List<PlayerRecordDTO>() { firstOne };
DataAccess.SaveRecords(records);