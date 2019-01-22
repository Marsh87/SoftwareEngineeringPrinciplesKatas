using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using NSubstitute;
using NUnit.Framework;

namespace CharacterCopyKata_20190121.Tests
{
    [TestFixture]
    public class CopierTests
    {
        [Test]
        public void Copy_GivenSourceReturnsWithCharacte_ShouldCallDestinationWithCharacter()
        {
            //--------------- Set up test pack --------------------
            var source = Substitute.For<ISource>();
            var destination = Substitute.For<IDestination>();
            var character = 'A';
            source.ReadChar().Returns(character);
            var sut = new  Copier(source,destination);
            //---------------- Execute Test ----------------------
            sut.Copy();
            // --------------- Test Result ------------------------
            destination.Received().WriteChar(character);
        }

        [Test]
        public void Copy_GivenSourceReturnsNewLineDestination_ShouldNotCallDestinationWithNewLine()
        {
            //--------------- Set up test pack --------------------
            var source = Substitute.For<ISource>();
            var destination = Substitute.For<IDestination>();
            var character = '\n';
            source.ReadChar().Returns(character);
            var sut = new  Copier(source,destination);
            //---------------- Execute Test ----------------------
            sut.Copy();
            // --------------- Test Result ------------------------
            destination.DidNotReceiveWithAnyArgs().WriteChar(Arg.Any<char>());
        }
    }
}
