// TODO some of these usings are not used.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using NSubstitute;
using NUnit.Framework;
using PeanutButter.RandomGenerators;

namespace CharacterCopyKata_20190121.Tests
{
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

            var writtenChars = new List<char>();
            var sut = CreateCopier(c => writtenChars.Add(c), character, newLine);
            //---------------- Execute Test ----------------------
            sut.Copy();
            // --------------- Test Result ------------------------
            CollectionAssert.AreEqual(writtenChars, new []{ character });
        }

        [Test]
        public void Copy_GivenSourceReturnsTwoCharactersBeforeNewLine_ShouldCallWriteCharWithAllCharactersBeforeNewline()
        {
            //--------------- Set up test pack --------------------
            var firstCharacter = RandomValueGen.GetRandom<char>();
            var secondCharacter = RandomValueGen.GetRandom<char>();
            var newLine = '\n';

            var writtenChars = new List<char>();
            var sut = CreateCopier(c => writtenChars.Add(c), firstCharacter, secondCharacter, newLine);
            //---------------- Execute Test ----------------------
            sut.Copy();
            // --------------- Test Result ------------------------
            CollectionAssert.AreEqual(writtenChars, new[] { firstCharacter,secondCharacter });
        }

        [Test]
        public void Copy_GivenSourceReturnsThreeCharactersBeforeNewLine_ShouldCallWriteCharWithAllCharactersBeforeNewline()
        {
            //--------------- Set up test pack --------------------
            var firstCharacter = RandomValueGen.GetRandom<char>();
            var secondCharacter = RandomValueGen.GetRandom<char>();
            var thirdCharacter = RandomValueGen.GetRandom<char>();
            var newLine = '\n';

            var writtenChars = new List<char>();
            var sut = CreateCopier(c => writtenChars.Add(c), firstCharacter, secondCharacter, thirdCharacter ,newLine);
            //---------------- Execute Test ----------------------
            sut.Copy();
            // --------------- Test Result ------------------------
            CollectionAssert.AreEqual(writtenChars, new[] { firstCharacter, secondCharacter, thirdCharacter });
        }

        [Test]
        public void Copy_GivenSourceReturnsCharacterAfterNewLine_ShouldNotCallWriteCharacterAfterNewLine()
        {
            //--------------- Set up test pack --------------------
            var characterAfterNewLine = RandomValueGen.GetRandom<char>();
            var newLine = '\n';

            var writtenChars = new List<char>();
            var sut = CreateCopier(c => writtenChars.Add(c), newLine, characterAfterNewLine);
            //---------------- Execute Test ----------------------
            sut.Copy();
            // --------------- Test Result ------------------------
            CollectionAssert.IsEmpty(writtenChars);
        }

        private static string[] GetExceptionMessageParts(ArgumentNullException ex)
        {
            var exceptionMessageParts = ex.Message.Split(new string[] {"\r", "\n"}, StringSplitOptions.RemoveEmptyEntries);
            return exceptionMessageParts;
        }

        private static ISource CreateSource()
        {
            return Substitute.For<ISource>();
        }

        private static Copier CreateCopier(ISource source, IDestination destination)
        {
            return new Copier(source, destination);
        }

        private static Copier CreateCopier(Action<char> charSnapshot, params char[] sourceChars)
        {
            var source = Substitute.For<ISource>();
            source.ReadChar().Returns(sourceChars.First(), sourceChars.Skip(1).ToArray());

            var destination = Substitute.For<IDestination>();
            destination.WriteChar(Arg.Do(charSnapshot));

            return CreateCopier(source, destination);
        }

        private static IDestination CreateDestination()
        {
            return Substitute.For<IDestination>();
        }
    }
}
