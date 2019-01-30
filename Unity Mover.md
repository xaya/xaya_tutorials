# Mover Sample Game in C# with Unity

This tutorial demonstrates how to wire up a game to run on the XAYA platform. Of particular interest, it shows how to use libxayagame and various RPCs. But most importantly, it shows how to write a simple game on the XAYA platform. Portions of this tutorial repeat portions of other tutorials. 

You'll need to download the code. It's available here. Extract the ZIP file. You'll find 3 projects inside the folder. 

1. BitcoinLib: The RPC library
2. XAYAWrapper: The libxayagame wrapper
3. XAYAUnity: The example game. Uses #1 and #2

# The FAST Way to Get Started

If you're impatient and simply want to get started, here's how. For a thorough tutorial, skip to here.

1. Compile BitcoinLib and XAYAWrapper
2. Create your game in Unity with Mover as a template
3. Write your game logic
4. Write your front end

The most important things you'll also need to know are:

- How to write the callbacks for XAYA
- How to consume a GameState

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

The game logic resides in the XAYAMoverGame namespace, which is found in HelperFunctions.cs, JSONClasses.cs, and CallbackFunctions.cs. See below for explanations of the various callbacks. This is **meat** for creating a new XAYA game. 

## 4) Write Your Front End

MoveGUIAndGameController is the front end and where all the Unity code resides. This is where you consume the `GameState`. You'll need to change the front end for your game. Take note of how the XAYAConnector and XAYAClient classes are used in the various controls in Mover. 

There are 2 methods of particular interest to get started quickly:

- `Update`
- `RedrawGameClient`

The `needsRedraw` flag in the `Update` method is set in the `XAYAConnector`. Check the `WaitForChangesInner` method for how that's done. If the screen needs to be redrawn, the `RedrawGameClient` method is called. 

Similarly for `RedrawGameClient`, the data (i.e. the `GameState`) you need to consume comes from XAYAConnector. However, that data comes from XAYAWrapper (libxayagame) but is set in the callbacks that you must write. In this case, they're in the `XAYAMoverGame` namespace, and in particular, in the `forwardCallbackResult` and `backwardCallbackResult` methods. 

There is no 'fast' way to explain the callbacks. Please see below for more information.

## Further Info 

If you have further questions from the FAST way, see the relevant portions below. 

# Tutorial Requirements

For this tutorial, you'll need several pieces of software:

- Unity
- Visual Studio
- VS Class Diagram
- MoverUnity.zip
- Knowledge of Unity

Visual Studio no longer ships with Class Diagram. To get it, type "class diagram" into the Quick Launch in the upper-left corner of Visual Studio and search. It will return a link to install VS Class Diagram. 

MoverUnity.zip contains all the code for this tutorial. 

This tutorial doesn't delve into explaning Unity elements. You should have a basic understanding of Unity already. If not, you will need to read the code, explore, and search online for information about Unity.

# How Mover is Structured in Unity

This Unity implementation of Mover is structured as illustrated below.

![Structure](img/xaya-mover-unity-structure.png)

<font color=red>Red</font> signifies "black box" code. You don't need to change anything here. Simply compile it and add the reference.

Yellow signifies code that you can edit if you wish.

<font color=green>Green</font> signifies code that you must write in its entirety. This is YOUR game.

## BitcoinLib

Starting from the bottom of the diagram, we have BitcoinLib. It's the RPC library used in this tutorial. It's referenced in:

- XAYAWrapper
- XAYAClient
- XAYAConnector

We'll examine what's being done when we get to that code. The BitcoinLib code is included in the download. You can edit it as you wish to add in more RPC methods. (See the [XAYA RPC Methods](XAYA%20RPC%20Methods.md) and [Interacting with the XAYA Wallet Through RPC in C#](RPC%20Windows%20C%23%20Tutorial/XAYA%20RPC%20Tutorial.md) for more information.)

## XAYAWrapper

XAYAWrapper wraps the static libxayagame library. You need only add a reference to this in your own projects. Full C# source code is provided. libxayagame is written in C++ and can be found [here](https://github.com/xaya/libxayagame). 

For our purposes, this is a "black box" until we look at the XAYAMoverGame namespace where we implement several libxayagame callbacks. 

## XAYAConnector

This connects to and disconnects from `XAYAWrapper`. It gets data through RPC (BitcoinLib) and updates information for `MoveGUIAndGameController` so that `MoveGUIAndGameController` can update the UI. This will be examined in more depth later. 

## XAYAClient

This is used for some RPC calls through BitcoinLib, and more specifically to get a list of XAYA names in the user's wallet and to send moves to the XAYA blockchain. 

It also sets the XAYAConnector to subscribe for updates from libxayagame. Those updates that XAYAConnector receives, as mentioned above, are then asynchronously updated in the front end, i.e. MoveGUIAndGameController. (This will be examined in more depth later. 

## XAYAMoverGame

It is up to you to write this as this is the core game logic. We'll examine this in great detail below and explain the callbacks extensively. 

## XAYA Unity - MoveGUIAndGameController

This is your front end. It launches and disconnects from XAYAConnector. It uses XAYAClient to get a list of XAYA names from the user's wallet and to send moves to the blockchain.

It uses XAYAMoverGame for the GameState, which is used to update the UI for each block where there are moves. 

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

Scroll to the bottom and you'll see how new functionality can be easily added. For more information on XAYA RPCs, refer to [XAYA RPC Methods](XAYA%20RPC%20Methods.md) and [Interacting with the XAYA Wallet Through RPC in C#](RPC%20Windows%20C%23%20Tutorial/XAYA%20RPC%20Tutorial.md).

# XAYAWrapper

XAYAWrapper wraps libxayagame and exposes several fields and methods.

![XAYAWrap Class Diagram](img/XAYAWrapperClassDiagram.png)

The imporant fields are:

- initialCallback
- forwardCallback
- backwardsCallback
- xayaGameService

We'll examine the callbacks later on when we look at the game logic. In order to use libxayagame, this is perhaps the most important part to understand, 

xayaGameService is part of BitcoinLib and can be used to send RPC calls. While we've used BitcoinLib for RPCs, you can choose any RPC library that you prefer. 

There are 4 methods:

- Connect: Connects to the daemon
- ShutdownDaemon: Stops the daemon
- Stop: Stops BitcoinLib 
- XayaWrapper: Constructor

Wiring up XAYAWrapper is very easy. 

1\. Instantiate a XAYAWrapper as a member variable as seen in XAYAConnector:

	public XayaWrapper wrapper;

2\. Call it's constructor as in XAYAConnector:

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

3\. Connect as in XAYAConnector:

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

We'll look at getting data (new game states) from libxayagame below.

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

FILL THESE IN

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
3. `XAYAConnector.StartEnum` uses Ciela Spike's ThreadNinja to start an asynchronous coroutine with `DaemonAsync`
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

With the name list populated,












************




# Getting a Player

The front end (`MoveGUIAndGameController`) needs to get a list of names from the XAYA wallet in order for 




# Connecting from the Front End

The CONNECT button in MoveGUIAndGameController is responsible for connecting and disconnecting XAYAConnect, which is a member of MoveGUIAndGameController:

	public XAYAConnector xayaConnector; 

Here's the basic code where the connection/disconnection is done:


















































\#4 is the simplest to explain. `MoverObject` isn't a part of the core game files. Instead, it inherits from MonoBehaviour. A instance is constructed and set in the Redraw method, but it is never used. When set, it's given a value from the game state. This illustrates how you can pull data from the game state and create some object not strictly defined inside of the game state processor (GSP) or game logic to use in the front end. 

For example, you may have many other properties defined for a "game piece", but only important parts for the game logic are stored or processed in the game logic. 















































