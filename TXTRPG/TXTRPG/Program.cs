// 필요한 네임스페이스를 불러옵니다. 
// 각 네임스페이스는 특정 기능을 사용하기 위해 포함됩니다.
using System;  // 기본적인 입출력 및 시스템 관련 기능
using System.IO;  // 파일을 읽고 쓰기 위한 기능
using System.Text.Json;  // JSON 형식으로 데이터를 변환하거나 파싱하기 위한 기능
using System.Collections.Generic;  // List, Dictionary 등 자료구조 사용
using System.Threading;  // 출력 시 지연을 구현하기 위한 기능

// 메인 프로그램 클래스
class Program
{
    // 게임 데이터가 저장될 파일 경로를 저장하는 변수
    static string saveFilePath = "saveData.json";

    // 랜덤 값을 생성하기 위한 객체, 게임 내에서 적의 능력치나 이벤트 발생 등을 랜덤하게 처리할 때 사용
    static Random random = new Random();

    // 적 캐릭터의 정보를 저장하는 클래스입니다.
    class Enemy
    {
        // 적의 이름을 저장하는 속성입니다.
        public string Name { get; set; } = string.Empty;
        // 적의 체력을 저장하는 속성입니다.
        public int? Health { get; set; }
        // 적의 공격력을 저장하는 속성입니다.
        public int? Attack { get; set; }
        // 적의 방어력을 저장하는 속성입니다.
        public int? Defense { get; set; }
        // 적의 ASCII 아트를 저장하는 속성입니다.
        public string AsciiArt { get; set; } = string.Empty;
    }

    // 게임 내에서 아이템(무기 또는 방어구)를 정의하는 클래스입니다.
    class Item
    {
        // 아이템 이름을 저장하는 속성입니다.
        public string Name { get; set; } = string.Empty;
        // 아이템의 종류 (무기 또는 방어구)를 저장하는 속성입니다.
        public string Type { get; set; } = string.Empty;
        // 아이템의 가격을 저장하는 속성입니다.
        public int? Price { get; set; }
        // 아이템의 능력치 (공격력 또는 방어력)를 저장하는 속성입니다.
        public int? Power { get; set; }
        // 아이템이 판매 완료된 상태인지 추적하는 속성입니다.
        public bool IsSoldOut { get; set; } = false;
    }

    // 플레이어 캐릭터를 정의하는 클래스입니다.
    class Character
    {
        // 캐릭터 이름을 저장하는 속성입니다.
        public string Name { get; set; } = string.Empty;
        // 캐릭터 직업을 저장하는 속성입니다.
        public string Job { get; set; } = string.Empty;
        // 캐릭터의 레벨을 저장하는 속성입니다. 기본값은 1입니다.
        public int Level { get; set; } = 1;
        // 캐릭터의 공격력을 저장하는 속성입니다.
        public int Attack { get; set; } = 10;
        // 캐릭터의 방어력을 저장하는 속성입니다.
        public int Defense { get; set; } = 5;
        // 캐릭터의 체력을 저장하는 속성입니다.
        public int Health { get; set; } = 100;
        // 캐릭터가 가진 골드를 저장하는 속성입니다.
        public int Gold { get; set; } = 1500;
        // 캐릭터가 소유한 아이템 목록 (인벤토리)입니다.
        public List<Item> Inventory { get; set; } = new List<Item>();
        // 캐릭터가 장착한 무기를 저장하는 속성입니다.
        public Item? EquippedWeapon { get; set; }
        // 캐릭터가 장착한 방어구를 저장하는 속성입니다.
        public Item? EquippedArmor { get; set; }
    }

    // 현재 플레이어 캐릭터를 저장하는 변수. 초기에는 null로 설정됩니다.
    static Character? playerCharacter;

    // 상점에서 판매할 아이템 목록을 저장하는 문장입니다.
    static List<Item> shopItems = new List<Item>();

    // 적의 이름과 ASCII 아트를 연관짓는 문장입니다.
    static Dictionary<string, string> enemyAsciiArt = new Dictionary<string, string>
    {
        { "김록기 매니저", @"
              ________
             /        \
            |  ( ^_^ )  < 제 능력을 의심하시나요?
             \   ||   / 
               ------   < 맛점하세요!>
"
        },
        { "안혜린 매니저", @"

              { O.O }  < 음 그러시군요!
               /| |\   
              /_|_|_\  
                | |
                |_|    < 모두 잊지마시기 바랍니다.>
"
        },
        { "강채린 매니저", @" 

                (\(\ 
               (=':')  <안내드립니다! 
                |  |
                |__|   <스포트라이트를 켜봤어요!>
"
        }
    };

    // 프로그램의 시작 지점입니다. 여기서 게임이 시작됩니다.
    static void Main(string[] args)
    {
        // 상점에 판매할 아이템을 초기화합니다.
        InitializeShop();
        // 게임 타이틀 화면을 출력합니다.
        StartScreen();
        // 메인 메뉴를 표시합니다.
        ShowMenu();
    }

    // 게임 타이틀 화면을 출력하는 함수입니다.
    static void StartScreen()
    {
        Console.Clear(); // 콘솔 화면을 지웁니다.
        // ASCII 아트로 타이틀을 출력합니다.
        Console.WriteLine(@"
           
     
         /|                           |\
        / |                           | \
       /  /                           \  \
      /  /     _                _      \  \   
     |  |     | |   ________   | |     |  |      
     |  |     | |  |   __   |  | |     |  |      
     |  |     |_|  |  |__|  |  |_|     |  |      
     |  |______|   |________|   |______|  |
     \                                    /
      \__________________________________/
           \  ______  _______  ______  /
            \/      \/       \/      \/
             |    l  코딩 던전  l    |
             |                       |
             |  캠프의 비밀          |
             |     알고리즘을 찾아라 |
             |_______________________|



");
        // 환영 메시지를 출력합니다.
        Console.WriteLine("코딩 던전에 오신 것을 환영합니다!\n");
        // 사용자에게 아무 키나 눌러 계속 진행하라는 메시지를 표시합니다.
        Console.WriteLine("아무 키나 눌러 계속하세요...");
        Console.ReadKey(); // 사용자가 키를 누를 때까지 대기합니다.
    }

    // 메인 메뉴를 표시하는 함수입니다.
    static void ShowMenu()
    {
        while (true)
        {
            Console.Clear(); // 화면을 지우고 메뉴를 새로 출력합니다.
            // 메뉴 항목을 출력합니다.
            Console.WriteLine("1. 새 게임");
            Console.WriteLine("2. 이어하기");
            Console.WriteLine("3. 종료");
            Console.Write("옵션을 선택하세요: ");
            string? choice = Console.ReadLine(); // 사용자 입력을 받습니다.

            // 입력된 값에 따라 다른 동작을 수행합니다.
            switch (choice)
            {
                case "1":
                    // 새 게임을 시작합니다.
                    NewGame();
                    break;
                case "2":
                    // 저장된 게임을 불러옵니다.
                    LoadGame();
                    break;
                case "3":
                    // 게임을 종료합니다.
                    ExitGame();
                    return;
                default:
                    // 잘못된 입력이 있을 경우 메시지를 출력하고 다시 입력을 받습니다.
                    Console.WriteLine("잘못된 입력입니다. 다시 시도하세요.");
                    break;
            }
        }
    }

    // 새로운 게임을 시작하는 함수입니다.
    static void NewGame()
    {
        Console.Clear(); // 화면을 지웁니다.
        Console.WriteLine("새 게임을 시작합니다...");

        // 캐릭터의 이름을 입력받습니다.
        Console.Write("캐릭터의 이름을 입력하세요: ");
        string? name = Console.ReadLine();

        // 캐릭터의 직업을 선택합니다.
        string job = ChooseJob();

        // 새로운 캐릭터를 생성하고 초기값을 설정합니다.
        playerCharacter = new Character
        {
            Name = name ?? "Unknown", // 이름이 입력되지 않으면 "Unknown"으로 설정
            Job = job
        };

        // 캐릭터 생성 완료 메시지를 출력하고 상태를 표시합니다.
        Console.WriteLine("\n캐릭터 생성이 완료되었습니다!");
        DisplayCharacterStatus();

        // 게임 데이터를 저장합니다.
        SaveGame();

        // 마을로 이동합니다.
        EnterTown();
    }

    // 캐릭터 직업을 선택하는 함수입니다.
    static string ChooseJob()
    {
        while (true)
        {
            Console.Clear(); // 화면을 지웁니다.
            // 직업 선택 메뉴를 출력합니다.
            Console.WriteLine("직업을 선택하세요:");
            Console.WriteLine("1. 기획전사");
            Console.WriteLine("2. 개발마법사");
            Console.WriteLine("3. PM도적");
            Console.Write("번호를 입력하세요: ");

            // 사용자의 입력을 받습니다.
            string? choice = Console.ReadLine();

            // 입력된 번호에 따라 직업을 반환합니다.
            switch (choice)
            {
                case "1":
                    return "기획전사";
                case "2":
                    return "개발마법사";
                case "3":
                    return "PM도적";
                default:
                    // 잘못된 입력이 들어오면 다시 입력을 받습니다.
                    Console.WriteLine("잘못된 입력입니다. 다시 시도하세요.");
                    break;
            }
        }
    }

    // 캐릭터의 현재 상태를 출력하는 함수입니다.
    static void DisplayCharacterStatus()
    {
        // 캐릭터의 이름, 직업, 레벨, 공격력, 방어력, 체력, 골드를 출력합니다.
        Console.WriteLine($"\n--- {playerCharacter.Name}의 상태 ---");
        Console.WriteLine($"직업: {playerCharacter.Job}");
        Console.WriteLine($"레벨: {playerCharacter.Level}");
        Console.WriteLine($"공격력: {playerCharacter.Attack}");
        Console.WriteLine($"방어력: {playerCharacter.Defense}");
        Console.WriteLine($"체력: {playerCharacter.Health}");
        Console.WriteLine($"골드: {playerCharacter.Gold} G");

        // 장착된 무기가 있는지 확인하고 출력합니다.
        if (playerCharacter.EquippedWeapon != null)
        {
            Console.WriteLine($"장착 무기: {playerCharacter.EquippedWeapon.Name}");
        }
        else
        {
            Console.WriteLine("장착 무기: 없음");
        }

        // 장착된 방어구가 있는지 확인하고 출력합니다.
        if (playerCharacter.EquippedArmor != null)
        {
            Console.WriteLine($"장착 방어구: {playerCharacter.EquippedArmor.Name}");
        }
        else
        {
            Console.WriteLine("장착 방어구: 없음");
        }

        // 계속 진행할 수 있도록 안내 메시지를 출력합니다.
        Console.WriteLine("\n계속하려면 아무 키나 누르세요...");
        Console.ReadKey(); // 사용자가 아무 키나 누를 때까지 대기합니다.
    }

    // 게임 데이터를 저장하는 함수입니다.
    static void SaveGame()
    {
        try
        {
            // 캐릭터 데이터를 JSON 형식으로 변환하여 파일에 저장합니다.
            string jsonString = JsonSerializer.Serialize(playerCharacter);
            File.WriteAllText(saveFilePath, jsonString);
            Console.WriteLine("게임이 저장되었습니다.");
        }
        catch (Exception ex)
        {
            // 저장 중 오류가 발생할 경우 메시지를 출력합니다.
            Console.WriteLine($"게임 저장 중 오류가 발생했습니다: {ex.Message}");
        }

        // 저장 후 계속 진행할 수 있도록 안내 메시지를 출력합니다.
        Console.WriteLine("계속하려면 아무 키나 누르세요...");
        Console.ReadKey();
    }

    // 저장된 게임 데이터를 불러오는 함수입니다.
    static void LoadGame()
    {
        // 저장된 파일이 있는지 확인합니다.
        if (File.Exists(saveFilePath))
        {
            try
            {
                // 파일에서 데이터를 읽어와서 캐릭터 객체로 복원합니다.
                string jsonString = File.ReadAllText(saveFilePath);
                playerCharacter = JsonSerializer.Deserialize<Character>(jsonString);

                Console.WriteLine("저장된 게임을 불러왔습니다!");
                DisplayCharacterStatus(); // 불러온 캐릭터의 상태를 출력합니다.
                EnterTown(); // 마을로 이동합니다.
            }
            catch (Exception ex)
            {
                // 데이터를 불러오는 중 오류가 발생할 경우 메시지를 출력합니다.
                Console.WriteLine($"저장된 데이터를 불러오는 중 오류가 발생했습니다: {ex.Message}");
            }
        }
        else
        {
            // 저장된 파일이 없을 경우 메시지를 출력합니다.
            Console.WriteLine("저장된 게임이 없습니다.");
        }

        // 계속 진행할 수 있도록 안내 메시지를 출력합니다.
        Console.WriteLine("계속하려면 아무 키나 누르세요...");
        Console.ReadKey();
    }

    // 마을에 진입하는 함수입니다.
    static void EnterTown()
    {
        Console.Clear(); // 화면을 지웁니다.
        Console.WriteLine("당신은 마을에 도착했습니다.");
        Console.WriteLine("마을이 위험에 처해 있습니다! 근처에 던전이 나타났습니다.");
        Console.WriteLine("어디로 가시겠습니까?");

        while (true)
        {
            // 마을에서 선택할 수 있는 옵션을 출력합니다.
            Console.WriteLine("\n1. 상태창");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전");
            Console.WriteLine("5. 휴식하기");
            Console.WriteLine("6. 게임 저장");
            Console.WriteLine("7. 게임 종료");
            Console.Write("옵션을 선택하세요: ");
            string? choice = Console.ReadLine(); // 사용자 입력을 받습니다.

            // 선택한 옵션에 따라 다른 동작을 수행합니다.
            switch (choice)
            {
                case "1":
                    // 캐릭터 상태를 출력합니다.
                    DisplayCharacterStatus();
                    break;
                case "2":
                    // 인벤토리를 엽니다.
                    OpenInventory();
                    break;
                case "3":
                    // 상점을 방문합니다.
                    VisitShop();
                    break;
                case "4":
                    // 던전에 진입합니다.
                    EnterDungeon();
                    break;
                case "5":
                    // 캐릭터가 휴식합니다.
                    CaracterRest();
                    break;
                case "6":
                    // 게임 데이터를 저장합니다.
                    SaveGame();
                    break;
                case "7":
                    // 게임을 종료합니다.
                    ExitGame();
                    return;
                default:
                    // 잘못된 입력이 있을 경우 다시 입력받습니다.
                    Console.WriteLine("잘못된 입력입니다. 다시 시도하세요.");
                    break;
            }
        }
    }

    // 상점을 방문하는 함수입니다.
    static void VisitShop()
    {
        Console.Clear(); // 화면을 지웁니다.
        Console.WriteLine("상점에 오신 것을 환영합니다!");
        Console.WriteLine($"현재 보유 골드: {playerCharacter?.Gold} G"); // 현재 골드를 출력합니다.

        while (true)
        {
            // 상점에서 판매하는 아이템 목록을 출력합니다.
            Console.WriteLine("\n--- 상점 아이템 ---");
            for (int i = 0; i < shopItems.Count; i++)
            {
                string functionDescription;
                if (shopItems[i].Type == "무기")
                {
                    functionDescription = $"공격력 +{shopItems[i].Power}";
                }
                else if (shopItems[i].Type == "방어구")
                {
                    functionDescription = $"방어력 +{shopItems[i].Power}";
                }
                else
                {
                    functionDescription = $"효과: +{shopItems[i].Power}";
                }

                // 판매 완료된 아이템이면 "판매 완료" 표시
                string priceDisplay = shopItems[i].IsSoldOut ? "판매 완료" : $"{shopItems[i].Price} G";

                Console.WriteLine($"{i + 1}. {shopItems[i].Name} ({functionDescription}, 가격: {priceDisplay})");
            }

            Console.Write("\n구매할 아이템 번호를 선택하세요 (나가기는 아무 키나 누르세요): ");
            string? choice = Console.ReadLine();
            int index;

            if (int.TryParse(choice, out index))
            {
                if (index >= 1 && index <= shopItems.Count)
                {
                    // 선택한 아이템을 구매합니다.
                    BuyItem(shopItems[index - 1]);
                }
                else
                {
                    // 잘못된 입력일 경우 다시 시도합니다.
                    Console.WriteLine("잘못된 입력입니다. 다시 시도하세요.");
                }
            }
            else
            {
                // 상점 나가기 옵션을 선택하면 마을로 돌아갑니다.
                Console.Clear();
                Console.WriteLine("상점을 떠납니다...");
                Console.WriteLine("마을로 돌아가려면 아무 키나 누르세요.");
                Console.ReadKey();
                EnterTown();
                return;
            }
        }
    }

    // 아이템을 구매하는 함수입니다.
    static void BuyItem(Item item)
    {
        if (item.IsSoldOut)
        {
            // 이미 판매된 아이템이면 구매할 수 없다는 메시지를 출력합니다.
            Console.WriteLine("이미 판매된 아이템입니다.");
        }
        else if (playerCharacter?.Gold >= item.Price)
        {
            // 골드가 충분할 경우 아이템을 구매합니다.
            playerCharacter.Gold -= item.Price ?? 0; // 아이템 가격만큼 골드를 차감합니다.
            playerCharacter.Inventory.Add(item); // 인벤토리에 아이템을 추가합니다.
            item.IsSoldOut = true; // 아이템을 판매 완료 상태로 설정합니다.
            Console.WriteLine($"{item.Name}을(를) {item.Price} G에 구매했습니다.");
        }
        else
        {
            // 골드가 부족할 경우 메시지를 출력합니다.
            Console.WriteLine("골드가 부족합니다.");
        }

        // 구매 완료 후 계속 진행할 수 있도록 안내 메시지를 출력합니다.
        Console.WriteLine("계속하려면 아무 키나 누르세요...");
        Console.ReadKey();
    }

    // 던전에 진입하는 함수입니다.
    static void EnterDungeon()
    {
        Console.Clear(); // 화면을 지웁니다.
        Console.WriteLine("던전에 진입합니다...");

        // 적의 수를 랜덤으로 결정합니다. (2~3명 사이)
        int enemiesCount = random.Next(2, 4);
        List<Enemy> enemies = new List<Enemy>();

        // 적의 이름 목록입니다.
        string[] enemyNames = { "김록기 매니저", "안혜린 매니저", "강채린 매니저" };

        // 적을 생성하여 리스트에 추가합니다.
        for (int i = 0; i < enemiesCount; i++)
        {
            string enemyName = enemyNames[random.Next(enemyNames.Length)];
            int originalDefense = random.Next(2, 10);
            int halvedDefense = originalDefense / 2;

            enemies.Add(new Enemy
            {
                Name = enemyName,
                Health = random.Next(25, 51),
                Attack = random.Next(5, 16),
                Defense = halvedDefense,
                AsciiArt = enemyAsciiArt[enemyName]
            });
        }

        // 각 적과의 전투를 처리합니다.
        foreach (var enemy in enemies)
        {
            Console.Clear(); // 화면을 지우고 적의 정보를 출력합니다.
            Console.WriteLine($"\n야생의 {enemy.Name}가 나타났습니다!");
            Console.WriteLine(enemy.AsciiArt); // 적의 ASCII 아트를 출력합니다.

            // 적과 플레이어 간의 전투를 진행합니다.
            while (enemy.Health > 0 && playerCharacter?.Health > 0)
            {
                Console.WriteLine($"\n{enemy.Name} - 체력: {enemy.Health}");
                Console.WriteLine($"{playerCharacter?.Name} - 체력: {playerCharacter?.Health}");
                Console.WriteLine("1. 공격");
                Console.WriteLine("2. 도망");
                Console.Write("행동을 선택하세요: ");
                string? action = Console.ReadLine();

                if (action == "1")
                {
                    // 플레이어가 적을 공격합니다.
                    int damageToEnemy = Math.Max(1, playerCharacter?.Attack - enemy.Defense ?? 0);
                    enemy.Health -= damageToEnemy;
                    Console.WriteLine($"당신은 {enemy.Name}에게 {damageToEnemy} 피해를 입혔습니다!");

                    // 적이 살아있다면 반격합니다.
                    if (enemy.Health > 0)
                    {
                        int damageToPlayer = Math.Max(1, enemy.Attack - playerCharacter?.Defense ?? 0);
                        playerCharacter.Health -= damageToPlayer;
                        Console.WriteLine($"{enemy.Name}이(가) 당신에게 {damageToPlayer} 피해를 입혔습니다!");
                    }
                }
                else if (action == "2")
                {
                    // 플레이어가 도망을 선택하면 마을로 돌아갑니다.
                    Console.WriteLine("전투에서 도망쳤습니다!");
                    EnterTown();
                    return;
                }
                else
                {
                    // 잘못된 입력이 있을 경우 메시지를 출력합니다.
                    Console.WriteLine("잘못된 입력입니다. 다시 시도하세요.");
                }
            }

            // 플레이어가 패배한 경우 게임 오버 처리합니다.
            if (playerCharacter?.Health <= 0)
            {
                Console.WriteLine("전투에서 패배했습니다...");
                GameOver();
                return;
            }

            // 적을 처치한 경우 보상을 줍니다.
            Console.WriteLine($"{enemy.Name} : 잘하고 계십니다! 캠프 졸업하는 그날까지 파이팅!");
            Thread.Sleep(2000);
            Console.WriteLine($"{enemy.Name}을(를) 처치했습니다!");
            int goldReward = random.Next(300, 601);
            playerCharacter.Gold += goldReward;
            Console.WriteLine($"{goldReward} 골드를 획득했습니다.");

        }

        // 던전을 클리어한 경우 레벨업 처리하고 마을로 돌아갑니다.
        Console.WriteLine("던전을 클리어했습니다!");
        LevelUp();
        EnterTown();
    }

    // 레벨업 처리를 하는 함수입니다.
    static void LevelUp()
    {
        if (playerCharacter == null) return; // 플레이어 캐릭터가 없으면 종료합니다.

        // 레벨을 1 증가시킵니다.
        playerCharacter.Level += 1;
        // 공격력과 방어력을 증가시킵니다.
        playerCharacter.Attack += 1;
        playerCharacter.Defense += 2;

        // 레벨업 메시지를 출력합니다.
        Console.WriteLine($"\n--- 레벨 업! ---");
        Console.WriteLine($"{playerCharacter.Name}의 레벨이 {playerCharacter.Level}이(가) 되었습니다.");
        Console.WriteLine($"공격력: {playerCharacter.Attack}, 방어력: {playerCharacter.Defense}");
        Console.WriteLine("계속하려면 아무 키나 누르세요...");
        Console.ReadKey(); // 사용자가 아무 키나 누를 때까지 대기합니다.
    }

    // 게임 오버 처리를 하는 함수입니다.
    static void GameOver()
    {
        Console.Clear(); // 화면을 지웁니다.
        // 게임 오버 메시지를 출력합니다.
        Console.WriteLine("게임 오버! 전투에서 패배했습니다.");
        Console.WriteLine("메인 메뉴로 돌아가려면 아무 키나 누르세요...");
        Console.ReadKey(); // 사용자가 키를 누르면 메인 메뉴로 돌아갑니다.
        ShowMenu();
    }

    // 인벤토리를 여는 함수입니다.
    static void OpenInventory()
    {
        while (true)
        {
            Console.Clear(); // 화면을 지웁니다.
            // 인벤토리가 비어 있을 경우 메시지를 출력합니다.
            if (playerCharacter?.Inventory.Count == 0)
            {
                Console.WriteLine("인벤토리가 비어 있습니다.");
                break;
            }

            // 인벤토리 내용을 출력합니다.
            Console.WriteLine("인벤토리:");
            for (int i = 0; i < playerCharacter?.Inventory.Count; i++)
            {
                var item = playerCharacter.Inventory[i];
                string equippedIndicator = "";

                // 장착된 아이템이면 [E] 표시
                if (item == playerCharacter.EquippedWeapon || item == playerCharacter.EquippedArmor)
                {
                    equippedIndicator = " [E]";
                }

                // 아이템 이름과 장착 여부를 출력합니다.
                Console.WriteLine($"{i + 1}. {item.Name} ({item.Type}){equippedIndicator}");
            }

            Console.WriteLine($"{playerCharacter?.Inventory.Count + 1}. 인벤토리 나가기");

            // 장착할 아이템을 선택하도록 유도합니다.
            Console.Write("\n장착/해제할 아이템 번호를 선택하세요: ");
            string? choice = Console.ReadLine();
            int index;

            // 유효한 숫자인지 확인하고 처리합니다.
            if (int.TryParse(choice, out index))
            {
                if (index >= 1 && index <= playerCharacter?.Inventory.Count)
                {
                    var selectedItem = playerCharacter?.Inventory[index - 1];
                    if (selectedItem != null) EquipOrUnequipItem(selectedItem); // 선택한 아이템을 장착/해제
                }
                else if (index == playerCharacter?.Inventory.Count + 1)
                {
                    // 인벤토리 나가기 옵션을 선택한 경우 종료
                    break;
                }
                else
                {
                    // 잘못된 입력일 경우 메시지를 출력합니다.
                    Console.WriteLine("잘못된 입력입니다. 다시 시도하세요.");
                }
            }
            else
            {
                // 잘못된 입력일 경우 메시지를 출력합니다.
                Console.WriteLine("잘못된 입력입니다. 다시 시도하세요.");
            }
        }
    }

    // 아이템을 장착하거나 해제하는 함수입니다.
    static void EquipOrUnequipItem(Item item)
    {
        if (playerCharacter == null) return; // 캐릭터가 없으면 종료

        // 아이템이 무기일 경우 처리
        if (item.Type == "무기")
        {
            if (playerCharacter.EquippedWeapon == item)
            {
                // 이미 장착된 무기일 경우 해제
                Console.WriteLine($"{item.Name}을(를) 장착 해제했습니다.");
                playerCharacter.Attack -= item.Power ?? 0;
                playerCharacter.EquippedWeapon = null;
            }
            else
            {
                // 다른 무기를 장착 중이면 해제 후 새로운 무기 장착
                if (playerCharacter.EquippedWeapon != null)
                {
                    Console.WriteLine($"{playerCharacter.EquippedWeapon.Name}을(를) 장착 해제했습니다.");
                    playerCharacter.Attack -= playerCharacter.EquippedWeapon.Power ?? 0;
                }

                Console.WriteLine($"{item.Name}을(를) 장착했습니다.");
                playerCharacter.EquippedWeapon = item;
                playerCharacter.Attack += item.Power ?? 0;
            }
        }
        // 아이템이 방어구일 경우 처리
        else if (item.Type == "방어구")
        {
            if (playerCharacter.EquippedArmor == item)
            {
                // 이미 장착된 방어구일 경우 해제
                Console.WriteLine($"{item.Name}을(를) 장착 해제했습니다.");
                playerCharacter.Defense -= item.Power ?? 0;
                playerCharacter.EquippedArmor = null;
            }
            else
            {
                // 다른 방어구를 장착 중이면 해제 후 새로운 방어구 장착
                if (playerCharacter.EquippedArmor != null)
                {
                    Console.WriteLine($"{playerCharacter.EquippedArmor.Name}을(를) 장착 해제했습니다.");
                    playerCharacter.Defense -= playerCharacter.EquippedArmor.Power ?? 0;
                }

                Console.WriteLine($"{item.Name}을(를) 장착했습니다.");
                playerCharacter.EquippedArmor = item;
                playerCharacter.Defense += item.Power ?? 0;
            }
        }

        // 장착/해제 완료 후 메시지 출력
        Console.WriteLine("계속하려면 아무 키나 누르세요...");
        Console.ReadKey();
    }


    // 캐릭터를 휴식하게 하는 함수입니다.
    static void CaracterRest()
    {
        // 체력을 최대치로 회복합니다.
        playerCharacter.Health = 100;
        Console.WriteLine($"{playerCharacter.Name}의 체력이 {playerCharacter.Health}로 회복되었습니다.");
    }


    // 상점에서 판매할 아이템을 초기화하는 함수입니다.
    static void InitializeShop()
    {
        // 상점에 무기 아이템 추가
        shopItems.Add(new Item { Name = "스파르타의 마우스", Type = "무기", Price = 2500, Power = 7 });
        shopItems.Add(new Item { Name = "낡은 키보드", Type = "무기", Price = 600, Power = 2 });
        shopItems.Add(new Item { Name = "청동 마우스", Type = "무기", Price = 1500, Power = 5 });
        shopItems.Add(new Item { Name = "금단의 챗지피티", Type = "무기", Price = 5000, Power = 50 });

        // 상점에 방어구 아이템 추가
        shopItems.Add(new Item { Name = "스파르타의 손목보호대", Type = "방어구", Price = 3000, Power = 9 });
        shopItems.Add(new Item { Name = "낡은 마우스패드", Type = "방어구", Price = 800, Power = 4 });
        shopItems.Add(new Item { Name = "청동 등받이쿠션", Type = "방어구", Price = 1700, Power = 6 });
    }

    // 게임을 종료하는 함수입니다.
    static void ExitGame()
    {
        Console.Clear(); // 화면을 지우고 종료 메시지를 출력합니다.
        Console.WriteLine("게임을 종료합니다. 안녕히 가세요!");
        Environment.Exit(0); // 프로그램을 종료합니다.
    }
}
