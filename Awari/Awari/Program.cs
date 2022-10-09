/* OE-NIK # Ruzsa Gergely Gábor # BR1GHH
 * SzTF1 - Féléves projekt - Maják kincse (Awari)
 * Váltózok nyelve: angol / camelCase
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Awari
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AwariGame game = new AwariGame();
            game.drawGameplay();
            while (true)
            {
                game.askUserMove();
            }

            Console.ReadKey();
        }
    }
}
