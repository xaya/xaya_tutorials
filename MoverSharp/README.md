# Mover Sharp - A C# Implementation

This tutorial walks through example code in C# for &quot;Mover&quot;, a simple MMO game where a player can walk in any direction. The aim of this tutorial is to explain a &quot;bare bones&quot; game so that developers have a simple reference to help them get started. If you haven&#39;t already, you may wish to read the high-level overview of [Mover here](https://github.com/xaya/xaya_tutorials/blob/master/Mover.md).

XAYA enabled games consist of 3 main parts:

- XAYA wallet (this runs the XAYA daemon)
- Game State Processor (GSP)
- Front end (GUI)

The XAYA daemon processes transactions and notifies libxayagame when new blocks come in. This is where much of the heavy lifting is done for the XAYA blockchain.

The GSP is where libxayagame and the game logic resides. It calculates game states and notifies the front end so that it can redraw the screen or otherwise update the GUI. In this Mover example, the GSP, and specifically libxayagame using glog, sends output to the console (front end).

Normally the front end subscribes and listens to the GSP using the RPC `waitforchange` command. It also sends RPC commands for player moves to the daemon so that they can be entered into the blockchain. However, this particular Mover example doesn&#39;t allow sending moves. See the [Unity Mover](Unity%20Mover.md) example for a more interactive version where you can move your player.

Note: The front end and GSP run in separate processes on different threads.

The front end and GSP each use a TCP port for RPC commands. The GSP communicates to the front end through one port (port 8900), and the front end communicates with the daemon through another port (port 8396), e.g.:

	GSP <–port 1–> Front end <–port 2–> Daemon

So the GSP and daemon use 1 port while the front end uses 2 ports. See [libxayagame Component Relationships](libxayagame%20Component%20Relationships.md) for diagrams and information on different ways that games can wire up libxayagame.

In order to focus on coding for XAYA, the example here for &quot;Mover&quot; has no GUI. Instead, we will write a console application and libxayagame will display output there. Also, the TCP port is set to 0 in libxayagame.

To get started, download the example code here:

[MoverSharp.zip](Code/MoverSharp.zip)

You must make sure that you leave the project build configuration set to `x64` as the libxayagame\_wrapper.dll is 64-bit. If you have a 32-bit system, you won&#39;t be able to run this.

Full source code for libxayagame is available here:

[libxayagame](https://github.com/xaya/libxayagame)

In this example game, Mover, users control a player on a square-grid map and move their player around. There are no borders, so a player can move infinitely in any direction. That&#39;s all there is to it.

# JSONClasses.cs

To start, let&#39;s examine the data structures in the game as they&#39;re very simple and critically important to understand later code in the game. They&#39;re all found in the JSONClasses.cs file:

- Direction
- PlayerState
- GameState
- PlayerUndo
- UndoData

These are very basic and your game will absolutely be much more complex. However, they should suffice to illustrate the principles used to create games on XAYA.

The game map is a grid of squares with Cartesian coordinates. Players start at the origin, (0, 0).

`Direction` is a simple enum of 9 directions (including NONE) that a player can move in.

    // A possible direction of movement.
    public enum Direction
    {
        // NONE is the direction for players that are not moving,
        // in particular, after steps_left has counted down to zero.

        NONE = 0,
        RIGHT = 1,
        LEFT = 2,
        UP = 3,
        DOWN = 4,
        RIGHT_UP = 5,
        RIGHT_DOWN = 6,
        LEFT_UP = 7,
        LEFT_DOWN = 8
    }

`PlayerState` contains all the information we need to know about a player, i.e. its position on the map as x and y coordinates, the direction the player is moving in (the `Direction` enum above), and the number of steps left for it to take as an integer.

    // The state of a particular player in Mover.
    public class PlayerState
    {
        // The current x coordinate.
        public int x;
        // The current y coordinate. 
        public int y;

        // The direction of movement.
        public Direction dir = Direction.UP;
        // The remaining number of movement steps left. 
        public Int32 steps_left;
    }

You can think of this as, &quot;I&#39;m here (our x and y Cartesian coordinates on the map) and I&#39;m going to walk 5 m (steps) north (a direction).&quot; It&#39;s that simple.

`GameState` is a very simple class. It contains a dictionary of all the players and their states, i.e. `PlayerState`s. It&#39;s easy to access any given player by name, e.g. `GameState.players[name]`.

    // The full game state.
    public class GameState
    {
        // All players on the map and their current state. 
        public Dictionary<string, PlayerState> players;
    }

If we were to draw the map, we could iterate over the game state to find each player, then place them on the map with the `PlayerState` x and y values.

The `UndoData` class is very similar to the `GameState` class. Again, it contains a dictionary, but instead of tracking `PlayerState` data, it tracks `PlayerUndo` data.

It&#39;s possible that we may need to &quot;rewind&quot;, so the `UndoData` class keeps the undo information that we need for every player on the map.

    // The full undo data for a block. 
    public class UndoData
    {
        // Undo data for each player that needs one.
        public Dictionary<string, PlayerUndo> players;
    }

For the &quot;rewind&quot; scenario, we must know how to do that for each individual player. Individual player undo information is contained in the `PlayerUndo` class. It tells us:

- `is_new`: Whether a player is new or not (new players start at the origin (0, 0))
- `previous_dir`: The previous direction a player was moving in
- `previous_steps_left`: The previous number of steps left for it to take
- `finished_dir`: The direction that a player finished moving in

The direction that a player finished moving in is for the case where they have finished moving and their direction has been set to `Direction.NONE` while their steps left has been set to zero.

As seen above, `PlayerUndo` is used in the `UndoData` class.

    // The undo data for a single player. 
    public class PlayerUndo
    {
        // Set to true if the player was not previously present, i.e. if it was
        // first moved and created on the map for this block.
        public bool is_new;

        // Previous direction of the player, if it was changed explicitly.
        public Direction previous_dir = Direction.NONE;

        // Previous steps left if the number was changed explicitly by a move.
        public Int32 previous_steps_left = 99999999;

        // Previous direction of the player if it counted down to zero and was
        // changed to NONE in this block.
        // 
        // In theory, this field could be merged with previous_dir. It is possible
        // that both are set, namely when a move with steps = 1 was made. But this case
        // could be reversed using the move data. The potential space savings
        // here seem minor though, so we use a separate field to simplify the logic.
        public Direction finished_dir = Direction.NONE;
    }

With those data structures, it&#39;s easier to understand the game flow.

1. For each new block we get new player moves
2. We then compute the new game state (position of players on the map, and their individual states)
3. We update the game with the game state
4. If we encounter a fork and need to revert (reorg), our &quot;backward&quot; callback lets us use all the `PlayerUndo` data in `UndoData` to revert backwards by 1 block

# HelperFunctions.cs

Our `HelperFunctions` class contains static methods that we&#39;ll use in the game logic. Note that for some we have return values from parameters that we pass in by reference, i.e. ref type var.

- `ParseMove`: Takes a JSON object, sets some parameters, and returns `true` if the move is valid
- `ParseDirection`: Takes a string and returns a `Direction` enum
- `GetDirectionOffset`: Takes a `Direction` enum then sets an x and y offset for that direction
- `DirectionToString`: Takes a `Direction` enum and returns plain English for a valid direction or an empty string

As there&#39;s nothing particularly special in this class, further examination of it is left to the reader to pursue on their own. The only remaining point that should be made is that there should be thorough error checking, and particularly for data received through the blockchain, which in this case would be the `JObject` passed to `ParseMove`. See the error checking in that method for an example. 

To make the case for extreme error checking, consider that anyone could issue a `name_update` operation through the daemon or XAYA QT wallet console. That data would be entirely arbitrary. Each and every bit of data from the blockchain **MUST** be checked. While normal people just want to play the game, there are some people that just want to see if they can break things. You must guard against them. An example invalid move is shown below in [A Quick Look at Moves](#A%20Quick%20Look%20at%20Moves).

# Program.cs

Moving on, let&#39;s examine the Program.cs code.

## Member Variables

We'll need a few variables to connect to the wrapper.

        static string dPath = AppDomain.CurrentDomain.BaseDirectory;
        static int chainType = 0;
        static string storageType = "memory";
        static string FLAGS_xaya_rpc_url = "xayagametest:xayagametest@127.0.0.1:8396";
        static string host_s = "http://127.0.0.1";
        static string gamehostport_s = "8900";

- `dPath`: This is the path to the application directory
- `chainType`: This chooses between mainnet, testnet, and regtestnet. Values are 0, 1, and 2, respectively 
- `storageType`: This chooses between "memory", "sqlite", and "lmdb"
- `FLAGS_xaya_rpc_url`: This is the URL in the form "http://user:password@IP-address:port"
- `host_s`: This is the host address
- `gamehostport_s`: This is the game host port. It's 8900 for Mover

We could accept arguments from the command line, parse them, and then assign those values, but for clarity it&#39;s easier to hard code these values for the purpose of illustration.

The dPath is set to the same path as the example game&#39;s executable file.

	static string dPath = AppDomain.CurrentDomain.BaseDirectory;

We&#39;ll connect to libxayagame on port 8900. This can be any free port.

	static string gamehostport_s = "8900";

Our game is configured to run on mainnet. However, normally you would use testnet or regtestnet during development. Mainnet uses real XAYA names and real CHI. Should you make a bad mistake here, there&#39;s no going back and your error will be visible for eternity on the blockchain and any CHI you spend will be truly spent. On the positive side, you&#39;ll be able to see other people that have moved their player in the game, i.e. Mover is truly an extremely minimalistic massively multiplayer online game (MMOG).

	static int chainType = 0;

For this example we are using memory for storage. In larger games this may not be practical, in which case the high-performance, low-footprint database [SQLite](https://sqlite.org/) is available. There&#39;s 1 other option available, &quot;lmdb&quot;. The memory option doesn&#39;t require a data directory to be declared. All other options, i.e. &quot;lmdb&quot; and &quot;sqlite&quot; require a data directory to be defined.

	static string storageType = "memory";

The XAYA daemon runs on port 8396 and we&#39;ll be connecting over the local loopback. Our username and password in this example are both &quot;xayagametest&quot;. **You&#39;ll need to configure this for yourself to run the example.**

	static string FLAGS_xaya_rpc_url = "xayagametest:xayagametest@127.0.0.1:8396";

While it's possible to connect to a wallet running on another machine, for `host_s` this should most likely be the local loopback, i.e. 127.0.0.1. 

	static string host_s = "http://127.0.0.1";

These configuration parameters will depend on how you are running xayad. If you&#39;re using the Electron wallet, it&#39;s already well configured. You will need to get the user name and password from the .cookie file in the "%appdata%\Xaya" folder. However, you can also specify these if you set configurations in the xaya.conf file, e.g.:

	rpcuser=xayagametest
	rpcpassword=xayagametest
	rpcport=8396
	server=1
	zmqpubhashtx=tcp://127.0.0.1:28332
	zmqpubhashblock=tcp://127.0.0.1:28332
	zmqpubrawblock=tcp://127.0.0.1:28332
	zmqpubrawtx=tcp://127.0.0.1:28332
	zmqpubgameblocks=tcp://127.0.0.1:28332
	rpcallowip=127.0.0.1

The alternative is to start up the wallet/daemon with all the required configuration parameters from the command line.

The ZMQ values must also be set as shown. You can see ZMQ values set by looking in the BAT files in this folder:

> C:\Program Files\Xaya\XAYA-Electron\resources\daemon

However, it's much easier to simply run the XAYA Electron wallet. After all, it's designed for games.

With the configuration complete, it&#39;s time to load the wrapper.

## Instantiate the Wrapper

Instantiate the wrapper by calling it's constructor. It's signature is:

	public XayaWrapper(string dataPath, 
		string host_s, 
		string gamehostport_s,  
		ref string result, 
		InitialCallback inCal, 
		ForwardCallback forCal, 
		BackwardCallback backCal)

The first 3 we already have above. The result is a string message for us so we create an empty string to hold that value (it's passed by reference).

	string functionResult = "";

The callbacks we've not looked at yet. They're explained below. For now we just pass them in.

	XayaWrapper wrapper = new XayaWrapper(dPath, 
		host_s, 
		gamehostport_s, 
		ref functionResult, 
		CallbackFunctions.initialCallbackResult, 
		CallbackFunctions.forwardCallbackResult, 
		CallbackFunctions.backwardCallbackResult);

With our wrapper constructed, we send ourselves the result message and wait until ENTER is pressed. 

	Console.WriteLine(functionResult);
	Console.ReadLine();

If all went well, we should see "Wrapper Initialised" displayed. We then connect to the wrapper. It's signature is:

	public string Connect(string dataPath, 
		string FLAGS_xaya_rpc_url, 
		string gamehostport_s, 
		string chain_s, 
		string storage_s, 
		string gamenamespace, 
		string databasePath, 
		string glogsPath)

We've already seen most of those variables. The new ones are:

- `gamenamespace`: The "g/" name of the game
- `databasePath`: The path to the SQLite or other database
- `glogsPath`: The path to the glog output folder

libxayagame uses glog for logging and that folder must be set. 

The following connects the wrapper.

	wrapper.Connect(dPath, 
		FLAGS_xaya_rpc_url, 
		gamehostport_s, 
		chainType.ToString(), 
		storageType, 
		"mv", 
		dPath + "\\..\\XayaStateProcessor\\database\\", 
		dPath + "\\..\\XayaStateProcessor\\glogs\\");

If all goes well, we should now be connected to the game and it will begin with its starting block to display data.

**NOTE:** The `Connect` method is a blocking operation. Because of this, we will only received data from XAYAWrapper and will not be able to enter any moves. To deal with this, the Connect method must be called from its own thread. 

Now, in keeping with tradition, we&#39;ve saved the best for last. Game logic!

# CallbackFunctions.cs and Game Logic

**THIS** is what you&#39;ve been waiting for. Game logic. The good stuff. The juicy, lovely, delectable game code. Everything we&#39;ve done so far has worked up to this point.

There are 3 methods (callbacks or delegates) in our CallbackFunctions class.

- `initialCallbackResult`
- `forwardCallbackResult`
- `backwardCallbackResult`

The first merely sets some parameters for us when we initially start the game.

The `forwardCallbackResult` is where we process our game logic for regular moves.

The `backwardCallbackResult` is where we rewind in the case of a fork. This is where we use undo data.

Let&#39;s jump in.

***

# initialCallbackResult

The `initialCallbackResult` reads which chain we plan to use, then sets the `height` to start at, and the hash (`hashHex`) for that block in hexadecimal. It&#39;s very straight forward.
	
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

We must know what block we should start reading at. There&#39;s no sense in reading blocks prior to a game&#39;s existence.

For `hashHex`, to find out a block hash, you can use the official XAYA explorer available at [https://explorer.xaya.io/](https://explorer.xaya.io/). As an example, this is block zero (0), also known as the genesis block:

[https://explorer.xaya.io/block/0](https://explorer.xaya.io/block/0)

Its hash is &quot;e5062d76e5f50c42f493826ac9920b63a8def2626fd70a5cec707ec47a4c4651&quot;.

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

We only need to undo a player if they exist in our undo data, so we create a boolean flag for us to use.

	bool undoIt = undo.players.ContainsKey(name);

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

# Summary

We looked at how to wire up Mover to process moves. 

In JSONClasses.cs, we looked at the data structures for the game.

For HelperFunctions.cs, we briefly explained the methods, but didn't look at any code as they're all very simple. 

In the Program.cs file, we connected to libxayawrap.dll. We edited `FLAGS_xaya_rpc_url` specifically for our own machines by changing the password so that the program would properly connect.  

XAYAWrapper exposed 3 callbacks from libxayagame for us to implement our game logic. We did this in 3 classes in CallbackFunctions.cs:

- initialCallbackResult
- forwardCallbackResult
- backwardCallbackResult


