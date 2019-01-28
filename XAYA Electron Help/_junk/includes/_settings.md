## Settings

The Settings screen lets you change connection settings, back up your wallets, 
and switch to testnet.

![settings-screen-backups](img/settings-screen-backups.zoom672.png)

Advanced users can change connection settings.

![advanced-connection-settings](img/advanced-connection-settings.png)

Unless you fully understand the connection settings, you should leave them as 
is.

Advanced users and miners can make the changes they need, then click the SUBMIT 
SETTINGS button to save their new connection settings.

The DataDir Override allows you to change the directory where the XAYA 
blockchain, wallets, and other data is stored. The default directory is:

%APPDATA%\\Xaya\\

e.g. C:\\Users\\\<user>\\AppData\\Roaming\\Xaya

On Linux the datadir is here:

~/.xaya/

Or here:

~/home/\<user>/xaya/

On OS X (Mac) the datadir is here:

~/Library/Application Support/Xaya/

People with limited space on their system drive may wish to change the datadir 
folder location.

### Backing Up Your Wallets

You can back up your wallets by clicking the wallet backup buttons.

![wallet-backup-feature](img/wallet-backup-feature.png)

1. Click either of the WALLET BACKUP buttons.

2. Browse to a folder where you want to back up your wallet.  
   
![select-folder-and-enter-backup-file-name](img/select-folder-and-enter-backup-file-name.png)


3. Accept the default backup filename (either "game.dat" or "vault.dat"), or enter a name for your backup as illustrated in the figure above.

4. Click the Save button.

5. Click OK.  
   
![back-up-finished](img/back-up-finished.png)  
 

6. Browse to your backup in your file explorer and perform any additional backup 
plans you have, e.g. save it to a USB stick or external drive.

See [Back Up Wallets](#back-up-wallets) for additional information or 
[Manually Backing Up Wallets](#manually-backing-up-wallets) for 
how to do it manually.

### Delete Chain

If your wallet will not fully synchronise, you can click DELETE CHAIN to delete the current blockchain data and resynchronise. This should get your wallet properly sync'ing again. 

<aside class="warning">Keep in mind that it can take a significant amount of time to fully resync your wallet, and largely depends on your network speed. </aside> 

If you still experience problems, post in the <a href="https://forum.xaya.io/forum/16-support/" target="_blank">Support forums here</a>. 

### Testnet

<aside class="warning">Most people have no need for testnet. Proceed with caution. </aside>

To switch to testnet, check the Testnet checkbox then click the SUBMIT SETTINGS 
button and restart the XAYA Electron wallet.

Testnet is used for testing. Most people have no need for testnet.

However, if you wish, you can use testnet to try out different features and 
become familiar with them before you "go live" and try them on mainnet.


