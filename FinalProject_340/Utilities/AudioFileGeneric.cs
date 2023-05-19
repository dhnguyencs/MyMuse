using static TagLib.File;

namespace FinalProject_340.Utilities
{
    public class GenericAudioStream : IFileAbstraction
    {
        private readonly string _fileName;
        private readonly Stream _stream;

        public GenericAudioStream(string fileName, Stream stream)
        {
            _fileName = fileName;
            _stream = stream;
        }

        public Stream ReadStream
        {
            get { return _stream; }
        }

        public Stream WriteStream
        {
            get { return _stream; }
        }

        public string Name
        {
            get { return _fileName; }
        }

        public void CloseStream(Stream stream)
        {
            // do nothing, as the stream is managed outside of this class
        }
    }
}
