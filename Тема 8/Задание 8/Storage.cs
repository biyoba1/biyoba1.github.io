using System;
using System.Collections.Generic;
using System.IO;

namespace Задание_8
{
    public static class Storage
    {
        private static readonly string FilePath = "results.dat";

        public static void SaveResult(GameResult result)
        {
            using (var stream = new FileStream(FilePath, FileMode.Append, FileAccess.Write))
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(result.Login);
                writer.Write(result.Date.ToBinary());
                writer.Write(result.IsWin);
                writer.Write(result.TimeSeconds);
                writer.Write(result.Difficulty);
            }
        }

        public static List<GameResult> LoadResults(string login)
        {
            var results = new List<GameResult>();
            if (!File.Exists(FilePath)) return results;

            using (var stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            using (var reader = new BinaryReader(stream))
            {
                try
                {
                    while (stream.Position < stream.Length)
                    {
                        string l = reader.ReadString();
                        long dateTicks = reader.ReadInt64();
                        bool win = reader.ReadBoolean();
                        int time = reader.ReadInt32();
                        int diff = reader.ReadInt32();

                        if (l == login)
                        {
                            results.Add(new GameResult(l, win, time, diff)
                            {
                                Date = DateTime.FromBinary(dateTicks)
                            });
                        }
                    }
                }
                catch (EndOfStreamException) { }
            }
            return results;
        }
    }
}