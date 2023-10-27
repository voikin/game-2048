using game_2048.DataLayer;
using game_2048.DataLayer.dtos;
using game_2048.PresentationLayer;

PlayerRecordDTO firstOne = new PlayerRecordDTO("Arseni");
List<PlayerRecordDTO> records = new List<PlayerRecordDTO>() { firstOne };

Presentation app = new Presentation(null);
app.Start();