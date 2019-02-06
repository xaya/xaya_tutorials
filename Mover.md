# Mover Sample Game Overview

If you haven't already, make certain to [download or clone the libxayagame repository](https://github.com/xaya/libxayagame). You will need to build libxayagame from source. See the [libxayagame README here](https://github.com/xaya/libxayagame) for how to build it. 

Before proceeding, you should read the [Mover README document](https://github.com/xaya/libxayagame/blob/master/mover/README.md).

The following tutorial is not a code walk through. It is a high-level overview of some of the most important elements and concepts that the Mover sample game demonstrates. You may wish to open and read the code when it's being described below. 

# Using Mover as a Template

Depending on the complexity of your game, you may wish to use Mover a template for your game and simply replace the game logic with your own. 

## Mover is a Live MMOG and Practically Unhackable

While Mover is very simple, it should be pointed out that it is a live game that anyone can write a client for, and it is a massively multiplayer online game. It can accommodate many thousands of players. 

Undoubtably some people may _try_ to break Mover, but if you code your version of Mover properly, those attempts to break it will be meaningless. The only thing needed is decent error checking. 

Blockchain games, such as Mover, are resistant to hacking attempts because moves are entered into the blockchain. This requires using blockchain public-private key encryption, and breaking that encryption simply isn't feasible. Consequently the only issue for moves is to check whether they are valid or not. If they are valid, then they are processed. If they are invalid, then they are ignored. 

Despite the simplicity of Mover, the same basic principles apply to any other game written the same way. So, while these claims may seem bold for such a simple game, they hold true at any scale. 

Are you ready for this adventure?

# Mover Gameplay

In Mover, players use a XAYA name to join the game. The first move is always the same for everyone: the player is placed on the 2D-map at the origin, (0, 0). 

Players can then enter moves which consist of:

- 1 of 8 possible directions
- A number of steps to take in that direction

The 2D-map is extends infinitely in both the x and y axis.

Players can enter new moves at any time. Moves are sent to the XAYA blockchain, and then read back into the game where they are processed and the game state is updated. 

Any step in any direction is considered equal, i.e. 1 step of a horizontal or vertical move is considered equivalent to 1 step travelling along a diagonal. 

As you can see, Mover is a very simple game with very simple logic. This makes examining the sample game and code easier and we can focus on how games can run on the XAYA platform instead of complex game logic.

# Getting Started with Mover

Now that you have downloaded or cloned the code, look in the `mover` folder. There are several files there. Of interest to us are these files:

- logic.hpp
- logic.cpp
- main.cpp

In there we'll find libxayagame used, and the game logic for Mover.

## logic.hpp

The `logic.hpp` file is simple enough. It contains some definitions for methods that we'll create in `logic.cpp`. The most important methods that we want to examine are:

- GetInitialState 
- ProcessForward 
- ProcessBackwards 

However, do take note of the includes. The includes for libxayagame are:

	#include "xayagame/gamelogic.hpp"
	#include "xayagame/storage.hpp"

In your project, you will likely need to include these headers similar to the following:

	#include <xayagame/gamelogic.hpp>
	#include <xayagame/storage.hpp>

## logic.cpp

There's quite a bit in the logic.cpp file, but of most interest to us are the callbacks that implement the game logic.

- GetInitialState: Sets the chain and block height for the game
- ProcessForward: Processes all players moves and updates the game state
- ProcessBackwards: Rewinds the game in case there's a fork or reorg

### GetInitialState

The game logic in `GetInitialState` sets the chain that the game runs on as well as the block height where the game begins. In your game, you'll likely want to start on testnet or regtestnet where you can instantly mine new blocks. Regtestnet is perfect for testing. Remember to include the proper hash for the block it begins on. 

### ProcessForward

The game logic in `ProcessForward` is the main game logic. This is where you get 3 critical pieces of information:

- The current game state (the game as it is right now)
- The block data with new moves in it 
- Undo information that you need to update

For your game, with that information, you need to take the current game state and update it with the new moves you've received through the blockchain, and process that data to come up with a new game state, i.e. you process the game forward by 1 move.

At the same time, you must create some undo data.

#### Undo Data

Undo data is a set of data that allows you to rewind a game to a previous state. But, why is this needed? 

Time and order on a blockchain differs from our every day conception of time and order. Consequently, on most blockchains inevitably there are some events that need to be dealt with, e.g. reorgs. 

Events such as reorgs are extremely difficult to deal with. Luckily, libxayagame takes care of all the heavy lifting here. However, in order to do that heavy lifting, game developers must keep an inventory of undo data. How you manage this is largely up to you, e.g. database or flat files, but it must be done. 

Take note how the undo data is created in the Mover example as you'll need to implement something similar. Mover keeps this in memory, but you will likely wish to commit it to a database, such as SQLite, or other storage system. 

With undo data, you must be able to compute the *inverse* of the state transition, i.e. given a *new state* and undo data, compute the corresponding *old state*. 

A cheap hack is to return the old state as the undo data. However, this won't be very efficient as you'll need to store orders of magnitude more data. That is, processing undo data lets you use far less drive storage space. Still, it should be sufficient for simple games or for getting started quickly. For an example of this, see [here](https://github.com/xaya/libxayagame/blob/6d14d5331c000a156eaefbc1bf1f2eaddafd181a/xayagame/gamelogic.cpp#L52). 

#### Returning Data

Once you've processed the new moves and created an updated game state, you must return data so that you can update your front-end GUI for your players. 

Note the `ProcessForward` signature and how arguments are passed in, and the last few lines in the method. While `ProcessForward` returns data, it also updates data values for arguments passed by reference. This allows multiple variable to be updates easily for consumption in the front-end GUI. 

## ProcessBackwards

The `ProcessBackwards` callback is similar to the `ProcessForward` callback in some ways. However, its purpose is to rewind the game state by 1 block.

In `ProcessForward`, we produced undo data. Here we consume that undo data. 

The "rewind" logic for Mover is nice and simple. Follow how the undo data is consumed, and how the previous game state is reconstructed. 

The only return value here is the rewound game state. In the event that you need to rewind further, you'll need your stored undo data. 

## Compiling and Running Mover

See `Makefile.am` in the `mover` folder.

## Summary

We took a high-level overview of the Mover sample game and went over the purpose and methodology of some of the most important elements and concepts. 





