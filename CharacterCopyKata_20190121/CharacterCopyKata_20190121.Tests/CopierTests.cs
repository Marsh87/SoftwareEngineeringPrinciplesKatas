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
        public void Copy_GivenSourceReturnsMultipleCharacters_ShouldCallWriteCharWithAllCharactersUntilNewLineIsReturned()
        {
            //--------------- Set up test pack --------------------
            var firstCharacter = 'A';
            var secondCharacter = 'B';
            var newLine = '\n';
            var source = CreateSource();
            source.ReadChar().Returns(firstCharacter, secondCharacter, newLine);
            var destination = CreateDestination();
            var sut = CreateCopier(source, destination);
            //---------------- Execute Test ----------------------
            sut.Copy();
            // --------------- Test Result ------------------------
            destination.Received(1).WriteChar(firstCharacter);
            destination.Received(1).WriteChar(secondCharacter);
            destination.DidNotReceive().WriteChar(newLine);
        }

        [Test]
        public void Copy_GivenSourceReturnsMultipleCharacters_ShouldNotCallWriteCharWithCharacterThatWasReturnedAfterNewLineWasReturned()
        {
            //--------------- Set up test pack --------------------
            var firstCharacter = 'A';
            var secondCharacter = 'B';
            var newLine = '\n';
            var characterAfterNewLine = 'C';
            var source = CreateSource();
            source.ReadChar().Returns(firstCharacter, secondCharacter, newLine, characterAfterNewLine);
            var destination = CreateDestination();
            var sut = CreateCopier(source, destination);
            //---------------- Execute Test ----------------------
            sut.Copy();
            // --------------- Test Result ------------------------
            destination.Received(1).WriteChar(firstCharacter);
            destination.Received(1).WriteChar(secondCharacter);
            destination.DidNotReceive().WriteChar(newLine);
            destination.DidNotReceive().WriteChar(characterAfterNewLine);
        }

        private static ISource CreateSource()
        {
           return Substitute.For<ISource>();
        }

        private static Copier CreateCopier(ISource source, IDestination destination)
        {
            return new Copier(source, destination);
        }

        private static IDestination CreateDestination()
        {
           return  Substitute.For<IDestination>();
        }
    }
}
