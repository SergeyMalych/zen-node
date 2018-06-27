﻿using Consensus;
using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallet.core.Data;

namespace ConsoleApp5
{
    public class Utils
    {

        public static Types.Block GetGenesisBlock()
        {
            var nonce = new byte[10];

            new Random().NextBytes(nonce);

            var header = new Types.BlockHeader(
                0,
                new byte[] { },
                0,
                new byte[] { },
                new byte[] { },
                new byte[] { },
                ListModule.OfSeq<byte[]>(new List<byte[]>()),
                DateTime.Now.ToUniversalTime().Ticks,
                0,
                nonce
            );

            var txs = new List<Types.Transaction>();

            txs.Add(GetTx());

            return new Types.Block(header, ListModule.OfSeq(txs));
        }

        public static Types.Transaction GetTx()
        {
            return new Types.Transaction(
                0,
                ListModule.OfSeq(new List<Types.Outpoint>()),
                ListModule.OfSeq(new List<byte[]>()),
                ListModule.OfSeq(new List<Types.Output>()),
                null);
        }

        public static Types.Transaction GetCoinbaseTx(uint blockNumber)
        {
            var reward = 1000u;

            var outputs = new List<Types.Output>
            {
                new Types.Output(Types.OutputLock.NewPKLock(Key.Create().Address.Bytes), new Types.Spend(Consensus.Tests.zhash, reward))
            };

            var witness = new List<byte[]>
            {
                BitConverter.GetBytes(blockNumber)
            };

            return new Types.Transaction(
                0,
                ListModule.Empty<Types.Outpoint>(),
                ListModule.OfSeq(witness),
                ListModule.OfSeq(outputs),
                FSharpOption<Types.ExtendedContract>.None
            );
        }

        public static Types.Output GetOutput(Address address, byte[] asset, ulong amount)
        {
            return new Types.Output(
                address.GetLock(),
                new Types.Spend(asset, amount)
            );
        }

        public static Types.Output GetContractOutput(byte[] contractHash, byte[] data, byte[] asset, ulong amount)
        {
            return new Types.Output(
                Types.OutputLock.NewContractLock(contractHash, data),
                new Types.Spend(asset, amount));
        }

        public static Types.Output GetContractSacrificeLock(byte[] contractHash, ulong zenAmount)
        {
            return new Types.Output(
                Types.OutputLock.NewContractSacrificeLock(
                    new Types.LockCore(0, ListModule.OfSeq(new byte[][] { contractHash }))
                ),
                new Types.Spend(Consensus.Tests.zhash, zenAmount)
            );
        }
    }
}