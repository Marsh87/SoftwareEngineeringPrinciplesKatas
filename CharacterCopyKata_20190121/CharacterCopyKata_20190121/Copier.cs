using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterCopyKata_20190121
{
    public class Copier
    {
        private readonly ISource _source;
        private readonly IDestination _destination;

        const char NewLine = '\n';
        public Copier(ISource source, IDestination destination)
        {
            // TODO the null exception code is not tested. I don't mind if it is or isn't there, but if you choose to have it it should be tested.
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _destination = destination ?? throw new ArgumentNullException(nameof(destination));
        }

        public void Copy()
        {
            // TODO the below code is currently doing two things, reading and writing, can you think of a way to pull those responsibilities apart?
            /*var characterFromSource = GetCharacterFromSource();
            while (CharacterIsNotNewLine(characterFromSource))
            {
                _destination.WriteChar(characterFromSource);
                characterFromSource = GetCharacterFromSource();
            }*/
            // TODO the above implementation is too much for the tests that you have. I could replace it with the below code and all you tests would pass. Please improve the tests.
            _destination.WriteChar('A');
            _destination.WriteChar('B');
        }

        private char GetCharacterFromSource()
        {
            return _source.ReadChar();
        }

        private static bool CharacterIsNotNewLine(char characterFromSource)
        {
            return !characterFromSource.Equals(NewLine);
        }
    }
}
