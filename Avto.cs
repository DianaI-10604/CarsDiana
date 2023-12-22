using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cars
{
    class Avto
    {
        private string? carNumber;                  //номер машины
        private double maxFuelVolume;               //максимальный объем бака
        private double fuelVolume;                  //текущий объем бака
        private double rashod;                      //расход бака на 100 км
        private double fullTime = 0;                //все время в пути
        private double distance;                    //все расстояние, которое необходимо проехать
        private double distanceTravelled = 0;       //пройденное расстояние
        private double canDrive;                    //можем проехать при текущем объеме бака
        private double coordinatesDistance = 0;     //все расстояние по координатам
        private double startProbeg;                 //начальный пробег
        private double fullProbeg;                  //итоговый пробег, который должен становиться начальным, если машина проехала весь путь
        private double speed;                       //скорость машины
        public Avto[] cars = new Avto[3];           //массив машинЫ

        public Avto(string? carNumber, double maxFuelVolume, double fuelVolume, double distance, double rashod, double startProbeg, double fullProbeg)
        {
            this.carNumber = carNumber;
            this.maxFuelVolume = maxFuelVolume;
            this.fuelVolume = fuelVolume;
            this.distance = distance;
            this.rashod = rashod;
            this.startProbeg = startProbeg;
            this.fullProbeg = fullProbeg;
        }

        public void Move(Avto[] cars) //ЕЗДА
        {
            Random rnd = new Random();

            bool continueMove = true;
            while (continueMove) //будем выбирать машины до тех пор, пока cotinueMove не станет False
            {
                CarChioice(cars);
                int choice = Convert.ToInt32(Console.ReadLine()); //выбираем машину для езды

                if (choice == 4) //то есть "Выйти"
                {
                    Console.Clear();
                    Environment.Exit(0); //выходим
                }
                else
                {
                    int i = choice - 1;

                    Console.Clear(); //очистили консоль и вывели информацию о выбранной машине

                    CarsOutput(cars, i); //вывели информацию о выбранной машине
                    Console.WriteLine();

                    if (cars[i].startProbeg < cars[i].fullProbeg && cars[i].distanceTravelled < cars[i].distance)  //Если мы выбрали машину, которая не проехала весь свой путь
                    {
                        Console.WriteLine($"Вы проехали {Math.Round(cars[i].distanceTravelled, 3)}/{cars[i].distance}. Желаете продолжить ехать?\n1. Да\n2. Нет");
                        int user_choice = Convert.ToInt32(Console.ReadLine());

                        switch (user_choice)
                        {
                            case 1:
                                FuelFilling(cars, i);  //заправляемся и едем
                                drive(cars, i);
                                break;
                            case 2:
                                Console.WriteLine("Вы отказались ехать дальше");  //запускаем цикл езды заново
                                Console.WriteLine();
                                CarsOutput(cars, i);
                                Move(cars);
                                break;
                        }
                    }

                    else if (cars[i].distanceTravelled == cars[i].distance) //если машина уже проехала расстояние
                    {
                        Console.WriteLine("Вы проехали необходимое расстояние. Желаете ехать дальше?\n1. Да\n2. Нет");
                        int user_choice = Convert.ToInt32(Console.ReadLine());

                        switch (user_choice)
                        {
                            case 1:
                                cars[i].distance = rnd.Next(1, 3000);  //заново генерируем расстояние
                                cars[i].distanceTravelled = 0;

                                Console.WriteLine($"Необходимо проехать: {cars[i].distance} км");
                                drive(cars, i);  //заново запускаем езду
                                break;

                            case 2:
                                Move(cars);  //заново запускаем езду и выбираем машину
                                break;
                        }
                    }

                    else
                    {
                        drive(cars, i); //если машина впервые едет, то просто запускаем езду
                    }
                }
            }

            void drive(Avto[] cars, int i)  //МЕТОД ЕЗДА ВНУТРИ ЕЗДЫ
            {
                Random rnd = new Random();

                while (cars[i].distance > cars[i].distanceTravelled)  //пока расстояние больше чем пройденное 
                {
                    cars[i].speed = rnd.Next(60, 90);  //задаем случайную скорость

                    //Console.WriteLine($"Ваша скорость: {cars[i].speed} км/ч");

                    while (cars[i].fuelVolume == 0)  //проверка на дурака
                    {
                        Console.WriteLine("Ваш бак пуст. Заправляемся");
                        cars[i].fuelVolume = FuelFilling(cars, i);

                        Console.WriteLine();
                        CarsOutput(cars, i);

                        Console.WriteLine();
                    }

                    ///
                    Console.WriteLine($"Необходимо проехать: {cars[i].distance - cars[i].distanceTravelled} км");

                    cars[i].canDrive = (cars[i].fuelVolume / cars[i].rashod) * 100;  //сколько км можем проехать 
                    Console.WriteLine($"При объеме бака {cars[i].fuelVolume} л вы можеет проехать {Math.Round(cars[i].canDrive, 3)} км");
                    Console.WriteLine("(Формула расчета возможного расстояния: (объем бака / расход) * 100)\n");

                    Console.Write("Нажмите любую клавишу, чтобы ехать: ");
                    Console.ReadKey();
                    Console.Clear();

                    speedUpDown(cars, i);  //метод изменения скорости во время езды

                    if (cars[i].distance - (cars[i].distanceTravelled + cars[i].canDrive) <= 0) //если щас проедем и это будет больше необходимого расстояния
                    {
                        cars[i].canDrive = cars[i].distance - cars[i].distanceTravelled; //потому что в конце цикла мы выполняем обновление пробега через canDrive
                        cars[i].distanceTravelled = cars[i].distance;  //приравниваем пройденное расстояние ко всему расстоянию            
                        cars[i].fuelVolume = 0;  //бак пустой

                        Probeg(cars, i);
                        Console.WriteLine($"\nВы проехали {cars[i].distanceTravelled}/{cars[i].distance} км");
                        Console.WriteLine($"Новое значение пробега: {cars[i].fullProbeg} км\n");
                    }

                    else  //если щас проедем и это будет меньше всего расстояния
                    {
                        cars[i].distanceTravelled += cars[i].canDrive;  //обновили пройденное расстояние
                        cars[i].fuelVolume = 0;   //обновили значение объема бака

                        Probeg(cars, i);
                        Console.WriteLine($"\nВы проехали {cars[i].distanceTravelled}/{cars[i].distance} км");
                        Console.WriteLine($"Новое значение пробега: {cars[i].fullProbeg} км\n");

                        Console.WriteLine("Ваш бак пуст. Желаете заправиться?\n1. Да\n2. Нет");
                        int choice = Convert.ToInt32(Console.ReadLine());

                        switch (choice)
                        {
                            case 1:
                                cars[i].fuelVolume = FuelFilling(cars, i); //залили бензин и едем дальше
                                break;
                            case 2:
                                Console.WriteLine("Вы отказались заправляться. Выводим информацию о машине: ");
                                CarsOutput(cars, i);

                                Console.Write("\nНажмите любую клавишу, чтобы выбрать машину: ");
                                Console.ReadKey();
                                Console.Clear();

                                Move(cars);
                                break;
                        }
                    }

                    time(cars, i);
                }

                cars[i].startProbeg = cars[i].fullProbeg; //чтобы при следующей поездке начальным проегом был уже пройденный

                Console.WriteLine();
                CarsOutput(cars, i);

                Console.WriteLine("\nВы проехали необходимое расстояние! Желаете ехать дальше?\n1. Да\n2. Нет");
                int user_choice = Convert.ToInt32(Console.ReadLine());

                switch (user_choice)
                {
                    case 1:
                        Console.Clear();

                        cars[i].distance = rnd.Next(1, 3000);  //заново генерируем расстояние
                        cars[i].distanceTravelled = 0;

                        Console.WriteLine();
                        CarsOutput(cars, i);

                        Console.WriteLine("Ваш бак пуст. Перед тем как ехать нужно заправиться.");
                        cars[i].fuelVolume = FuelFilling(cars, i);

                        Console.WriteLine($"Теперь необходимо проехать: {cars[i].distance} км\n");
                        drive(cars, i);  //заново запускаем езду
                        break;

                    case 2:
                        Console.Clear();
                        Move(cars);  //заново запускаем езду и выбираем машину
                        break;
                }
            }
        }

        private void Probeg(Avto[] cars, int i)  //расчет пробега
        {
            cars[i].fullProbeg += cars[i].canDrive;
        }

        private void time(Avto[] cars, int i)  //считаем все время в пути
        {
            cars[i].fullTime += cars[i].canDrive / cars[i].speed;
        }

        private void speedUpDown(Avto[] cars, int i)  //изменение скорости
        {
            Console.WriteLine($"Вы движетесь со скоростью {cars[i].speed} км/ч");
            Console.WriteLine($"Выберите действие: \n1. Ускориться\n2. Притормозить\n3. Двигаться с текущей скоростью");

            int user_choice = Convert.ToInt32(Console.ReadLine());

            switch (user_choice)
            {
                case 1:
                    Console.Write($"Скорость {cars[i].speed} ->");
                    cars[i].speed += 20;
                    Console.Write($" {cars[i].speed} км/ч");
                    break;
                case 2:
                    Console.Write($"Скорость {cars[i].speed} ->");
                    cars[i].speed -= 20;
                    Console.Write($" {cars[i].speed} км/ч");
                    break;
                case 3:
                    Console.WriteLine($"Скорость {cars[i].speed} км/ч");
                    break;
            }

            Console.WriteLine();
        }

        private double FuelFilling(Avto[] cars, int i)  //заправка
        {
            Console.Write($"Сколько литров залить? (максимум {cars[i].maxFuelVolume}): ");
            double add = Convert.ToDouble(Console.ReadLine());

            while (add <= 0 || add > cars[i].maxFuelVolume) //проверка на дурака
            {
                Console.Write("Введите правильное значение: ");
                add = Convert.ToDouble(Console.ReadLine());
            }

            cars[i].fuelVolume = add;
            return cars[i].fuelVolume;
        }

        private void Coordinates(double CanDrive) //считаем перемещение по координатам
        {
            coordinatesDistance += CanDrive * 3;  //один км пути = 3 пунктам по оси X
        }

        private void DriveTime(double speed, double CanDrive, ref double fullTime) //время ы пути
        {
            fullTime = CanDrive / speed;   //время = расстояние / скорость
        }

        private void CarChioice(Avto[] cars) //вывод машин для выбора
        {
            Console.WriteLine("Выберите машину: ");
            for (int i = 0; i < cars.Length; i++)
            {
                Console.WriteLine($"{i + 1}. Машина {cars[i].carNumber}");  //обращаемся к машине по ее номеру
            }
            Console.WriteLine("4. Выйти");
        }
        public void CarsOutput(Avto[] cars, int i)  //вывод информации о всех машинах
        {
            Console.WriteLine($"Номер машины {cars[i].carNumber}");
            Console.WriteLine($"Максимальный объем бака: {cars[i].maxFuelVolume}л ");
            Console.WriteLine($"Текущий объем бака: {cars[i].fuelVolume} л");
            Console.WriteLine($"Расход: {cars[i].rashod} л/100км");
            Console.WriteLine($"Пробег: {cars[i].fullProbeg} км");
            Console.WriteLine($"Нужно проехать: {cars[i].distance} км");
            Console.WriteLine($"Времени в пути: {Math.Round(cars[i].fullTime, 3)} ч");
        }

    }
}


