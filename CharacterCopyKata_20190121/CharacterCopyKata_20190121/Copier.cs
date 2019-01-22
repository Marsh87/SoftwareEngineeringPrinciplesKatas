using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CharacterCopyKata_20190121
{
    public class Copier
    {
        private readonly ISource _source;
        private readonly IDestination _destination;

        public Copier(ISource source, IDestination destination)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _destination = destination ?? throw new ArgumentNullException(nameof(destination));
        }

        public void Copy()
        {
            var characterFromSource =  _source.ReadChar();
            var newLine = '\n';
            if (SourceCharacterIsNewLine(characterFromSource, newLine)) return;
            _destination.WriteChar(characterFromSource);
        }

        private static bool SourceCharacterIsNewLine(char characterFromSource, char newLine)
        {
            if (characterFromSource.Equals(newLine))
            {
                return true;
            }
            return false;
        }
    }
}
