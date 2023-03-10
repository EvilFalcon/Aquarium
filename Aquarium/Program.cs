﻿using System;
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
        private int _currentAge;
        private int _maxAge;
        private ConsoleColor _color;
        private static int _ids = 0; //?
        private int _idCount = 1;

        public Fish(int maxSecondTimeLife, ConsoleColor color)
        {
            _maxAge = maxSecondTimeLife;
            _color = color;
            UniqueNumber = ++_ids;
        }

        public bool IsALife => _currentAge <= _maxAge;
        public int UniqueNumber { get; }

        public void AddUnitAge()
        {
            _currentAge++;
        }

        public void ShowInfo()
        {
            ConsoleColor defaultColor = ConsoleColor.White;
            string status = "";

            if (IsALife)
            {
                status = "жива";
            }
            else
            {
                status = "Умерла";
            }

            Console.ForegroundColor = _color;

            Console.WriteLine($"|Рыбка под номером {UniqueNumber}|Время жизни  {_currentAge}|Статус рыбки {status}|");
            Console.ForegroundColor = defaultColor;
        }
    }

    class Aquarium
    {
        private List<Fish> _fishes = new List<Fish>();
        private int _maxCountFishe = 10;
        private CreatorFish _creatorFish = new CreatorFish();

        public Aquarium()
        {
            for (int i = 0; i < _maxCountFishe; i++)
            {
                _fishes.Add(_creatorFish.Creator());
            }
        }

        public void StartLife()
        {
            int delitaTime = 0;
            int oneSecond = 100;
            int oneMiliseconds = 1;
            int menuInfoPosition = 15;
            bool isWork = true;

            const ConsoleKey CommandAddFish = ConsoleKey.NumPad1;
            const ConsoleKey CommandDeleteFish = ConsoleKey.Delete;
            const ConsoleKey CommandExitProgram = ConsoleKey.Escape;

            ShowInfoFishes();

            while (isWork)
            {
                Console.SetCursorPosition(0, menuInfoPosition);
                Console.WriteLine(
                    $"| [{CommandAddFish}] Добавить рыбку  \n| [{CommandDeleteFish}] Удалить рыбку \n| [{CommandExitProgram}] Выйти из Программы |");
                
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

                    Console.SetCursorPosition(0, 0);
                }
                
                if (delitaTime == oneSecond)
                {
                    Console.Clear();
                    ShowInfoFishes();
                    delitaTime = 0;
                    UnitAge();
                }
                else
                {
                    ++delitaTime;
                }

                Thread.Sleep(oneMiliseconds);
            }
        }

        private void AddFish()
        {
            if (_maxCountFishe > _fishes.Count)
            {
                _fishes.Add(_creatorFish.Creator());
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
                if (_fishes[i].UniqueNumber == id)
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

        private void UnitAge()
        {
            foreach (Fish fish in _fishes)
            {
                fish.AddUnitAge();
            }
        }

        private void ShowInfoFishes()
        {
            foreach (Fish fish in _fishes)
            {
                fish.ShowInfo();
            }
        }
    }

    class CreatorFish
    {
        public Fish Creator()
        {
            Random random = new Random();

            List<ConsoleColor> _color = new List<ConsoleColor>()
            {
                ConsoleColor.Blue, ConsoleColor.Magenta, ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.DarkYellow
            };
            int maxSecondTimeLife = 60;
            int minSecondTimeLife = 10;
            int _secondTimeLife = random.Next(minSecondTimeLife, maxSecondTimeLife + 1);

            return new Fish(_secondTimeLife, _color[random.Next(_color.Count)]);
        }
    }
}