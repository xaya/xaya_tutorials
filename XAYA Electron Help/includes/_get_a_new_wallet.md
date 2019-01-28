## Get a New Wallet

If for any reason you want or need a new wallet, it's very simple to do.

<aside class="warning">
This is simple for advanced users. If you aren't comfortable working outside of regular user folders, proceed with caution and always make backups of your wallets as described in <a href="#backing-up-your-wallets">Settings</a>.
</aside>

The XAYA Electron wallet has 2 wallets: a game wallet and a vault wallet. They 
are located here:

C:\\Users\\\<user>\\AppData\\Roaming\\Xaya\\wallets

On Linux with the QT wallet software, the wallet is located here:

~/.xaya/wallets

Or here:

~/home/\<user>/xaya/wallets

On OS X (Mac) with the QT wallet software, they are here:

~/Library/Application Support/Xaya/wallets/

On Windows, in that folder are 2 folders:

* game.dat
* vault.dat

Inside of each of those folders is a wallet.dat file.

To replace a wallet.dat file:

1. Shut down your wallet software if it's not already shut down.

2. Navigate to the folder of the wallet that you wish to replace.

3. Rename the wallet.dat file with a description that's meaningful to you. For 
example:  
   
Old wallet - empty.dat  
   
By renaming your wallet, you can go back and rename it back to wallet.dat so 
that you can use it again.  
   
At this stage, you will not have a file named "wallet.dat" in that folder.  
 

4. Restart your XAYA Electron wallet. It will automatically create a new, empty 
wallet.dat for you.

Take note that the process above does not delete the old wallet.dat file. If you 
are certain that the wallet has nothing of value, you may wish to delete it. If 
you are replacing a wallet because you have lost the password, you should still 
keep the wallet as it may be possible to retrieve the wallet's password.


