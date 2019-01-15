# Mover Sample Game

If you haven't already, make certain to [download or clone the libxayagame repository](https://github.com/xaya/libxayagame). You will need to build libxayagame from source. See the [libxayagame README here](https://github.com/xaya/libxayagame) for how to build it. 

Before proceeding, you should read the [Mover README document](https://github.com/xaya/libxayagame/blob/master/mover/README.md).

# Using Mover as a Template

Depending on the complexity of your game, you may wish to use Mover a template for your game and simply replace the game logic with your own. 

NEEDS MORE HERE

## Mover is a Live MMOG and Practically Unhackable

While Mover is very simple, it should be pointed out that it is a live game that anyone can write a client for, and it is a massively multiplayer online game. It can accomodate many thousands of players. 

Undoubtably some people may write code to _try_ to break Mover, but if you code your version of Mover properly, those attempts to break it will be meaningless. The only thing needed is decent error checking. 

Blockchain games, such as Mover, are resistant to hacking attempts because moves are entered into the blockchain. This requires using blockchain public-private key encryption, and breaking that encryption simply isn't feasible. Consequently the only issue for moves is to check whether they are valid or not. If they are valid, then they are processed. If they are invalid, then they are ignored. 

Despite the simplicity of Mover, the same basic principles apply to any other game written the same way. So, while these claims may seem bold for such a simple game, they hold true at any scale. 

Are you ready for this adventure?

# Getting Started with Mover

Now that you have downloaded or cloned the code, look in the "mover" folder. There are several files there. Of interest to us are these files:

- logic.cpp
- logic.hpp
- main.cpp

In there we'll find libxayagame used, and the game logic for Mover.

## logic.hpp

The logic.hpp file is simple enough. It contains some definitions for methods that we'll create in logic.cpp. The most important methods that we want to examine are:

- GetInitialState 
- ProcessForward 
- ProcessBackwards 

However, do take note of the includes. The includes for libxayagame are:

	#include "xayagame/gamelogic.hpp"
	#include "xayagame/storage.hpp"

## logic.cpp

There's quite a bit in the logic.cpp file, but of most interest to us are the callbacks that implement the game logic.










