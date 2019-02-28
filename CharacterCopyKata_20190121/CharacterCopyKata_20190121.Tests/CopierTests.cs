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
        // TODO is there a way you can get this re-usability without introducing class level state?
        //       class level state can introduce interactions between tests and just generally makes tests more brittle.
        const char FirstCharacter = 'A';
        const  char SecondCharacter = 'B';
        const  char ThirdCharacter = 'C';
        const  char NewLine = '\n';

        // TODO why not use random characters in this test as you have in the others
        [Test]
        public void Copy_GivenSourceReturnsOneCharacterBeforeNewLine_ShouldCallWriteCharWithCharacterBeforeNewline()
        {
            //--------------- Set up test pack --------------------
            var source = CreateSourceThatReturnsOneCharacterAndNewLine();
            var destination = CreateDestination();
            var sut = CreateCopier(source, destination);
            //---------------- Execute Test ----------------------
            sut.Copy();
            // --------------- Test Result ------------------------
            ExpectedDestinationCallForOneCharacter(destination);
            ExpectedDestinationCallForNewLine(destination);
        }

        // TODO why not use random characters in this test as you have in the others
        [Test]
        public void Copy_GivenSourceReturnsTwoCharactersBeforeNewLine_ShouldCallWriteCharWithAllCharactersBeforeNewline()
        {
            //--------------- Set up test pack --------------------
            var source = CreateSourceThatReturnsTwoCharactersAndNewLine();
            var destination = CreateDestination();
            var sut = CreateCopier(source, destination);
            //---------------- Execute Test ----------------------
            sut.Copy();
            // --------------- Test Result ------------------------
            ExpectedDestinationCallForTwoCharacters(destination);
            ExpectedDestinationCallForNewLine(destination);
        }

        [Test]
        public void Copy_GivenSourceReturnsThreeCharactersBeforeNewLine_ShouldCallWriteCharWithAllCharactersBeforeNewline()
        {
            //--------------- Set up test pack --------------------
            var source = CreateSourceThatReturnsThreeCharactersAndNewLine();
            var destination = CreateDestination();
            var sut = CreateCopier(source, destination);
            //---------------- Execute Test ----------------------
            sut.Copy();
            // --------------- Test Result ------------------------
            ExpectedDestinationCallForThreeCharacters(destination);
            ExpectedDestinationCallForNewLine(destination);
        }

        [Test]
        public void Copy_GivenSourceReturnsThreeRandomCharactersBeforeNewLine_ShouldCallWriteCharWithAllCharactersBeforeNewLine()
        {
            //--------------- Set up test pack --------------------
            var randomFirstCharacter = RandomValueGen.GetRandom<char>();
            var randomSecondCharacter = RandomValueGen.GetRandom<char>();
            var randomThirdCharacter = RandomValueGen.GetRandom<char>();
            var source = CreateSourceThatReturnsThreeRandomCharactersAndNewLine(
                randomFirstCharacter,
                randomSecondCharacter,
                randomThirdCharacter
                );
            var destination = CreateDestination();
            var sut = CreateCopier(source, destination);
            //---------------- Execute Test ----------------------
            sut.Copy();
            // --------------- Test Result ------------------------
            ExpectedDestinationCallForThreeRandomCharacters(destination, randomFirstCharacter, randomSecondCharacter,randomThirdCharacter);
            ExpectedDestinationCallForNewLine(destination);
        }

        [Test]
        public void Copy_GivenSourceReturnsCharacterAfterNewLine_ShouldNotCallWriteCharacterAfterNewLine()
        {
            //--------------- Set up test pack --------------------
            var characterAfterNewLine = RandomValueGen.GetRandom<char>();
            var source = CreateSourceThatReturnsCharacterAfterNewLine(characterAfterNewLine);
            var destination = CreateDestination();
            var sut = CreateCopier(source, destination);
            //---------------- Execute Test ----------------------
            sut.Copy();
            // --------------- Test Result ------------------------
            ExpectedDestinationCallsThatWereNotCalledForCharacterAfterNewLine(destination,characterAfterNewLine);
        }

        private static void ExpectedDestinationCallsThatWereNotCalledForCharacterAfterNewLine(
            IDestination destination,
            char characterAfterNewLine
            )
        {
            ExpectedDestinationCallForNewLine(destination);
            destination.DidNotReceive().WriteChar(characterAfterNewLine);
        }

        private static void ExpectedDestinationCallForNewLine(IDestination destination)
        {
            destination.DidNotReceive().WriteChar(NewLine);
        }

        private void ExpectedDestinationCallForOneCharacter(IDestination destination)
        {
            destination.Received(1).WriteChar(FirstCharacter);
        }

        private static void ExpectedDestinationCallForTwoCharacters(
            IDestination destination
            )
        {
            destination.Received(1).WriteChar(FirstCharacter);
            destination.Received(1).WriteChar(SecondCharacter);
        }

        private static void ExpectedDestinationCallForThreeCharacters(
            IDestination destination
            )
        {
            destination.Received(1).WriteChar(FirstCharacter);
            destination.Received(1).WriteChar(SecondCharacter);
            destination.Received(1).WriteChar(ThirdCharacter);
        }

        private void ExpectedDestinationCallForThreeRandomCharacters(
            IDestination destination,
            char randomFirstCharacter,
            char randomSecondCharacter,
            char randomThirdCharacter
        )
        {
            destination.Received(1).WriteChar(randomFirstCharacter);
            destination.Received(1).WriteChar(randomSecondCharacter);
            destination.Received(1).WriteChar(randomThirdCharacter);
        }

        private ISource CreateSourceThatReturnsOneCharacterAndNewLine()
        {
            var source = CreateSource();
            source.ReadChar().Returns(FirstCharacter, NewLine);
            return source;
        }

        private static ISource CreateSourceThatReturnsTwoCharactersAndNewLine()
        {
            var source = CreateSource();
            source.ReadChar().Returns(FirstCharacter, SecondCharacter,NewLine);
            return source;
        }

        private ISource CreateSourceThatReturnsThreeCharactersAndNewLine()
        {
            var source = CreateSource();
            source.ReadChar().Returns(FirstCharacter, SecondCharacter, ThirdCharacter,NewLine);
            return source;
        }

        private ISource CreateSourceThatReturnsThreeRandomCharactersAndNewLine(
            char randomFirstCharacter,
            char randomSecondCharacter,
            char randomThirdCharacter
        )
        {
            var source = CreateSource();
            source.ReadChar().Returns(randomFirstCharacter, randomSecondCharacter, randomThirdCharacter, NewLine);
            return source;
        }

        private ISource CreateSourceThatReturnsCharacterAfterNewLine(char characterAfterNewLine)
        {
            var source = CreateSource();
            source.ReadChar().Returns(NewLine, characterAfterNewLine);
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
