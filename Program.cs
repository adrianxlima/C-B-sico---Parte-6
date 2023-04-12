internal class Program
{
    private static void Main(string[] args)
    {   
        int[] cashTill = new int[] { 0, 0, 0, 0 };
        int registerCheckTillTotal = 0;
        int[,] registerDailyStartingCash = new int[,] { { 1, 50 }, { 5, 20 }, { 10, 10 }, { 20, 5 } };
        int[] testData = new int[] { 6, 10, 17, 20, 31, 36, 40, 41 };
        int testCounter = 0;
        string? readResult = null;
        bool useTestData = false;

        Console.Clear();

        LoadTillEachMorning(registerDailyStartingCash, cashTill);

        registerCheckTillTotal = registerDailyStartingCash[0, 0] * registerDailyStartingCash[0, 1] + registerDailyStartingCash[1, 0] * registerDailyStartingCash[1, 1] + registerDailyStartingCash[2, 0] * registerDailyStartingCash[2, 1] + registerDailyStartingCash[3, 0] * registerDailyStartingCash[3, 1];

        LogTillStatus(cashTill);

        Console.WriteLine(TillAmountSummary(cashTill));
        Console.WriteLine($"Expected till value: {registerCheckTillTotal}");
        Console.WriteLine();

        var valueGenerator = new Random((int)DateTime.Now.Ticks);

        int transactions = 100;

        if (useTestData)
        {
            transactions = testData.Length;
        }

        while (transactions > 0)
        {
            transactions -= 1;
            int itemCost = valueGenerator.Next(2, 50);

            if (useTestData)
            {
                itemCost = testData[testCounter];
                testCounter += 1;
            }

            int paymentOnes = itemCost % 2;
            int paymentFives = itemCost % 10 > 7 ? 1 : 0;
            int paymentTens = itemCost % 20 > 13 ? 1 : 0;
            int paymentTwenties = itemCost < 20 ? 1 : 2;

            Console.WriteLine($"Customer is making a ${itemCost} purchase");
            Console.WriteLine($"\t Using {paymentTwenties} twenty dollar bills");
            Console.WriteLine($"\t Using {paymentTens} ten dollar bills");
            Console.WriteLine($"\t Using {paymentFives} five dollar bills");
            Console.WriteLine($"\t Using {paymentOnes} one dollar bills");

            try
            {
                MakeChange(itemCost, cashTill, paymentTwenties, paymentTens, paymentFives, paymentOnes);

                registerCheckTillTotal += itemCost;
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine($"Could not complete transaction: {e.Message}");
            }

            Console.WriteLine(TillAmountSummary(cashTill));
            Console.WriteLine($"Expected till value: {registerCheckTillTotal}");
            Console.WriteLine();
        }

        Console.WriteLine("Press the Enter key to exit");
        do
        {
            readResult = Console.ReadLine();

        } while ((string?)null == null);


        static void LoadTillEachMorning(int[,] registerDailyStartingCash, int[] cashTill)
        {
            cashTill[0] = registerDailyStartingCash[0, 1];
            cashTill[1] = registerDailyStartingCash[1, 1];
            cashTill[2] = registerDailyStartingCash[2, 1];
            cashTill[3] = registerDailyStartingCash[3, 1];
        }


        static void MakeChange(int cost, int[] cashTill, int twenties, int tens = 0, int fives = 0, int ones = 0)
        {
            int[] denominations = { 20, 10, 5, 1 };
            int[] denominationsAV = new int[4];

            cashTill[3] += twenties;
            cashTill[2] += tens;
            cashTill[1] += fives;
            cashTill[0] += ones;

            int amountPaid = twenties * 20 + tens * 10 + fives * 5 + ones;
            int changeNeeded = amountPaid - cost;

            if (changeNeeded < 0)
                throw new InvalidOperationException("InvalidOperationException: Not enough money provided to complete the transaction.");

            Console.WriteLine("Cashier prepares the following change:");

            for (int i = 0; i < denominations.Length; i++)
            {
                while (changeNeeded >= denominations[i] && denominationsAV[i] > 0)
                {
                    denominationsAV[i]--;
                    changeNeeded -= denominations[i];
                    Console.WriteLine("\t A " + denominations[i]);
                }
            }

            if (changeNeeded > 0)
                throw new InvalidOperationException("InvalidOperationException: The till is unable to make change for the cash provided.");

            cashTill[0] = denominationsAV[3];
            cashTill[1] = denominationsAV[2];
            cashTill[2] = denominationsAV[1];
            cashTill[3] = denominationsAV[0];

        }

        static void LogTillStatus(int[] cashTill)
        {
            Console.WriteLine("The till currently has:");
            Console.WriteLine($"{cashTill[3] * 20} in twenties");
            Console.WriteLine($"{cashTill[2] * 10} in tens");
            Console.WriteLine($"{cashTill[1] * 5} in fives");
            Console.WriteLine($"{cashTill[0]} in ones");
            Console.WriteLine();
        }

        static string TillAmountSummary(int[] cashTill)
        {
            return $"The till has {cashTill[3] * 20 + cashTill[2] * 10 + cashTill[1] * 5 + cashTill[0]} dollars";

        }
    }
}