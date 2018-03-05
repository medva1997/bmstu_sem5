using System.Text;

namespace Laba8
{
    class Encoder: СonveyerBase<byte[]>
    {
        private RC4 coder;
        public Encoder(RC4 coder)
        {
            this.coder = coder;
            Name = "Encoder";
            Wait = false;
        }
        
        protected override byte[] Work(byte[] data)
        {
            return coder.Encode(data, data.Length);
        }

        protected override string ToString(byte[] data)
        {
            return ASCIIEncoding.ASCII.GetString(data);
        }
    }
    
    class Decoder: СonveyerBase<byte[]>
    {
        private RC4 coder;
        public Decoder(RC4 coder)
        {
            this.coder = coder;
            Name = "Decoder";
            Wait = true;
        }
        
        protected override byte[] Work(byte[] data)
        {
            return coder.Decode(data, data.Length);
        }
        
        protected override string ToString(byte[] data)
        {
            return ASCIIEncoding.ASCII.GetString(data);
        }
    }
}