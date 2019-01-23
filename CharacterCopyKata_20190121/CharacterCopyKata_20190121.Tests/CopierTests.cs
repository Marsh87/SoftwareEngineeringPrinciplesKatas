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
        public void Copy_GivenSourceReturnsWithCharacter_ShouldCallDestinationWithCharacter()
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
        public void Copy_GivenSourceReturnsNewLine_ShouldNotCallDestinationWithNewLine()
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

        [Ignore("Need to find determine if this is the right test")]
        [Test]
        public void Copy_GivenSourceReturnsWithString_ShouldCallDestinationWithMultipleCharacters()
        {
            //--------------- Set up test pack --------------------
            var source = Substitute.For<ISource>();
            var destination = Substitute.For<IDestination>();
            var characters = "ABC";
            source.ReadChars(Arg.Any<int>()).Returns(characters);
            var sut = new  Copier(source,destination);
            //---------------- Execute Test ----------------------
            sut.Copy();
            // --------------- Test Result ------------------------
            destination.Received().WriteChars(Arg.Is(characters.ToCharArray()));
        }
    }
}
