using System;
using System.Collections.Generic;
using System.Text;
using GTANetworkAPI;
using Server.model;

namespace Server.utils
{
    public class Track
    {
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
        public static void UpdateTrackRecordByTrackId(Player player, int trackid, int score)
        {
            foreach (var track in Main.Players1[player].TracksRecords)//перебираем List рекордов
            {
                if (track.TrackId == trackid)
                {
                    track.Score = score;
                    MySql.Query($"UPDATE `tracksrecords` SET `Score` = '{track.Score}' WHERE `TrackId` = '{track.TrackId}' AND `CharacterId` = '{Main.Players1[player].Character.Id}'");
                    break;
                }
            }
            
        }
        public static int  GetPlayerTrackRecordByTrackId(Player player, int trackid)
        {
            foreach (var track in Main.Players1[player].TracksRecords)//перебираем List рекордов
            {
                if (track.TrackId == trackid)
                {
                    return track.Score;
                }
            }
            return -1;
        }
        public static void InsertTrackRecordByTrackId(Player player, int trackid)
        {
            TracksRecords records = new TracksRecords();
            records.CharacterId = Main.Players1[player].Character.Id;
            records.TrackId = trackid;
            records.Score = 0;
            records.Id = records.Insert();
            Main.Players1[player].TracksRecords.Add(records);
        }
    }
}
