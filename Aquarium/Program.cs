using System;
using System.Diagnostics;

namespace AquariumLive
{
    internal class Program
    {
        static void Main()
        {
            Console.CursorVisible = false;
            Aquarium aquarium = new Aquarium();
            aquarium.StartLife();
        }
    }

    class Fish
    {
        private readonly int _maxAge;
        private readonly ConsoleColor _color;

        private int _currentAge;

        public Fish(int maxAge, ConsoleColor color, int id)
        {
            _maxAge = maxAge;
            _color = color;
            Id = id;
        }

        private string Status => IsAlive ? "Жива" : "Умерла";
        private bool IsAlive => _currentAge <= _maxAge;
        public int Id { get; }

        public void AddUnitAge()
        {
            _currentAge++;
        }

        public void ShowInfo()
        {
            ConsoleColor defaultColor = ConsoleColor.White;

            Console.ForegroundColor = _color;

            Console.WriteLine(
                $"|Рыбка под номером {Id,5}|Время жизни  {_currentAge,5}|Статус рыбки {Status,5}|");
            Console.ForegroundColor = defaultColor;
        }
    }

    class Aquarium
    {
        private List<Fish> _fishes = new List<Fish>();
        private int _maxFishesCount = 10;
        private CreatorFish _creatorFish = new CreatorFish();

        public Aquarium()
        {
            for (int i = 0; i < _maxFishesCount; i++)
            {
                _fishes.Add(_creatorFish.Create());
            }
        }

        public void StartLife()
        {
            const ConsoleKey CommandAddFish = ConsoleKey.NumPad1;
            const ConsoleKey CommandDeleteFish = ConsoleKey.Delete;
            const ConsoleKey CommandExitProgram = ConsoleKey.Escape;
            const int PositionTopMenuInfo = 15;
            
            int deltaTime = 0;
            int growTime = 100;
            const int waitingTime = 1;
            bool isWork = true;


            while (isWork)
            {
                Console.SetCursorPosition(0, PositionTopMenuInfo);
                Console.WriteLine(
                    $"| [{CommandAddFish}] Добавить рыбку  |\n| [{CommandDeleteFish}] Удалить рыбку |\n| [{CommandExitProgram}] Выйти из Программы |");

                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;

                    switch (key)
                    {
                        case CommandAddFish:
                            AddFish();
                            break;

                        case CommandDeleteFish:
                            DeleteFish();
                            break;

                        case CommandExitProgram:
                            isWork = false;
                            break;
                    }
                }

                Console.SetCursorPosition(0, 0);
                ShowFishesInfo();

                Thread.Sleep(waitingTime);

                if (deltaTime == growTime)
                {
                    Console.Clear();
                    deltaTime = 0;
                    GrowOld();
                }
                else
                {
                    deltaTime++;
                }
            }
        }

        private void AddFish()
        {
            if (_maxFishesCount > _fishes.Count)
            {
                _fishes.Add(_creatorFish.Create());
                Console.WriteLine("вы добавили рыбку");
            }
            else
            {
                Console.WriteLine("В аквариуме максимальное количество рыб");
            }
        }

        private void DeleteFish()
        {
            Console.WriteLine("введите номер рыбки чтобы ее удалить");
            int id = GetId();

            for (int i = 0; i < _fishes.Count; i++)
            {
                if (_fishes[i].Id == id)
                {
                    _fishes.Remove(_fishes[i]);
                }
            }
        }

        private int GetId()
        {
            int result;

            while (int.TryParse(Console.ReadLine(), out result) == false && result > 0)
            {
                Console.WriteLine("неверный ввод");
            }

            return result;
        }

        private void GrowOld() //&
        {
            foreach (Fish fish in _fishes)
            {
                fish.AddUnitAge();
            }
        }

        private void ShowFishesInfo()
        {
            foreach (Fish fish in _fishes)
            {
                fish.ShowInfo();
            }
        }
    }

    class CreatorFish
    {
        private int _id;

        public Fish Create()
        {
            Random random = new Random();

            int maxAge = 60;
            int minAge = 10;
            int maxFishAge = random.Next(minAge, maxAge + 1);

            return new Fish(maxFishAge, (ConsoleColor)random.Next(1, Enum.GetValues(typeof(ConsoleColor)).Length),
                _id++);
        }
    }
}