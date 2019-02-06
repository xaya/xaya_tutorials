# Overview

XAYA provides a convenient solution for developers to put their games up to 100% on the blockchain. The following materials can help you get started quickly. 

# Prerequisites

Before you get started, there are some basic prerequisites, e.g. get a wallet, CHI, etc.

> ![GO!](img/arrow-green-right-2a.png) [GOTO Prerequisites](Prerequisites.md)

# Getting Started

Once you've got the prerequisite downloads, the following will provide you with some background information and techniques that will be useful for when you start some code tutorials. 

## Getting Started with Background Information

Before you get started, you should be somewhat familiar with what a blockchain is. If not, please read [Blockchain Basics](Blockchain%20General.md).

> ![GO!](img/arrow-green-right-2a.png) [GOTO Blockchain Basics](Blockchain%20General.md)

Reading the [XAYA Specifications](https://github.com/xaya/xaya_docs) will give you a high-level overview and familiarise you with various concepts and details. **This is highly recommended reading and is considered reference material.** 

> ![GO!](img/arrow-green-right-2a.png) [GOTO XAYA Specifications](https://github.com/xaya/xaya_docs)

Wallets are critically important. Read the [Wallets](XAYA%20Electron%20Help/Top.md#Wallets) section of the [XAYA Electron wallet help](XAYA%20Electron%20Help/) for a quick high-level overview if you're not already familiar.

> ![GO!](img/arrow-green-right-2a.png) [Wallets](XAYA%20Electron%20Help/Top.md#Wallets)

Some tutorials use the XAYA Command Line Interface (`xaya-cli`). [Read up on `xaya-cli` here](xaya-cli.md).

> ![GO!](img/arrow-green-right-2a.png) [GOTO Getting Started with xaya-cli](xaya-cli.md)

Next, read about [XAYA RPC Methods](XAYA%20RPC%20Methods.md). You'll need to know how to send RPC calls to the XAYA daemon. This will get you up to speed quickly. 

> ![GO!](img/arrow-green-right-2a.png) [GOTO XAYA RPC Methods](XAYA%20RPC%20Methods.md)

The [libxayagame Component Relationships](libxayagame%20Component%20Relationships.md) shows you the basic architecture. It details how your game logic and front end interact with XAYA. 

> ![GO!](img/arrow-green-right-2a.png) [GOTO libxayagame Component Relationships](libxayagame%20Component%20Relationships.md)

## Getting Started with Coding

Now that you've familiarised yourself with some basics for XAYA, it's time to get coding.

The "How to Wire Up libxayagame in C#" is a minimalist tutorial that shows how to wire up XAYA very quickly. 

> ![GO!](img/arrow-green-right-2a.png) [GOTO How to Wire Up libxayagame in C#](How%20to%20Wire%20Up%20libxayagame%20in%20C%23.md)

Start with [Hello World](Hello%20World!/). It includes a fully functional example project and several videos. In this tutorial, you'll learn how to wire up a working XAYA game. This tutorial does not have any significant game logic, which makes it very easy to follow. 

> ![GO!](img/arrow-green-right-2a.png) [GOTO Hello World!](Hello%20World!/) 

Next, read the [Mover Overview](Mover.md), then try one of the Mover tutorials. The [console MoverSharp tutorial](MoverSharp) demonstrates simple game logic, but does not get into updating the front end. The [Mover in Unity tutorial](Unity%20Mover) goes over much of the same material as the console version, but adds in updating the front end. 

> ![GO!](img/arrow-green-right-2a.png) [GOTO Mover Sample Game Overview](Mover.md)
>
> ![GO!](img/arrow-green-right-2a.png) [GOTO Mover Sharp - A C# Implementation](MoverSharp/)
>
> ![GO!](img/arrow-green-right-2a.png) [GOTO Mover Sample Game in C# with Unity](Unity%20Mover/)

## Getting Started with Developer Support

If you have questions, visit the [XAYA Development forum](https://forum.xaya.io/forum/6-development/). You can ask questions and get answers there. 

# Guides and Tutorials

For game developers, this is the first place you should look to get started with XAYA game development. The sections below are loosely arranged in order. You can read them from top to bottom, but feel free to skip around. 

- **[Prerequisites](#Prerequisites)**
- **[Blockchain Basics](#Blockchain-Basics)**
- **[XAYA Specifications](#XAYA-Specifications)**
- **[XAYA RPC](#XAYA-RPC)**
	+ [XAYA RPC Methods](#XAYA-RPC-Methods)
	+ [RPC Windows C# Tutorial](#RPC-Windows-C-Tutorial)
- **[libxayagame](#libxayagame)**
	+ [Component Relationships](#Component-Relationships)
	+ [How to Wire Up libxayagame in C#](#How-to-Wire-Up-libxayagame-in-C)
	+ [Hello World](#Hello-World!)
	+ [Mover Sample Game Overview](#Mover-Sample-Game-Overview)
	+ [Mover Sharp - A C# Implementation](#mover-sharp---a-c-implementation)
	+ [Mover in Unity](#Mover-in-Unity)
- **[Regtestnet](#Regtestnet)**
- **[Getting Started with xaya-cli](#Getting-Started-with-xaya-cli)**
- **[XAYA Electron Wallet Help](#XAYA-Electron-Wallet-Help)**

Additional resources will be added on an ongoing basis. 

# Blockchain Basics

If you're unfamiliar with blockchain technology, [Blockchain Basics](Blockchain%20General.md) will give you a quick overview and highlight some important aspects. This is a quick, high-level overview and far from exhaustive. If you are already familiar, you should skip this.

> ![GO!](img/arrow-green-right-2a.png) [GOTO Blockchain Basics](Blockchain%20General.md)

# XAYA Specifications

The [XAYA Specifications](https://github.com/xaya/xaya_docs) repository contains technical specifications and design documents that
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

> ![GO!](img/arrow-green-right-2a.png) [GOTO XAYA Specifications](https://github.com/xaya/xaya_docs)

# XAYA RPC 

XAYA uses RPC (Remote Procedure Calls) to communicate with the XAYA daemon (xayad). The daemon contains many of the same methods that you may already know from Bitcoin. It also contains methods unique to XAYA. The documents here will help you with those unique methods and provide some additional guidance on their purpose. 

XAYA is language agnostic, so you can use any language you wish so long as it is capable of making RPC calls. 

## XAYA RPC Methods

The [XAYA RPC Methods](XAYA%20RPC%20Methods.md) document describes several of the most useful RPC methods and provides examples. It is language agnostic and you should read this first.

> ![GO!](img/arrow-green-right-2a.png) [GOTO XAYA RPC Methods](XAYA%20RPC%20Methods.md)

## RPC Windows C# Tutorial

The [RPC Windows C# Tutorial](RPC%20Windows%20C%23%20Tutorial/README.md) uses a 3rd party library to demonstrate using several XAYA RPC methods. A sample Windows Forms application is provided with comments to assist you.

> ![GO!](img/arrow-green-right-2a.png) [GOTO RPC Windows C# Tutorial](RPC%20Windows%20C%23%20Tutorial/README.md)

# libxayagame

The [libxayagame](https://github.com/xaya/libxayagame) library lets developers focus on building blockchain games without worrying about any of the inner workings of the blockchain, such as reorgs. That is, libxayagame does some very, heavy, heavy lifting for you. 

Instead, it provides a simple framework for you to code your games on top of the XAYA platform. 

Once you have libxayagame wired up, you only need to implement 3 callback methods. The Mover example shows you how to do this. 

Code ninjas and rock stars may enjoy browsing through the code or [code documentation available here](https://xaya.io/docs/libxayagame/).

> ![GO!](img/arrow-green-right-2a.png) [GOTO libxayagame](https://github.com/xaya/libxayagame) 

## Component Relationships

This tutorial provides a high-level overview of the component relationships and information flows when using libxayagame. While it aims to explain the Mover sample game, other scenarios are briefly outlined.

> ![GO!](img/arrow-green-right-2a.png) [GOTO libxayagame Component Relationships](libxayagame%20Component%20Relationships.md)

# How to Wire Up libxayagame in C#

This very short tutorial explains how to add libxayagame to a project.

> ![GO!](img/arrow-green-right-2a.png) [GOTO How to Wire Up libxayagame in C#](How%20to%20Wire%20Up%20libxayagame%20in%20C%23.md)

## Hello World!

This is a step-by-step "Hello World!" tutorial and a good place to start. 

> ![GO!](img/arrow-green-right-2a.png) [GOTO Hello World!](Hello%20World!/) 

## Mover Sample Game Overview

[Mover](https://github.com/xaya/libxayagame/tree/master/mover) is an extremely simple game that uses the [libxayagame](https://github.com/xaya/libxayagame) library. The goal of Mover is to help you easily build your game.

Both libxayagame and Mover are written in C++. The Mover sample includes a game test written in Python. For more information, see the [Mover Sample Game Overview](Mover.md) tutorial.

> ![GO!](img/arrow-green-right-2a.png) [GOTO Mover Sample Game Overview](Mover.md) 

## Mover Sharp - A C# Implementation

MoverSharp is a truncated C# implementation that does not allow user input. It merely displays moves. 

This tutorial illustrates how libxayagame is wired up for Mover and then explores game logic on the XAYA platform. 

> ![GO!](img/arrow-green-right-2a.png) [GOTO Mover Sharp - A C# Implementation](MoverSharp/)

## Mover in Unity

For C# developers, the Mover example has been ported to C# along with code to wrap the libxayagame C++ library. 

The Unity project provides a front-end GUI for Mover. The wrapper and additional code implement the Mover game itself.

> ![GO!](img/arrow-green-right-2a.png) [GOTO Mover Sample Game in C# with Unity](Unity%20Mover/)

# Regtestnet

Regtestnet is what you should primarily use during development instead of mainnet or testnet. At some point you may wish to move your testing to testnet, but that shouldn't be your first choice.

The regtest option gives you a private XAYA network where you have complete control.

You can instamine CHI at will and control exactly when and how blocks are mined. You can invalidate blocks, and undo that.

See the "[Getting Started with Regtestnet](Regtestnet.md)" tutorial for more information.

> ![GO!](img/arrow-green-right-2a.png) [GOTO Getting Started with Regtestnet](Regtestnet.md)

# Getting Started with xaya-cli

The XAYA Command Line Interface (xaya-cli) lets you easily interact with the XAYA daemon and XAYA wallets. It's used extensively in various tutorials. This tutorial introduces xaya-cli and gives examples that you can practice with to learn it very quickly. 

> ![GO!](img/arrow-green-right-2a.png) [GOTO Getting Started with xaya-cli](xaya-cli.md)

# XAYA Electron Wallet Help

This is end user documentation for the [XAYA Electron wallet](XAYA%20Electron%20Help).

> ![GO!](img/arrow-green-right-2a.png) [GOTO XAYA Electron wallet help](XAYA%20Electron%20Help)

















