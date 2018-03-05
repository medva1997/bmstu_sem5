using System.Text;

namespace Laba8
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] key = ASCIIEncoding.ASCII.GetBytes("Key");
            RC4 rc4= new RC4(key);
            RC4 rc42= new RC4(key);

            Encoder encoder= new Encoder(rc4);
            Decoder decoder= new Decoder(rc42);
            encoder.SetNextСonveyer(decoder);
            string testString = "Final laba";

            foreach (char c in testString)
            {
                byte[] testBytes = ASCIIEncoding.ASCII.GetBytes(c.ToString());
                encoder.Enqueue(testBytes);
            }
            encoder.Run();
            decoder.Run();
           
          
        }
    }
}