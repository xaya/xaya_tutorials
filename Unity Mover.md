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

Visual Studio no longer ships with Class Diagram. To get it, type "class diagram" into the Quick Launch in the upper-left corner of Visual Studio and search. It will return a link to install VS Class Diagram. 

MoverUnity.zip contains all the code for this tutorial. 

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

This is where `XAYAWrapper` is used. It gets data through RPC (BitcoinLib) and updates information for `MoveGUIAndGameController` so that `MoveGUIAndGameController` can update the UI. This will be examined in more depth later. 

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

## BitcoinLib

We've already explained the purpose of BitcoinLib above, but should mention that you can very easily extend it. In particular, see these files:

- BitcoinLib\Services\RpcServices\RpcService\IRpcService.cs
- BitcoinLib\Services\RpcServices\RpcService\RpcService.cs

Scroll to the bottom and you'll see how new functionality can be easily added. For more information on XAYA RPCs, refer to [XAYA RPC Methods](XAYA%20RPC%20Methods.md) and [Interacting with the XAYA Wallet Through RPC in C#](RPC%20Windows%20C%23%20Tutorial/XAYA%20RPC%20Tutorial.md).

## XAYAWrapper

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

1. Instantiate a XAYAWrapper as a member variable as seen in XAYAConnector:

	public XayaWrapper wrapper;

2. Call it's constructor as in XAYAConnector:

	wrapper = new XayaWrapper(dPath, MoveGUIAndGameController.Instance.host_s, MoveGUIAndGameController.Instance.gamehostport_s, ref functionResult, CallbackFunctions.initialCallbackResult, CallbackFunctions.forwardCallbackResult,CallbackFunctions.backwardCallbackResult);

The constructor's signature is:

	public XayaWrapper(string dataPath, string host_s, string gamehostport_s,  ref string result, InitialCallback inCal, ForwardCallback forCal, BackwardCallback backCal)

- dataPath: The path to the libxayagame DLL and its dependencies, i.e. the "XayaStateProcessor" folder
- host_s: The URL to connect to, e.g. http://user:password@127.0.0.1:8396
- gamehostport_s: This is 8900
- result: A string that tells you if the wrapper initialised ok or an error message
- inCal: The InitialCallback callback that you've written
- forCal: The ForwardCallback callback that you've written
- backCal: The BackwardCallback callback that you've written

3. Connect as in XAYAConnector:

        functionResult = wrapper.Connect(dPath, FLAGS_xaya_rpc_url, MoveGUIAndGameController.Instance.gamehostport_s, MoveGUIAndGameController.Instance.chain_s.ToString(), MoveGUIAndGameController.Instance.GetStorageString(MoveGUIAndGameController.Instance.storage_s), "mv", dPath + "\\..\\XayaStateProcessor\\database\\", dPath + "\\..\\XayaStateProcessor\\glogs\\" );

The Connect signature is:

	public string Connect(string dataPath, string FLAGS_xaya_rpc_url, string gamehostport_s, string chain_s, string storage_s, string gamenamespace, string databasePath, string glogsPath)

- dataPath: 
- FLAGS_xaya_rpc_url: 
- gamehostport_s: 
- chain_s: 
- storage_s: 
- gamenamespace: 
- databasePath: 
- glogsPath: 


We'll look at getting data (new game states) from libxayagame below.
































































