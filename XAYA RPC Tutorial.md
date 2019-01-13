## Interacting with the XAYA Wallet Through RPC

Sending and getting information to and from a XAYA wallet is relatively straight forward, but can involve a lot of coding. However, why reinvent the wheel? The following tutorial uses an extended fork of George Kimionis&#39; BitcoinLib library to include XAYA. You can download the code at the XAYA github repository.

The sample application is a Windows Forms App. The code is well commented, so you can jump directly into the code. The solution has 2 projects:

- BitcoinLib: This is the forked library updated for XAYA.
- XAYABitcoinLib: This is a Winforms project where you can see BitcoinLib used to communicate with a XAYA Electron wallet.

NOTE: The project is configured to use Newtonsoft.Json v12. If you clean the solution and remove that DLL, you may need to manually add it back in. A zip with the DLL is included. Make certain to use the netstandard2.0 DLL as that is the framework that BitcoinLib targets.

NOTE: You must have a fully sync&#39;d XAYA Electron wallet running to use this sample application.

Everything can be done through a single object, an ICoinService initialised as a XAYAService. First, we set a member variable for our XAYAService like so:

`private static ICoinService xayaCoinService;`

For the sake of ease, we initialise that in the Form1\_Load method. It takes several parameters:

- daemonURL: A URL to the daemon that includes the port and path to the wallet, e.g. http://localhost:8396/wallet/game.dat
- rpcUsername: The wallet requires a username and a password.
- rpcPassword: The wallet requires a username and a password.
- walletPassword: This is the password that encrypts the wallet. It is not used for game wallets, but is used for vault wallets. It&#39;s not used in the sample application.
- rpcRequestTimeoutInSeconds: This is a simple timeout. You can set this as you wish.

These can be set in the app.config file in &quot;&lt;appSettings&gt;&quot;.

	  <appSettings>
	
	  <!--  BitcoinLib settings start -->
	
	  <!-- Shared RPC settings start -->
	  <add key="RpcRequestTimeoutInSeconds" value="60" />
	  <!-- Shared RPC settings end -->
	
	  <!-- XAYA settings start -->
	  <add key="XAYA_DaemonUrl" value="http://localhost:8396/wallet/game.dat" />
	  <add key="XAYA_DaemonUrl_Testnet" value="http://localhost:18396" />
	  <add key="XAYA_WalletPassword" value="MyWalletPassword" />
	  <add key="XAYA_RpcUsername" value="__cookie__" />
	  <add key="XAYA_RpcPassword" value="b5abfa36d8b32edc45a94d31ad50062f832b77cccad35b94528e09833c36e502" />
	<!-- XAYA settings end -->


The RPC username and password come from the .cookie file in the %APPDATA%\Xaya folder, e.g.:

	C:\Users\USERNAME\AppData\Roaming\Xaya

The sample application uses the CookieReader class to get those values.

	CookieReader cookie = new CookieReader();
	Console.WriteLine("Username = " + cookie.Username);
	Console.WriteLine("Userpassword = " + cookie.Userpassword);

You can set the XAYAService parameters however you wish. The sample application shows more than 1 way to do it. The most important parameter is the rpcPassword as the XAYA Electron wallet resets its password on every restart. So, if you hard code a value, and restart the Electron wallet, the password will have changed. As such, something similar to the CookieReader is useful to get the correct password.

	daemonUrl = ConfigurationManager.AppSettings["XAYA_DaemonUrl"]; // + "/wallet/game.dat";
	rpcUsername = cookie.Username; // ConfigurationManager.AppSettings["XAYA_RpcUsername"];
	rpcPassword = cookie.Userpassword; // ConfigurationManager.AppSettings["XAYA_RpcPassword"];
	walletPassword = ConfigurationManager.AppSettings["XAYA_WalletPassword"];
	rpcRequestTimeoutInSeconds = 60;


Also note that you must include the path to the wallet when you set the path to the daemon. You can set the URL as &quot;http://localhost:8396/&quot;, but you will then need to include the wallet path, e.g. &quot;/wallet/game.dat&quot;. You do not need to include the wallet file name (wallet.dat) because it is always the same.

Once you have your parameters correct, you can instantiate your XAYAService as shown below:

	xayaCoinService = new XAYAService(daemonUrl, rpcUsername, rpcPassword, walletPassword, rpcRequestTimeoutInSeconds);

With that, we have our bright and shiny new XAYAService object ready to use. For a first test, we can set the title of the form to the current block height, a normal method found in other coin wallet toolkits as well.

	this.Text = xayaCoinService.GetBlockCount().ToString();

When you run the project, you&#39;ll then see the title bar text like this:

![](https://github.com/RenegadeMinds/XAYA-RPC-Tutorial/blob/master/XAYABitcoinLib%20title%20bar%20text.png)

The XAYAService object, xayaCoinService, contains many methods common to Bitcoin and other coins. It also contains several methods that are unique to XAYA. You can type &quot;xayaCoinService.&quot; to display the Visual Studio Intellisense:

![](https://github.com/RenegadeMinds/XAYA-RPC-Tutorial/blob/master/XAYAService%20intellisense.png)

Some of the methods you&#39;ll need include:

- ShowName
- GetNameList
- RegisterName
- NameUpdate

## ShowName = name\_show

ShowName runs the name\_show method available in the daemon. It&#39;s trivial to do.

	BitcoinLib.Responses.GetShowNameResponse r = xayaCoinService.ShowName(txtShowName.Text);

Accessing the data is equally easy:

	txtShowNameResult.Text += r.name + "\r\n"
	    + "\tismine = " + r.ismine + "\r\n"
	    + "\tvalue = " + r.value + "\r\n"
	    + "\ttxid = " + r.txid + "\r\n"
	    + "\theight = " + r.height + "\r\n"
	    + "\taddress = " + r.address + "\r\n";

You can try adding in some more data on your own there (intellisense is your friend). 

## GetNameList = name\_list

Similarly, getting a list of all the names in a wallet is simple. Just call xayaCoinService.GetNameList then iterate over the List. (Remember to always check ismine as names that are no longer in the wallet will still be listed.)

	
	private void btnGetWalletNames_Click(object sender, EventArgs e)
	{
	    List<BitcoinLib.Responses.GetNameListResponse> r = xayaCoinService.GetNameList();
	    StringBuilder sb = new StringBuilder();
	    foreach (var v in r)
	    {
	        sb.AppendLine("Name = " + v.name.ToString());
	        sb.AppendLine("\tismine = " + v.ismine.ToString());
	        sb.AppendLine("\taddress = " + v.address);
	        sb.AppendLine("\ttxid = " + v.txid);
	        sb.AppendLine("\tvalue = " + v.value);
	        sb.AppendLine("\theight = " + v.height);
	        // There are more values. Try adding one here:
	    }
	
	    txtGetWalletNamesResult.Text += sb.ToString();
	}

Again, try adding in another bit of data to display on your own. 

## RegisterName = name\_register

Creating a new name is done through RegisterName. Since we&#39;re sending data to the daemon, we should do some error checking. Luckily, the method has error checks and will return &quot;Failed.&quot; If the method fails for any reason.

Names must have a namespace prefix. The namespace for player accounts is &quot;p/&quot;. No value needs to be passed, however it must be valid JSON, so sending &quot;{}&quot; or &quot;[]&quot; is a minimum requirement. If you pass in an empty string, it&#39;s handled in the method easily enough. The parameter option can be a simple new/null object.

The return value for a name\_register operation (the underlying operation in the daemon) is a txid.

Here&#39;s how easy it is to register a new name and display the result:

	string r = xayaCoinService.RegisterName(txtRegisterName.Text, "{}", newobject());
	txtRegisterNameResult.Text = r.ToString();

The txid result will look like "[baa1a0e75b3ccda1c26336291b1f181cae3f98c8aae31ad7b0371f2725649c4d](https://explorer.xaya.io/tx/baa1a0e75b3ccda1c26336291b1f181cae3f98c8aae31ad7b0371f2725649c4d)". 

## NameUpdate = name\_update

For your game, this is going to be one of the methods that you use most. This lets you enter a move onto the blockchain where everyone else submits their moves. How you enter moves is entirely up to you with only 2 conditions:

- It must be valid JSON
- The data must be 2048 bytes or less.

That gives you an enormous amount of freedom to design how moves are submitted.

The following code snippet walks through some error checking. It would probably be better to instead put some of those error checks into the BitcoinLib code for XAYA, but for the sake of illustration, the BitcoinLib code omits error checks and leaves that for us to do here.

At the top of the code we check to see whether or not the name exists, and then whether or not we own it in our wallet. Beyond the next error checks, it only takes 1 line of code to update a name.

	private void btnUpdateName_Click(object sender, EventArgs e)
	{
	    // We should check that the name doesn't already exist.
	    GetShowNameResponse r = xayaCoinService.ShowName(txtUpdateNameName.Text);
	            
	    // We must make certain that the name exists.
	    // As per the method's description, numeric fields are -1 if the name does not exist. 
	    // This is a simple error check.
	    if (r.height < 0)
	    {
	        return;
	    }
	
	    // In order to update a name, it must belong to us and be in our wallet. 
	    // This makes the above check useless, but there are cases where you 
		// would want to only check if a name exists irrespective of whether or not 
		// the name is owned by you/the player/user.
	    if (r.ismine == false)
	    {
	        return;
	    }
	
	    // We should verify that both the name and value are valid. 
		// We have a simple Utils class to give us some reusable checks.
	    bool nameIsValid = Utils.IsValidName(txtUpdateNameName.Text);
	    bool valueIsValid = Utils.IsValidJson(txtUpdateNameValue.Text);
	
	    if (!nameIsValid || !valueIsValid)
	    {
	        // One of them is invalid, so we cancel the operation.
	        return;
	    }
	
	    // At this point, we know our input data is valid and can proceed with the call.
	    string result = xayaCoinService.NameUpdate(txtUpdateNameName.Text, txtUpdateNameValue.Text, new object());
	
	    // The return value is a txid that we display in the results text box. 
	    txtUpdateNameResult.Text += result + "\r\n";
	}


# Summary

The above tutorial showed how to create a XAYAService instance and then use it to perform different operations. It looked at some of the most important operations that are unique to XAYA and explained the requirements of the method&#39;s arguments.
