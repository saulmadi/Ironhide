using System;

namespace Ironhide.Login.Domain
{
    public class JwtSymmetricKeyProvider : IKeyProvider
    {
        public byte[] GetKey()
        {
            return getBytes("ThisIsAnImportantStringAndIHaveNoIdeaIfThisIsVerySecureOrNot!");
        }

        byte[] getBytes(string str)
        {
            var bytes = new byte[str.Length*sizeof (char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}