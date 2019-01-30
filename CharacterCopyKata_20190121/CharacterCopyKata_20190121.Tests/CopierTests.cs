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
        public void Copy_GivenSourceReturnsWithSingleCharacterDestination_ShouldBeCalledWithCharacter()
        {
            //--------------- Set up test pack --------------------
            var character = 'A';
            var source = CreateSource(character);
            var destination = CreateDestination();
            var sut = CreateCopier(source, destination);
            //---------------- Execute Test ----------------------
            sut.Copy();
            // --------------- Test Result ------------------------
            destination.Received().WriteChar(character);
        }

        [Test]
        public void Copy_GivenSourceReturnsMultipleCharactersDestination_ShouldBeCalledWithCharacterOneAtATimeUntilNewline()
        {
            //--------------- Set up test pack --------------------
            var firstCharacter = 'A';
            var secondCharacter = 'B';
            var newLine = '\n';
            var source = Substitute.For<ISource>();
            source.ReadChar().Returns(firstCharacter, secondCharacter, newLine);
            var destination = CreateDestination();
            var sut = CreateCopier(source, destination);
            //---------------- Execute Test ----------------------
            sut.Copy();
            // --------------- Test Result ------------------------
            destination.WriteChar(firstCharacter);
            destination.Received().WriteChar(secondCharacter);
            destination.DidNotReceive().WriteChar(newLine);
        }

        /*[Test]
        public void Copy_GivenSourceReturnsMultipleCharactersWithNewLineDestination_ShouldOnlyBeCalledWithCharactersBeforeNewLine()
        {
            //--------------- Set up test pack --------------------
            var firstCharacter = 'A';
            var secondCharacter = '\n';
            var thirdCharacter = 'C';
            var source = Substitute.For<ISource>();
            source.ReadChar().Returns(firstCharacter, secondCharacter, thirdCharacter);
            var destination = CreateDestination();
            var sut = CreateCopier(source, destination);
            //---------------- Execute Test ----------------------
            sut.Copy();
            // --------------- Test Result ------------------------
            destination.Received().WriteChar(firstCharacter);
            destination.DidNotReceive().WriteChar(secondCharacter);
            destination.DidNotReceive().WriteChar(thirdCharacter);
        }*/

        private static Copier CreateCopier(ISource source, IDestination destination)
        {
            var sut = new Copier(source, destination);
            return sut;
        }

        private static IDestination CreateDestination()
        {
            var destination = Substitute.For<IDestination>();
            return destination;
        }

        private static ISource CreateSource(char character)
        {
            var source = Substitute.For<ISource>();
            source.ReadChar().Returns(character);
            return source;
        }
    }
}
