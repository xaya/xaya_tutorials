# How to Wire Up libxayagame in C#

Before you get started, you'll need libxayagame. There are statically linked binaries available [here](https://github.com/xaya/xaya_tutorials/tree/master/XayaStateProcessor). You'll also need XAYAWrapper. It wraps the C++ libxayagame for use with C#. You can download that [here]() (coming soon). You'll also need to make RPC calls. That can be done any way you wish, but we've forked BitcoinLib and modified it for XAYA. You can get that project [here](https://github.com/xaya/xayaRPClib) (update coming soon). XAYAWrapper uses BitcoinLib and exposes it through `xayaGameService`, so you'll need that DLL as well. 

The following shows how to wire up libxayagame.

# Wiring Up XAYAWrapper

In your project:

1. Add a reference to XAYAWrapper by either adding the DLL or adding the project and then adding a reference to the project.

2. Add a `using` for XAYAWrapper.

		using XAYAWrapper;

3. Create a member variable for the wrapper.

		XAYAWrapper wrapper;

4. In a thread, construct XAYAWrapper and call its Connect method. 
	
		wrapper = new XayaWrapper(appPath, 
			host, 
			"8900", 
			ref result, 
			CallbackFunctions.initialCallbackResult, 
			CallbackFunctions.forwardCallbackResult, 
			CallbackFunctions.backwardCallbackResult);
	
		result = wrapper.Connect(appPath, 
			FLAGS_xaya_rpc_url, 
			"8900", 
			"0", 
			"memory", 
			"gamename", 
			appPath + "\\..\\XayaStateProcessor\\database\\", 
			appPath + "\\..\\XayaStateProcessor\\glogs\\");

5. In another thread, wait for notifications from the wrapper.

		wrapper.xayaGameService.WaitForChange();

	+ When a change comes in, get the current game state and deserialise it for your GameState definition.

			BitcoinLib.Responses.GameStateResult actualState = wrapper.xayaGameService.GetCurrentState();
			state = JsonConvert.DeserializeObject<GameState>(actualState.gamestate);

	+ Send the notification to the front end.

			sendingWorker.ReportProgress(0, state);

6. In the front end, get the game state from the notification thread and then update the front end.

		UpdateMyGame((GameState)e.UserState);

7. Implement your game logic the 3 callbacks defined in XAYAWrapper.
	- initialCallback
	- forwardCallback
	- backwardsCallback

Tutorials for how to implement those callbacks will be coming soon. 


