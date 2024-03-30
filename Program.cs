using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Diagnostics;

public class Program
{
    static List<Model> ManyDoc = new();
    static void Main()
    {
        Console.WriteLine("Target?");
        long Target = long.Parse(Console.ReadLine());

        Console.WriteLine("From?");
        long From = long.Parse(Console.ReadLine());
        long Current = From - 1;
        Console.WriteLine("------------------------------");
        long Wait = From;
        Stopwatch stopwatch = new();
        stopwatch.Start();
        while (Current++ < Target)
        {
            Cal(Current);
            if (++Wait % 100000 == 0)
            {
                Add();
                ManyDoc.Clear();
                Console.WriteLine("Finish Saving...{0}", Wait);
            }
        }
        stopwatch.Stop();
        Console.WriteLine($"Time taken: {stopwatch.Elapsed}");
    }

    static void Cal(long Max)
    {
        long Key = Max;
        long Count = 0;
        var Doc = new Model() { Printing = Key };
        do
        {
            Count++;
            if (Max % 2 == 0)
            {
                Max /= 2;
            }
            else
            {
                Max *= 3;
                Max++;
            }
            Doc.Result.Add(Max);
        } while (Max > 1);
        Doc.Count = Count;
        ManyDoc.Add(Doc);
    }

    static void Add()
    {
        new MongoClient("mongodb://localhost:27017")
           .GetDatabase("CollazConjecture")
           .GetCollection<Model>("Data").InsertMany(ManyDoc);
    }
    record Model
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; init; } = "";
        public required long Printing { get; init; } = 0;
        public List<long> Result { get; init; } = [];
        public long Count { get; set; } = 0;
    }
}
