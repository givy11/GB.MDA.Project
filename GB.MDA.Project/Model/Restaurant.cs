using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messaging;
using Microsoft.Extensions.Configuration;

namespace GB.MDA.Project.Model
{
    public class Restaurant
    {
        private readonly List<Table> _tables = new List<Table>();
        private readonly Producer _producer = new ("BookingNotification");

        public Restaurant()
        {
            for (int i = 0; i <= 10; i++)
            {
                _tables.Add(new Table(i));
            }
        }

        public void BookFreeTable(int personCount)
        {
            Console.WriteLine("Добрый день! Подождите секунду, я подберу столик и подтвержу вашу бронь. Оставайтесь на линии");
            var table = _tables.FirstOrDefault(t => t.SeatsCount >= personCount && t.State == State.Free);
            Thread.Sleep(5000);
            table?.SetState(State.Booked);
            Console.WriteLine(table == null ? "К сожалению, все столики заняты" : $"Готово! Ваш столик {table.Id}");
        }

        public void BookFreeTableAsync(int personCount)
        {
            Console.WriteLine("Добрый день! Подождите секунду, я подберу столик и подтвержу вашу бронь. Вам придет уведомление");
            Task.Run(async () =>
                {
                    var table = _tables.FirstOrDefault(t => t.SeatsCount >= personCount && t.State == State.Free);
                    Thread.Sleep(5000);
                    table?.SetState(State.Booked);
                    _producer.Send(table == null ? "Уведомление: К сожалению, все столики заняты" : $"Уведомление: Готово! Ваш столик {table.Id}");
                }
            );
        }
    }
}
