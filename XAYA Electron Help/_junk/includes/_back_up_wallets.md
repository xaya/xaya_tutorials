## Back Up Wallets

It is critically important that you back up your wallets. Failure to do so could 
result in the loss of your CHI and blockchain assets.

There are 2 ways to back up your wallets.

* [Use the back up feature in the XAYA Electron wallet to back them up to a safe 
location](#backing-up-your-wallets) 
* [Manually back up your wallets by copying them to a safe 
location](#manually-backing-up-wallets) 

### "Safe Location"

Your back ups should be stored in a "safe location". This will have different 
meanings for different people. One person may want a physical printout and 
multiple USB sticks stored in a bank vault, while another considers a file 
backup on the same drive a safe location. This has entirely to do with your risk 
tolerance and what you consider safe. The following backup methods are to help 
you decide on what is right for you.

### Wallet Backup Feature

The easiest way to back up your wallets is to use the backup feature in 
[Settings](#settings).

![wallet-backup-feature](img/wallet-backup-feature.png)

[Click here for help with the built-in backup 
feature](#backing-up-your-wallets).

<aside class="warning">Unless you are comfortable with manual backups, you should only use the built-in backup feature.</aside>

### Manually Backing Up Wallets

The following explains in detail how to back up your wallets manually.

<aside class="warning">NOTE: This is for advanced users. If you have any difficulty whatsoever with 
anything below, you should use the backup features in the Electron or QT wallet 
software.</aside>

### File-based Backups

XAYA has 3 distinct wallets depending on which wallet software you are running. 
One wallet is used by the XAYA QT wallet, while the XAYA Electron wallet has 2 
wallets, the game and vault wallets. You only need to copy each wallet.dat file 
to a safe location to back them up.

[Click here to skip to the wallet 
locations](#xaya-qt-wallet-folder).

### XAYA QT Wallet Folder

The XAYA QT wallet can be found in the datadir wallets folder on Windows here:

%APPDATA%\\Xaya\\wallets\\

e.g. C:\\Users\\\<user>\\AppData\\Roaming\\Xaya\\wallets\\

On Linux the wallet folder is here:

~/.xaya/wallets/

Or here:

~/home/\<user>/xaya/wallets/

On OS X (Mac) the wallet folder is here:

~/Library/Application Support/Xaya/wallets/

### XAYA Electron Wallets Folders

The XAYA Electron wallets are in the game.dat and vault.dat subfolders. On 
Windows, those are here:

%APPDATA%\\Xaya\\wallets\\game.dat\\

%APPDATA%\\Xaya\\wallets\\vault.dat\\

e.g.

C:\\Users\\\<user>\\AppData\\Roaming\\Xaya\\wallets\\game.dat\\

C:\\Users\\\<user>\\AppData\\Roaming\\Xaya\\wallets\\vault.dat\\

The XAYA Electron wallet is not yet available for Linux or OS X (Mac).

### Backing Up Wallet Files

Backing up wallet files is only a matter of copying them to a safe location. The 
Electron wallet has 2 wallet files while the QT wallet has only 1 wallet file.

The 3 possible wallet locations to back up are:

%APPDATA%\\Xaya\\wallets\\wallet.dat

%APPDATA%\\Xaya\\wallets\\game.dat\\wallet.dat

%APPDATA%\\Xaya\\wallets\\vault.dat\\wallet.dat

Or on Linux for the QT wallet:

~/.xaya/wallets/wallet.dat

And on OS X for the QT wallet:

~/Library/Application Support/Xaya/wallets/wallet.dat

If you haven't run the XAYA QT wallet, you will not have that file. This is 
normal.

To back up a wallet file:

1. Shut down the wallet software.

2. Copy the wallet.dat file to 1 or more safe backup locations.

3. Restart your wallet software.


