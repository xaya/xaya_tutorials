# Blockchain Basics

If you're new to blockchain technology, the following should help get you up to speed so that you can begin developing games on the XAYA platform.

If you're already familiar with blockchain, then you can probably skip this.

For many people, Bitcoin and other cryptocurrencies are merely speculative instruments. Here we will go far beyond that to examine the underlying technology and how we can use it. 

# Basics

A blockchain is a distributed ledger that is accessed through P2P. The original Bitcoin White Paper is titled "Bitcoin: A Peer-to-Peer Electronic Cash System". If you've not read it, you may wish to at least skim it, or at least read the Abstract. 

This distributed ledger is composed of individual "blocks" that are cryptographically linked to each other in a linear "chain". Each block contains transaction data. Effectively, it is a distributed database where everyone keeps their own copy of that database. As new blocks are created, they are P2P broadcast and each participant adds that block to their copy of the blockchain. 

While Bitcoin is mainly used for financial transactions, blockchain technology is more versatile. Blocks can contain other data. Namecoin is the first fork of Bitcoin. From the Namecoin website:

> _Bitcoin frees money – Namecoin frees DNS, identities, and other technologies._

> **Namecoin** was the first fork of [Bitcoin](https://bitcoin.org/) and still is one of the most innovative “altcoins”. It was first to implement [merged mining](https://bitcoin.stackexchange.com/questions/273/how-does-merged-mining-work) and a [decentralized DNS](https://bit.namecoin.org/). **Namecoin** was also the first solution to [Zooko’s Triangle](https://en.wikipedia.org/wiki/Zooko%27s_triangle), the long-standing problem of producing a naming system that is simultaneously secure, decentralized, and human-meaningful.

This extensibility of blockchain technology is opening up countless new opportunities for people. 

With respect to the XAYA blockchain, it derives in part from Bitcoin, Namecoin, and Huntercoin. It adds in new functionality and scalability in order to enable coding games up to 100% on the XAYA blockchain. 

## Decentralised

One of the main benefits of blockchain is that it decentralised. While there are centralised blockchains, we won't consider them here as either they are misusing blockchain where they should be using a centralised database, or there is a special use-case. 

Decentralisation allows anyone to participate. There are no gatekeepers to exclude anyone. For the case of Bitcoin, it is "the internet of money" and enables true financial freedom in the sense that no authority can prevent 2 willing participants from transacting with each other. The value here is obvious when considering the various regulations that one must comply with in order to send fiat money, e.g. amounts are limited, banks require various levels of KYC/AML (Know Your Customer/Anti-Money Laundering) for their customers, multiple levels of banks may be involved in clearing transactions, lack of privacy, etc. With a cryptocurrency running on a blockchain, people are free to transact as they please without restrictions. 

## No Need for Trusted 3rd Parties

As blockchain is decentralised, people can transact with each other directly with no need for any 3rd party involvement. 

Banks and credit card companies are good examples of trusted 3rd parties. When you make a purchase at a store with your credit card, you are trusting the credit card company to complete that financial transaction on your behalf. In turn, the store is trusting the credit card company to send them the money from that transaction. 

However, at the heart of most transactions there are only 2 parties and 3rd parties, such as banks or other payment processors, act only as middle-men, for which they receive a cut of the money involved in the transaction. Eliminating these middle-men results in greater efficiency for the transaction participants because they don't need to pay the middle-man. Further, eliminating the middle-man precludes the possibility of them refusing to process the transaction. 

Eliminating trusted 3rd parties creates a faster, cheaper, and more reliable way to transact.

## Consensus Mechanism

"[Byzantine Generals](https://en.wikipedia.org/wiki/Byzantine_fault_tolerance#Byzantine_Generals'_Problem)" is a famous logic problem that was once thought unsolvable. The basic problem is about achieving a consensus. This problem is solved by Bitcoin's consensus mechanism, proof-of-work (PoW). 

Consensus allows the network to agree upon a single state. 

## Mining and Block Creation

Miners mine cryptocurrency blocks. These blocks include transactions from the network's participants. Each block of transactions is then added to the blockchain. 

But, all network participants must achieve a consensus. Miners use PoW to achieve this consensus, and for their efforts, they are rewarded with a block reward. This reward is a set amount of coins, e.g. the first reward for Bitcoin was 50 BTC per block. 

# Coding Blockchain Technology

Imagine being responsible for a code base worth millions or billions of dollars to people. You would want to be careful. Some important considerations when writing core blockchain technology are:

- Security
- Resources
- Performance
- Determinism

## Security

It's imperative that code be absolutely correct. This is one of the reasons why blockchain development is so slow. It undergoes rigorous testing and review. There can be no mistakes. 

As the code is open source, anyone can find bugs or vulnerabilities. Should a bad actor find a vulnerability, they could exploit it to steal from people. 

## Resources

Code must be efficient and must handle resources efficiently. It is imperative that it stays up to date with the network and not fall behind. Both local and remote requests need to be handled in a timely manner. 

## Performance

Blockchains must have high performance. There are many different operations and different kinds. Transactions must be processed quickly, but they must also be processed 1 at a time in order. That is, they cannot be processed in parallel because that would introduce the opportunity for a double spend. 

## Determinism

There can be no variation in results for the same inputs. All operations must be deterministic, such as how hash operations are deterministic and their outputs are always the same for the same inputs. All machines running any given code must always produce the same results. 

# Time on the Blockchain

Time on the blockchain operates slightly differently from how we normally perceive time. Time is measured in "blocks" instead of seconds, minutes, etc. Difficulty adjustments are used to make blocks easier or more difficult to solve, which simply means that they're faster or slower to solve. Difficulty retargeting algorithms are used to determine difficulty levels that will be solved on average in a predetermined number of seconds or minutes. In this way, each block is (on average) solved in 10 minutes for Bitcoin, or 30 seconds for XAYA. 















