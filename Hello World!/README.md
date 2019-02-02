#  Hello World!

This code sample is minimalistic. It's goal is to demonstrate how to wire up an application with the XAYA platform quickly and easily. 

HelloXaya is a typical "Hello World!" example. It wires up the XAYA platform in a Windows forms project. 

HelloXaya lets you say "Hello World!" on the XAYA blockchain. That's all it does.

# Important Considerations

The XAYAWrapper DLL (libxayawrap.dll) is 64-bit. Consequently, your project **MUST** exclude 32-bit. See this HelloXaya setting:

![Uncheck Prefer 32-bit](img/Prefer-32-bit-unchecked.png)

Otherwise, you must set your project to be 64-bit (x64). 

![x64](img/x64.png)

# Threading

It's crucial that you create threads for XAYAWrapper. 

Portions of XAYAWrapper are blocking operations and **MUST** be run in separate threads.

## Threading in HelloXaya

In HelloXaya, we've used BackgroundWorkers. There are more robust threading patterns available, but BackgroundWorkers are simple to understand with little complexity. 

You can implement better threading structures on your own. 

# Implementing XAYA Simplified

To implement XAYA, all you need to do is to is:

- Instantiate XAYAWraper (1 line of code)
- Connect to XAYAWrapper (1 line of code)
- Implement game logic in:
	+ 3 callbacks
		- initialCallbackResult (20 lines of trivial code)
		- forwardCallbackResult
		- backwardCallbackResult
	+ Ancilliary game logic
		- JSON classes
		- Helper methods
- Send moves to the XAYA blockchain
	+ This requires RPCs (can be implemented in many ways)

# Sending Moves

You don't need to be running an instance of a game to send moves. 

Moves can be sent arbitrarily in many ways. Here are some ways:

- From xaya-cli
- From the XAYA QT console
- Sending to xayad
- Etc.

When procesing moves, you **must** guard against invalid moves with robust error checking. 

# Instantiate XAYAWrapper

Instantiating XAYAWrapper **must** be done in a thread. 




















































































































