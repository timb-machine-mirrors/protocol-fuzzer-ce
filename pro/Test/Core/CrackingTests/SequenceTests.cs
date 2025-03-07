﻿using System.Collections.Generic;
using System.IO;
using System.Xml;
using NUnit.Framework;
using Peach.Core;
using Peach.Core.Analyzers;
using Peach.Core.Cracker;
using Peach.Core.Dom;
using Peach.Core.IO;
using Peach.Core.Test;

namespace Peach.Pro.Test.Core.CrackingTests
{
	[TestFixture]
	[Category("Peach")]
	[Quick]
	class SequenceTests
	{
	[Test]
	public void CrackBlock1()
	{
		const string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Peach>\n" +
							"	<DataModel name=\"TheDataModel\">" +
							"		<Sequence>" +
							"			<Sequence>" +
							"				<Sequence>" +
							"				</Sequence>" +
							"			</Sequence>" +
							"		</Sequence>" +
							"	</DataModel>" +
							"</Peach>";

		var parser = new PitParser();
		var dom = parser.asParser(null, new MemoryStream(Encoding.ASCII.GetBytes(xml)));

		var data = Bits.Fmt("{0}", new byte[] { 1, 2, 3, 4, 5 });

		var cracker = new DataCracker();
		cracker.CrackData(dom.dataModels[0], data);

		Assert.AreEqual(0, data.PositionBits);
	}

	[Test]
	public void CrackBlock2()
	{
		const string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Peach>\n" +
							"	<DataModel name=\"TheDataModel\">" +
							"		<Sequence>" +
							"			<Sequence>" +
							"				<Sequence>" +
							"					<String name=\"FooString\" length=\"12\" />" +
							"				</Sequence>" +
							"			</Sequence>" +
							"		</Sequence>" +
							"	</DataModel>" +
							"</Peach>";

		var parser = new PitParser();
		var dom = parser.asParser(null, new MemoryStream(Encoding.ASCII.GetBytes(xml)));

		var data = Bits.Fmt("{0}", "Hello World!");

		var cracker = new DataCracker();
		cracker.CrackData(dom.dataModels[0], data);

		Assert.AreEqual("Hello World!", (string)dom.dataModels[0].find("FooString").DefaultValue);
	}

	[Test]
	public void CrackWritePitTest()
	{
		const string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Peach>\n" +
							"	<DataModel name=\"TheDataModel\">" +
							"		<Sequence>" +
							"					<String name=\"FooString\" length=\"12\" />" +
							"		</Sequence>" +
							"	</DataModel>" +
							"</Peach>";

		var parser = new PitParser();
		var dom = parser.asParser(null, new MemoryStream(Encoding.ASCII.GetBytes(xml)));

		var data = Bits.Fmt("{0}", "Hello World!");

		var cracker = new DataCracker();
		cracker.CrackData(dom.dataModels[0], data);

		Assert.AreEqual("Hello World!", (string)dom.dataModels[0].find("FooString").DefaultValue);

		using(var swriter = new StringWriter())
		using (var xmlWriter = new XmlTextWriter(swriter))
		{
			dom.dataModels[0].WritePit(xmlWriter);

			xmlWriter.Flush();
			var xmlString = swriter.ToString();

			Assert.AreEqual(@"<DataModel name=""TheDataModel""><Sequence><String name=""FooString"" lengthType=""bytes"" length=""12"" value=""Hello World!"" /></Sequence></DataModel>", xmlString);
		}
	}

	[Test]
	public void CrackBlock3()
	{
		const string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Peach>\n" +
							"	<DataModel name=\"TheDataModel\">" +
							"		<Sequence name=\"b1\">" +
							"			<String name=\"str1\" length=\"5\" />" +
							"			<String name=\"str2\" length=\"5\" />" +
							"		</Sequence>" +
							"		<Sequence name=\"b2\">" +
							"			<String name=\"str3\" length=\"1\"/>" +
							"		</Sequence>" +
							"	</DataModel>" +
							"</Peach>";

		var parser = new PitParser();
		var dom = parser.asParser(null, new MemoryStream(Encoding.ASCII.GetBytes(xml)));

		var data = Bits.Fmt("{0}", "HelloWorld!.........................");

		var cracker = new DataCracker();
		cracker.CrackData(dom.dataModels[0], data);


		Assert.AreEqual("Hello", (string)dom.dataModels[0].find("str1").DefaultValue);
		Assert.AreEqual("World", (string)dom.dataModels[0].find("str2").DefaultValue);
		Assert.AreEqual("!", (string)dom.dataModels[0].find("str3").DefaultValue);
	}

	[Test]
	public void CrackBlock4()
	{
		const string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Peach>\n" +
							"	<DataModel name=\"TheDataModel\">" +
							"		<Sequence name=\"b1\" occurs=\"2\">" +
							"			<String name=\"str1\" length=\"5\" />" +
							"			<String name=\"str2\" length=\"5\" />" +
							"		</Sequence>" +
							"		<Sequence name=\"b2\">" +
							"			<String name=\"str3\" length=\"1\"/>" +
							"		</Sequence>" +
							"	</DataModel>" +
							"</Peach>";

		var parser = new PitParser();
		var dom = parser.asParser(null, new MemoryStream(Encoding.ASCII.GetBytes(xml)));

		var data = Bits.Fmt("{0}", "HelloWorldHelloWorld!......................................................");

		var cracker = new DataCracker();
		cracker.CrackData(dom.dataModels[0], data);


		Assert.AreEqual("Hello", (string)dom.dataModels[0].find("b1_0.str1").DefaultValue);
		Assert.AreEqual("World", (string)dom.dataModels[0].find("b1_0.str2").DefaultValue);
		Assert.AreEqual("Hello", (string)dom.dataModels[0].find("b1_1.str1").DefaultValue);
		Assert.AreEqual("World", (string)dom.dataModels[0].find("b1_1.str2").DefaultValue);
		Assert.AreEqual("!", (string)dom.dataModels[0].find("str3").DefaultValue);
	}

	[Test]
	public void CrackBlock5()
	{
		const string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Peach>\n" +
							"	<DataModel name=\"TheDataModel\">" +
							"		<Number size=\"8\">" +
							"			<Relation type=\"count\" of=\"b1\"/>" +
							"		</Number>" +
							"		<Sequence name=\"b1\" minOccurs=\"1\">" +
							"			<String name=\"str1\" length=\"5\" />" +
							"			<String name=\"str2\" length=\"5\" />" +
							"		</Sequence>" +
							"		<Sequence name=\"b2\">" +
							"			<String name=\"str3\" length=\"1\"/>" +
							"		</Sequence>" +
							"	</DataModel>" +
							"</Peach>";

		var parser = new PitParser();
		var dom = parser.asParser(null, new MemoryStream(Encoding.ASCII.GetBytes(xml)));

		var data = Bits.Fmt("{0}", "\x02HelloWorldHelloWorld!......................................................");

		var cracker = new DataCracker();
		cracker.CrackData(dom.dataModels[0], data);


		Assert.AreEqual("Hello", (string)dom.dataModels[0].find("b1_0.str1").DefaultValue);
		Assert.AreEqual("World", (string)dom.dataModels[0].find("b1_0.str2").DefaultValue);
		Assert.AreEqual("Hello", (string)dom.dataModels[0].find("b1_1.str1").DefaultValue);
		Assert.AreEqual("World", (string)dom.dataModels[0].find("b1_1.str2").DefaultValue);
		Assert.AreEqual("!", (string)dom.dataModels[0].find("str3").DefaultValue);
	}

	[Test]
	public void CrackBlock6()
	{
		const string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Peach>\n" +
							"	<DataModel name=\"TheDataModel\">" +
							"		<Number size=\"8\">" +
							"			<Relation type=\"count\" of=\"b1\"/>" +
							"		</Number>" +
							"		<String name=\"unsized\"/>" +
							"		<Sequence name=\"b1\" minOccurs=\"1\">" +
							"			<String name=\"str1\" length=\"5\" />" +
							"			<String name=\"str2\" length=\"5\" />" +
							"		</Sequence>" +
							"		<Sequence name=\"b2\">" +
							"			<String name=\"str3\" length=\"1\"/>" +
							"		</Sequence>" +
							"	</DataModel>" +
							"</Peach>";

		var parser = new PitParser();
		var dom = parser.asParser(null, new MemoryStream(Encoding.ASCII.GetBytes(xml)));

		var data = Bits.Fmt("{0}", "\x02.....HelloWorldHelloWorld!");

		var cracker = new DataCracker();
		cracker.CrackData(dom.dataModels[0], data);


		Assert.AreEqual(".....", (string)dom.dataModels[0].find("unsized").DefaultValue);
		Assert.AreEqual("Hello", (string)dom.dataModels[0].find("b1_0.str1").DefaultValue);
		Assert.AreEqual("World", (string)dom.dataModels[0].find("b1_0.str2").DefaultValue);
		Assert.AreEqual("Hello", (string)dom.dataModels[0].find("b1_1.str1").DefaultValue);
		Assert.AreEqual("World", (string)dom.dataModels[0].find("b1_1.str2").DefaultValue);
		Assert.AreEqual("!", (string)dom.dataModels[0].find("str3").DefaultValue);
	}

	[Test]
	public void CrackBlock7()
	{
		const string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Peach>\n" +
							"	<DataModel name=\"TheDataModel\">" +
							"		<Number size=\"8\">" +
							"			<Relation type=\"size\" of=\"b1\"/>" +
							"		</Number>" +
							"		<Sequence name=\"b1\"/>" +
							"	</DataModel>" +
							"</Peach>";

		var parser = new PitParser();
		var dom = parser.asParser(null, new MemoryStream(Encoding.ASCII.GetBytes(xml)));

		var data = Bits.Fmt("{0}", new byte[] { 2, 0, 0 });

		var cracker = new DataCracker();
		cracker.CrackData(dom.dataModels[0], data);

		// Ensure we actually advance the BitStream over sized blocks with no children
		Assert.AreEqual(3, data.Position);
		Assert.AreEqual(24, data.PositionBits);
	}

	[Test]
	public void CrackBlock8()
	{
		const string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Peach>\n" +
							"	<DataModel name=\"TheDataModel\">" +
							"			<Number name=\"n1\" size=\"8\">" +
							"				<Relation type=\"size\" of=\"b1\"/>" +
							"			</Number>" +
							"			<Sequence name=\"b1\">" +
							"				<Number name=\"n2\" size=\"8\">" +
							"					<Relation type=\"size\" of=\"b2\"/>" +
							"				</Number>" +
							"			<Sequence name=\"b2\">" +
							"				<Number name=\"n3\" size=\"8\"/>" +
							"			</Sequence>" +
							"		</Sequence>" +
							"	</DataModel>" +
							"</Peach>";

		var parser = new PitParser();
		var dom = parser.asParser(null, new MemoryStream(Encoding.ASCII.GetBytes(xml)));

		var data = Bits.Fmt("{0}", new byte[] { 8, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0 });

		var place = new Dictionary<string, string>();

		var cracker = new DataCracker();

		cracker.EnterHandleNodeEvent += delegate(DataElement de, long pos, BitStream bs)
		{
			place.Add(de.fullName, pos.ToString());
		};

		cracker.ExitHandleNodeEvent += delegate(DataElement de, long pos, BitStream bs)
		{
			place[de.fullName] += "," + pos.ToString();
		};

		cracker.CrackData(dom.dataModels[0], data);

		Assert.AreEqual(6, place.Count);
		Assert.AreEqual("0,72", place["TheDataModel"]);
		Assert.AreEqual("0,8", place["TheDataModel.n1"]);
		Assert.AreEqual("8,72", place["TheDataModel.b1"]);
		Assert.AreEqual("8,16", place["TheDataModel.b1.n2"]);
		Assert.AreEqual("16,48", place["TheDataModel.b1.b2"]);
		Assert.AreEqual("16,24", place["TheDataModel.b1.b2.n3"]);

	}

	[Test]
	public void CrackBlock9()
	{
		const string xml = @"
<Peach>
	<DataModel name='TheDataModel'>
		<Sequence name='b1'>
			<Number name='num' size='8'>
				<Relation type='size' of='b1'/>
			</Number>
			<Sequence name='b2'/>
		</Sequence>
		<String name='str' length='5'/>
	</DataModel>
</Peach>";

			var parser = new PitParser();
			var dom = parser.asParser(null, new MemoryStream(Encoding.ASCII.GetBytes(xml)));

			var data = Bits.Fmt("{0:L8}{1:L32}{2:ascii}", 5, 0, "HelloWorld");

			var place = new Dictionary<string, string>();

			var cracker = new DataCracker();

			cracker.EnterHandleNodeEvent += delegate(DataElement de, long pos, BitStream bs)
			{
				place.Add(de.fullName, pos.ToString());
			};

			cracker.ExitHandleNodeEvent += delegate(DataElement de, long pos, BitStream bs)
			{
				place[de.fullName] += "," + pos.ToString();
			};

			cracker.CrackData(dom.dataModels[0], data);

			Assert.AreEqual(5, place.Count);
			Assert.AreEqual("0,80", place["TheDataModel"]);
			Assert.AreEqual("0,40", place["TheDataModel.b1"]);
			Assert.AreEqual("0,8", place["TheDataModel.b1.num"]);
			Assert.AreEqual("8,8", place["TheDataModel.b1.b2"]);
			Assert.AreEqual("40,80", place["TheDataModel.str"]);

		}
	}
}
