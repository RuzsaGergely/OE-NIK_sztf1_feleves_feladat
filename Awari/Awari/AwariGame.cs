/* OE-NIK # Ruzsa Gergely Gábor # BR1GHH
 * SzTF1 - Féléves projekt - Maják kincse (Awari)
 * Váltózok nyelve: angol / camelCase
 */
using System;

namespace Awari
{
    public class AwariGame
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

        public bool gameIsEnded = false;

        // Counts total turns of the game
        private int turnCounter = 1;

        // Counts total seed moves (only used for the statistics at the end)
        private int seedCounter = 0;

        public AwariGame()
        {
            pitsOfA = new int[] { 4,4,4,4,4,4 };
            pitsOfB = new int[] { 4,4,4,4,4,4 };
            drawWelcomeMessage();
            drawTurnSeparator();
        }

        public void drawGameplay()
        {
            Console.WriteLine("{0}{1}. kör - {2} lép", turn == 1 ? "\n" : "", turnCounter, turn == 1 ? "Számítógép" : "Felhasználó");
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

        private void drawTurnSeparator()
        {
            for (int i = 0; i < Console.WindowWidth; i++)
            {
                Console.Write("-");
            }
            Console.Write("\n");
        }

        private void drawWelcomeMessage()
        {
            // Resource: https://stackoverflow.com/questions/21917203/how-do-i-center-text-in-a-console-application
            string message = "## Awari - A Maják kincse ##";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (message.Length / 2)) + "}", message));
            message = "### Tárgy: SzTF1 - Írta: Ruzsa Gergely Gábor - Neptun: BR1GHH ###";
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (message.Length / 2)) + "}", message));
        }

        public void askUserMove()
        {
            int input = 0;
            do
            {
                drawGameplay();
                Console.Write("Lépés? {0...5}: ");
                int.TryParse(Console.ReadLine(), out input);
            } while ((input > pitsOfA.Length-1 || pitsOfA[input] == 0) && !gameIsEnded);
            moveSeeds(turn, input);
            turn = 1;
        }

        public void askComputerMove()
        {
            if (turn == 1 && !gameIsEnded)
            {
                drawGameplay();
                int best_move = calculateBestMoveForComputer();
                Console.WriteLine($"Számítógép választása: {best_move}");
                moveSeeds(turn, best_move);
            }
            turn = 0;
            turnCounter++;
            drawTurnSeparator();
        }

        private int calculateBestMoveForComputer()
        {
            int[] pitsPercentage = new int[pitsOfB.Length];
            for (int i = 0; i < pitsPercentage.Length; i++)
            {
                int simulationResult = simulateMovement(i);
                if(simulationResult == 2)
                {
                    pitsPercentage[i] = 1;
                } else if (simulationResult == 3)
                {
                    pitsPercentage[i] = 2;
                } else
                {
                    pitsPercentage[i] = 0;
                }
            }

            int maxPercentage = 0;
            int maxPercentageIndex = 0;
            for (int i = 0; i < pitsPercentage.Length; i++)
            {
                if (pitsPercentage[i] > maxPercentage)
                {
                    maxPercentage = pitsPercentage[i];
                    maxPercentageIndex = i;
                }
            }
            return maxPercentageIndex;
        }

        // Copied from moveSeeds() method and modified it
        private int simulateMovement(int pit)
        {
            // If the given pit is empty, returns -1 without even testing anything else
            if (pitsOfB[pit] == 0)
                return -1;

            int[] copyOfPitsOfB = pitsOfB;
            int[] copyOfPitsOfA = pitsOfA;
            int leftover = copyOfPitsOfB[pit];
            copyOfPitsOfB[pit] = 0;
            int current_side = 1;
            int last_pit = 0;

            for (int i = pit - 1; i > 0 && leftover > 0; i--)
            {
                copyOfPitsOfB[i]++;
                leftover--;
            }

            while (leftover > 0)
            {
                for (int i = 0; i < copyOfPitsOfA.Length && leftover > 0; i++)
                {
                    copyOfPitsOfA[i]++;
                    last_pit = i;
                    leftover--;
                    current_side = 0;
                }

                for (int i = copyOfPitsOfB.Length - 1; i > 0 && leftover > 0; i--)
                {
                    copyOfPitsOfB[i]++;
                    leftover--;
                    current_side = 1;
                }
            }

            if (current_side == 0)
            {
                return copyOfPitsOfA[last_pit];
            } else
            {
                return -1;
            }
        }

        private void moveSeeds(int player, int pit)
        {
            // Leftover seeds
            int leftover = 0;

            // Current side of the board: 0 = user; 1 = computer
            int current_side = 0;

            // The last pit where leftover reached 0
            int last_pit = 0;

            // If user's turn
            if (player == 0)
            {
                leftover = pitsOfA[pit];
                seedCounter += leftover;
                pitsOfA[pit] = 0;
                current_side = 0;
                for (int i = pit + 1; i < pitsOfA.Length && leftover > 0; i++)
                {
                    pitsOfA[i]++;
                    leftover--;
                }
                while (leftover > 0)
                {
                    for (int i = pitsOfB.Length-1; i >= 0 && leftover > 0; i--)
                    {
                        pitsOfB[i]++;
                        last_pit = i;
                        leftover--;
                        current_side = 1;
                    }
                    
                    for (int i = 0; i < pitsOfA.Length && leftover > 0; i++)
                    {
                        if(i != pit)
                        {
                            pitsOfA[i]++;
                            leftover--;
                            current_side = 0;
                        }
                    }
                }
            } 
            // If computer's turn
            else
            {
                leftover = pitsOfB[pit];
                seedCounter += leftover;
                pitsOfB[pit] = 0;
                current_side = 1;
                
                for (int i = pit - 1; i > -1 && leftover > 0; i--)
                {
                    pitsOfB[i]++;
                    leftover--;
                }
                while (leftover > 0)
                {
                    for (int i = 0; i < pitsOfA.Length && leftover > 0; i++)
                    {
                        pitsOfA[i]++;
                        last_pit = i;
                        leftover--;
                        current_side = 0;
                    }

                    for (int i = pitsOfB.Length-1; i > -1 && leftover > 0; i--)
                    {
                        if(i != pit)
                        {
                            pitsOfB[i]++;
                            leftover--;
                        }
                        current_side = 1;
                    }
                }
            }
            checkIfPlayerGetsPoint(player, current_side, last_pit);
        }

        private void checkIfPlayerGetsPoint(int player, int current_side, int last_pit)
        {
            //Console.WriteLine($"Játékos: {(player == 0 ? "Felhasználó" : "Számítógép")} - {(current_side == 0 ? "A" : "B")} oldal - Utolsó ház: {last_pit}");
            // Check if User gets points
            if(player == 0)
            {
                if(current_side == 1)
                {
                    if (pitsOfB[last_pit] == 2 || pitsOfB[last_pit] == 3)
                    {
                        Console.WriteLine($"\n! Felhasználó pontot szerzett ({pitsOfB[last_pit]}) !");
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
                        Console.WriteLine($"\n! Számítógép pontot szerzett ({pitsOfA[last_pit]}) !");
                        pointsOfB += pitsOfA[last_pit];
                        pitsOfA[last_pit] = 0;
                    }
                }
            }
            checkIfGameEnded();
        }

        private void checkIfGameEnded()
        {
            int winner = 0;
            if(pointsOfA == 24 && pointsOfB == 24)
            {
                gameIsEnded = true;
                winner = -1;
            } else if(pointsOfA >= 25)
            {
                gameIsEnded = true;
            }
            else if (pointsOfB >= 25)
            {
                gameIsEnded = true;
                winner = 1;
            }
            if(gameIsEnded)
                printGameStats(winner);
        }

        private void printGameStats(int winner)
        {
            drawTurnSeparator();
            string winner_string = "";
            if(winner == -1)
            {
                winner_string = "A játék eredménye döntetlen lett!";
            } else if(winner == 0)
            {
                winner_string = "A játékot a felhasználó nyerte!";
            } else
            {
                winner_string = "A játékot a számítógép nyerte!";
            }
            Console.WriteLine(String.Format("{0," + ((Console.WindowWidth / 2) + (winner_string.Length / 2)) + "}", winner_string));
            Console.WriteLine($"Statisztika:\n\tFelhasználó pontjai: {pointsOfA}\n\tSzámítógép pontjai: {pointsOfB}\n\tAkkumulált magmozgás: {seedCounter}\n\tÖsszes lejátszott kör: {turnCounter}");
        }
    }
}
