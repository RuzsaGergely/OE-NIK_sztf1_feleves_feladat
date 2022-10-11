/* OE-NIK # Ruzsa Gergely Gábor # BR1GHH
 * SzTF1 - Féléves projekt - Maják kincse (Awari)
 * Váltózok nyelve: angol / camelCase
 */
using System;

namespace Awari
{
    internal class AwariGame
    {
        // Pits of the user
        public int[] pitsOfA;
        // Pits of the computer
        public int[] pitsOfB;

        // Points of the user
        public int pointsOfA = 23;
        // Points of the computer
        public int pointsOfB = 0;

        // 0 - user ; 1 - computer
        private int turn = 0;

        public bool gameIsEnded = false;

        private Random random = new Random();

        private int turnCounter = 1;

        public AwariGame()
        {
            pitsOfA = new int[] { 4,4,4,4,4,4 };
            pitsOfB = new int[] { 4,4,4,4,4,4 };
        }

        public void drawGameplay()
        {
            //Console.Clear();
            Console.WriteLine("{0}. kör - {1} lép", turnCounter, turn == 1 ? "Számitógép" : "Felhasználó");
            // Draw computer's pits
            Console.Write($"({pointsOfB}) ");
            for (int i = 0; i < pitsOfB.Length; i++)
            {
                Console.Write($"[{pitsOfB[i]}] ");
            }
            Console.Write($" A\n");
            // Draw user's pits
            Console.Write($" B  ");
            for (int i = 0; i < pitsOfA.Length; i++)
            {
                Console.Write($"[{pitsOfA[i]}] ");
            }
            Console.Write($"({pointsOfA})\n\n");
        }

        public void askUserMove()
        {
            int input = 0;
            do
            {
                drawGameplay();
                Console.Write("Lépés? {0...5}: ");
                int.TryParse(Console.ReadLine(), out input);
            } while (input > pitsOfA.Length-1 || pitsOfA[input] == 0);
            moveSeeds(turn, input);
            turn = 1;
        }

        public void askComputerMove()
        {
            if (turn == 1)
            {
                int choice = random.Next(0, pitsOfB.Length);
                do
                {
                    choice = random.Next(0, pitsOfB.Length);
                } while (pitsOfB[choice] == 0);

                moveSeeds(turn, choice);
            }
            turn = 0;
            turnCounter++;
        }

        private void moveSeeds(int player, int pit)
        {
            int leftover = 0;
            // enemy_pit 0 = user; 1 = computer
            int current_side = 0;
            // last pit 
            int last_pit = 0;
            // user
            if (player == 0)
            {
                leftover = pitsOfA[pit];
                pitsOfA[pit] = 0;
                current_side = 0;
                for (int i = pit + 1; i < pitsOfA.Length && leftover > 0; i++)
                {
                    pitsOfA[i]++;
                    leftover--;
                }
                while (leftover > 0)
                {
                    for (int i = 0; i < pitsOfB.Length && leftover > 0; i++)
                    {
                        pitsOfB[i]++;
                        last_pit = i;
                        leftover--;
                        current_side = 1;
                    }
                    
                    for (int i = 0; i < pitsOfA.Length && leftover > 0; i++)
                    {
                        pitsOfA[i]++;
                        leftover--;
                        current_side = 0;
                    }
                }
                checkIfPlayerGetsPoint(player, current_side, last_pit);
            } 
            // computer
            else
            {
                leftover = pitsOfB[pit];
                pitsOfB[pit] = 0;
                current_side = 1;
                for (int i = pit - 1; i > 0 && leftover > 0; i--)
                {
                    pitsOfB[i]++;
                    leftover--;
                }
                while (leftover > 0)
                {
                    for (int i = 0; i < pitsOfA.Length && leftover > 0; i++)
                    {
                        pitsOfA[i]++;
                        last_pit=i;
                        leftover--;
                        current_side = 0;
                    }
                    
                    for (int i = pitsOfB.Length-1; i > 0 && leftover > 0; i--)
                    {
                        pitsOfB[i]++;
                        leftover--;
                        current_side = 1;
                    }
                }
                checkIfPlayerGetsPoint(player, current_side, last_pit);
            }
            drawGameplay();
        }


        private void checkIfPlayerGetsPoint(int player, int current_side, int last_pit)
        {
            Console.WriteLine($"Játékos: {(player == 0 ? "Felhasználó" : "Számitógép")} - {(current_side == 0 ? "A" : "B")} oldal - Utolsó ház: {last_pit}");
            // check if user gets point
            if(player == 0)
            {
                if(current_side == 1)
                {
                    if (pitsOfB[last_pit] == 2 || pitsOfB[last_pit] == 3)
                    {
                        Console.WriteLine($"! Felhasználó pontot szerzett ({pitsOfB[last_pit]}) !");
                        pointsOfA += pitsOfB[last_pit];
                        pitsOfB[last_pit] = 0;
                    }
                }
            } else
            {
                if (current_side == 0)
                {
                    if (pitsOfA[last_pit] == 2 || pitsOfA[last_pit] == 3)
                    {
                        Console.WriteLine($"! Számitógép pontot szerzett ({pitsOfA[last_pit]}) !");
                        pointsOfB += pitsOfA[last_pit];
                        pitsOfA[last_pit] = 0;
                    }
                }
            }
            checkIfGameEnded();
        }

        private void checkIfGameEnded()
        {
            if(pointsOfA == 24 && pointsOfB == 24)
            {
                Console.WriteLine("Döntetlen.");
                gameIsEnded = true;
            } else if(pointsOfA >= 25)
            {
                Console.WriteLine("A felhasználó nyert.");
                gameIsEnded = true;
            }
            else if (pointsOfB >= 25)
            {
                Console.WriteLine("A gép nyert.");
                gameIsEnded = true;
            }
        }   
    }
}
