using System.Linq;

namespace Laba8
{
    public class RC4
    {
        /// <summary>
        /// Перестановки, содержащей все возможные байты от 0x00 до 0xFF
        /// </summary>
        byte[] S = new byte[256];
        
        //Переменные-счетчики x и y.
        int x = 0;
        int y = 0;
        
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="key"></param>
        public RC4(byte[] key)
        {
            init(key);
        }
        
        /// <summary>
        /// поменять два элемента массива местами
        /// </summary>
        /// <param name="s">массив</param>
        /// <param name="i"></param>
        /// <param name="j"></param>
        private static void Swap(byte[] s, int i, int j)
        {
            byte c = s[i];
            s[i] = s[j];
            s[j] = c;
        }
        
        /// <summary>
        /// генератор псевдослучайной последовательности
        /// </summary>
        /// <returns></returns>
        private byte keyItem()
        {
            x = (x + 1) % 256;
            y = (y + S[x]) % 256;

            Swap(S,x, y);

            return S[(S[x] + S[y]) % 256];
        } 
        
        /// <summary>
        /// начальной инициализация вектора-перестановки ключём
        /// </summary>
        /// <param name="key">Ключ шифрования</param>
        private void init(byte[] key)
        {
            int keyLength = key.Length;

            for (int i = 0; i < 256; i++)
            {
                S[i] = (byte)i;
            }

            int j = 0;
            
            for (int i = 0; i < 256; i++)
            {
                j = (j + S[i] + key[i % keyLength]) % 256;
                Swap(S,i, j);      
            }
        }
        
        public byte[] Encode(byte[] dataB, int size)
        {   
            byte[] data = dataB.Take(size).ToArray();       

            byte[] cipher = new byte[data.Length];

            for (int m = 0; m < data.Length; m++)
            {       
                cipher[m] = (byte)(data[m] ^ keyItem());
            }

            return cipher;
        }
        
        public byte[] Decode(byte[] dataB, int size)
        {
            return Encode(dataB, size);
        }
        
    }
    
}