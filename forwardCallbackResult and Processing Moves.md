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
