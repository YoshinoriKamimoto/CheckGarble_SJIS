using System.Text;
using System.Globalization;
internal class Program
{
    private static void Main(string[] args)
    {
        bool result;
        result = IsGarble("�");

        if (result == true)
        {
            Console.WriteLine("文字化けあり");
        }
        else
        {
            Console.WriteLine("文字化けなし");
        }
    }

    // 文字化けするかチェックするメソッド
    private static bool IsGarble(string str)
    {
        // サロゲートペア・結合文字が存在するかチェック
        StringInfo si = new StringInfo(str);
        if (str.Length != si.LengthInTextElements)
        {
            Console.WriteLine("サロゲートペア・結合文字あり");
            return true;
        }
        

        // // Shift_JISでエンコード
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Encoding sjis = Encoding.GetEncoding("Shift-JIS");

        // 1文字ずつ取り出してチェック
        for (int i = 0; i < str.Length; i++)
        {
            Console.WriteLine($"{i + 1}文字目:{str[i]}");
            byte[] bytes = sjis.GetBytes(str[i].ToString());

            int code = 0;
            foreach (byte item in bytes)
            {
                code = item + code * 256;
            }

            Console.WriteLine($"Shift_JIS文字コード(10進数):{code}");
            

            // 文字化けする可能性がある範囲かチェック
            if (str[i].ToString() != "?" && code == 63)
            {
                return true;
            }

            if (0x8540 <= code && code <= 0x889e)
            {
                return true;
            }

            if (0xeb40 <= code && code <= 0xeffc)
            {
                return true;
            }

            if (0xf040 <= code)
            {
                return true;
            }
        }


        // UTF-8でエンコード
        // Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        // Encoding utf8 = Encoding.GetEncoding("UTF-8");

        // // 1文字ずつ取り出してチェック
        // for (int i = 0; i < str.Length; i++)
        // {
        //     Console.WriteLine($"{i + 1}文字目:{str[i]}");
        //     byte[] bytes = utf8.GetBytes(str[i].ToString());

        //     int code = 0;
        //     foreach (byte item in bytes)
        //     {
        //         code = item + code * 256;
        //     }

        //     Console.WriteLine($"UTF-8文字コード(10進数):{code}");
            

        //     // 文字化けする可能性がある範囲かチェック
        //     if (str[i].ToString() != "?" && code == 63)
        //     {
        //         return true;
        //     }

        //     if (0x8540 <= code && code <= 0x889e)
        //     {
        //         return true;
        //     }
        // }


        // 文字化けなし
        return false;        
    }
}