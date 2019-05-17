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
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _destination = destination ?? throw new ArgumentNullException(nameof(destination));
        }

        public void Copy()
        {
            // Unsure how refactor this method

            // TODO there are still 2 concepts in this code that are not fully encapsulated by the methods you refactored out, see below:
            var characterFromSource = GetCharacterFromSource(); // <== read
            while (CharacterIsNotNewLine(characterFromSource))
            {
                WriterCharacterToDestination(characterFromSource); // <== read
                characterFromSource = GetCharacterFromSource(); // <== write
            }
        }

        private void WriterCharacterToDestination(char characterFromSource)
        {
            _destination.WriteChar(characterFromSource);
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
