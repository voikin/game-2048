using game_2048.DataLayer;
using game_2048.DataLayer.dtos;
using game_2048.LogicLayer;
using game_2048.PresentationLayer;

var dataAccess = new DataAccess("records.json");
var sessionDataAcces = new SessionDataAccess();
var logic = new Logic(dataAccess, sessionDataAcces);
var app = new Presentation(logic);

app.Start();