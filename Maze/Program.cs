using System.Runtime.InteropServices;

namespace Maze
{
    internal class Program
    {
        // 미로 찾기 게임을 만들 것이다.
        // 첫 작업으로는 게임이 구조를 작성한다.
        // 이후 플레이어를 배치하고 맵의 경우 [10,15] 크기의 정방향 맵을 일단 작성한다.
        // 기존에 작업했던 대로 플레이어 위치는 구조체로 작성한다.

        struct Position
        {
            public int x;
            public int y;

        }

        static void Main(string[] args)
        {

            while (true)
            {
                {
                    Console.Clear();

                    bool gameOver = false;
                    Position playerPos;
                    char[,] map;
                    int clearCounter = 0;

                    StartComment();
                    while (clearCounter <= 1)
                    {
                        Start(out playerPos, out map, clearCounter);
                        // 플레이어의 깜빡임이 심해 맵 전체를 갱신하는 것이 아닌 플레이어만 갱신하는 방식으로 접근
                        Console.Clear();
                        PrintMap(map);
                        Render(playerPos, playerPos);
                        bool exitedByEsc = false;

                        while (!gameOver)
                        {
                            // 플레이어 이전 위치 저장
                            Position playerPrePos = playerPos;
                            
                            ConsoleKey key = Input();
                            if (key == ConsoleKey.Escape)
                            {
                                exitedByEsc = true;
                                Console.Clear();
                                break;
                            }

                            Update(key, ref playerPos, ref map, ref clearCounter, ref gameOver);
                            Render(playerPrePos, playerPos);

                        }


                        if (exitedByEsc)
                        {
                            Console.Clear();

                            Console.WriteLine("처음 화면으로 돌아갑니다.");
                            Thread.Sleep(2000);
                            gameOver = false;
                            break;
                        }

                        if (gameOver && (clearCounter < 1))
                        {
                            Console.Clear();
                            Console.WriteLine("아직까지 미로에 갖힌듯 하다.");
                            Console.WriteLine("다음 스테이지로 이동");
                            Thread.Sleep(3000);
                            clearCounter++;
                            gameOver = false;

                        }

                        if (gameOver)
                        {
                            End();
                            clearCounter++;
                        }
                        
                       
                    }

                    
                }

            }

            static void StartComment()
            {
                Console.WriteLine("---------------------------");
                Console.WriteLine("당신의 심장이 미로에 빠지다.");
                Console.WriteLine("---------------------------");
                Console.WriteLine();
                Console.WriteLine("아무키나 눌러주세요.");
                Console.ReadKey(true);
            }




            //static void Start(out Position playerPos, out char[,] map)
            static void Start(out Position playerPos, out char[,] map, int clearCounter)
            {
                // 플레이어 위치, 맵
                Console.CursorVisible = false;

                playerPos.x = 1;
                playerPos.y = 1;
                map = new char[5, 5];

                if (clearCounter == 0)
                {
                    playerPos.x = 1;
                    playerPos.y = 1;

                    map = new char[10, 15]
                    {       // 0   1   2   3   4   5   6   7   8   9   10  11  12  13  14
                /*0*/{'▒','▒','▒','▒','▒','▒','▒','▒','▒','▒','▒','▒','▒','▒','▒'},
                /*1*/{'▒',' ',' ',' ','▒',' ','▒','▒',' ',' ',' ',' ','▒',' ','▒'},
                /*2*/{'▒',' ','▒',' ','▒',' ',' ','▒',' ','▒','▒','▒','▒',' ','▒'},
                /*3*/{'▒',' ','▒',' ','▒','▒',' ','▒',' ','▒',' ',' ',' ',' ','▒'},
                /*4*/{'▒',' ','▒',' ',' ','▒',' ','▒',' ','▒',' ','▒','▒',' ','▒'},
                /*5*/{'▒',' ','▒','▒',' ',' ',' ',' ',' ','▒','○','▒',' ',' ','▒'},
                /*6*/{'▒',' ',' ','▒',' ','▒',' ','▒','▒','▒','▒','▒',' ','▒','▒'},
                /*7*/{'▒',' ','▒','▒',' ','▒',' ','▒',' ',' ',' ',' ',' ','▒','▒'},
                /*8*/{'▒',' ','▒',' ',' ','▒',' ',' ',' ','▒',' ','▒',' ',' ','▒'},
                /*9*/{'▒','▒','▒','▒','▒','▒','▒','▒','▒','▒','▒','▒','▒','▒','▒'},

                    };
                }

                if (clearCounter >= 1)
                {
                    playerPos.x = 1;
                    playerPos.y = 1;

                    map = new char[10, 15]
                    {// 0   1   2   3   4   5   6   7   8   9   10  11  12  13  14
                /*0*/{'▒','▒','▒','▒','▒','▒','▒','▒','▒','▒','▒','▒','▒','▒','▒'},
                /*1*/{'▒',' ',' ',' ','▒',' ','▒','▒',' ',' ',' ',' ','▒',' ','▒'},
                /*2*/{'▒',' ','▒','○','▒',' ',' ','▒',' ','▒','▒','▒','▒',' ','▒'},
                /*3*/{'▒',' ','▒',' ','▒','▒',' ','▒',' ','▒',' ',' ',' ',' ','▒'},
                /*4*/{'▒',' ','▒',' ',' ','▒',' ','▒',' ','▒',' ','▒','▒',' ','▒'},
                /*5*/{'▒',' ','▒','▒',' ',' ',' ',' ','○','▒',' ','▒',' ',' ','▒'},
                /*6*/{'▒',' ',' ','▒',' ','▒',' ','▒','▒','▒','▒','▒',' ','▒','▒'},
                /*7*/{'▒',' ','▒','▒',' ','▒',' ','▒',' ',' ',' ',' ',' ','▒','▒'},
                /*8*/{'▒',' ','▒',' ',' ','▒',' ',' ',' ','▒','○','▒',' ',' ','▒'},
                /*9*/{'▒','▒','▒','▒','▒','▒','▒','▒','▒','▒','▒','▒','▒','▒','▒'},

                    };
                }


            }

            #region Render 기능

            static void Render(Position playerPrePos, Position playerPos)
            {
                // Print 플레이어, 맵
                Console.SetCursorPosition(0, 0);
                //PrintMap(map);
                //플레이이어 이전 위치 빈공간으로 변경
                PrintEmtSpace(playerPrePos);
                PrintPlayer(playerPos);


            }

            static void PrintEmtSpace(Position space)
            {

                Console.SetCursorPosition(space.x, space.y);
                Console.Write(" ");

            }


            static void PrintMap(char[,] map)
            {
                for (int y = 0; y < map.GetLength(0); y++)
                {
                    for (int x = 0; x < map.GetLength(1); x++)
                    {
                        Console.Write(map[y, x]);

                    }
                    Console.WriteLine();
                }
                PrintMapComment();

            }

            static void PrintMapComment()

            {
                Console.WriteLine("초기화를 원한다면 R 키를 눌러주세요.");
                Console.WriteLine("\'○\'까지 도달하면 성공입니다.");
                Console.WriteLine("\n시작화면으로 이동하고 싶다면 ESC를 누르세요.");
            }

            static void PrintPlayer(Position playerPos)
            {
                Console.SetCursorPosition(playerPos.x, playerPos.y);


                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("♥");
                Console.ResetColor();
            }
            #endregion


            static void Update(ConsoleKey key, ref Position playerPos, ref char[,] map, ref int clearCounter, ref bool gameOver)
            {
                Move(key, ref playerPos, map);
                IsClear(ref map, ref gameOver, ref clearCounter);

                Reset(key, ref playerPos, map, ref clearCounter);
            }

            static void Move(ConsoleKey key, ref Position playerPos, char[,] map)
            {
                Position targetPos = playerPos;

                switch (key)
                {
                    case ConsoleKey.A:
                    case ConsoleKey.LeftArrow:
                        targetPos.x -= 1;
                        break;

                    case ConsoleKey.D:
                    case ConsoleKey.RightArrow:
                        targetPos.x += 1;

                        break;

                    case ConsoleKey.W:
                    case ConsoleKey.UpArrow:
                        targetPos.y -= 1;

                        break;

                    case ConsoleKey.S:
                    case ConsoleKey.DownArrow:
                        targetPos.y += 1;

                        break;

                }
                if (map[targetPos.y, targetPos.x] != '▒')
                {

                    // 🔹 플레이어가 `○`로 이동하면 `○`를 빈 공간으로 변경
                    if (map[targetPos.y, targetPos.x] == '○')
                    {
                        map[targetPos.y, targetPos.x] = ' '; // `○` 제거
                    }

                    playerPos = targetPos;

                }
                //벽일때는 아무것도 하지마.
                else if (map[targetPos.y, targetPos.x] == '▒')
                {

                }

            }

            static void Reset(ConsoleKey key, ref Position playerPos, char[,] map, ref int clearCounter)
            {
                if (key == ConsoleKey.R)
                {
                    //초기화 부분 Main 함수내 구조와 같음.
                    Start(out playerPos, out map, clearCounter);
                    Console.Clear();
                    PrintMap(map);
                    Render(playerPos, playerPos);

                }
                //else if (clearCounter == 1)
                //{
                //    Start(out playerPos, out map, clearCounter);
                //    Console.Clear();
                //    PrintMap(map);
                //    Render(playerPos, playerPos);
                //
                //}
            }


            static ConsoleKey Input()
            {

                return Console.ReadKey(true).Key;

            }

            static bool IsClear(ref char[,] map, ref bool gameOver, ref int clearCounter)
            {
                foreach (char tile in map)
                {
                    if (tile == '○')
                    {
                        return false;
                    }

                }

                gameOver = true;
                return true;

            }

            static void End()
            {
                Console.Clear();
                Console.WriteLine("축하합니다.\n성공적으로 미로를 탈출 했습니다.");
                Thread.Sleep(2000);
                Console.Clear();
                Console.WriteLine("처음 화면으로 돌아갑니다....");
                Thread.Sleep(3000);
            }


        }
    }
}
