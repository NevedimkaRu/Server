using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.model
{
    public class TracksRecords : DB_Tables
    {
        public int CharacterId { get; set; }
        public int TrackId { get; set; }
        public int Score { get; set; }

        public static TracksRecords GetTrackRecordsByTrackId(Player player, int trackid)
        {
            TracksRecords model = new TracksRecords();

            foreach (var track in Main.Players1[player].TracksRecords)//перебираем List рекордов
            {
                if (track.TrackId == trackid)
                {
                    return track;
                }
            }
            return model;//Возвращаем пустую модель, если не найдено
        }
    }
}
