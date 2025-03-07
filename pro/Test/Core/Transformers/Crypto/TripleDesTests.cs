﻿using System;
using System.IO;
using NUnit.Framework;
using Peach.Core;
using Peach.Core.Analyzers;
using Peach.Core.Test;

namespace Peach.Pro.Test.Core.Transformers.Crypto
{
	[TestFixture]
	[Quick]
	[Peach]
    class TripleDesTests : DataModelCollector
    {
        [Test]
        public void KeySize128Test()
        {
            RunTest("ae1234567890aeaffeda214354647586fefdfaddefeeaf12", "aeaeaeaeaeaeaeae", new byte[] { 0x5d, 0xa5, 0x88, 0x82, 0x44, 0x24, 0x05, 0x67 });
        }

        [Test]
        public void KeySize192Test()
        {
            RunTest("ae1234567890aeaffeda214354647586", "aeaeaeaeaeaeaeae", new byte[] { 0x95, 0x4d, 0x29, 0x9a, 0xbc, 0x9d, 0x07, 0x5e });
        }

        [Test]
        public void WrongSizedKeyTest()
        {
            const string msg = "Error, unable to create instance of 'Transformer' named 'TripleDes'.\nExtended error: Exception during object creation: Specified key is not a valid size for this algorithm.";
            var ex = Assert.Throws<PeachException>(() => RunTest("aaaa", "aeaeaeaeaeaeaeae", new byte[]{}));
            Assert.AreEqual(msg, ex.Message);
        }

        [Test]
        public void WeakKeyTest()
        {
            const string msg = "Error, unable to create instance of 'Transformer' named 'TripleDes'.\nExtended error: Exception during object creation: Specified key is a known weak key for 'TripleDES' and cannot be used.";
            var ex = Assert.Throws<PeachException>(() => RunTest("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "aeaeaeaeaeaeaeae", new byte[] { }));
            Assert.AreEqual(msg, ex.Message);
        }

        [Test]
        public void WrongSizedIV()
        {
            const string msg = "Error, unable to create instance of 'Transformer' named 'TripleDes'.\nExtended error: Exception during object creation: Specified initialization vector (IV) does not match the block size for this algorithm.";
            var ex = Assert.Throws<PeachException>(() => RunTest("ae1234567890aeaffeda214354647586", "aaaa", new byte[] { }));
            Assert.AreEqual(msg, ex.Message);
        }

        public void RunTest(string key, string iv, byte[] expected)
        {
            // standard test

            string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
                "<Peach>" +
                "   <DataModel name=\"TheDataModel\">" +
                "        <Blob name=\"Data\" value=\"Hello\">" +
                "           <Transformer class=\"TripleDes\">" +
                "               <Param name=\"Key\" value=\"{0}\"/>" +
                "               <Param name=\"IV\" value=\"{1}\"/>" +
                "           </Transformer>" +
                "        </Blob>" +
                "   </DataModel>" +

                "   <StateModel name=\"TheState\" initialState=\"Initial\">" +
                "       <State name=\"Initial\">" +
                "           <Action type=\"output\">" +
                "               <DataModel ref=\"TheDataModel\"/>" +
                "           </Action>" +
                "       </State>" +
                "   </StateModel>" +

                "   <Test name=\"Default\">" +
                "       <StateModel ref=\"TheState\"/>" +
                "       <Publisher class=\"Null\"/>" +
                "   </Test>" +
                "</Peach>";
            xml = string.Format(xml, key, iv);
            PitParser parser = new PitParser();

            Peach.Core.Dom.Dom dom = parser.asParser(null, new MemoryStream(ASCIIEncoding.ASCII.GetBytes(xml)));

            RunConfiguration config = new RunConfiguration();
            config.singleIteration = true;

            Engine e = new Engine(this);
            e.startFuzzing(dom, config);

            // verify values
            // -- this is the pre-calculated result on the blob: "Hello"
            Assert.AreEqual(1, values.Count);
            Assert.AreEqual(expected, values[0].ToArray());
        }
    }
}
