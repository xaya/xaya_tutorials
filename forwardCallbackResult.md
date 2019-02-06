# forwardCallbackResult

The forwardCallbackResult method is where the main XAYA game logic resides. Its signature is:

	public static string forwardCallbackResult(string oldState, 
		string blockData, 
		string undoData, 
		out string newData)

`oldState` is the current game state. It must be cast as a `GameState`.

	GameState state = JsonConvert.DeserializeObject<GameState>(oldState);

`blockData` contains all the moves and should be deserialised.

	dynamic blockDataS = JsonConvert.DeserializeObject(blockData);

`undoData` is the undo data for the current game state. It needs to be updated so that the current set of moves can be rewound. For this, we create a new set of undo data for the current set of moves and store that in `undoData` at the end of the method.

`newData` is where the new game state is stored at the end of the method.











