# Getting Started with Regtestnet

During development you'll need to interact with the XAYA blockchain. However, using real CHI is expensive and permanent on the blockchain. Using testnet is a much better option, but you will still need to mine testnet CHI to use as testnet mimics mainnet, including how mining is done.

Regtestnet is a better solution for developers looking to put their games and apps on the XAYA blockchain. It is a private network where no peers are needed and you can mine new blocks at will.

# What is Regtestnet?

Regtestnet is a private blockchain where no peers are needed. It is entirely separate from mainnet and testnet, and you can mine blocks whenever you wish.

Regtestnet is also called:

- Regression test mode
- regtest
- regtest mode

Note that the RPC port numbers differ for the various networks:

- Mainnet = 8396
- Testnet = 18396
- Regtestnet = 18493

As do the P2P ports:

- Mainnet = 8394
- Testnet = 18394
- Regtestnet = 18495

# Back Up  Your Wallets

Before doing any kind of development, make sure that you've backed up your wallets. See the [XAYA forums](https://forum.xaya.io/topic/238-how-to-and-video-help/) for tutorials and videos about how to do that. Which ever interface you choose to use is entirely up to you. 

# Xaya-cli vs. XAYA QT Console

The examples here use xaya-cli from a console (command prompt/terminal) instead of using the QT wallet's console. 

For those unfamiliar, you can simply delete the "xaya-cli -regtest" from the beginning and the run the rest of the command in the QT console, e.g. the xaya-cli command:

	xaya-cli -regtest getbalance

In the QT console would simply become:

	getbalance

One major advantage of using xaya-cli is that you can pipe output to a file. e.g.:

	xaya-cli -regtest getbalance >mybalance.txt

Also, should you wish, you can program against xaya-cli very easily, whereas remotely controlling a GUI application, such as the QT wallet, is very difficult. 

# Firing Up Regtestnet

To use regtestnet, you'll most likely want to work with the XAYA QT wallet and xaya-cli. If you haven't already, you can download that [here](https://github.com/xaya/xaya/releases).

Open a command prompt or terminal and navigate to the folder where you extracted the QT files.

Next, run the QT wallet with the `-regtest` and `server=1` flags as shown below. You must use the `-server=1` flag in order to use xaya-cli, otherwise you'll get errors from xaya-cli because it cannot connect.

	xaya-qt -regtest -server=1

The XAYA QT wallet program will open up and you'll have a zero CHI balance. You can ignore the first screen; click the Hide button.

![Fire up regtestnet](img/XAYA%20QT%20wallet%20-regtest%20-server=1.png)

You're now running your own regtestnet. 

## Multiple Peers in Your Regtestnet Network

**NOTE:** You may wish to skip this section and return to it once you've finished this tutorial. Don't get hung up on this to start. This will be most useful to you once you've got your game well into development. 

You may wish to have more than a single instance of your game running on regtestnet. To accomplish this, use the `addnode` command. 

You'll need to know your local area network IP addresses, e.g. 192.168.0.123 and 192.168.0.135. If you need to use ports when specifying the IP address, use the P2P port, e.g. 192.168.0.123:18495. If you're running the daemon with a custom xaya.conf file and you've changed ports in there, then you'll need to specify them as per your xaya.conf file entries.

On the "123" machine, run the following command:

	xaya-cli -regtest addnode "192.168.0.135" "add"

This will add the "135" machine as a node to the "123" machine.

On the "135" machine, run the following command:

	xaya-cli -regtest addnode "192.168.0.123" "add"

This will add the "123" machine as a node to the "135" machine.

**NOTE:** The return value for `addnode` is `null` and `getnodeaddresses` will return an empty set. This is normal. Instead, run `getpeerinfo`.

	xaya-cli -regtest getpeerinfo

That will return a set of information about peers. Check the `addr` field to see the IP address of the connected node.

They should sync very quickly, however, you may wish to mine a few blocks. See below for how to do that.

# Check Your CHI Balance

Back in your command prompt or terminal, check your balance with the `-regtest` flag and the `getbalance` command:

	xaya-cli -regtest getbalance

You should see your zero balance as "0.00000000", i.e. to 8 decimal places.

# Create a CHI Address

Next, you'll need to create address so that you can mine regtestnet CHI into. The `getnewaddress` command takes 2 optional arguments. In this example, we only set the first one for the label. Enter the following command into your command line:

	xaya-cli -regtest getnewaddress "My label"

That will output an address for you, e.g. "ceeABTxXdFaeL4eJKrAHqEatXXb7mwk1tS". You can also check that it is in your restestnet wallet in the QT client through File > Receiving addresses...

![A new regtestnet CHI address](img/New%20regtestnet%20address.png)

# Mine Some CHI

When coins are mined, they are "immature" and cannot be spent until they become "mature" after 100 confirmation.

Consequently, in order to get 1 block reward of coins that are spendable, you must mine at least 101 blocks. 

Before continuing, you may wish to set the number of buffers in your command prompt or terminal to a higher number so that you can scroll up. For our purposes here, it should be more than 101. However, this is entirely optional and does not affect anything in regtestnet; it is purely for convenience.

The following command uses the address generated above to mine 101 blocks.

	xaya-cli -regtest generatetoaddress 101 ceeABTxXdFaeL4eJKrAHqEatXXb7mwk1tS

This outputs a set of 101 block hashes, e.g.:

	[
	  "7e5ae5e5ba085957dd94e7c6fb0d77847797e85d45e900a99228b3ef91058566",
	  "eab4868ab84ef0b9f66b2c7fc8105323ddf5f085530d0f6aed5345dc27e7aa74",
	  ...
	  "3cb0837fcda37070faf455e6143e8fd494b483362707616647a370638f844951"
	]

You can run the `getbalance` command used above to see that you now have 50 CHI in your wallet. Remember, newly mined coins require 100 blocks to mature and be spendable. You can also look in the QT client to see the results.

Here we see the Overview with 50 CHI available, and 5000 CHI immature.

![XAYA QT Overview 50 CHI.png](img/XAYA%20QT%20Overview%2050%20CHI.png)

You can also check the Transactions tab and scroll to see that only 1 is fully confirmed.

![XAYA QT Transactions](img/XAYA%20QT%20Transactions.png)

You can repeat the above `generatetoaddress` command and you'll see that your balance increases. However, as you mine more blocks, the number of immature coins will slowly decline because immature coins require 100 confirmations to mature and the block reward declines very rapidly compared to mainnet, e.g. by block 1200 the reward will be 0.19531250 CHI.

## Mining at Will

As you can see from the above, you can mine new regtestnet blocks/CHI at will. However, unless you specifically mine new CHI, no more blocks will be mined. 

Depending upon your requirements, you may wish to create a small program that lets you mine new blocks in a controlled way, e.g. 1 block at a time, or perhaps spurts of 10 or more blocks at once, or perhaps on a timer at specific intervals, e.g. every 30 seconds. 

## Forcing Reorgs to Test Undo 

There are times when a blockchain undergoes a reorg. These situations are difficult to deal with, but that hard work is all done for you through the [libxayagame](https://github.com/xaya/libxayagame/) library. 

During the development of your game, you'll need to test your undo data, but this can't realistically be done on mainnet or testnet as you cannot predict when a reorg event will happen. Consequently, you must use regtestnet. You'll need the `invalidateblock` and `reconsiderblock` methods to do this.

Your testing workflow will typically follow this pattern:

1. Call the `invalidateblock` method
2. Do some alternative moves in your game
3. Call the `generatetoaddress` method to mine the move in step #2 onto the blockchain
4. Possibly repeat steps #2 and #3 to get more moves into the blockchain
5. Call the `reconsiderblock` method

You can then see how your code handles that situation and react accordingly. 

The `invalidateblock` and `reconsiderblock` methods take a blockhash as their parameters. You can call them as shown below:

	xaya-cli -regtest invalidateblock "12d102d12ddbe8fe0dabc03e9488efad481d87c0b084398307a7fb54592fb26c"
	xaya-cli -regtest reconsiderblock "12d102d12ddbe8fe0dabc03e9488efad481d87c0b084398307a7fb54592fb26c"

## Resetting Regtestnet

To reset regtestnet, simply rename or delete the regtest folder in the data directory. 

# Summary

In this tutorial we looked at:

- How to start regtestnet
	+ How to add nodes (other machines) to regtestnet
- How to get a balance in the wallet
- How to create a CHI address
- How to mine new blocks and get regtestnet CHI
- How to force reorgs to test undo data

# Where to Go from Here?

There are other tutorials, but perhaps what would be most useful at this point is for you to create a regtestnet program that lets you mine new blocks in a controlled way. Using xaya-cli may be an option for you if you like using consoles. However, you may wish to create a program with a GUI. If so, you'll most likely want to use the RPC interface to issue commands directly to the daemon. There are 2 RPC tutorials to help you get started there:

- [XAYA RPC Methods](XAYA%20RPC%20Methods.md)
- [RPC Windows C# Tutorial](RPC%20Windows%20C%23%20Tutorial/README.md)

