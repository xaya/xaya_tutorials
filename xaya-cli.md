# Getting Started with xaya-cli

The XAYA Command Line Interface (xaya-cli) program is a command line interface for interacting with the XAYA daemon and XAYA wallets. Much of what can be done in xaya-cli can also be done in the XAYA QT wallet's console. However, there are advantages to using xaya-cli instead of the QT console. As such, much of the XAYA documentation uses xaya-cli.

The following is to help you get up to speed on how to use xaya-cli. If you are already comfortable with CLIs, then you can simply run it with the `-?` and `help` parameters to get the information you need and skip this tutorial. 

This tutorial is designed as a relaxing walk through that you can follow along with and experiment with as you read. Take time to run some of the commands and see how they work for you. Experience is a great teacher. 

# Additional Documentation from Bitcoin

Much of what you will need to know about commands is already documented in [Bitcoin documentation](https://bitcoin.org/en/developer-reference). This tutorial is not a substitute for upstream documentation. 

# Getting Ready...

If you haven't already, [download the XAYA QT wallet software here](https://github.com/xaya/xaya/releases) and extract the contents of the ZIP file. On Windows, you'll have the following files:

- xaya-cli.exe
- xaya-hash.exe
- xaya-qt.exe
- xaya-tx.exe
- xayad.exe

On Linux you'll have executable binaries. You'll need to CHMOD them to make them executable. 

You don't need to run the XAYA QT wallet to use xaya-cli. It will work with the XAYA Electron wallet. Our examples here will use the XAYA Electron wallet. If you wish to use the XAYA QT wallet, you must run it with the `-server=1` option as it does not run as a server by default. 

	xaya-qt -server=1

On the other hand, the Electron wallet does run as a server by default. It's pre-configured to work with games. If you've not already got the Electron wallet running, start it. 

# Let's Get Started!

Open a console (command prompt in Windows or terminal in Linux) and navigate to that folder. 

You can get help for xaya-cli with this command:

	xaya-cli -?

Run that now and have a quick glance.

At the top of the help you'll see some usage cases. Here we'll only examine the first one:

> xaya-cli [options] &lt;command&gt; [params]

The `[options]` are from the help that you just viewed. For many operations we don't need to specify any options and can simply send a command with parameters. Here are some examples. They are all are perfectly safe to use, so feel free to try them right now.

	xaya-cli getblockcount
	xaya-cli getdifficulty
	xaya-cli getblockchaininfo
	xaya-cli getblockhash 123
	xaya-cli gettxoutsetinfo
	xaya-cli uptime
	xaya-cli game_sendupdates "g/mv" "2aed5640a3be8a2f32cdea68c3d72d7196a7efbfe2cbace34435a3eef97561f2" "3b365d712d87e354a36a6b0445fd022322e559bb9b4dc493f8eea3328d670197"
	xaya-cli trackedgames
	xaya-cli trackedgames "add" "mv"

## Playing Safely

As above, these are safe commands and you won't do any damage by using them. We'll warn you for commands that have real consequences. 

To interact with XAYA with zero fear, you can run regtestnet. See the [Getting Started with Regtestnet](Regtestnet.md) tutorial for information about that. For now, we return back to the real world mainnet.

## Options vs. Commands

The options are for xaya-cli. It passes those options on to the XAYA daemon. Some of the options that you will be more likely to need are:

- -testnet
- -regtest
- -rpcwallet=&lt;walletname&gt;
- -rpcuser=&lt;user&gt;
- -rpcpassword=&lt;pw&gt;

Commands are the meat and potatoes. You can get a complete list of commands in the QT console by typing "help", or you can get them via xaya-cli into a text file for easy reference as shown below:

	xaya-cli help >command-help.txt

That will create a "command-help.txt" file in the same folder as the xaya-cli program.

Commands "do things". Where options "tell us where to go" to get things done, the commands say what should be done once we get there. 

Commands can put data onto the blockchain, get data from the blockchain, manage wallets, and much more. Skim through the command-help.txt file to get a feel for the kinds of commands that are available to you. 

For more in-depth information, run the following where "&lt;command name&gt;" is the name of the command that you want more information on:

	xaya-cli help <command name>

So, for example, if you want information about creating new addresses, use this:

	xaya-cli help getnewaddress

You can also check the [Bitcoin documentation](https://bitcoin.org/en/developer-reference) for command documentation as much of it is the same. 

## Continuing on...

Whether or not you need options will in part depend upon the wallet software that you are running (Electron vs. QT) and any starting parameters that were passed to the wallet software when it started up, or any parameters that were passed to the wallet from inside the xaya.conf file in the data directory. You can find the data directory here on Windows, Linux, and Mac OS X, respectively:

	%appdata%\Xaya\xaya.conf
	~/.xaya/xaya.conf
	~/Library/Application Support/Xaya/xaya.conf

## Dealing with Wallets

When working with real wallets, such as the Electron game wallet, we must specify which wallet to use with the `-rpcwallet=&lt;walletname&gt;` flag. The following command gets the balance from the game wallet and vault wallet, respectively.

	xaya-cli -rpcwallet=game.dat getbalance
	xaya-cli -rpcwallet=vault.dat getbalance

To load the wallet.dat file used by the QT client, use the following command:

	xaya-cli loadwallet "wallet.dat"

This will not cause the wallet to appear in the Electron UI, but it will make the wallet available to the XAYA daemon, which is what xaya-cli is querying. 

You can get a list of currently loaded wallets using the following command:

	xaya-cli listwallets

The result after having loaded the normal QT wallet would look like so (because we're running the Electron wallet):

	[
	  "vault.dat",
	  "game.dat",
	  "wallet.dat"
	]

You can unload the default QT wallet as shown below:

	xaya-cli unloadwallet "wallet.dat"

# Some Cooler Stuff You Can Do

Now that we've looked a bit at wallets, let's do some cooler stuff starting with backing up a wallet. For this, we must specify the wallet to back up with a xaya-cli option, e.g. "-rpcwallet=game.dat". We must also specify a complete path, e.g. "C:\XAYA Backups\MyFirstBackup.dat". Make certain that the folder exists.

	xaya-cli -rpcwallet=game.dat backupwallet "C:\XAYA Backups\MyFirstBackup.dat"

## Creating a CHI Address

Now that we have a backup of our game wallet, let's do some more cool stuff like creating a new address.

	xaya-cli -rpcwallet=game.dat getnewaddress "This is my new address!"

The command will run and then return an address, e.g. "CXTdDBYrZEmSHWrsq8hFv5R78NrH8Q2Bxd". 

You can then go to your Electron wallet and verify that the address is in there. Check the Receive tab and scroll down. Addresses are listed alphabetically by label.

Any cryptocurrency will let you do that as it's very basic. However, there are methods that you won't find in any other cryptocurrency, other than perhaps Namecoin and Huntercoin. But even those don't have all the options you have in XAYA. 

## Registering a Name

**NOTE:** This operation costs CHI and is permanent on the blockchain. 

If your user doesn't already have a name in their wallet to use in your game. You can register a name for them. You would normally do this inside of your game and through the RPC interface, but you can also register names from the console with xaya-cli. Here's an example:

	xaya-cli -rpcwallet=game.dat name_register "p/Bugs Bunny" "{}"

The arguments are:

- Name: "p/Bugs Bunny"
- Value: "{}"

The name must have a namespace. For player accounts this is "p/". Everything after that is the "name" per se. 

The value must be valid JSON. In this case we simply send an empty value. There's no reason to do more than that if the goal is to simply register a name. However, it could include an initial move in a game. 

The return value for that command was "1e57a1136711e008f5046025613c1835515eb7adf175a15bab8a52b3eec28e5f". You can verify this on the XAYA blockchain by using xaya-cli, or you can check the XAYA blockchain explorer [here](https://explorer.xaya.io/tx/1e57a1136711e008f5046025613c1835515eb7adf175a15bab8a52b3eec28e5f).

As mentioned above, you can get more help on the `name_register` operation with this command:

	xaya-cli help name_register

# Summary

We've taken a slow journey into examining xaya-cli. We walked through different kinds of examples. In some examples we used options for xaya-cli and in others we didn't need any options. We looked at several generic commands that are also found in Bitcoin. We also looked at some commands that are specific to XAYA. 

The techniques you learned above through examples can be applied more generally. Check the documentation for the options and commands as shown above. 

Remember to check the [regtestnet tutorial](Regtestnet.md) to find out how you can experiment in a private blockchain without fear of making a mistake. 


















