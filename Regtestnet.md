# Getting Started with Regtestnet

During development you'll need to interact with the XAYA blockchain. However, using real CHI is expensive and permanent on the blockchain. Using testnet is a much better option, but you will still need to mine testnet CHI to use as testnet mimics mainnet, including how mining is done.

Regtestnet is a better solution for developers looking to put their games and apps on the XAYA blockchain. It is a private network with no peers and you can mine new blocks at will.

## What is Regtestnet?

Regtestnet is a private blockchain with no peers. It is entirely separate from mainnet and testnet, and you can mine blocks whenever you wish.

This puts you fully in control

## Back Up  Your Wallets

Before doing any kind of development, make sure that you've backed up your wallets. See the [XAYA forums](https://forum.xaya.io/topic/238-how-to-and-video-help/) for tutorials and videos about how to do that.

## Firing Up Regtestnet

To use regtestnet, you'll most likely want to work with the XAYA QT wallet and xaya-cli. If you haven't already, you can download that [here](https://github.com/xaya/xaya/releases).

Open a command prompt or terminal and navigate to the folder where you extracted the QT files.

Next, run the QT wallet with the `-regtest` and `server=1` flags as shown below.

	xaya-qt -regtest -server=1

The XAYA QT wallet program will open up and you'll have a zero CHI balance. You can ignore the first screen; click the Hide button.

![xaya-qt -regtest -server=1](XAYA QT wallet%20-regtest%20-server=1.png)

You're now running on regtestnet. 

### Check Your CHI Balance

Back in your command prompt or terminal, check your balance with the `-regtest` flag and the `getbalance` command:

	xaya-cli -regtest getbalance

You should see your zero balance as "0.00000000", i.e. to 8 decimal places.

## Create a CHI Address

Next, you'll need to create address so that you can mine regtestnet CHI into. The `getnewaddress` command takes 2 optional arguments. In this example, we only set the first one for the label. Enter the following command into your command line:

	xaya-cli -regtest getnewaddress "My label"

That will output an address for you, e.g. "ceeABTxXdFaeL4eJKrAHqEatXXb7mwk1tS". You can also check that it is in your restestnet wallet in the QT client through File > Receiving addresses...

![](New%20regtestnet%20address.png)

### Mine Some CHI

When coins are mined, they are "immature" and cannot be spent until they become "mature" after 100 confirmation.

Consequently, in order to get 1 block reward of coins that are spendable, you must mine at least 101 blocks. 

Before continuing, you may wish to set the number of buffers in your command prompt or terminal to a higher number so that you can scroll up. For our purposes here, it should be more than 101. However, this is entirely optional and does not affect anything in regtestnet; it is purely for convenience.

The following command uses the address generated above to mine 101 blocks.

	xaya-cli -regtest generatetoaddress 101 ceeABTxXdFaeL4eJKrAHqEatXXb7mwk1tS

This outputs a set of 101 transaction IDs (txid), e.g.:

	[
	  "7e5ae5e5ba085957dd94e7c6fb0d77847797e85d45e900a99228b3ef91058566",
	  "eab4868ab84ef0b9f66b2c7fc8105323ddf5f085530d0f6aed5345dc27e7aa74",
	  ...
	  "3cb0837fcda37070faf455e6143e8fd494b483362707616647a370638f844951"
	]

You can run the `getbalance` command used above to see that you now have 50 CHI in your wallet. Remember, newly mined coins require 100 blocks to mature and be spendable. You can also look in the QT client to see the results.

Here we see the Overview with 50 CHI available, and 5000 CHI immature.

![XAYA QT Overview 50 CHI.png](XAYA%20QT%20Overview%2050%20CHI.png)

You can also check the Transactions tab and scroll to see that only 1 is fully confirmed.

![XAYA QT Transactions](XAYA%20QT%20Transactions.png)

You can repeat the above `generatetoaddress` command. You'll see that your balance increases, and that your number of immature coins remains the same, i.e. immature coins will always be 5,000 when 100 or more blocks are mined because they require 100 confirmations to mature. 

### Mining at Will

As you can see from the above, you can mine new regtestnet blocks/CHI at will. However, unless you specifically mine new CHI, no more blocks will be mined. 

Depending upon your requirements, you may wish to create a small program that lets you mine new blocks in a controlled way, e.g. 1 block at a time, or perhaps spurts of 10 or more blocks at once, or perhaps on a timer at specific intervals, e.g. every 30 seconds. 

## Summary

In this tutorial we looked at:

- How to start regtestnet
- How to get a balance in the wallet
- How to create a CHI address
- How to mine new blocks and get regtestnet CHI

## Where to Go from Here?

There are other tutorials, but perhaps what would be most useful at this point is for you to create a regtestnet program that lets you mine new blocks in a controlled way. Using xaya-cli may be an option for you if you like using consoles. However, you may wish to create a program with a GUI. If so, you'll most likely want to use the RPC interface to issue commands directly to the daemon. There are 2 RPC tutorials to help you get started there:

- [XAYA RPC Methods](XAYA%20RPC%20Methods.md)
- [RPC Windows C# Tutorial](RPC%20Windows%20C#%20Tutorial/XAYA%20RPC%20Tutorial.md)

