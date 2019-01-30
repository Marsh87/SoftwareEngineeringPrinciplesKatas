using System;
using System.Collections.Generic;
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
            while (!_source.ReadChar().Equals('\n'))
            {
                var characterFromSource = _source.ReadChar();
                _destination.WriteChar(characterFromSource);
            }
        }
    }
}
