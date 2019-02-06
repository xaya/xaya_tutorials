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

Several other methods are also described below, though their usefulness to you may be limited during the development of your game.

## Sending RPC Commands

The XAYA daemon communicates through JSON-RPC over HTTP (see [jsonrpc.org](https://www.jsonrpc.org/) for protocol information). You can use the XAYA QT wallet's console, the `xaya-cli` command line program that comes with the QT wallet, or any RPC library in any language that you prefer. 

The examples below use xaya-cli and a XAYA game wallet. 

Most likely, you will wish to use an RPC library inside of your game for RPC calls. 

See [Interacting with the XAYA Wallet Through RPC in C#](RPC%20Windows%20C%23%20Tutorial/README.md) for a sample application and tutorial of using the RPC methods through a 3rd party library. 

# Names

Names are unique entries in the XAYA blockchain. They can be used for game accounts or to create games. See [Name and Value Restrictions](blockchain.md#name-and-value-restrictions-) for information on restrictions and limitations. 

All names exists in a [namespace](https://github.com/xaya/xaya_docs/blob/master/blockchain.md#name-and-value-restrictions-), e.g. `p/` is for player accounts and `g/` is for games. A namespace ends with the `/` character, and everything following that is the name. The following are examples of valid names:

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

- **name**: The name itself, including the [namespace](https://github.com/xaya/xaya_docs/blob/master/blockchain.md#name-and-value-restrictions-)
- **name_encoding**: This is the character encoding for the name, and **must** be UTF-8, although they _can_ be non-ASCII UTF-8
- **value**: All names can have a value associated with them. Values are contained in JSON. The maximum length of a value is 2048 bytes. Developers should try to minimize the length of values
- **value_encoding**: This is the character encoding for the value
- **txid**: This is the transaction ID for the last transaction involving the name
- **vout**: This is the index of the last transaction (`txid`) where the name output is
- **address**: This is the CHI address that owns/contains the name
- **height**: This is the block height of the last transaction for this name
- **ismine**: This indicates whether or not the name is owned by the current wallet
- **op**: Not in all name data. Indicates an operation for the transaction, e.g. `name_register`

The order and appearance of those properties may vary slightly in how/if they are returned in different methods. 

The value is where game moves are. See [`name_update`](#name_update) below for how to submit moves into the XAYA blockchain.

# Commands

The following are RPC commands that you can issue.

## name_list

This returns a list of names in a wallet if no name is passed to it. It takes one optional parameter.

- **name**: A name including a [namespace](https://github.com/xaya/xaya_docs/blob/master/blockchain.md#name-and-value-restrictions-).

If a name is passed, it returns an empty set if the name doesn't exist on the XAYA blockchain, and if it exists, the method returns the name with additional data about the name, including whether it is owned by the wallet (ismine). 

The following command tests to see if "p/xaya" exists in the current wallet.

	xaya-cli -rpcwallet=game.dat name_list "p/xaya"

If it isn't owned by the current wallet, the following result is returned:

	[
	]

That result will be the same for any wallet with no names in it if the method is called without a name as a parameter.

If "p/xaya" exists in the current wallet, the following result is returned:

	[
		{
		  "name": "p/xaya",
		  "name_encoding": "utf8",
		  "value": "{}",
		  "value_encoding": "ascii",
		  "txid": "c87eb7b9c71146a18f8ebaea93b74bfeb4795b265f151ba5a1e62ef32017bc34",
		  "vout": 1,
		  "address": "CWP23M9HUAjt917x97DY4X9WnhkL7gJqEH",
		  "ismine": true
		  "height": 15,
		}
	]

Running that command will return zero results for you. Create 1 or more names first in order to get a non-empty result set.

Without a name as a parameter, the complete list of names in a wallet is returned. For some wallets this can be a large number.

### For Your Game

Your users may or may not have names in their wallet. The `name_list` method gives you a convenient way to check their wallets and see what names they have or if they have any names already. 

If they have names, you must always check the `ismine` property to ensure that it is `true`. 

If they have no names, you can prompt them to create a name to use in your game.

## name_register

This method registers a name on the XAYA blockchain. That name can then be used in games. It takes 2 mandatory parameters and 1 optional parameter. 

- **name**: The name to create. It must include a [namespace](https://github.com/xaya/xaya_docs/blob/master/blockchain.md#name-and-value-restrictions-), e.g. `p/` for player account names
- **value**: Valid JSON passed as a string. This can simply be `"{}"` or `"[]"`
- **options**: This must be passed directly as a JSON object

**NOTE:** When passing `options` to xaya-cli, it will automatically convert it to a JSON object, but when calling through a JSON-RPC library, you must cast `options` as a JSON object yourself.

It returns a `txid` if the transaction is successful or an error if it fails.

	xaya-cli -rpcwallet=game.dat name_register "p/xaya" "{}"

The `txid` returned for that command is "c87eb7b9c71146a18f8ebaea93b74bfeb4795b265f151ba5a1e62ef32017bc34". 

## name_update

This method updates the value associated with a name. It takes 2 mandatory parameters and 1 optional parameter. 

- **name**: The name to update
- **value**: Valid JSON passed as a string and containing data
- **options**: This must be passed directly as a JSON object

**NOTE:** When passing `options` to xaya-cli, it will automatically convert it to a JSON object, but when calling through a JSON-RPC library, you must cast `options` as a JSON object yourself.

In order to make the best use of XAYA, developers should follow the format for name updates outlined in [Name and Value Restrictions](https://github.com/xaya/xaya_docs/blob/master/blockchain.md#name-and-value-restrictions-), [Moves](https://github.com/xaya/xaya_docs/blob/master/games.md#moves-), and [Sending Moves](https://github.com/xaya/xaya_docs/blob/master/interface.md#sending-moves).

The following command updates the name `p/Name pending test 1` to an empty value, i.e. `{}`. (Remember that the name must exist in the wallet.)

	xaya-cli -rpcwallet=game.dat name_update "p/Name pending test 1" "{}"

The return value for a `name_update` is a `txid`, e.g. "4909229f9d50690e7de9bd8fca10df8a554859eee4b64e441a9955fbf4d57253". 

An error will be returned for unsuccessful calls, e.g. `Input tx not found in wallet (code -4)`.

This will likely be the most important RPC call for most game developers as it is how game moves are entered into the blockchain. 

## name_scan

This returns a list of names existing on the XAYA blockchain at the current block height. It takes optional arguments.

- **start**: The name to start scanning at
- **count**: The number of names to return
- **options**: A further set of optional parameters

The following command outputs the first 1000 names to a file.

	xaya-cli -rpcwallet=game.dat name_scan "" 1000 >outputfile.txt

The returned data list is too long to show here. 

The following command starts at "p/xaya" and outputs the first 1000 names to a file.

	xaya-cli -rpcwallet=game.dat name_scan "p/xaya" 1000 >outputfile.txt

That output begins with "p/xaya" in the list of 1000 names. e.g.:

	[
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
	  },
	  {
	    "name": "p/xbox",
	    "name_encoding": "utf8",
	    "value": "{\"p/xbox\":\"\"}",
	    "value_encoding": "ascii",
	    "txid": "114862be7bcbfe9a28c28b8f23c10a286346f59ee6ce609a13baf9450c5989b7",
	    "vout": 1,
	    "address": "CPeJUANTM5BsHvK8mU376ELw1SnXBMByoH",
	    "height": 3491,
	    "ismine": false
	  },
	  ...
	 ]

Using the `count` parameter, it is easier to page through names in smaller batches. This can signficantly reduce the amount of CPU and RAM resources required. To begin the next batch, set the name to the last name in the previous list, remembering to not include that twice in any result set that you build. 

While `name_scan` may be of little interest to game developers, it may be of greater interest to XAYA enthusiasts. 

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

Should the owner of "p/xaya" run the command, the only difference would be that `ismine` would be `true`.

## sendtoname

This method sends CHI by specifying a name instead of a regular CHI address. It has 2 mandatory parameters.

- **name**: The name to send the CHI to
- **amount**: The amount of CHI to send

Here's an example:

	xaya-cli -rpcwallet=game.dat sendtoname "p/xaya" "0.1"

The returned result is a `txid` for a successful transaction or an error message. 

`sendtoname` is a convenient way for people to specify where to send coins. Names are human readable whereas crypto addresses are long, complex and difficult to remember.

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

## name_history

This method returns a complete history of a name. It is generally not needed, and requires the blockchain to be specifically organised to use this method (you cannot run this method in a default XAYA wallet installation). As such, further description is omitted. 








