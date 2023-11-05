using game_2048.DataLayer;
using game_2048.LogicLayer;
using game_2048.PresentationLayer;

var dataAccess = new DataAccess();
var sessionDataAccess = new SessionDataAccess();
var logic = new Logic(dataAccess, sessionDataAccess);
var app = new Presentation(logic);

app.Start();