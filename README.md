# Overview

XAYA provides a convenient solution for developers to put their games up to 100% on the blockchain. The following materials can help get you started quickly.

# Some title

# XAYA Specifications

The XAYA [Specifications](https://github.com/xaya/xaya_docs) repository contains technical specifications and design documents that
describe how the various components and layers in the ecosystem interact.
In particular, important topics are:

* Xaya's [*triple-purpose mining*](https://github.com/xaya/xaya_docs/blob/master/mining.md) that secures the blockchain
* The basic Xaya [blockchain consensus protocol](https://github.com/xaya/xaya_docs/blob/master/blockchain.md)
* How [games](https://github.com/xaya/xaya_docs/blob/master/games.md) should interact with the core blockchain
* A standard for [currencies](https://github.com/xaya/xaya_docs/blob/master/currencies.md) on the Xaya platform
* Network rules for [restricting transactions](https://github.com/xaya/xaya_docs/blob/master/addressrestrictions.md)
  to addresses
* The core daemon [interface](https://github.com/xaya/xaya_docs/blob/master/interface.md) for game engines

The specifications are required reading. Other documents will refer to specific sections of the specifications. 

# XAYA RPC 

RPC (Remote Procedure Calls) 

The [XAYA RPC Methods](XAYA%20RPC%20Methods.md) document describes several of the most useful RPC methods and provides examples. 

The [RPC Windows C# Tutorial](RPC%20Windows%20C%23%20Tutorial/XAYA%20RPC%20Tutorial.md) uses a 3rd party library to demonstrate using several XAYA RPC methods. A sample Windows Forms application is provided with comments to assist you.

# libxayagame

The [libxayagame](https://github.com/xaya/libxayagame) library lets developers focus on building blockchain games without worrying about any of the inner workings of the blockchain, such as reorgs. 

Instead, it provides a simple framework for you to code your games on top of the XAYA platform. 

Once you have libxayagame wired up, you only need to implement 3 callback methods. The Mover example shows you how to do this. 

## Mover Sample Game

Mover is an extremely simple game that uses the [libxayagame](https://github.com/xaya/libxayagame) library. The goal of Mover is to help you easily build your game.

Both libxayagame and Mover are written in C++. The Mover sample includes a game test written in Python. For more information, see the [Mover](Mover.md) tutorial.

## Mover in Unity

For C# developers, the Mover example has been ported to C# along with code to wrap the libxayagame C++ library. 

The Unity project provides a front-end GUI for Mover. The wrapper and additional code implement the Mover game itself.























