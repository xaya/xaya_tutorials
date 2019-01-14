# XAYA RPC Methods

XAYA has many RPC methods over and above those available in Bitcoin. These are some notable ones that will be valuable to you in building your game. 

- name_list ("name")
- name_register "name" "value" ("options")
- name_update "name" "value" ("options")
- name_scan ("start" ("count" ("options")))
- name_show "name"
- sendtoname "name" amount ( "comment" "comment_to" subtractfeefromamount replaceable conf_target "estimate_mode")
- name_pending ("name")
- name_history "name"

## Most Commonly Used

During the development of your game, the most commonly used RPC methods will likely be (in no particular order):

- [name_list](#name_list)
- [name_register](#name_register)
- [name_update](#name_update)

Several other methods are described below, though their usefulness to you may be limited during the development of your game.

## Sending RPC Commands

The XAYA daemon communicates through plain text and JSON. You can use the XAYA QT wallet's console, the xaya-cli command line program that comes with the QT wallet, or any RPC library in any language that you prefer. 

The examples below use xaya-cli and a XAYA game wallet. 

Most likely, you will wish to use an RPC library inside of your game for RPC calls. 

See [Interacting with the XAYA Wallet Through RPC in C#](https://github.com/xaya/xaya_tutorials/blob/master/RPC%20Windows%20C%23%20Tutorial/XAYA%20RPC%20Tutorial.md) for a sample application and tutorial of using the RPC methods through a 3rd party library. 

## Names

Names are unique entries in the XAYA blockchain. They can be used for game accounts or to create games. 

The maximum length of a name is 256 bytes.

All names exists in a namespace, e.g. "p/" is for player accounts and "g/" is for games. A namespace ends with the "/" character, and everything following that is the name. The following are examples of valid names:

- p/John Doe
- p/Larry
- g/mv
- g/tftr

The following are examples of invalid names:

- John Doe
- Larry
- mv
- tftr

Names have data associated with them on the XAYA blockchain:

- **name**: The name itself, including the namespace
- **name_encoding**: This is the character encoding for the name, and is usually UTF-8
- **value**: All names can have a value associated with them. Values are contained in JSON. The maximum length of a value is 2048 bytes. Developers should try to minimize the length of values
- **value_encoding**: This is the character encoding for the value
- **txid**: This is the transaction ID for the last transaction involving the name
- **vout**: This is something that Daniel should explain
- **height**: This is the block height of the last transaction for this name
- **ismine**: This indicates whether or not the name is owned by the current wallet
- **op**: Not in all name data. Indicates an operation for the transaction, e.g. "name_register"

The value is where game moves are. See name_update below for how to submit moves into the XAYA blockchain.

## name_list

This returns a list of names in a wallet if no name is passed to it. It takes one optional parameter.

- **name**: A name including a namespace.

If a name is passed, it returns an empty set if the name doesn't exist on the XAYA blockchain, and if it exists, the method returns the name with additional data about the name, including whether it is owned by the wallet (ismine). 

The following command tests to see if "p/xaya" exists in the current wallet.

	xaya-cli -rpcwallet=game.dat name_list "p/xaya"

As it isn't owned by the current wallet, the following result is returned:

	[
	]

That result will be the same for any wallet with no names in it if the method is called without a name as a parameter.

The following command tests to see if "p/Name pending test 1" exists in the current wallet.

	xaya-cli -rpcwallet=game.dat name_list "p/Name pending test 1"

As it does exist in the author's current wallet, the following result is returned:

	[
	  {
	    "name": "p/Name pending test 1",
	    "name_encoding": "utf8",
	    "value": "{}",
	    "value_encoding": "ascii",
	    "txid": "6d8335340447690d3e37ce3c32ffdc3ef1071aaa21dd2bdb7143de49cf034921",
	    "vout": 1,
	    "address": "CaSGURq6BS3FQsqj1zQUrRj9iwbw4Rgg1k",
	    "ismine": true,
	    "height": 508545
	  }
	]

Running that command will return zero results for you. Create 1 or more names first in order to get a non-empty result set.

Without a name as a parameter, the complete list of names in a wallet is returned.

### For Your Game

Your users may or may not have names in their wallet. The name_list method gives you a convenient way to check their wallets and see what names they have or if they have any names already. 

If they have names, you must always check the "ismine" property to ensure that it is "true". 

If they have no names, you can prompt them to create a name to use in your game.

## name_register

This method registers a name on the XAYA blockchain. That name can then be used in games. It takes 2 mandatory parameters and 1 optional parameter. The mandatory parameters are:

- **name**: The name to create. It must include a namespace, e.g. "p/" for player account names
- **value**: Valid JSON. This can simply be "{}" or "[]"

It returns a txid if the transaction is successful or an error if it fails.

	xaya-cli -rpcwallet=game.dat name_register "p/xaya" "{}"

The txid returned for that command is "c87eb7b9c71146a18f8ebaea93b74bfeb4795b265f151ba5a1e62ef32017bc34". 

## name_update

This method updates the value associated with a name. It takes 2 mandatory parameters and 1 optional parameter. The mandatory parameters are:

- **name**: The name to update
- **value**: Valid JSON containing data

In order to make the best use of XAYA, developers should follow the format for name updates outlined in [Name and Value Restrictions](https://github.com/xaya/xaya_docs/blob/master/blockchain.md#name-and-value-restrictions-), [Moves](https://github.com/xaya/xaya_docs/blob/master/games.md#moves-), and [Sending Moves](https://github.com/xaya/xaya_docs/blob/master/interface.md#sending-moves).

The following command updates the name "p/Name pending test 1" to an empty value, i.e. "{}". (Remember that the name must exist in the wallet.)

	xaya-cli -rpcwallet=game.dat name_update "p/Name pending test 1" "{}"

The return value for a name_update is a txid, e.g. "4909229f9d50690e7de9bd8fca10df8a554859eee4b64e441a9955fbf4d57253". 

An error will be returned for unsuccessful calls, e.g. "Input tx not found in wallet (code -4)".

This will likely be the most important RPC call for most game developers as it is how game moves are entered into the blockchain. 

## name_scan

This returns a list of existing names on the XAYA blockchain. It takes optional arguments.

- **start**: The block to start scanning at
- **end**: The block to end scanning at
- **options**: A further set of optional parameters

With no parameters it returns 500 names, which isn't particularly useful to you. 

The following command outputs all name data for blocks 1 to 500,000 to a file.

	xaya-cli -rpcwallet=game.dat name_scan 1 500000 >outputfile.txt

The returned data list is too long to show here. 

Similar to name data above, the following is 1 example name from the complete result set to illustrate how is presented to you:

	  {
	    "name": "p/xaya",
	    "name_encoding": "utf8",
	    "value": "{}",
	    "value_encoding": "ascii",
	    "txid": "c87eb7b9c71146a18f8ebaea93b74bfeb4795b265f151ba5a1e62ef32017bc34",
	    "vout": 1,
	    "address": "CWP23M9HUAjt917x97DY4X9WnhkL7gJqEH",
	    "height": 15,
	    "ismine": false
	  }

## name_show

This returns the name and associated data if the name exists. It takes a name as its single parameter.

- **name**: The name to return if it exists.

The following command returns the results for "p/xaya".

	xaya-cli -rpcwallet=game.dat name_show "p/xaya"

The returned data is:

	{
	  "name": "p/xaya",
	  "name_encoding": "utf8",
	  "value": "{}",
	  "value_encoding": "ascii",
	  "txid": "c87eb7b9c71146a18f8ebaea93b74bfeb4795b265f151ba5a1e62ef32017bc34",
	  "vout": 1,
	  "address": "CWP23M9HUAjt917x97DY4X9WnhkL7gJqEH",
	  "height": 15,
	  "ismine": false
	}

Should the owner of "p/xaya" run the command, the only differences would be that:

- "ismine" would be "true"
- the address may be different if the name were sent to another address

## sendtoname

This method sends CHI by specifying a name instead of a regular CHI address. It has 2 mandatory parameters.

- **name**: The name to send the CHI to
- **amount**: The amount of CHI to send

Here's an example:

	xaya-cli -rpcwallet=game.dat sendtoname "p/xaya" "0.1"

The returned result is a txid for a successful transaction or an error message. 

sendtoname is a convenient way for people to specify where to send coins. Names are human readable whereas crypto addresses are long, complex and difficult to remember.

## name_pending

This returns a list of currently pending names if no name is passed to it. It returns an empty set if no names are currently pending. Passing a name to this method checks to see if that name is currently pending. An empty set is returned if the name does not exist or already exists and is not pending. It takes 1 optional parameter.

- **name**: The name to check whether or not it is currently pending

Pending means that the name is in the mempool but isn't confirmed yet, i.e. it has 0 confirmations.

At block height 508,544, the following examples returned the same result.

	xaya-cli -rpcwallet=game.dat name_pending
	xaya-cli -rpcwallet=game.dat name_pending "p/Name pending test 1"

Their result was:

	[
	  {
	    "name": "p/Name pending test 1",
	    "name_encoding": "utf8",
	    "value": "{}",
	    "value_encoding": "ascii",
	    "txid": "6d8335340447690d3e37ce3c32ffdc3ef1071aaa21dd2bdb7143de49cf034921",
	    "vout": 1,
	    "address": "CaSGURq6BS3FQsqj1zQUrRj9iwbw4Rgg1k",
	    "ismine": true,
	    "op": "name_register"
	  }
	]

Had another name been pending at the time, it would have appeared in that list.

To test this method and not return an empty set, you may wish to quickly create 1 or more names and then run the name_pending method quickly. 

## name_history

This is generally not needed, and requires the blockchain to be specifically organised to use this method. As such, further description is omitted. 








