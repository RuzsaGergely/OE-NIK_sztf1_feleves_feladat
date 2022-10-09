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
    internal class AwariGame
    {
        // Pits of the user
        public int[] pitsOfA;
        // Pits of the computer
        public int[] pitsOfB;

        // Points of the user
        public int pointsOfA = 0;
        // Points of the computer
        public int pointsOfB = 0;

        // 0 - user ; 1 - computer
        private int turn = 0;

        public AwariGame()
        {
            pitsOfA = new int[] { 4,4,4,4,4,4 };
            pitsOfB = new int[] { 4,4,4,4,4,4 };
        }

        public void drawGameplay()
        {
            Console.Clear();
            // Draw computer's pits
            Console.Write($"[{pointsOfB}] ");
            for (int i = 0; i < pitsOfB.Length; i++)
            {
                Console.Write($"[{pitsOfB[i]}] ");
            }
            Console.Write($"A\n");
            Console.Write($" B ");
            for (int i = 0; i < pitsOfA.Length; i++)
            {
                Console.Write($"[{pitsOfA[i]}] ");
            }
            Console.Write($"[{pointsOfA}]\n\n");
        }

        public void askUserMove()
        {
            int input = 0;
            do
            {
                Console.Write("Lépés?: ");
                int.TryParse(Console.ReadLine(), out input);
            } while (pitsOfA[input] == 0);
            moveSeeds(turn, input);
        }

        private void moveSeeds(int player, int pit)
        {
            int leftover = pitsOfA[pit];
            pitsOfA[pit] = 0;
            for (int i = pit+1; i < pitsOfA.Length && leftover > 0; i++)
            {
                pitsOfA[i]++;
                leftover--;
            }
            while (leftover > 0)
            {
                for (int i = 0; i < pitsOfB.Length && leftover > 0; i++)
                {
                    pitsOfB[i]++;
                    leftover--;
                }
                for (int i = 0; i < pitsOfA.Length && leftover > 0; i++)
                {
                    pitsOfA[i]++;
                    leftover--;
                }
            }
            drawGameplay();
        }


        private void checkIfPlayerGetsPoint()
        {

        }

        
    }
}
