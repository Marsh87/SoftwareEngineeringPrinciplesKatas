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
            var characterFromSource = GetCharacterFromSource();
            while (CharacterIsNotNewLine(characterFromSource))
            {
                _destination.WriteChar(characterFromSource);
                characterFromSource = GetCharacterFromSource();
            }
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
