# Overview

XAYA provides a convenient solution for developers to put their games up to 100% on the blockchain. The following materials can help get you started quickly.

# Getting Started

For game developers, this is the first place you should look to get started with XAYA game development. The sections below are arranged in order. You should read them from top to bottom, but feel free to skip to any section if you're already familiar with the basics. 

- **[Blockchain Basics](#Blockchain-Basics)**
- **[XAYA Specifications](#XAYA-Specifications)**
- **[XAYA RPC](#XAYA-RPC)**
	+ [XAYA RPC Methods](#XAYA-RPC-Methods)
	+ [RPC Windows C# Tutorial](#RPC-Windows-C-Tutorial)
- **[libxayagame](#libxayagame)**
	+ [Component Relationships](#Component-Relationships)
	+ [Mover Sample Game](#Mover-Sample-Game)
	+ [Mover in Unity](#Mover-in-Unity)
- **[Regtestnet](#Regtestnet)**
- **[XAYA Electron Wallet Help](#XAYA-Electron-Wallet-Help)**

Additional resources will be added on an ongoing basis. 

# Blockchain Basics

If you're unfamiliar with blockchain technology, [Blockchain Basics]() will give you a quick overview and highlight some important aspects. This is a quick, high-level overview and far from exhaustive. If you are already familiar, you should skip this.

> [GOTO Blockchain Basics](Blockchain%20General.md)

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

> [GOTO Specifications](https://github.com/xaya/xaya_docs)

# XAYA RPC 

XAYA uses RPC (Remote Procedure Calls) to communicate with the XAYA daemon (xayad). The daemon contains many of the same methods that you may already know from Bitcoin. It also contains methods unique to XAYA. The documents here will help you with those unique methods and provide some additional guidance on their purpose. 

XAYA is language agnostic, so you can use any language you wish so long as it is capable of making RPC calls. 

## XAYA RPC Methods

The [XAYA RPC Methods](XAYA%20RPC%20Methods.md) document describes several of the most useful RPC methods and provides examples. It is language agnostic and you should read this first.

> [GOTO XAYA RPC Methods](XAYA%20RPC%20Methods.md)

## RPC Windows C# Tutorial

The [RPC Windows C# Tutorial](RPC%20Windows%20C%23%20Tutorial/XAYA%20RPC%20Tutorial.md) uses a 3rd party library to demonstrate using several XAYA RPC methods. A sample Windows Forms application is provided with comments to assist you.

> [GOTO RPC Windows C# Tutorial](RPC%20Windows%20C%23%20Tutorial/XAYA%20RPC%20Tutorial.md)

# libxayagame

The [libxayagame](https://github.com/xaya/libxayagame) library lets developers focus on building blockchain games without worrying about any of the inner workings of the blockchain, such as reorgs. That is, libxayagame does some very, heavy, heavy lifting for you. 

Instead, it provides a simple framework for you to code your games on top of the XAYA platform. 

Once you have libxayagame wired up, you only need to implement 3 callback methods. The Mover example shows you how to do this. 

Code ninjas and rock stars may enjoy browsing through the code or [code documentation available here](https://xaya.io/#The-doxygen-documentation-still-needs-to-be-uploaded).

> [GOTO libxayagame](https://github.com/xaya/libxayagame) 

## Component Relationships

This tutorial provides a high-level overview of the component relationships and information flows when using libxayagame. While it aims to explain the Mover sample game, other scenarios are briefly outlined.

> [GOTO libxayagame Component Relationships](libxayagame%20Component%20Relationships.md)

## Mover Sample Game

Mover is an extremely simple game that uses the [libxayagame](https://github.com/xaya/libxayagame) library. The goal of Mover is to help you easily build your game.

Both libxayagame and Mover are written in C++. The Mover sample includes a game test written in Python. For more information, see the [Mover](Mover.md) tutorial.

> [GOTO Mover](https://github.com/xaya/libxayagame/tree/master/mover) 

## Mover in Unity

For C# developers, the Mover example has been ported to C# along with code to wrap the libxayagame C++ library. 

The Unity project provides a front-end GUI for Mover. The wrapper and additional code implement the Mover game itself.

> [GOTO Unity Mover.md](Unity%20Mover.md)

# Regtestnet

Regtestnet is what you should primarily use during development instead of mainnet or testnet. At some point you may wish to move your testing to testnet, but that shouldn't be your first choice.

The regtest option gives you a private XAYA network where you have complete control.

You can instamine CHI at will and control exactly when and how blocks are mined. You can invalidate blocks, and undo that.

See the "[Getting Started with Regtestnet](Regtestnet.md)" tutorial for more information.

> [GOTO Getting Started with Regtestnet](Regtestnet.md)


# XAYA Electron Wallet Help

This is end user documentation for the [XAYA Electron wallet]().

> [GOTO XAYA Electron wallet help]()

















