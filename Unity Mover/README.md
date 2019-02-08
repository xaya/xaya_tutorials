# Mover Sample Game in C# with Unity

This tutorial demonstrates how to wire up a game to run on the XAYA platform. Of particular interest, it shows how to use libxayagame and various RPCs. But most importantly, it shows how to write a simple game on the XAYA platform. Portions of this tutorial repeat portions of other tutorials. This tutorial is written such that you can skip around easily to those parts of greatest interest to you. 

You'll need to download the code. It's available [here](Unity%20Mover%20Code.zip). Extract the ZIP file. You'll find 3 projects inside the folder. 

1. BitcoinLib: The RPC library
2. XAYAWrapper: The libxayagame wrapper
3. XAYAUnity: The example game. Uses #1 and #2

# The FAST Way to Get Started

If you're impatient and simply want to get started, you can use the code as a template. For a thorough tutorial, [skip to here](#Tutorial-Requirements).

1. Compile BitcoinLib and XAYAWrapper
2. Use Mover as a template
3. Write your game logic
4. Write your front end

The most important things you'll also need to know are:

- [How to write the callbacks for XAYA](#Callbacks)
- [How to consume a GameState and update the front end](#Update-the-Front-End-with-a-GameState)

## 1) Compile BitcoinLib and XAYAWrapper

Open up the BitcoinLib and XAYAWrapper projects and compile them. They're referenced in the Unity project. The libxayagame library (wrapped by XAYAWrapper) does the difficult, heavy lifting of handling various blockchain operations for you. For RPCs (Remote Procedure Calls), we've modified BitcoinLib.

## 2) Create Your Game in Unity with Mover as a Template

Open up the XAYAUnity folder in Unity then click XAYA in SampleScene. Click in the Inspector to open up the scripts in Visual Studio. The files of interest are:

- XAYAConnector.cs
- XAYAClient.cs
- HelperFunctions.cs
- JSONClasses.cs
- CallbackFunctions.cs
- MoveGUIAndGameController.cs

No edits are needed for XAYAConnector.cs, although you may wish to make some changes in the `WaitForChangesInner` `IEnumerator`.

Game moves are submitted through XAYAClient, so you'll need to change the ExecuteMove method to match however you create moves. 

## 3) Write Your Game Logic

The game logic resides in the [XAYAMoverGame](#Game-Logic) namespace, which is found in HelperFunctions.cs, JSONClasses.cs, and CallbackFunctions.cs. See below for explanations of the various [callbacks](#callbacks). This is **meat** for creating a new XAYA game. 

## 4) Write Your Front End

`MoveGUIAndGameController` is the front end and where all the Unity code resides. This is where you consume the `GameState`. You'll need to change the front end for your game. Take note of how the `XAYAConnector` and `XAYAClient` classes are used in the various controls in Mover. 

There are 2 methods of particular interest to get started quickly:

- `Update`
- `RedrawGameClient`

The `needsRedraw` flag in the `Update` method is set in the `XAYAConnector`. Check the `WaitForChangesInner` method for how that's done. If the screen needs to be redrawn, the `RedrawGameClient` method is called. 

Similarly for `RedrawGameClient`, the data (i.e. the `GameState`) you need to consume comes from XAYAConnector. However, that data comes from XAYAWrapper (libxayagame) but is set in the callbacks that you must write. In this case, they're in the `XAYAMoverGame` namespace, and in particular, in the `forwardCallbackResult` and `backwardCallbackResult` methods. 

There is no 'fast' way to explain the callbacks. Refer to the section on [callbacks](#callbacks) for more information.

## Further Info 

If you have further questions from the FAST way, see the relevant portions below. You can also post for help in the [Development section of the XAYA forums](https://forum.xaya.io/forum/6-development/).

********

# Tutorial Requirements

For this tutorial, you'll need several pieces of software:

- Unity
- Visual Studio
- VS Class Diagram
- [Mover Unity projects](https://github.com/xaya/xayalib_unity)
- Knowledge of Unity

Visual Studio no longer ships with Class Diagram. To get it, type "class diagram" into the Quick Launch in the upper-left corner of Visual Studio and search. It will return a link to install VS Class Diagram. 

[The Mover Unity projects](https://github.com/xaya/xayalib_unity) contain all the code for this tutorial. 

This tutorial doesn't delve into explaining Unity elements. You should have a basic understanding of Unity already. If not, you will need to read the code, explore, and search online for information about Unity.

# How Mover is Structured in Unity

This Unity implementation of Mover is structured as illustrated below.

![Structure](img/xaya-mover-unity-structure.png)

<font color=red>Red</font> signifies "black box" code. You don't need to change anything here. Simply compile it and add the reference or copy the DLL.

Yellow signifies code that you can edit if you wish.

<font color=green>Green</font> signifies code that you must write in its entirety. This is YOUR game.

## BitcoinLib

Starting from the bottom of the diagram, we have BitcoinLib. It's the RPC library used in this tutorial. It's referenced in:

- XAYAWrapper
- XAYAClient
- XAYAConnector

We'll examine what's being done when we get to that code. The BitcoinLib code is included in the download. You can edit it as you wish to add in more RPC methods. (See the [XAYA RPC Methods](XAYA%20RPC%20Methods.md) and [Interacting with the XAYA Wallet Through RPC in C#](RPC%20Windows%20C%23%20Tutorial/README.md) for more information.)

## XAYAWrapper

XAYAWrapper wraps the statically linked libxayagame library. You need only add a reference to this in your own projects. Full C# source code is provided. libxayagame is written in C++ and can be found [here](https://github.com/xaya/libxayagame). 

For our purposes, this is a "black box" until we look at the XAYAMoverGame namespace where we implement several libxayagame callbacks. 

## XAYAConnector

This connects to and disconnects from `XAYAWrapper`. It gets data through RPC (BitcoinLib) and updates information for `MoveGUIAndGameController` so that `MoveGUIAndGameController` can update the UI. This will be examined in more depth [later](#using-settings-to-start-xayawrapper). 

## XAYAClient

This is used for some RPC calls through BitcoinLib, and more specifically to get a list of XAYA names in the user's wallet and to send moves to the XAYA blockchain. 

It also sets the XAYAConnector to subscribe for updates from libxayagame. Those updates that XAYAConnector receives, as mentioned above, are then asynchronously updated in the front end, i.e. MoveGUIAndGameController. This will be examined in more depth [later](#connecting-xayaclient). 

## XAYAMoverGame

It is up to you to write this as this is the core game logic. We'll examine this in great detail [below](#game-logic) and explain the callbacks extensively. 

## XAYA Unity - MoveGUIAndGameController

This is your front end. It launches and disconnects from XAYAConnector. It uses XAYAClient to get a list of XAYA names from the user's wallet and to send moves to the blockchain.

It uses XAYAMoverGame for the GameState, which is used to update the UI for each block where there are moves. 

********

# The Projects

As above, there are 3 projects:

- BitcoinLib (RPC library)
- XAYAWrapper (wraps libxayagame)
- XAYAUnity (the game)

We'll look at the first 2 very briefly then dive into the lovely goodness of actual game coding.

# BitcoinLib

We've already explained the purpose of BitcoinLib above, but should mention that you can very easily extend it. In particular, see these files:

- BitcoinLib\Services\RpcServices\RpcService\IRpcService.cs
- BitcoinLib\Services\RpcServices\RpcService\RpcService.cs

Scroll to the bottom and you'll see how new functionality can be easily added. For more information on XAYA RPCs, refer to [XAYA RPC Methods](XAYA%20RPC%20Methods.md) and [Interacting with the XAYA Wallet Through RPC in C#](RPC%20Windows%20C%23%20Tutorial/README.md).

# XAYAWrapper

XAYAWrapper wraps libxayagame and exposes several fields and methods.

![XAYAWrap Class Diagram](img/XAYAWrapperClassDiagram.png)

The imporant fields are:

- initialCallback
- forwardCallback
- backwardsCallback
- xayaGameService

We'll examine the [callbacks](#Callbacks) later on when we look at the [game logic](#game-logic). In order to use libxayagame, this is perhaps the most important part to understand.

xayaGameService is part of BitcoinLib and can be used to send RPC calls. While we've used BitcoinLib for RPCs, you can choose any RPC library that you prefer. 

There are 4 methods:

- `Connect`: Connects to the daemon
- `ShutdownDaemon`: Stops the daemon
- `Stop`: Stops BitcoinLib 
- `XayaWrapper`: Constructor

## Wiring Up XAYAWrapper

Wiring up XAYAWrapper is very easy, but it must be done in 2 threads. 

1\. Instantiate a XAYAWrapper as a member variable as seen in XAYAConnector:

	public XayaWrapper wrapper;

2\. Call it's constructor as in XAYAConnector (this is done in a thread):

	wrapper = new XayaWrapper(dPath, 
		MoveGUIAndGameController.Instance.host_s, 
		MoveGUIAndGameController.Instance.gamehostport_s, 
		ref functionResult, 
		CallbackFunctions.initialCallbackResult, 
		CallbackFunctions.forwardCallbackResult,
		CallbackFunctions.backwardCallbackResult);

The constructor's signature is:

	public XayaWrapper(string dataPath, 
		string host_s, 
		string gamehostport_s,  
		ref string result, 
		InitialCallback inCal, 
		ForwardCallback forCal, 
		BackwardCallback backCal)

- dataPath: The path to the libxayagame DLL and its dependencies, i.e. the "XayaStateProcessor" folder
- host_s: The URL to connect to, e.g. http://user:password@127.0.0.1:8396
- gamehostport_s: This is 8900
- result: A string that tells you if the wrapper initialised ok or an error message
- inCal: The InitialCallback callback that you've written
- forCal: The ForwardCallback callback that you've written
- backCal: The BackwardCallback callback that you've written

3\. Connect as in XAYAConnector (same thread as #2):

        functionResult = wrapper.Connect(dPath, 
		FLAGS_xaya_rpc_url, 
		MoveGUIAndGameController.Instance.gamehostport_s, 
		MoveGUIAndGameController.Instance.chain_s.ToString(), 
		MoveGUIAndGameController.Instance.GetStorageString(
			MoveGUIAndGameController.Instance.storage_s), 
		"mv", 
		dPath + "\\..\\XayaStateProcessor\\database\\", 
		dPath + "\\..\\XayaStateProcessor\\glogs\\" );


The Connect signature is:

	public string Connect(string dataPath, 
		string FLAGS_xaya_rpc_url, 
		string gamehostport_s, 
		string chain_s, 
		string storage_s, 
		string gamenamespace, 
		string databasePath, 
		string glogsPath)

- dataPath: Unused. You can use this to modify the wrapper if you wish
- FLAGS_xaya_rpc_url: The URL to connect to, e.g. http://user:password@127.0.0.1:8396
- gamehostport_s: 8900
- chain_s: This is the network to use: MAIN, TESTNET, or REGTEST
- storage_s: One of "sqlite", "lmdb", or "memory"
- gamenamespace: The game name. This is "mv" for Mover
- databasePath: The path to the sqlite or lmdb database
- glogsPath: The path to glog

We'll look at [getting data](#subscribeforblockupdates) (new game states) from libxayagame below.

**********

# XAYAUnity

The XAYAUnity project is where the game is written. There are 8 files that we should look at. They fall into 4 categories:

1. Core game files
	- CallbackFunctions.cs
	- HelperFunctions.cs
	- JSONClasses.cs
2. "Wiring up" and utility files
	- XAYAClient.cs
	- XAYAConnector.cs
3. Front end
	- MoveGUIAndGameController.cs
4. Ancilliary examples
	- MoverObject.cs

Rather than follow a file-by-file approach, we'll instead do our examination by starting with fundamental elements. We'll then progress through the Mover game code, gradually adding new elements. 

Here we'll look at:

1. [Connection Settings](#Connection-Settings-for-XAYAWrapper)
	1. [Saving Settings](#Saving-Settings)
	2. [Loading Settings](#Loading-Settings)
	3. [Using Settings to Start XAYAWrapper](#Using-Settings-to-Start-XAYAWrapper)
		1. [Starting XAYAWrapper](#Starting-XAYAWrapper)
		2. [Starting XAYAWrapper in a Separate Thread](#Starting-XAYAWrapper-in-a-Separate-Thread)
	4. [Disconnecting XAYAWrapper](#Disconnecting-XAYAWrapper)
2. [Connecting XAYAClient](#Connecting-XAYAClient)
3. [Getting and Populating Player Names](#Getting-and-Populating-Player-Names)
	1. [Getting the Names](#Getting-the-Names)
4. [Making a Move](#Making-a-Move)
5. [SubscribeForBlockUpdates](#SubscribeForBlockUpdates)
6. [Game Logic](#game-logic)
	1. [JSONClasses](#JSONClasses)
	2. [HelperFunctions](#HelperFunctions)
	3. [Callbacks](#Callbacks)
		1. [initialCallbackResult](#initialCallbackResult)
		2. [forwardCallbackResult and Processing Moves](#forwardCallbackResult-and-Processing-Moves)
			1. [A Quick Look at Moves](#A-Quick-Look-at-Moves)
			2. [The First Loop](#The-First-Loop)
			3. [The Second Loop](#The-Second-Loop)
		3. [backwardCallbackResult and Undoing a Game State Step](#backwardCallbackResult-and-Undoing-a-Game-State-Step)
7. [Update the Front End with a GameState](#Update-the-Front-End-with-a-GameState)


# Connection Settings for XAYAWrapper

In `MoveGUIAndGameController`, there are several inputs for the XAYAWrapper's connection settings. In your own game, you won't have inputs like this for your users. Instead, you'll have other code to get that information. The inputs here are for demonstration purposes.

Those inputs appear in the Unity designer as illustrated below.

![Settings](img/Unity-connection-settings.png)

When running and filled in, those settings will appear as illustrated below.

![Settings filled in](img/Unity-connection-settings-filled-in.png)

These settings are used with XAYAWrapper as we'll see below.

How these settings make their way to XAYAWrapper follows this path:

1. Settings are saved as member variables in `MoveGUIAndGameController`
2. `XAYAConnector.LaunchMoverStateProcessor` sets 2 additional parameters and starts a [coroutine](https://docs.unity3d.com/ScriptReference/MonoBehaviour.StartCoroutine.html) with `StartEnum` in order to continue starting the XAYAWrapper
3. `XAYAConnector.StartEnum` uses [Ciela Spike's Thread Ninja](https://assetstore.unity.com/packages/tools/thread-ninja-multithread-coroutine-15717) to start an asynchronous coroutine with `DaemonAsync`
4. `XAYAConnector.DaemonAsync` finally constructs the `XAYAWrapper` (`wrapper`) and calls its `Connect` method

The following walks through those steps all the way from initially getting the settings to finally connecting and disconnecting the XAYAWrapper.

## Saving Settings

The APPLY button saves the settings (`OnButton_SettingsSave` in `MoveGUIAndGameController`). 

    public void OnButton_SettingsSave()
    {
        PlayerPrefs.SetString("host",host.text);
        PlayerPrefs.SetString("hostport", hostport.text);
        PlayerPrefs.SetString("tcpport", gameport.text);
        PlayerPrefs.SetString("rpcuser", rpcuser.text);
        PlayerPrefs.SetString("rpcpassword", rpcpassword.text);
        PlayerPrefs.SetInt("storage", storage.value);
        PlayerPrefs.SetInt("chain", chain.value);
        PlayerPrefs.Save();

        FillSettingsFromPlayerPrefs();
    }

## Loading Settings

The `FillSettingsFromPlayerPrefs` method sets connection member variables from those settings.

    void FillSettingsFromPlayerPrefs()
    {
        host_s = PlayerPrefs.GetString("host", "http://127.0.0.1");
        hostport_s = PlayerPrefs.GetString("hostport", "8396");
        gamehostport_s = PlayerPrefs.GetString("tcpport", "8900");
        rpcuser_s = PlayerPrefs.GetString("rpcuser", "xayagametest");
        rpcpassword_s = PlayerPrefs.GetString("rpcpassword", "xayagametest");
        storage_s = PlayerPrefs.GetInt("storage", 0);
        chain_s = PlayerPrefs.GetInt("chain", 0);
    }

## Using Settings to Start XAYAWrapper

Starting and stopping the XAYAWrapper begins in the OnButton_DaemonLaunch method, but it is the XAYAConnector that actually starts the XAYAWrapper. 

    public void OnButton_DaemonLaunch()
    {
        if (btnLaunchText.text != "STOP") // This is the "LAUNCH" button.
        {
            xayaConnector.LaunchMoverStateProcessor();
            btnLaunchText.text = "STOP";
        }
        else
        {   
            xayaConnector.Disconnect();
        }
    }

The method to disconnect XAYAWrapper is in the button code as well. See [Disconnecting XAYAWrapper](#Disconnecting-XAYAWrapper) below for how that is done.

### Starting XAYAWrapper

Here you can see the settings being used. Starting XAYAWrapper begins in the `XAYAConnector.LaunchMoverStateProcessor` method.

    public void LaunchMoverStateProcessor()
    {
        Instance = this;
        dPath = Application.dataPath;
        FLAGS_xaya_rpc_url = MoveGUIAndGameController.Instance.rpcuser_s + ":" 
		+ MoveGUIAndGameController.Instance.rpcpassword_s + "@" 
		+ MoveGUIAndGameController.Instance.host_s + ":" 
		+ MoveGUIAndGameController.Instance.hostport_s;
        // Clean last session logs
        if (Directory.Exists(dPath + "\\..\\XayaStateProcessor\\glogs\\"))
        {
            DirectoryInfo di = new DirectoryInfo(dPath + "\\..\\XayaStateProcessor\\glogs\\");
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }
        StartCoroutine(StartEnum());
    }

In order to prevent UI thread blocking, the XAYAWrapper must be in a separate thread. This begins with `StartCoroutine(StartEnum())`. 

#### Starting XAYAWrapper in a Separate Thread

Actually starting the wrapper is done in a separate thread beginning with `StartCoroutine(StartEnum())`. 

    IEnumerator StartEnum()
    {
        Task task;
        this.StartCoroutineAsync(DaemonAsync(), out task);
        yield return StartCoroutine(task.Wait());

        if (task.State == TaskState.Error)
        {
            MoveGUIAndGameController.Instance.ShowError(task.Exception.ToString());
            Debug.LogError(task.Exception.ToString());
        }
    }

This starts `DaemonAsync` in a thread. We've already seen some of this code when we looked at wiring up [XAYAWrapper](#XAYAWrapper) above.

    IEnumerator DaemonAsync()
    {
        string functionResult = "";

        wrapper = new XayaWrapper(dPath, MoveGUIAndGameController.Instance.host_s, 
		MoveGUIAndGameController.Instance.gamehostport_s, 
		ref functionResult, 
		CallbackFunctions.initialCallbackResult, 
		CallbackFunctions.forwardCallbackResult,
		CallbackFunctions.backwardCallbackResult);

        yield return Ninja.JumpToUnity;
        Debug.Log(functionResult);
        yield return Ninja.JumpBack;

        functionResult = wrapper.Connect(dPath, 
		FLAGS_xaya_rpc_url, 
		MoveGUIAndGameController.Instance.gamehostport_s, 
		MoveGUIAndGameController.Instance.chain_s.ToString(), 
		MoveGUIAndGameController.Instance.GetStorageString(
			MoveGUIAndGameController.Instance.storage_s), 
		"mv", 
		dPath + "\\..\\XayaStateProcessor\\database\\", 
		dPath + "\\..\\XayaStateProcessor\\glogs\\" );

        yield return Ninja.JumpToUnity;
        Debug.Log(functionResult);
        yield return Ninja.JumpBack;

        Debug.Log("Check if fatal?");

        CheckIfFatalError();
    }

`wrapper.Connect` is called and if all goes well, we're successfully connected to libxayagame. 

### Disconnecting XAYAWrapper

The XAYAConnection `Disconnect` method is:

    public void Disconnect()
    {
        StartCoroutine(TryAndStop());
        Instance = null;
    }

It calls TryAndStop asynchronously:

    IEnumerator TryAndStop()
    {
        if (wrapper != null)
        {
            wrapper.Stop();
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(TryAndStop());
        }
    }
    
That method recursively tries to stop the connection to the wrapper. 

# Connecting XAYAClient

The CONNECT button in the front end calls the `ConnectClient` method. This method does 3 things:

1. Connects `xayaClient`

	xayaClient.Connect()

2. Submits moves 

	ShowError(xayaClient.ExecuteMove(nameSelected, 
		DirectionDropdownToMoverDir(directionSelected), distanceSelected));

3. Toggles between "CONNECT" and "MOVE!"

When `xayaClient` connects, it:

1. Creates a XAYAService (from BitcoinLib) for RPC calls 
2. Subscribes the XAYAConnector to the wrapper to listen for updates.

Connecting `xayaService` is done in its constructor:

	xayaService = new XAYAService(MoveGUIAndGameController.Instance.host_s + ":" 
		+ MoveGUIAndGameController.Instance.hostport_s + "/wallet/game.dat", 
		MoveGUIAndGameController.Instance.rpcuser_s, 
		MoveGUIAndGameController.Instance.rpcpassword_s, 
		"", 
		10);

If the connection is successful, `XAYAClient` calls:

	connector.SubscribeForBlockUpdates();

This subscribes the XAYAConnector instance to updates from libxayagame (`XAYAWrapper` or `wrapper`). This happens in a separate thread to prevent blocking. See [SubscribeForBlockUpdates](#SubscribeForBlockUpdates) below for how this is done. 

# Getting and Populating Player Names

Once connected, we can get a list of names from the user's wallet and populate the name list drop down menu.

![Getting Names](img/Populate-names.png)

`FillNameList` gets and populates the menu.

    public void FillNameList()
    {
        nameList = xayaClient.GetNameList();

        playernamelist.ClearOptions();
        playernamelist.AddOptions(nameList);

        if(nameList.Count > 0 && nameSelected.Length <= 1)
        {
            nameSelected = nameList[0];
        }
    }

There is no error checking there for the sake of simplicity. However, in general you should check that:

* You only use names in the `p/` namespace
* You ensure that elements of your game properly handle long or otherwise complex names
* You filter for script injection attacks

## Getting the Names

`xayaClient.GetNameList` returns a `List<string>` of names. 

    public List<string> GetNameList()
    {
        List<string> allMyNames = new List<string>();

        List<GetNameListResponse> nList = xayaService.GetNameList();

        foreach(var nname in nList)
        {
            if (nname.ismine == true)
            {
                allMyNames.Add(nname.name);
            }
        }

        return allMyNames;
    }

The `xayaService.GetNameList` is the RPC call to the XAYA wallet to get the names. 

With the name list populated, users can select a name and play the game. 

# Making a Move

Users choose a direction and a number of steps to take. When they click `MOVE!`, their move is submitted to the XAYA blockchain, as mentioned above under [Connecting XAYAClient](#Connecting-XAYAClient). 

	ShowError(xayaClient.ExecuteMove(nameSelected, DirectionDropdownToMoverDir(directionSelected), distanceSelected));

The `ExecuteMove` method is:

    public string ExecuteMove(string playername, string direction, string distance)
	{   
         return xayaService.NameUpdate(
		playername, 
		"{\"g\":{\"mv\":{\"d\":\"" + direction + "\",\"n\":" + distance + "}}}", 
		new object()); 		
	}

It uses xayaService (from BitcoinLib) to send a `name_update` RPC to the XAYAWallet. The wallet then broadcasts the `name_update` to the XAYA network and a miner somewhere in the world then mines the transaction into the blockchain. Once that's done, XAYAWrapper (libxayagame) picks up all the moves for all players and passes that data to the XAYAConnector, which then asynchronously updates member variables in MoveGUIAndGameController so that the front end can update itself for the new game state.

Here are a couple example moves:

	{"g":{"mv":{"d":"n","n":1}}}
	{"g":{"mv":{"d":"k","n":5}}}

- g: This indicates the game namespace
- mv: This is the XAYA game name for "Mover"
- d: This is a direction. See game logic for more information
- n: This is the number of steps to take

# SubscribeForBlockUpdates

As mentioned above, when we connect the XAYAClient, it subscribes the XAYAConnector to XAYAWrapper in a new thread. The XAYAConnector then listens for updates from XAYAWrapper (libxayagame) and asynchronously updates member variables in MoveGUIAndGameController so that it can update the front end with the new game state.

The path for this begins in `XAYAClient.Connect`:

	connector.SubscribeForBlockUpdates();

The `SubscribeForBlockUpdates` method starts a coroutine with `WaitForChanges`:

	StartCoroutine(WaitForChanges());

`WaitForChanges` uses Thread Ninja to start a new thread with `WaitForChangesInner`:

    IEnumerator WaitForChanges()
    {
        Task task;
        this.StartCoroutineAsync(WaitForChangesInner(), out task);
        yield return StartCoroutine(task.Wait());
    }

`WaitForChangesInner` runs a `while(true)` loop. The portion we are concerned with is:

	wrapper.xayaGameService.WaitForChange();

	GameStateResult actualState = wrapper.xayaGameService.GetCurrentState();

	if (actualState != null)
	{
		if (actualState.gamestate != null)
		{
		    GameState state = JsonConvert.DeserializeObject<GameState>(actualState.gamestate);

		    MoveGUIAndGameController.Instance.state = state;
		    MoveGUIAndGameController.Instance.totalBlock = client.GetTotalBlockCount();
		    var currentBlock = client.xayaService.GetBlock(actualState.blockhash);
		    MoveGUIAndGameController.Instance._sVal = currentBlock.Height;

		    MoveGUIAndGameController.Instance.needsRedraw = true;

In there the XAYAWrapper (`wrapper`) waits for a change and when one happens, it gets the current state as a `GameStateResult`. The `GameStateResult` is then deserialised as a `GameState`.

Member variables of `MoveGUIAndGameController` are then set. Most importantly:

- state: The `GameState`
- needsRedraw: A flag that tells the front end to update itself with the new `state` value

# Game Logic

At long last we arrive at the game logic. It is in the `XAYAMoverGame` namespace and contained in 3 files:

- JSONClasses.cs
- HelperFunctions.cs
- CallbackFunctions.cs

The following is a class diagram of `XAYAMoverGame`.

![XAYA Mover Game Class Diagram.png](img/XAYAMoverGameClassDiagram.png)

## JSONClasses

JSONClasses.cs has the core classes for game elements.

- Direction: This is the direction for the player to move
- PlayerState: This tells where the player is on the map, their direction, and the number of steps left for them to take
- GameStateResultFromResponse: This is for receiving a GameStateResult. It has been implemented in BitcoinLib as `GameStateResult` to illustrate another technique 
- GameState: This is a dictionary of all players, i.e. their `PlayerState`s. `GameState` is used to update the front end
- PlayerUndo: This is the undo information for a single player
- UndoData: This is a `Dictionary` of `PlayerUndo`s. It is used to rewind the current game state by 1 block

Because a user could encounter a bad block or require a reorg, we need to keep undo data so that we can "rewind" the game if needed. You can think of this as a user being sucked into an alternate reality that's not compatible with the game. Undoing (or rewinding) then brings them back into reality, i.e. a valid game state. 

We'll examine rewinding when we look at the [backwardCallbackResult](#backwardCallbackResult).

## HelperFunctions

Our HelperFunctions class contains static methods that we’ll use in the game logic. Note that for some we have return values from parameters that we pass in by reference, i.e. ref type var.

* `ParseMove`: Takes a JSON object, sets some parameters, and returns true if the move is valid
* `ParseDirection`: Takes a string and returns a Direction enum
* `GetDirectionOffset`: Takes a `Direction` `enum` then sets an x and y offset for that direction
* `DirectionToString`: Takes a `Direction` `enum` and returns plain English for a valid direction or an empty string

As there's nothing particularly special in this class, further examination of it is left to the reader to pursue on their own. The only remaining point that should be made is that there should be thorough error checking, and particularly for data received through the blockchain, which in this case would be the `JObject` passed to `ParseMove`. See the error checking in that method for an example. 

To make the case for extreme error checking, consider that anyone could issue a `name_update` operation through the daemon or XAYA QT wallet console. That data would be entirely arbitrary. Each and every bit of data from the blockchain **MUST** be checked. While normal people just want to play the game, there are some people that just want to see if they can break things. You must guard against them. For example, someone could issue a `name_update` like so:

	{ "g": { "mv": { "d": "Dr. Evil", "n": "1 million dollars!" } } }

This is obviously an invalid move for our Mover game. As such, it is critically important to ensure that you do proper error checking and exclude invalid moves.

## Callbacks

If you recall from above, these callbacks are implementations of the callbacks in libxayagame (XAYAWrapper). 

- initialCallbackResult: 
- forwardCallbackResult: 
- backwardCallbackResult: 

This is where the main game logic resides. 

***

# initialCallbackResult

The initialCallbackResult reads which chain we plan to use, then sets the height to start at, and the hash for that block in hexadecimal. It’s very straight forward.

	public static string initialCallbackResult(out int height, out string hashHex)
	{
	    if (Program.chainType == 0)
	    {
		height = 125000;
		hashHex = "2aed5640a3be8a2f32cdea68c3d72d7196a7efbfe2cbace34435a3eef97561f2";
	    }
	    else if (Program.chainType == 1)
	    {
		height = 10000;
		hashHex = "73d771be03c37872bc8ccd92b8acb8d7aa3ac0323195006fb3d3476784981a37";
	    }
	    else
	    {
		height = 0;
		hashHex = "6f750b36d22f1dc3d0a6e483af45301022646dfc3b3ba2187865f5a7d6d83ab1";
	    }

	    return "";
	}

We must know what block we should start reading at. There’s no sense in reading blocks prior to a game’s existence, so you should set your `height` to the block where your game first went "live".

For the hashHex, to find out a block hash, you can use the official XAYA explorer available at https://explorer.xaya.io/. As an example, this is block zero (0), also known as the genesis block:

https://explorer.xaya.io/block/0

Its hash is “e5062d76e5f50c42f493826ac9920b63a8def2626fd70a5cec707ec47a4c4651”.

***

# forwardCallbackResult and Processing Moves

First, we must clarify some language used here. While the name of the game is "Mover", and players "move" in the game, when we talk about a moving or non-moving player, this has nothing to do with a player moving on the map in the Mover game. 

**MOVING** (or "moving") means that the player has a set of move orders that are active.

**NON-MOVING** (or "non-moving) means that the player does not currently have a set of move orders.

A "move" or "move orders" are whatever instructions the player has told the game to do. These instructions/orders are sent to the XAYA blockchain in a XAYA name_update as a value. (See [A Quick Look at Moves](#A-Quick-Look-at-Moves).) We receive those orders as a "move" through the `blockData` parameter. 

For example, the following:

> Verify that the move is valid.
>
> This is how moves are processed.

are equivalent to:

> Verify that the player's orders are valid.
>
> This is how orders are processed.

To be more specific, the `blockData` parameter returns `moves` which is an array of `move` data. Here's one example:

	{
	  "block": {
		"hash": "dda7eccde4857742e5000bd66cf72154ce26c22876582654bc8b8d78dadbce8c",
		"height": 558369,
		"parent": "18f72c91c7b9223e9c7d0525216277e4016d748a2c81be4ba9d4a2b30eaed92d",
		"rngseed": "b36747498ce183b9da32b3ab6e0d72f2a17aa06859c08cf1d1e91907cb09dddc",
		"timestamp": 1549056526
	  },
	  "moves": [
		{
		  "move": {
			"m": "Hello world!"
		  },
		  "name": "ALICE",
		  "out": {
			"CMBPmRos5QADg2T8kvkQhMaMV5WzpzfedR": 3443.7832612
		  },
		  "txid": "edd0d7a7662a1b5f8ded16e333f114eb5bea343a432e6c72dfdbdcfef6bf4d44"
		}
	  ],
	  "reqtoken": "1fba0f4f9e76a65b1f09f3ea40a59af8"
	}

As such, when we say "moves" or "move", it is that data in `blockData` that we are referring to.

# Moving On... (ok, bad pun)

`forwardCallbackResult` runs whenever a new block is received. It processes the moves (or game logic) to create a new game state and creates undo data. Let&#39;s examine it in detail.

In the callback there are general tasks that need to be done.

1. [**Get data** that's passed in into variables](#get-data)
2. [**Check errors** for the game state and players. Construct them if they're null. That only ever happens once](#check-errors)
3. [**Update moves**, i.e. Loop over all **new moves for each player**](#update-moves)
	1. [Verify the move is valid. If so, assign it in a variable](#verify-move-is-valid)
	2. [Update the player state from the previous block data](#update-player-state)
	3. [Create player undo data for all **MOVING** players](#Create-Player-Undo-for-Moving-Players)
	4. [Update the player's move in the player state](#Update-New-Move-in-Player-State)
4. [**Process moves**, i.e. Loop over **each player state**](#Process-Moves) 
	1. [Check if the player is moving](#Check-Whether-Player-is-Moving)
	2. [Process the move for the player](#Process-the-Move)
	3. [Add player undo data for the player if they have just completed their moves, i.e. they are now stationary or non-moving players](#Add-Player-Undo-Data-for-Non-Moving-Players)
5. [**Update the new game state and new undo data** then return them](#Update-New-Game-State-and-New-Undo-Data)

# Get Data

Here&#39;s the signature:

	public static string forwardCallbackResult(string oldState, 
		string blockData, 
		string undoData, 
		out string newData)

- `oldState`: This string contains the game state as it currently is
- `blockData`: This contains all the new moves that have come in from the blockchain
- `undoData`: This is the undo data that will be created. This is the return value of the callback
- `newData`: This is an out parameter and will store the updated game state

First, we deserialise the `oldState` JSON string as a `GameState` object. Remember that most of our string data like this is actually JSON.

	GameState state = JsonConvert.DeserializeObject<GameState>(oldState);

Similarly, we deserialise the block we received from the XAYA daemon as a `dynamic` type.

	dynamic blockDataS = JsonConvert.DeserializeObject(blockData);

We&#39;ll be creating undo data to hedge against the possibility of encountering a fork/reorg, so we initialise a `Dictionary` for that with the `PlayerUndo` type.

	Dictionary<string, PlayerUndo> undo = new Dictionary<string, PlayerUndo>();

# Check Errors

It&#39;s possible that there are no moves for us to process, so we check for that and if there are no new moves, we simply exit the method.

	if (blockData.Length <= 1)
	{
	    newData = "";
	    return "";
	}

While we&#39;re developing our example game, it&#39;s nice to have console feedback. This would be commented out or removed in our final release.

	Console.WriteLine("Got new forward block at height: " + blockDataS["block"]["height"]);

If this is the first move of the game, then we should create a new instance of our game.

	if (state == null)
	{
	    state = new GameState();
	}

If you remember from above in JSONClasses.cs, our `GameState` class merely contains a `Dictionary` of `PlayerStates`.

	public class GameState
	{
	    public Dictionary<string, PlayerState> players;
	}

So for the `players` property of our `GameState`, if it&#39;s null, then we should initialise it.

	if (state.players == null)
	{
	    state.players = new Dictionary<string, PlayerState>();
	}

Let&#39;s remind ourselves about the players property being a `PlayerState`. Again, that is found in the JSONClasses.cs file.

	public class PlayerState
	{
	    public int x;
	    public int y;
	    public Direction dir = Direction.UP;
	    public Int32 steps_left;
	}

That completes the basic setup and initialisation for us to process a move.

# Update Moves

The rest of our game logic consists of 2 loops:

- A loop to get moves and create undo data for moving players
- A loop to process moves and create undo data for non-moving players

We then set our game state and undo data variables and return them.

## A Quick Look at Moves

Before proceeding, let&#39;s look at what a typical move will look like for any given name that wishes to create that move.

	{
	  "g": {
	    "mv": {
	      "d": "u",
	      "n": 10
	    }
	  }
	}

Or, as a single line:

	{ "g": { "mv": { "d": "u", "n": 10 } } }

The `g`; tells us that we&#39;re in the game name namespace for the XAYA blockchain. Inside of that, the first element, `mv`, tells us that this `name_update` is for our Mover example game, i.e. the XAYA name for Mover is &quot;mv&quot;. Inside of `mv` is a move. `d` is the direction, which will be resolved by our `HelperFunctions.ParseDirection` method. `n` is the number of steps to take. (&quot;u&quot; is `Direction.RIGHT_UP`.)

Moves are done through the `name_update` operation in the XAYA daemon. It&#39;s possible for people to issue these `name_update`s through the XAYA QT wallet or directly into the daemon with arbitrary data. For example, someone could issue a `name_update` like so:

	{ "g": { "mv": { "d": "Dr. Evil", "n": "1 million dollars!" } } }

This is obviously an invalid move for our Mover game. As such, it is critically important to ensure that you do proper error checking and exclude invalid moves.

## The First Loop

Let&#39;s look into our first loop inside `forwardCallbackResult`.

	foreach (var m in blockDataS["moves"])

Here, `blockDataS` contains many moves that we will iterate over, storing each one as a `var` in `m`.

First, we extract the player&#39;s name from `m`.

	string name = m["name"].ToString();

Next, we put the move into a `JObject` that we will pass to `ParseMove` to verify. Note that we&#39;re using the Newtonsoft JSON library here.

	JObject obj = JsonConvert.DeserializeObject<JObject>(m["move"].ToString());

All moves have a direction and a number of steps to take, so we initialise a couple variables to hold those values. The initial values are arbitrary and will change in `ParseMove`.

	Direction dir = Direction.UP;
	Int32 steps = 0;

## Verify Move is Valid

As stated above, error checking is critical. Our `ParseMove` method will determine if a move is valid or not, and will update values for the parameters we pass in as they are being passed by reference (ref). In particular, we&#39;ll be using the values for `dir` and `steps` later on.

	if (!HelperFunctions.ParseMove(ref obj, ref dir, ref steps))
	{
	    continue;
	}

If the move isn&#39;t valid, we `continue`, i.e. we stop where we are in the loop and start over with the next move (`m`) inside of our `blockDataS` object.

## Update Player State

We need a `PlayerState`, so we allocate memory for one.

	PlayerState p;

It&#39;s important to know whether we have an existing name (game account) or if this player is already in the game. In our first step above, we assigned a value to our string variable `name`. Here we check to see if it already exists in our `GameState` object, `state`.

	bool isNotNew = state.players.ContainsKey(name);

If it exists, then we set our `PlayerState` object (`p`) to that name. If not, we initialise our `PlayerState` `p` as a new instance of a `PlayerState` and then add it to our `GameState` (`state`).

	if (isNotNew)
	{
	    p = state.players[name];
	}
	else
	{
	    p = new PlayerState();
	    state.players.Add(name, p);
	}

At this point, the player has been added to the game state, but we've not yet processed the move.

## Create Player Undo for Moving Players

Here we create player undo data for **MOVING** players. 

In the second loop, we'll add those players that have just completed their move, i.e. they are now **NON-MOVING** players. We can't add the non-moving players here because we process moves in the second loop. 

We must create undo data for each player, so we initialise a new instance of `PlayerUndo`.

	PlayerUndo u = new PlayerUndo();

We&#39;ve not changed the `PlayerState` yet, so what we have in `p` originally comes from our `oldState` parameter, which we deserialised as `state`. We must preserve this as undo data, so we add it to our `undo` `Dictionary`.

	undo.Add(name, u);

If we have a new player, then we set the `is_new` property of our `PlayerUndo` object to `true` and update our `PlayerState` (`p`) to place the player on the map at the origin, i.e. (0, 0).

Otherwise, we update the `previous_dir` and `previous_steps_left` with the current values in our `PlayerState` (`p`).

	if (!isNotNew)
	{
	    u.is_new = true;
	    p.x = 0;
	    p.y = 0;
	}
	else
	{
	    u.previous_dir = p.dir;
	    u.previous_steps_left = p.steps_left;
	}

## Update New Move in Player State

Finally, we update our `PlayerState` (`p`) with the new direction and number of steps left. Recall from above that we obtained these values when we called the `HelperFunctions.ParseMove` method with `dir` and `steps` being passed in by reference. Refer to the `ParseMove` method for how this is done.

	p.dir = dir;
	p.steps_left = steps;

That completes our first loop. To summarize what we did here:

1. We initialised variables
2. We checked to see if we had a valid move (this updated values for us)
3. We determined if we had a new or existing player and updated as required
4. We saved the `PlayerState` as undo data and stored it in our `undo` object
5. We finally updated the move in our `PlayerState` (this did not process the move - see below for that)
6. We looped back and did 1-5 for all **moves**

# Process Moves

Our second loop iterates over each player state to process the move that was added to the player state above, and to add undo data for players that are no longer moving.

## The Second Loop

The second loop iterates over all players. Here&#39;s the loop declaration:

	foreach (var mi in state.players)

For each player, we get the name and `PlayerState` into variables.

	string name = mi.Key;
	PlayerState p = mi.Value;

## Check Whether Player is Moving

If the player isn&#39;t moving, then we stop and skip back to the beginning of the loop and start again with a new player.

	if (p.dir == Direction.NONE)
	{
	    continue;
	}

Similarly for steps, if they have 0 or fewer steps to go, we skip back to the top of the loop. For situations like this, you should do error checking as people may issue commands through the QT or daemon for negative steps in a direction, which is equivalent to positive steps in the diametrically opposed direction. We&#39;re skipping those kinds of error checks here for simplicity, but you should be aware that people can issue arbitrary commands, so error checking is an absolute imperative.

	if (p.steps_left <= 0)
	{
	    continue;
	}

## Process the Move

Next, we initialise a couple integers for the player&#39;s move, then update those variables by passing them by reference to our `HelperFunctions.GetDirectionOffset` method, and update our `PlayerState` (`p`).

	Int32 dx = 0, dy = 0;
	HelperFunctions.GetDirectionOffset(p.dir, ref dx, ref dy);
	p.x += dx;
	p.y += dy;

As we&#39;ve now &quot;used&quot; that move by updating the `PlayerState`, we must decrement the number of steps left for it to go.

	p.steps_left -= 1;

If there are no steps left for that player, then we set undo data and do some cleanup.

## Add Player Undo Data for Non-Moving Players

In the first loop, we added undo data for **MOVING** players. Now we must add undo data for players that have just completed their move.

For the undo data, we check whether the player already exists in our `undo` `Dictionary` and add the player by name. If not, we create a new `PlayerUndo` and then add that to our `undo` `Dictionary` with the player&#39;s name.

To clean up, we set the `finished_dir` of the `PlayerUndo` object and set the PlayerState&#39;s `dir` property to `Direction.NONE`, i.e. there are no steps left.

	if (p.steps_left == 0)
	{
	    PlayerUndo u;
	
	    if (undo.ContainsKey(name))
	    {
	        u = undo[name];
	    }
	    else
	    {
	        u = new PlayerUndo();
	        undo.Add(name, u);
	    }
	
	    u.finished_dir = p.dir;
	    p.dir = Direction.NONE;
	}

# Update New Game State and New Undo Data

Finally, we set the `undoData` parameter and the `newData` (the new game state) parameter (that was passed by reference) and return `undoData`.

	undoData = JsonConvert.SerializeObject(undo);
	newData = JsonConvert.SerializeObject(state);
	return undoData;

#Summary

To quickly summarize `forwardCallbackResult`:

1. We **received data** and set up variables, including a `GameState`
2. We **checked for errors** and new moves
3. We **updated moves** for all players in our game state 
4. We **created undo data** for moving players in case we encounter a fork/reorg
5. We **processed all moves**
6. We **added undo data** for non-moving players 
7. We **updated our `GameState` and undo data** 
8. We **returned** our `GameState` and undo data

***

# backwardCallbackResult and Undoing a Game State Step

`backwardCallbackResult` rolls back the game state by 1 block with the undo data from the previous block. It is similar to `forwardCallbackResult`, but we don&#39;t create any undo data because we&#39;re consuming some undo data.

In a production game, you will likely want to store more undo data than just for 1 block. This allows you to have a greater buffer in the unlikely event that you discover that you&#39;ve been on a fork for more than 1 block. Remember, you can always post questions in the XAYA Development forums at [https://forum.xaya.io/forum/6-development/](https://forum.xaya.io/forum/6-development/).

Here&#39;s the method signature:

	public static string backwardCallbackResult(string newState, string blockData, string undoData)

- `newState`: Our `GameState` data
- `blockData`: This is unused in this example
- `undoData`: This is the data we use to roll back the game state by 1 block

To start, we initialise `GameState` and `UndoData` objects with deserialised data from our parameters.

	GameState state = JsonConvert.DeserializeObject<GameState>(newState);
	UndoData undo = JsonConvert.DeserializeObject<UndoData>(undoData);

Any given block can have new players join, so we need to keep track of those independently. We&#39;ll do that in a string list.

	List<string> playersToRemove = new List<string>();

We need to check each player to see if they need to be rolled back. We do this by iterating through all players in the game state.

	foreach (var mi in state.players)

To start our loop, we initialise some variables. We need to know the player&#39;s name and `PlayerState`. We get this from `mi`. A `PlayerUndo` variable is also created as null.

	string name = mi.Key;	
	PlayerState p = mi.Value;	
	PlayerUndo u;

We only need to undo a player if they exist in our undo data, so we create a boolean flag for us to use and set its value.

	undoIt = undo.players.ContainsKey(name);

The first thing to do if a player needs to be rewound, is to check if they are new players and add them to our `playersToRemove` list. We get the specific player through `undo.players[name]` and then we check the `is_new` property. Also, if the player is a new player, then we skip to the top of the loop. We&#39;ll remove the new players all at once later with our `playersToRemove` list.

	if (undoIt)
	{
	    u = undo.players[name];
	
	    if (u.is_new)
	    {
	        playersToRemove.Add(name);
	        continue;
	    }
	}

Next, if the player has not finished moving according to the undo data, i.e. their `Direction` is not `Direction.NONE`, then we must check whether or not their current direction is `NONE` and they have no steps left. If so, we set their current direction to their undo data direction.

	if (undoIt)
	{
	    u = undo.players[name];
	
	    if (u.finished_dir != Direction.NONE)
	    {
	        if (p.dir == Direction.NONE && p.steps_left == 0)
	        {
	            p.dir = u.finished_dir;
	        }
	    }
	}

Now, for all players we check if their current direction is not `NONE`. If so, we add a step and subtract the direction offset from their current position.

	if (p.dir != Direction.NONE)
	{
	    p.steps_left += 1;
	    Int32 dx = 0, dy = 0;
	    HelperFunctions.GetDirectionOffset(p.dir, ref dx, ref dy);
	    p.x -= dx;
	    p.y -= dy;
	}

To undo a player move we must set their current player state to their undo player state if our `undoIt` boolean flag is set for this player (this was set above in `bool undoIt = undo.players.ContainsKey(name);`).

So, for all players in the undo data, if their direction is not `NONE`, we set their current player state direction to the direction in the undo data. This effectively undoes their direction.

We also set their current player state steps to the number of steps in their undo data if it&#39;s not our default value of 99999999.

	if (undoIt)
	{
	    u = undo.players[name];
	
	    if (u.previous_dir != Direction.NONE)
	    {
	        p.dir = u.previous_dir;
	    }
	
	    if (u.previous_steps_left != 99999999)
	    {
	        p.steps_left = u.previous_steps_left;
	    }
	}

This effectively completes undoing the player&#39;s last move so we return back to the start of the loop, i.e.:

	foreach (var mi in state.players)

That completes our loop over the players. The only remaining step to rewind 1 block is to remove all the new players that we stored in `playersToRemove`.

	foreach (string nm in playersToRemove)
	{
	    state.players.Remove(nm);
	}

Finally, we return the serialised `GameState` object so we can update the game state.

	return JsonConvert.SerializeObject(state);

Your game will require more complex logic to undo a block, but the above should suffice to illustrate the general technique. 

***

# Update the Front End with a GameState

After creating the game logic, it's time to update the front end for the user. There are 2 important methods in `MoveGUIAndGameController`.

- Update
- RedrawGameClient

`Update` is automatically called by Unity. In it, we check to see if the game screen needs to be redrawn/updated.

    void Update()
    {
        if (needsRedraw)
        {
            needsRedraw = false;
            RedrawGameClient();
        }
    }

As we saw above, the `needsRedraw` flag is set to true whenever new information comes in from XAYAWrapper. It was set in `XAYAConnector.WaitForChangesInner`.

	MoveGUIAndGameController.Instance.needsRedraw = true;

If `needsRedraw` is `true`, it's set to `false` and `RedrawGameClient` is called.

`RedrawGameClient` is where the `GameState` (`state`) is consumed in order to process all the moves that were included in the last mined block and update the display.

Mover is very simple. To update the map, it's cleared and then each player is added inside a loop.

	foreach (KeyValuePair<string, PlayerState> pDic in state.players)

`pDic` is used to set the coordinates as the player is placed on the map.

	player.GetComponent<RectTransform>().anchoredPosition 
		= new Vector3(pWidth * pDic.Value.x, pWidth * pDic.Value.y, 0);

Most everything else in the `RedrawGameClient` method is regular code. 










