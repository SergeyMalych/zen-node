using System;
using System.IO;
using System.Reflection;
using Consensus;
using ConsoleApp5;
using Miner;
using Wallet.core;

namespace ZenMinerTest
{
	class Program
    {
        private readonly Random _Random = new Random();
        protected const string WALLET_DB = "temp_wallet";
        protected const string BLOCKCHAIN_DB = "temp_blockchain";

        static protected BlockChain.BlockChain _BlockChain;
        static protected Types.Block _GenesisBlock;
        static protected WalletManager _WalletManager;

        static protected MinerManager _MinerManager;

        static void Main(string[] args)
        {
            Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _GenesisBlock = Utils.GetGenesisBlock();

            var genesisHash = Merkle.blockHeaderHasher.Invoke(_GenesisBlock.header);
            _BlockChain = new BlockChain.BlockChain(BLOCKCHAIN_DB, genesisHash);

            _WalletManager = new WalletManager(_BlockChain, WALLET_DB);
			var keys = _WalletManager.GetUnusedKey();
            

			_MinerManager = new MinerManager(_BlockChain, keys.Address);
            _MinerManager.OnMined += _MinerManager_OnMined;
        }


        private static void _MinerManager_OnMined(Types.Block obj)
        {
			var a = obj;

        }
    }
    
}
