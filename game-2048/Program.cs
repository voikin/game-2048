using game_2048.DataLayer;
using game_2048.DataLayer.dtos;

PlayerRecordDTO firstOne = new PlayerRecordDTO("Arseni");
List<PlayerRecordDTO> records = new List<PlayerRecordDTO>() { firstOne };
// File.Open("records.json", FileMode.OpenOrCreate);
// Console.WriteLine(DataAccess.GetRecordByName("Arseni"));
// DataAccess.CreateOrUpdateRecord("Arseni", 10);
// DataAccess.CreateOrUpdateRecord("nikita", 20);
// DataAccess.GetRecords().ForEach(Console.WriteLine);

// commit for dev branch