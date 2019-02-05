# Prerequisites

In order to develop games using the XAYA platform you'll need a XAYA wallet or `xayad` and [some CHI](#Get-Some-CHI) (mainnet, testnet, or regtestnet CHI are all fine). You'll also need to have your wallet properly configured. 

You will also need [libxayagame](#libxayagame). This is part of the Game State Processor (GSP). [See below for more information](#libxayagame).

- [Wallets](#Get-a-Wallet)
- [CHI](#Get-Some-CHI)
- [libxayagame](#libxayagame)
- [RPC and JSON libraries](#RPC-and-JSON-libraries)

# Get a Wallet

There are 4 XAYA wallet distributions.

1. [XAYA Electron wallet for Windows](https://github.com/xaya/xaya_electron/releases)
2. [XAYA QT wallet for Windows](https://github.com/xaya/xaya/releases)
3. [XAYA QT wallet for Linux](https://github.com/xaya/xaya/releases)
4. [XAYA QT wallet for Mac OS X](https://github.com/xaya/xaya/releases)

When you first run them, they need to download and sync the blockchain. This can take some time and largely depends on your network connection. 

## About the Electron Wallet

The XAYA Electron wallet is designed for gaming. Out-of-the-box it comes preconfigured for games with all the proper flags. This is the easiest option.

Further, the Electron wallet contains 2 distinct wallets:

- game.dat: Has no password. Meant for storing smaller amounts of CHI to play games
- vault.dat: Is password protected. Meant for storing larger amounts of CHI

Refer to the [Wallets](XAYA%20Electron%20Help/Top.md#Wallets) section of the [XAYA Electron Help](XAYA%20Electron%20Help) documentation for a more in depth description. 

However, even if you do choose to use the Electron wallet, you should still download the QT wallet as it has some [**important tools** that you'll enjoy using](#XAYA-QT-Toolkit). 

## About the QT Wallets

The QT wallets are all standard wallets and very similar to the Bitcoin Core QT wallet. There aren't any significant differences between the Windows, Linux, and Mac OS X distributions. 

If you choose to use a QT wallet during development, you can refer to the [Bitcoin Core help](https://bitcoin.org/en/bitcoin-core/help) for those parts that are common to both XAYA and Bitcoin. 

You can also get help through the XAYA QT console by typing "help" or "help &lt;command&gt;".

However, when you run the QT wallet, you must set several flags or you won't be able to connect to it. Run the QT wallet or xayad with these flags (note that datadir is optional):

- -wallet=vault.dat: Loads the vault.dat wallet
- -wallet=game.dat: Loads the game.dat wallet
- -server: Sets xayad to run as a server
- -rpcallowip=127.0.0.1: The IP to allow RPC calls on
- -datadir=&lt;DataDirPath&gt;: **OPTIONAL** Changes the default data directory to the one specified
- -zmqpubhashtx=tcp://127.0.0.1:28332: See below 
- -zmqpubhashblock=tcp://127.0.0.1:28332: See below
- -zmqpubrawblock=tcp://127.0.0.1:28332: See below
- -zmqpubrawtx=tcp://127.0.0.1:28332: See below
- -zmqpubgameblocks=tcp://127.0.0.1:28332: See below

For information about the ZeroMQ flags, refer to the Bitcoin documentation on it [here](https://github.com/bitcoin/bitcoin/blob/master/doc/zmq.md).

By default, the wallet runs on mainnet. If you wish to use testnet or regtestnet, you can use 1 of these flags (but not both).

- -testnet
- -regtest

### XAYA QT Toolkit

The XAYA QT wallet download includes useful tools that will make your life much easier and more enjoyable.

The XAYA Command Line Interface (CLI), xaya-cli, is very useful for running commands quickly. You can think of this as a more convenient version of the QT wallet's console. 

Here's a [quick tutorial on xaya-cli](xaya-cli.md) that you may wish to read later.

## About xayad

xayad is the core daemon and where the "work" is done. The QT wallet is just a convenient GUI for it. 

# Get Some CHI

You can develop your game using testnet or regtestnet, in which case you don't need to purchase any real CHI. 

For testnet CHI, you can mine some yourself or you can contact us to send you some.

For regtestnet CHI, you must mine this yourself. Find out more about [regtestnet here](Regtestnet.md).

Both testnet and regtestnet are free, but at some point you will want to run your game on mainnet. You can purchase CHI on an exchange. See the [XAYA website](https://xaya.io/) for more information.

# libxayagame

The libxayagame daemon does much of the heavy lifting that blockchain programming requires. There are several places to look for it. Click any of the following for what you need. If you have questions, you can check a tutorial or post in the [Development forum](https://forum.xaya.io/).

- [libxayagame](https://github.com/xaya/libxayagame): The C++ source code
- [libxayagame_wrapper](https://github.com/xaya/libxayagame_wrapper): A wrapper for libxayagame. Creates a statically linked library for Windows
- [Precompiled binaries for Windows](XayaStateProcessor/): Statically linked library with all its dependencies
- XAYAWrapper: A C# project that wraps the libxayagame DLL 

You don't need to decide on this right now, but you will need to download whichever option you need when it comes time to start coding. 

For information on GSPs and how libxayagame relates to the XAYA daemon and your game, see [libxayagame Component Relationships](libxayagame%20Component%20Relationships.md)

# RPC and JSON libraries

Creating a game with XAYA involves a great deal of RPC calls and using JSON. You may wish to use existing libraries rather than roll your own. Whichever you choose is entirely up to you. 




