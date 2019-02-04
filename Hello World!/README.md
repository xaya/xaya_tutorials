#  Hello World!

This code sample is minimalistic. It's goal is to demonstrate how to wire up an application with the XAYA platform quickly and easily. 

HelloXaya is a typical "Hello World!" example. It wires up the XAYA platform in a Windows forms project. 

HelloXaya lets you say "Hello World!" on the XAYA blockchain. That's all it does.

[Download the HelloXaya project here]().

## Inside HelloXaya

There are 4 distinct elements inside of HelloXaya.

- HelloXaya project: This is our "game"
- BitcoinLib project: This is used for RPCs
- XAYAWrapper project: This wraps the C++ 64-bit libxayagame statically linked libarary in the XayaStateProcessor folder
- XayaStateProcessor folder: This has the precompiled libxayagame binary for Windows, libxayawrap.dll, and all its dependencies

While this tutorial only explains HelloXaya, you can find more information about other elements elsewhere in the XAYA documentation and tutorials. 

# Important Considerations

[**VIDEO** tutorial here.](https://www.youtube.com/watch?v=v_jM8i2TZK8)

[![Set your XAYA project to 64 bit.png](img/Set%20your%20XAYA%20project%20to%2064-bit.png)](https://www.youtube.com/watch?v=v_jM8i2TZK8)

The XAYAWrapper DLL (libxayawrap.dll) is 64-bit. Consequently, your project **MUST** exclude 32-bit or explicitly be set as 64-bit. See this HelloXaya setting:

![Uncheck Prefer 32-bit](img/Prefer-32-bit-unchecked.png)

Otherwise, you must set your project to be 64-bit (x64). 

![x64](img/x64.png)

# Implementing XAYA Simplified

[**VIDEO** Simplified overview of implementing XAYA](https://www.youtube.com/watch?v=W0kD0ywyyqQ)

[![Simplified overview of implementing XAYA](img/Simplified%20overview%20of%20implementing%20XAYA.png)](https://www.youtube.com/watch?v=W0kD0ywyyqQ)

To implement XAYA, all you need to do is to is:

- Instantiate XAYAWraper (1 line of code)
- Connect to XAYAWrapper (1 line of code)
- Set up a listener thread to receive game states
- Implement game logic in:
	+ 3 callbacks
		- initialCallbackResult (20 lines of trivial code)
		- forwardCallbackResult
		- backwardCallbackResult
	+ Ancillary game logic
		- JSON classes
		- Helper methods
- Send moves to the XAYA blockchain
	+ This requires RPCs (can be implemented in many ways)

# Threading

It's crucial that you create threads for XAYAWrapper. 

Portions of XAYAWrapper are blocking operations and **MUST** be run in separate threads.

## Threading in HelloXaya

[**VIDEO** tutorial on threading here.]()

In HelloXaya, we've used BackgroundWorkers. There are more robust threading patterns available, but BackgroundWorkers are simple to understand with little complexity. 

You can implement better threading structures on your own. 

# Instantiate and Connect to XAYAWrapper

[**VIDEO** Instantiate and Connect to XAYAWrapper](https://www.youtube.com/watch?v=wKKFf-xPyLU)

[![Instantiate and Connect to XAYAWrapper](img/Instantiate%20and%20Connect%20to%20XAYAWrapper.png)](https://www.youtube.com/watch?v=wKKFf-xPyLU)

Instantiating and Connecting to XAYAWrapper **must** be done in a thread. The connection is a blocking operation. However, once connected, XAYAWrapper will begin sending log data immediately. This log data isn't game state data though; that is examined below.

To instantiate the wrapper, call it's constructor:

	wrapper = new XayaWrapper(dataPath, // The path to the game's executable file. 
		Properties.Settings.Default.Host, // The host, e.g. localhost or 127.0.0.1
		"8900", // The game host port. Fixed at 8900 in libxayagame.
		ref result, // An error or success message.
		CallbackFunctions.initialCallbackResult, 
		CallbackFunctions.forwardCallbackResult, 
		CallbackFunctions.backwardCallbackResult);

To connect, call the Connect method:

	result = wrapper.Connect(dataPath, // The path to the game's executable file. 
		FLAGS_xaya_rpc_url, // The URL for RPC calls.
		"8900",  // The game host port. Fixed at 8900 in libxayagame.
		"0", // Which network to use: Mainnet, Testnet, or Regtestnet.
		"memory", // The storage type: memory, sqlite, or lmdb.
		"helloworld", // The name of the game in the 'g/' namespace.
		dataPath + "\\..\\XayaStateProcessor\\database\\", // Path to the database folder, e.g. SQLite.
		dataPath + "\\..\\XayaStateProcessor\\glogs\\"); // Path to glog output folder.

# Listening for new `GameState`s

[**VIDEO** Listening for new GameStates]()



We must listen for updates in a thread. There are 4 important lines of code.

In our listener, we use the wrapper to get game states. The first thing to do is to tell the wrapper to wait until there's a new game state.

	wrapper.xayaGameService.WaitForChange();

Once there's a game state, the thread resumes and we get the game state.

	BitcoinLib.Responses.GameStateResult actualState = 
		wrapper.xayaGameService.GetCurrentState();

We then cast that as a `GameState` by deserialising the JSON.

	state = JsonConvert.DeserializeObject<GameState>(actualState.gamestate);

The final step in our listening thread is to send the game state to the main UI thread.

	sendingWorker.ReportProgress(0, state);

# Update the UI with the New GameState

Our listener thread casts the event argument as a `GameState` and sends it to a method that updates the game.

	UpdateHelloChat((GameState)e.UserState);

HelloXaya merely updates a text box with what other people have said. It loops through all players and stores data in a StringBuilder.

	sb.AppendLine(v.Key + " said \"" + v.Value.hello + "\"");

We then update the textbox in the UI.

	txtHelloGameState.Text = sb.ToString();

# Sending Moves

You don't need to be running an instance of a game to send moves. 

Moves can be sent arbitrarily in many ways. Here are some ways:

- From xaya-cli
- From the XAYA QT console
- Sending to xayad
- Etc.

When procesing moves, you **must** guard against invalid moves with robust error checking.

In HelloXaya, we build the move in a string then send it:

	string hello = "{\"g\":{\"helloworld\":{\"m\":\"" + txtHelloWorld.Text + "\"}}}";
	xayaService.NameUpdate(this.cbxNames.GetItemText(this.cbxNames.SelectedItem), 
		hello, 
		new object());



Done!









































































































