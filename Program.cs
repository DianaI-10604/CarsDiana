using System;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            Avto avto = new Avto("", 0, 0, 0, 0, 0, 0);  //связали классы
            Avto[] cars = new Avto[3];                   //массив машин
            Random rnd = new Random();

            for (int i = 0; i < cars.Length; i++)
            {
                Console.WriteLine($"Машина {i + 1}");
                Console.Write("Введите номер машины: ");
                string? carNumber = Console.ReadLine();

                Console.Write("Введите максимальный объем бака: ");
                double maxFuelVolume = Convert.ToDouble(Console.ReadLine());

                while (maxFuelVolume <= 0) //проверка на дурака
                {
                    Console.Write("Введите заново: ");
                    maxFuelVolume = Convert.ToDouble(Console.ReadLine());
                }

                Console.Write("Введите текущий обьем бака: ");
                double fuelVolume = Convert.ToDouble(Console.ReadLine());

                while (fuelVolume > maxFuelVolume) //проверка на дурака
                {
                    Console.Write("Объем бензина в баке не может превышать объем бака. Введите заново: ");
                    fuelVolume = Convert.ToDouble(Console.ReadLine());
                }

                double distance = rnd.Next(1, 3000);

                Console.Write("Введите расход бака (л/100 км): ");
                double rashod = Convert.ToDouble(Console.ReadLine());

                while (rashod == 0 || rashod >= maxFuelVolume || rashod >= 10)
                {
                    Console.Write("Введите верное значение расхода: ");
                    rashod = Convert.ToDouble(Console.ReadLine());
                }

                Console.Write("Введите начальный пробег: ");
                double startProbeg = Convert.ToDouble(Console.ReadLine());

                double fullProbeg = startProbeg;

                Console.WriteLine();

                cars[i] = new Avto(carNumber, maxFuelVolume, fuelVolume, distance, rashod, startProbeg, fullProbeg);
            }

            avto.Move(cars);
        }
    }
}


