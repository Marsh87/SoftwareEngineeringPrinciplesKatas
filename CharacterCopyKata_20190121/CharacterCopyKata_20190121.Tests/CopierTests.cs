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
            // TODO why is the setup of the substitute being done outside of the factory method that creates it? 
            //      This makes tests brittle as all of them now have knowledge of function signature, 
            //      moving that setup into the factory function consolidated that knowledge in one place.
            var source = CreateSourceThatReturnsThreeCharacters(firstCharacter, secondCharacter, newLine);
            var destination = CreateDestination();
            var sut = CreateCopier(source, destination);
            //---------------- Execute Test ----------------------
            sut.Copy();
            // --------------- Test Result ------------------------
            // TODO these asserts suffer from the same issue as mentioned above, all tests now have knowledge of the
            //       WriteChar function signature, this makes them more brittle. Can you think of a way around this?
            ExpectedDestinationCallBeforeNewLine(destination, firstCharacter, secondCharacter);
            ExpectedDestinationCallForNewLine(destination, newLine);
        }

        [Test]
        public void Copy_GivenSourceReturnsMultipleCharacters_ShouldNotCallWriteCharWithCharacterThatWasReturnedAfterNewLineWasReturned()
        {
            //--------------- Set up test pack --------------------
            var firstCharacter = 'A';
            var secondCharacter = 'B';
            var newLine = '\n';
            var characterAfterNewLine = 'C';
            var source = SourceReturnsFourCharacters(firstCharacter, secondCharacter, newLine, characterAfterNewLine);
            var destination = CreateDestination();
            var sut = CreateCopier(source, destination);
            //---------------- Execute Test ----------------------
            sut.Copy();
            // --------------- Test Result ------------------------
            ExpectedDestinationCallBeforeNewLine(destination, firstCharacter, secondCharacter);
            DestinationCallsThatWereNotCalled(destination, newLine, characterAfterNewLine);
        }

        private static void DestinationCallsThatWereNotCalled(IDestination destination, char newLine,
            char characterAfterNewLine)
        {
            ExpectedDestinationCallForNewLine(destination, newLine);
            destination.DidNotReceive().WriteChar(characterAfterNewLine);
        }

        private static void ExpectedDestinationCallForNewLine(IDestination destination, char newLine)
        {
            destination.DidNotReceive().WriteChar(newLine);
        }

        private static void ExpectedDestinationCallBeforeNewLine(IDestination destination, char firstCharacter,
            char secondCharacter)
        {
            destination.Received(1).WriteChar(firstCharacter);
            destination.Received(1).WriteChar(secondCharacter);
        }

        private static ISource SourceReturnsFourCharacters(char firstCharacter, char secondCharacter, char newLine,
            char characterAfterNewLine)
        {
            var source = CreateSource();
            source.ReadChar().Returns(firstCharacter, secondCharacter, newLine, characterAfterNewLine);
            return source;
        }

        private static ISource CreateSourceThatReturnsThreeCharacters(char firstCharacter, char secondCharacter, char newLine)
        {
            var source = CreateSource();
            source.ReadChar().Returns(firstCharacter, secondCharacter, newLine);
            return source;
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
