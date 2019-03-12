// TODO some of these usings are not used.
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using NSubstitute;
using NUnit.Framework;
using PeanutButter.RandomGenerators;

namespace CharacterCopyKata_20190121.Tests
{
    // TODO all of the asserts in this class are behaviour based, is there a way you can move them to be state based?
    [TestFixture]
    public class CopierTests
    {   
        [Test]
        public void Copy_GivenSourceIsNull_ShouldThrowANE()
        {
            //--------------- Set up test pack --------------------
          
            var destination = CreateDestination();
            //---------------- Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => CreateCopier(null,destination));
            // --------------- Test Result ------------------------
            var exceptionMessageParts = GetExceptionMessageParts(exception);
            var errorMessage = exceptionMessageParts[0];
            var paramName = exceptionMessageParts[1];
            Assert.That(errorMessage,Is.EqualTo("Value cannot be null."));
            Assert.That(paramName,Is.EqualTo( "Parameter name: source"));
        }

        [Test]
        public void Copy_GivenDestinationIsNull_ShouldThrowANE()
        {
            //--------------- Set up test pack --------------------
          
            var source = CreateSource();
            //---------------- Execute Test ----------------------
            var exception = Assert.Throws<ArgumentNullException>(() => CreateCopier(source,null));
            // --------------- Test Result ------------------------
            var exceptionMessageParts = GetExceptionMessageParts(exception);
            var errorMessage = exceptionMessageParts[0];
            var paramName = exceptionMessageParts[1];
            Assert.That(errorMessage,Is.EqualTo("Value cannot be null."));
            Assert.That(paramName,Is.EqualTo( "Parameter name: destination"));
        }

        [Test]
        public void Copy_GivenSourceReturnsOneCharacterBeforeNewLine_ShouldCallWriteCharWithCharacterBeforeNewline()
        {
            //--------------- Set up test pack --------------------
            var character = RandomValueGen.GetRandom<char>();
            var newLine = '\n';
            var source = CreateSourceThatReturnsOneCharacterAndNewLine(character,newLine);
            var destination = CreateDestination();
            var sut = CreateCopier(source, destination);
            //---------------- Execute Test ----------------------
            sut.Copy();
            // --------------- Test Result ------------------------
            ExpectedDestinationCallForOneCharacter(destination, character);
            ExpectedDestinationCallForNewLine(destination,newLine);
        }

        [Test]
        public void Copy_GivenSourceReturnsTwoCharactersBeforeNewLine_ShouldCallWriteCharWithAllCharactersBeforeNewline()
        {
            //--------------- Set up test pack --------------------
            var firstCharacter = RandomValueGen.GetRandom<char>();
            var secondCharacter = RandomValueGen.GetRandom<char>();
            var newLine = '\n';
            var source = CreateSourceThatReturnsTwoCharactersAndNewLine(firstCharacter,secondCharacter,newLine);
            var destination = CreateDestination();
            var sut = CreateCopier(source, destination);
            //---------------- Execute Test ----------------------
            sut.Copy();
            // --------------- Test Result ------------------------
            ExpectedDestinationCallForTwoCharacters(destination, firstCharacter, secondCharacter);
            ExpectedDestinationCallForNewLine(destination,newLine);
        }

        [Test]
        public void Copy_GivenSourceReturnsThreeCharactersBeforeNewLine_ShouldCallWriteCharWithAllCharactersBeforeNewline()
        {
            //--------------- Set up test pack --------------------
            var firstCharacter = RandomValueGen.GetRandom<char>();
            var secondCharacter = RandomValueGen.GetRandom<char>();
            var thirdCharacter = RandomValueGen.GetRandom<char>();
            var newLine = '\n';
            var source = CreateSourceThatReturnsThreeCharactersAndNewLine(firstCharacter,secondCharacter,thirdCharacter,newLine);
            var destination = CreateDestination();
            var sut = CreateCopier(source, destination);
            //---------------- Execute Test ----------------------
            sut.Copy();
            // --------------- Test Result ------------------------
            ExpectedDestinationCallForThreeCharacters(destination,firstCharacter,secondCharacter,thirdCharacter);
            ExpectedDestinationCallForNewLine(destination,newLine);
        }

        [Test]
        public void Copy_GivenSourceReturnsThreeRandomCharactersBeforeNewLine_ShouldCallWriteCharWithAllCharactersBeforeNewLine()
        {
            //--------------- Set up test pack --------------------
            var firstCharacter = RandomValueGen.GetRandom<char>();
            var secondCharacter = RandomValueGen.GetRandom<char>();
            var thirdCharacter = RandomValueGen.GetRandom<char>();
            var newLine = '\n';
            var source = CreateSourceThatReturnsThreeCharactersAndNewLine(firstCharacter, secondCharacter, thirdCharacter, newLine);
            var destination = CreateDestination();
            var sut = CreateCopier(source, destination);
            //---------------- Execute Test ----------------------
            sut.Copy();
            // --------------- Test Result ------------------------
            ExpectedDestinationCallForThreeCharacters(destination, firstCharacter, secondCharacter, thirdCharacter);
            ExpectedDestinationCallForNewLine(destination,newLine);
        }

        [Test]
        public void Copy_GivenSourceReturnsCharacterAfterNewLine_ShouldNotCallWriteCharacterAfterNewLine()
        {
            //--------------- Set up test pack --------------------
            var characterAfterNewLine = RandomValueGen.GetRandom<char>();
            var newLine = '\n';
            var source = CreateSourceThatReturnsCharacterAfterNewLine(newLine,characterAfterNewLine);
            var destination = CreateDestination();
            var sut = CreateCopier(source, destination);
            //---------------- Execute Test ----------------------
            sut.Copy();
            // --------------- Test Result ------------------------
            ExpectedDestinationCallsThatWereNotCalledForCharacterAfterNewLine(destination, characterAfterNewLine,newLine);
        }

        private static string[] GetExceptionMessageParts(ArgumentNullException ex)
        {
            var exceptionMessageParts = ex.Message.Split(new string[] {"\r", "\n"}, StringSplitOptions.RemoveEmptyEntries);
            return exceptionMessageParts;
        }

        private static void ExpectedDestinationCallsThatWereNotCalledForCharacterAfterNewLine(
            IDestination destination,
            char characterAfterNewLine,
            char newLine
            )
        {
            ExpectedDestinationCallForNewLine(destination,newLine);
            destination.DidNotReceive().WriteChar(characterAfterNewLine);
        }

        private static void ExpectedDestinationCallForNewLine(IDestination destination,char newLine)
        {
            destination.DidNotReceive().WriteChar(newLine);
        }

        private void ExpectedDestinationCallForOneCharacter(IDestination destination, char expectedCharacter)
        {
            destination.Received(1).WriteChar(expectedCharacter);
        }

        private static void ExpectedDestinationCallForTwoCharacters(
            IDestination destination,
            char expectedFirstCharacter,
            char expectedSecondCharacter
            )
        {
            destination.Received(1).WriteChar(expectedFirstCharacter);
            destination.Received(1).WriteChar(expectedSecondCharacter);
        }

        private static void ExpectedDestinationCallForThreeCharacters(
            IDestination destination,
            char expectedFirstCharacter,
            char expectedSecondCharacter,
            char expectedThirdCharacter
            )
        {
            destination.Received(1).WriteChar(expectedFirstCharacter);
            destination.Received(1).WriteChar(expectedSecondCharacter);
            destination.Received(1).WriteChar(expectedThirdCharacter);
        }

        private ISource CreateSourceThatReturnsOneCharacterAndNewLine(
            char firstCharacter,
            char newLine
            )
        {
            var source = CreateSource();
            source.ReadChar().Returns(firstCharacter,newLine);
            return source;
        }

        private static ISource CreateSourceThatReturnsTwoCharactersAndNewLine(
            char firstCharacter,
            char secondCharacter,
            char newLine
            )
        {
            var source = CreateSource();
            source.ReadChar().Returns(firstCharacter, secondCharacter, newLine);
            return source;
        }

        private ISource CreateSourceThatReturnsThreeCharactersAndNewLine(
            char firstCharacter, 
            char secondCharacter,
            char thirdCharacter,
            char newLine
            )
        {
            var source = CreateSource();
            source.ReadChar().Returns(firstCharacter,secondCharacter,thirdCharacter,newLine);
            return source;
        }

        private ISource CreateSourceThatReturnsCharacterAfterNewLine(char newLine ,char characterAfterNewLine)
        {
            var source = CreateSource();
            source.ReadChar().Returns(newLine, characterAfterNewLine);
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
            return Substitute.For<IDestination>();
        }
    }
}
