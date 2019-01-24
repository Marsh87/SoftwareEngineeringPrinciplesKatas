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
        public void Copy_GivenSourceReturnsWithCharacterDestinationShouldBeCalledWithCharacter()
        {
            //--------------- Set up test pack --------------------
            var source = CreateSource(out var character);
            var destination = CreateDestination();
            var sut = CreateCopier(source, destination);
            //---------------- Execute Test ----------------------
            sut.Copy();
            // --------------- Test Result ------------------------
            destination.Received().WriteChar(character);
        }

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

        private static ISource CreateSource(out char character)
        {
            var source = Substitute.For<ISource>();
            character = 'A';
            source.ReadChar().Returns(character);
            return source;
        }
    }
}
