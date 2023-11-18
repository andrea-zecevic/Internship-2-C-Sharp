using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Xml.Linq;
using System.Xml.Schema;

class Program
{
    static Dictionary<string, Tuple<int, decimal, DateTime>> articlesDictionary = new Dictionary<string, Tuple<int, decimal, DateTime>>();
    static Dictionary<string, DateTime> workersDictionary = new Dictionary<string, DateTime>();
    static Dictionary<string, int> purchaseDictionary = new Dictionary<string, int>();
    static Dictionary<int, Tuple<DateTime, List<Tuple<string, int, decimal, decimal>>>> billDictionary = new Dictionary<int, Tuple<DateTime, List<Tuple<string, int, decimal, decimal>>>>();

    static void Main()
    {
        InitializeArticles();
        InitializeWorkers();

        int generalAction;

        do
        {
            Console.WriteLine("\tDobrodošli u trgovinu!\nMolimo Vas odaberite jednu od opcija:\n\n");

            Console.WriteLine("1 - Artikli\n2 - Radnici\n3 - Računi\n4 - Statistika\n0 - Izlaz iz aplikacije");
            generalAction = int.Parse(Console.ReadLine());

            switch (generalAction)
            {
                case 1:
                    SelectArticle();
                    break;
                case 2:
                    Console.WriteLine("Radnici");
                    SelectWorker();
                    break;
                case 3:
                    Console.WriteLine("Racuni");
                    SelectBill();
                    break;
                case 4:
                    Console.WriteLine("Statistika");
                    SelectStatistic();
                    break;
                case 0:
                    Console.WriteLine("Izlaz iz aplikacije ...\n\nLijep pozdrav!");
                    break;
                default:
                    Console.WriteLine("Pogrešan unos. Pokušajte ponovno.");
                    break;
            }
        } while (generalAction != 0);
    }

    static void InitializeArticles()
    {
        articlesDictionary = new Dictionary<string, Tuple<int, decimal, DateTime>>()
        {
            {"cokolada", Tuple.Create(15, 2.99m, DateTime.Parse("2024-05-10"))},
            {"voda", Tuple.Create(30, 1.50m, DateTime.Parse("2024-12-25"))},
            {"kruh", Tuple.Create(15, 2.20m, DateTime.Parse("2023-11-18"))},
            {"jogurt", Tuple.Create(10, 0.60m, DateTime.Parse("2023-12-15"))},
        };
    }

    static void InitializeWorkers()
    {
        workersDictionary = new Dictionary<string, DateTime>()
        {
            {"Ivan Ivic", DateTime.Parse("2000-05-10")},
            {"Katarina Katic", DateTime.Parse("1995-05-10")},
            {"Ljubica Ljubic", DateTime.Parse("1970-11-11")},
            {"Marinko Ljubic", DateTime.Parse("1965-12-12")},
        };
    }

    //Artikli
    static void SelectArticle()
    {
        int articleAction;

        do
        {
            Console.WriteLine("Artikl akcije:\n");

            Console.WriteLine("1 - Unos artikla\n2 - Brisanje artikla\n3 - Uređivanje artikla\n4 - Ispis\n0 - Povratak na glavni izbornik");
            articleAction = int.Parse(Console.ReadLine());

            switch (articleAction)
            {
                case 1:
                    Console.WriteLine("Unos artikla");
                    InsertArticle();
                    break;
                case 2:
                    Console.WriteLine("Brisanje artikla");
                    DeleteArticle();
                    break;
                case 3:
                    Console.WriteLine("Uređivanje artikla");
                    ModifyArticle();
                    break;
                case 4:
                    Console.WriteLine("Ispis");
                    PrintArticles();
                    break;
                case 0:
                    Console.WriteLine("Povratak na glavni izbornik");
                    break;
                default:
                    Console.WriteLine("Pogrešan unos. Pokušajte ponovno.");
                    break;
            }
        } while (articleAction != 0);
    }

    static void InsertArticle()
    {
        int count;
        decimal price;
        DateTime expiryDate;
        bool isValidDate;

        Console.WriteLine("\nUnesite ime artikla:");
        var name = Console.ReadLine();

        do
        {
            Console.WriteLine("\nUnesite količinu:");
            count = int.Parse(Console.ReadLine());
            if (count < 0)
                Console.WriteLine("Krivi unos kolicine, pokusajte ponovno.");
        } while (count < 0);

        do
        {
            Console.WriteLine("\nUnesite cijenu:");
            price = decimal.Parse(Console.ReadLine());
            if (price < 0)
                Console.WriteLine("Krivi unos cijene, pokusajte ponovno.");
        } while (price <= 0);

        do
        {
            Console.WriteLine("\nUnesite datum isteka roka trajanja u formatu yyyy-MM-dd:");
            var userInput = Console.ReadLine();

            isValidDate = DateTime.TryParseExact(userInput, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out expiryDate);

            if (!isValidDate || expiryDate < DateTime.Now)
            {
                Console.WriteLine("Krivi unos datuma, pokusajte ponovno.");
            }
        } while (!isValidDate || expiryDate < DateTime.Now);

        articlesDictionary.Add(name, new Tuple<int, decimal, DateTime>(count, price, expiryDate));

        Console.WriteLine("Artikl uspješno dodan.\n");
    }

    static void DeleteArticle()
    {
        Console.WriteLine("Kako zelis brisati artikl?\na - Po imenu artikla\nb - Po datumu isteka roka\n");
        var actionForDelete = Console.ReadLine();

        switch (actionForDelete)
        {
            case "a":
                Console.WriteLine("Brisanje artikla po imenu. Unesi ime artikla kojeg zelis izbrisati:");
                string nameOfTheArticle = Console.ReadLine();

                if (articlesDictionary.ContainsKey(nameOfTheArticle))
                {
                    articlesDictionary.Remove(nameOfTheArticle);
                    Console.WriteLine("Uspjesno ste obrisali artikl.");
                }
                else
                {
                    Console.WriteLine("Krivi unos. Taj artikl na postoji u trgovini.\n");
                }
                break;
            case "b":
                Console.WriteLine("Brisanje artikla po datumu isteka roka.\nBrisem artikle ....\n");
                var currentDateTime = DateTime.Now;

                var keysToRemove = articlesDictionary
                    .Where(article => article.Value.Item3.Date <= currentDateTime)
                    .Select(article => article.Key).ToList();

                foreach (var key in keysToRemove)
                {
                    articlesDictionary.Remove(key);
                }

                Console.WriteLine("Artikli su obrisani.\n");
                break;
            default:
                Console.WriteLine("Pogresan unos. Povratak na izbornik.\n");
                break;
        }

    }

    static void ModifyArticle()
    {
        Console.WriteLine("Uredi artikl:\na - zaseban proizvod\nb - popust/poskupljenje na sve proizvode u trgovini\n");
        var actionForModify = Console.ReadLine();

        switch (actionForModify)
        {
            case "a":
                Console.WriteLine("Uredujete zaseban proizvod.\nUnesite ime proizvoda kojeg zelite izmijeniti: ");
                var articleToModify = Console.ReadLine();


                if (articlesDictionary.ContainsKey(articleToModify))
                {
                    ModifyTheArticleThatUserPicked(articleToModify);
                }
                else
                {
                    Console.WriteLine("Pogresan unos imena artikla. Taj artikl ne postoji. Pokusajte ponovno");
                }
                break;
            case "b":
                Console.WriteLine("Unosite popust/poskupljenje za sve proizvode.\nAko zelite popust unesite negativnu," +
                    "a ako zelite poskupljenje pozitivnu vrijednost.\n");

                var percent = decimal.Parse(Console.ReadLine());

                foreach (var article in articlesDictionary)
                {
                    var currentPrice = article.Value.Item2;
                    var newPrice = percent >= 0 ? currentPrice * (1 + (percent / 100)) : currentPrice * (1 - (Math.Abs(percent) / 100));

                    articlesDictionary[article.Key] = new Tuple<int, decimal, DateTime>(article.Value.Item1, newPrice, article.Value.Item3);
                }
                Console.WriteLine("Uspjesno ste dodali popust/poskupljenje.");

                break;
            default:
                Console.WriteLine("Pogresan unos. Povratak na izbornik...");
                break;
        }
    }

    static void ModifyTheArticleThatUserPicked(string articleToModify)
    {
        Console.WriteLine("Odaberite sto zelite izmijeniti:\n1 - Kolicina\n2 - Cijena\n3 - Datum isteka\n");
        var modifyOption = int.Parse(Console.ReadLine());

        if (modifyOption == 1)
        {
            int newValue;
            do
            {
                Console.WriteLine("Unesite novu vrijednost za kolicinu:");
                var isValidInput = int.TryParse(Console.ReadLine(), out newValue);

                if (!isValidInput || newValue < 0)
                {
                    Console.WriteLine("Krivi unos. Unesite pozitivan broj.");
                }
                else
                {
                    articlesDictionary[articleToModify] = new Tuple<int, decimal, DateTime>(newValue, articlesDictionary[articleToModify].Item2, articlesDictionary[articleToModify].Item3);
                }
            } while (newValue < 0);
            Console.WriteLine("Uspjesno ste izmjenili kolicinu.");
        }

        else if (modifyOption == 2)
        {
            decimal newValueForPrice;

            do
            {
                Console.WriteLine("Unesite novu vrijednost za cijenu:");
                var isValidInput = decimal.TryParse(Console.ReadLine(), out newValueForPrice);

                if (!isValidInput || newValueForPrice < 0)
                {
                    Console.WriteLine("Krivi unos. Unesite pozitivan broj.");
                }
                else
                {
                    articlesDictionary[articleToModify] = new Tuple<int, decimal, DateTime>(articlesDictionary[articleToModify].Item1, newValueForPrice, articlesDictionary[articleToModify].Item3);
                }
            } while (newValueForPrice <= 0);
            Console.WriteLine("Uspjesno ste izmijenili cijenu.");
        }


        else if (modifyOption == 3)
        {
            DateTime newExpiryDate;
            bool isValidInput;

            do
            {
                Console.WriteLine("Unesite novu vrijednost za datum isteka roka trajanja (u formatu yyyy-MM-dd):");
                isValidInput = DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out newExpiryDate);

                if (!isValidInput || newExpiryDate < DateTime.Now)
                {
                    Console.WriteLine("Krivi unos. Unesite datum u buducnosti u formatu yyyy-MM-dd.");
                }
                else
                {
                    articlesDictionary[articleToModify] = new Tuple<int, decimal, DateTime>(articlesDictionary[articleToModify].Item1, articlesDictionary[articleToModify].Item2, newExpiryDate);
                }

            } while (!isValidInput || newExpiryDate < DateTime.Now);

            Console.WriteLine("Uspjesno ste izmijenili datum isteka roka trajanja.\n");
        }

        else
        {
            Console.WriteLine("Pogresan unos. Povratak na izbornik.");
        }


    }

    static void PrintArticles()
    {

        Console.WriteLine("Ispisi artikle:\na - ispis svih artikala kako su spremljeni \n" +
                "b - ispis artikala sortirano po imenu\nc - ispis silazno po datumu\n" +
                "d - ispis uzlazno po datumu\ne - ispis svih artikala sortirano po kolicini\n" +
                "f - najprodavaniji artikl\ng - najmanje prodavan artikl");

        var actionForPrint = Console.ReadLine();

        switch (actionForPrint)
        {
            case "a":
                Console.WriteLine("Ispis svih artikala kako su spremljeni:");
                PrintAllArticles();
                break;
            case "b":
                Console.WriteLine("Ispis artikala sortirano po imenu:");
                PrintArticlesSortedByName();
                break;
            case "c":
                Console.WriteLine("Ispis artikala silazno po datumu:");
                PrintArticlesSortedByDateDown();
                break;
            case "d":
                Console.WriteLine("Ispis artikala uzlazno po datumu:");
                PrintArticlesSortedByDateUp();
                break;
            case "e":
                Console.WriteLine("Ispis svih artikala sortirano po kolicini:");
                PrintArticlesSortedByQuantity();
                break;
            case "f":
                Console.WriteLine("Ispis, najprodavaniji artikl:");
                PrintMostPopularArticle();
                break;
            case "g":
                Console.WriteLine("Ispis, najmanje prodavan artikl:");
                PrintLeastPopularArticle();
                break;
            default:
                Console.WriteLine("Pogresan unos. Povratak na izbornik.");
                break;
        }

    }

    static void PrintAllArticles()
    {
        foreach (var article in articlesDictionary)
        {
            Console.WriteLine($"Naziv: {article.Key}, Kolicina: {article.Value.Item1}, Cijena: {article.Value.Item2:C}, Datum isteka: {article.Value.Item3.ToShortDateString()}");
        }
    }

    static void PrintArticlesSortedByName()
    {
        foreach (var article in articlesDictionary.OrderBy(x => x.Key))
        {
            Console.WriteLine($"Naziv: {article.Key}, Kolicina: {article.Value.Item1}, Cijena: {article.Value.Item2:C}, Datum isteka: {article.Value.Item3.ToShortDateString()}");
        }
    }

    static void PrintArticlesSortedByDateDown()
    {
        foreach (var article in articlesDictionary.OrderByDescending(x => x.Value.Item3))
        {
            Console.WriteLine($"Naziv: {article.Key}, Kolicina: {article.Value.Item1}, Cijena: {article.Value.Item2:C}, Datum isteka: {article.Value.Item3.ToShortDateString()}");
        }
    }

    static void PrintArticlesSortedByDateUp()
    {
        foreach (var article in articlesDictionary.OrderBy(x => x.Value.Item3))
        {
            Console.WriteLine($"Naziv: {article.Key}, Kolicina: {article.Value.Item1}, Cijena: {article.Value.Item2:C}, Datum isteka: {article.Value.Item3.ToShortDateString()}");
        }
    }

    static void PrintArticlesSortedByQuantity()
    {
        foreach (var article in articlesDictionary.OrderBy(x => x.Value.Item1))
        {
            Console.WriteLine($"Naziv: {article.Key}, Kolicina: {article.Value.Item1}, Cijena: {article.Value.Item2:C}, Datum isteka: {article.Value.Item3.ToShortDateString()}");
        }
    }

    static void PrintMostPopularArticle()
    {
        Dictionary<string, int> productSales = new Dictionary<string, int>();

        foreach (var bill in billDictionary)
        {
            foreach (var item in bill.Value.Item2)
            {
                string productName = item.Item1;

                if (productSales.ContainsKey(productName))
                {
                    productSales[productName] += item.Item2;
                }
                else
                {
                    productSales.Add(productName, item.Item2);
                }
            }
        }
        string bestSellingProduct = productSales.OrderByDescending(x => x.Value).FirstOrDefault().Key;
        string leastSellingProduct = productSales.OrderBy(x => x.Value).FirstOrDefault().Key;
        Console.WriteLine($"Najprodavaniji je: {bestSellingProduct}");
        Console.WriteLine($"Najmanje prodavan je: {leastSellingProduct}");
    }

    static void PrintLeastPopularArticle()
    {
        Dictionary<string, int> productSales = new Dictionary<string, int>();

        foreach (var bill in billDictionary)
        {
            foreach (var item in bill.Value.Item2)
            {
                string productName = item.Item1;

                if (productSales.ContainsKey(productName))
                {
                    productSales[productName] += item.Item2;
                }
                else
                {
                    productSales.Add(productName, item.Item2);
                }
            }
        }
        string leastSellingProduct = productSales.OrderBy(x => x.Value).FirstOrDefault().Key;
        Console.WriteLine($"Najmanje prodavan je: {leastSellingProduct}");
    }


    //Radnici
    static void SelectWorker()
    {
        int workerAction;

        do
        {
            Console.WriteLine("Akcije radnici:\n");

            Console.WriteLine("1 - Unos radnika\n2 - Brisanje radnika\n3 - Uređivanje radnika\n4 - Ispis\n0 - Povratak na glavni izbornik");
            workerAction = int.Parse(Console.ReadLine());

            switch (workerAction)
            {
                case 1:
                    Console.WriteLine("Unos radnika:");
                    InsertWorker();
                    break;
                case 2:
                    Console.WriteLine("Brisanje radnika:");
                    DeleteWorker();
                    break;
                case 3:
                    Console.WriteLine("Uređivanje radnika");
                    ModifyWorker();
                    break;
                case 4:
                    Console.WriteLine("Ispis");
                    PrintWorkers();
                    break;
                case 0:
                    Console.WriteLine("Povratak na glavni izbornik");
                    break;
                default:
                    Console.WriteLine("Pogrešan unos. Pokušajte ponovno.");
                    break;
            }
        } while (workerAction != 0);
    }

    static void InsertWorker()
    {
        DateTime dateOfBirth;
        bool isValidDate;

        Console.WriteLine("\nUnesite ime i prezime radnika:");
        var name = Console.ReadLine();

        do
        {
            Console.WriteLine("\nUnesite datum rodjenja radnika u formatu yyyy-MM-dd:");
            var userInput = Console.ReadLine();

            isValidDate = DateTime.TryParseExact(userInput, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out dateOfBirth);

            if (!isValidDate || dateOfBirth > DateTime.Now)
            {
                Console.WriteLine("Krivi unos datuma, pokusajte ponovno.");
            }
        } while (!isValidDate || dateOfBirth > DateTime.Now);

        workersDictionary.Add(name, dateOfBirth);
        Console.WriteLine("Radnik uspjesno dodan.\n");
    }

    static void DeleteWorker()
    {
        Console.WriteLine("Kako zelis brisati radnika?\na - Izbrisi radnika po imenu\nb - Izbrisi sve starije od 65\n");
        var actionToDeleteWorker = Console.ReadLine();

        switch (actionToDeleteWorker)
        {
            case "a":
                Console.WriteLine("Brisanje radnika po imenu. Unesi ime radnika kojeg zelis izbrisati:");
                var nameOfTheWorker = Console.ReadLine();

                if (workersDictionary.ContainsKey(nameOfTheWorker))
                {
                    workersDictionary.Remove(nameOfTheWorker);
                    Console.WriteLine("Uspjesno ste obrisali radnika.");
                }
                else
                {
                    Console.WriteLine("Krivi unos imena, nema tog radnika u trgovini.\n");
                }
                break;
            case "b":
                Console.WriteLine("Brisanje radnika starijih od 65 godina.\n");
                DateTime currentDate = DateTime.Now;

                var workersToRemove = workersDictionary
                    .Where(worker => (currentDate.Year - worker.Value.Year) > 65)
                    .Select(worker => worker.Key).ToList();

                foreach (var workerToRemove in workersToRemove)
                {
                    workersDictionary.Remove(workerToRemove);
                }

                Console.WriteLine("Radnici su obrisani.\n");
                break;
            default:
                Console.WriteLine("Pogresan unos. Povratak na izbornik.\n");
                break;
        }

    }

    static void ModifyWorker()
    {
        Console.WriteLine("Unesi ime radnika cije prezime zelis promijeniti:");
        var modifyWorker = Console.ReadLine();

        if (workersDictionary.ContainsKey(modifyWorker))
        {
            Console.WriteLine($"Unesite novo prezime za {modifyWorker}");
            var newSurname = Console.ReadLine();

            var birthDate = workersDictionary[modifyWorker];
            workersDictionary.Remove(modifyWorker);
            workersDictionary.Add($"{modifyWorker} {newSurname}", birthDate);
            Console.WriteLine("Uspjesno ste promijenili ime radniku.");
        }
        else
        {
            Console.WriteLine("Taj radnik ne postoji. Pokusaj ponovno.");
        }
    }

    static void PrintWorkers()
    {
        Console.WriteLine("Ispisi radnike:\na - ispis svih radnika\nb - ispis onih kojima je rodjendan u ovom mjesecu\n");

        var actionPrintWorkers = Console.ReadLine();

        switch (actionPrintWorkers)
        {
            case "a":
                Console.WriteLine("Ispis svih radnika u formatu ime - broj godina:");
                foreach (var worker in workersDictionary)
                {
                    Console.WriteLine($"{worker.Key} - {DateTime.Now.Year - worker.Value.Year} godina");
                }
                break;

            case "b":
                Console.WriteLine("Ispis radnika kojima je rodjendan u ovom mjesecu:");

                var workersThisMonth = workersDictionary
                    .Where(worker => worker.Value.Month == DateTime.Now.Month)
                    .ToList();

                if (workersThisMonth.Any())
                {
                    foreach (var worker in workersThisMonth)
                    {
                        Console.WriteLine($"{worker.Key}: Datum rodjenja: {worker.Value.ToShortDateString()}");
                    }
                }
                else
                {
                    Console.WriteLine("Ni jednom radniku nije rodjendan ovaj mjesec.");
                }

                break;
            default:
                Console.WriteLine("Pogresan unos. Povratak na izbornik.");
                break;
        }

    }

    //Racuni
    static void SelectBill()
    {
        int billAction;

        do
        {
            Console.WriteLine("Racun - akcije:");

            Console.WriteLine("1 - Unos racuna\n2 - Ispis racuna\n0 - Povratak na glavni izbornik");
            billAction = int.Parse(Console.ReadLine());

            switch (billAction)
            {
                case 1:
                    Console.WriteLine("Unos racuna");
                    NewBill();
                    break;
                case 2:
                    Console.WriteLine("Ispis racuna");
                    PrintAllBills();
                    break;
                case 0:
                    Console.WriteLine("Povratak na glavni izbornik");
                    break;
                default:
                    Console.WriteLine("Pogrešan unos. Pokušajte ponovno.");
                    break;
            }
        } while (billAction != 0);
    }

    static void NewBill()
    {
        string nameOfTheProduct;

        Console.WriteLine("Ispis svih artikala dostupnih u ducanu:");
        PrintAllArticles();

        do
        {
            Console.WriteLine("Unesi IME proizvoda kojeg zelis kupiti:");
            nameOfTheProduct = Console.ReadLine().ToLower();

            if (nameOfTheProduct != "izlaz" && articlesDictionary.ContainsKey(nameOfTheProduct))
            {
                if (purchaseDictionary.ContainsKey(nameOfTheProduct))
                {
                    Console.WriteLine("Taj proizvod je vec unesen.");
                    continue;
                }

                Console.WriteLine($"Unesite KOLICINU. Dostupno je {articlesDictionary[nameOfTheProduct].Item1} {nameOfTheProduct}");
                var quantityOfTheProduct = int.Parse(Console.ReadLine());

                purchaseDictionary.Add(nameOfTheProduct, quantityOfTheProduct);
            }

            else if (nameOfTheProduct != "izlaz" && !articlesDictionary.ContainsKey(nameOfTheProduct))
            {
                Console.WriteLine("Taj proizvod nije dostupan.");
                continue;
            }

            Console.Write("Unesi IZLAZ za kraj unosa ili ");

        } while (nameOfTheProduct != "izlaz");

        CurrentBillStatus();
        ContinueOrQuit();
    }

    static void CurrentBillStatus()
    {
        var totalValue = 0.0;
        Console.WriteLine("Na racunu se nalazi:");

        foreach (var product in purchaseDictionary)
        {
            var productName = product.Key;
            var quantity = product.Value;

            if (articlesDictionary.TryGetValue(productName, out var article))
            {
                var articlePrice = article.Item2;
                var singleProductPrice = (double)articlePrice * quantity;

                Console.WriteLine($"Proizvod: {productName} - Kolicina: {quantity}, Ukupna cijena: {singleProductPrice:C}");
                totalValue += singleProductPrice;
            }
        }

        Console.WriteLine($"Ukupan iznos racuna je: {totalValue}");
    }

    static void ContinueOrQuit()
    {
        string billAction;
        do
        {
            Console.WriteLine("Unesi:\n PRINT za ispis\n PONISTI za ponistavanje racuna.");
            billAction = Console.ReadLine().ToLower();

            if (billAction == "print")
            {
                Console.WriteLine("Mijenjam stanje ...");
                foreach (var product in purchaseDictionary)
                {
                    string productName = product.Key;
                    int quantityThatUserIsBuying = product.Value;

                    if (articlesDictionary.TryGetValue(productName, out var article))
                    {
                        int newQuantityOfTheProduct = article.Item1 - quantityThatUserIsBuying;

                        articlesDictionary[productName] = new Tuple<int, decimal, DateTime>(newQuantityOfTheProduct, article.Item2, article.Item3);
                    }
                }

                AddTheBill();
            }
            else if (billAction == "ponisti")
            {
                Console.WriteLine("Ponistavam racun.");

                List<string> keysTORemove = new List<string>(purchaseDictionary.Keys);
                foreach (var key in keysTORemove)
                {
                    purchaseDictionary.Remove(key);
                }
            }

        } while (billAction != "print" && billAction != "ponisti");
    }

    static void AddTheBill()
    {
        var newBillNumber = billDictionary.Count + 1;
        DateTime currentDate = DateTime.Now;

        List<Tuple<string, int, decimal, decimal>> billItems = new List<Tuple<string, int, decimal, decimal>>();

        foreach (var product in purchaseDictionary)
        {
            string productName = product.Key;
            int quantityThatUserIsBuying = product.Value;

            if (articlesDictionary.TryGetValue(productName, out var articleInfo))
            {
                decimal articlePrice = articleInfo.Item2;
                decimal totalProductPrice = articlePrice * quantityThatUserIsBuying;

                billItems.Add(Tuple.Create(productName, quantityThatUserIsBuying, articlePrice, totalProductPrice));
            }
        }

        billDictionary.Add(newBillNumber, Tuple.Create(currentDate, billItems));
        PrintDetailedBill(newBillNumber);
        purchaseDictionary.Clear();
    }
    static void PrintAllBills()
    {
        foreach (var bill in billDictionary)
        {
            var items = bill.Value.Item2;
            var totalAmount = 0.0m;

            foreach (var item in items)
            {
                totalAmount += item.Item4;
            }
            Console.WriteLine($" Racun ID: {bill.Key} - Datum izdavanja: {bill.Value.Item1} - Ukupni iznos: {totalAmount}\n\n");
        }

        Console.WriteLine("Ako zelis detaljan ispis nekog racuna unesi njegov ID:");
        var billId = Console.ReadLine();

        if (int.TryParse(billId, out var id))
        {
            if (billDictionary.ContainsKey(id))
            {
                PrintDetailedBill(id);
            }
            else
            {
                Console.WriteLine("Ne postoji racun s unesenim ID-om.");
            }
        }
    }

    static void PrintDetailedBill(int billId)
    {
        Console.WriteLine("Ispisujem racun...");
        if (billDictionary.TryGetValue(billId, out var bill))
        {
            Console.WriteLine($" Racun ID: {billId} - Datum izdavanja: {bill.Item1}");
            var items = bill.Item2;
            var totalAmount = 0.0m;

            Console.WriteLine("Proizvodi:");
            foreach (var item in items)
            {
                Console.WriteLine($"\tProizvod: {item.Item1} - Kolicina: {item.Item2}");
                totalAmount += item.Item4;
            }

            Console.WriteLine($"Ukupni iznos racuna: {totalAmount}\n\n");
        }
    }

    //Statistika
    static void SelectStatistic()
    {
        int articleAction;

        do
        {
            Console.WriteLine("Statistika akcije:\n1 - Ukupan broj artikala u trgovini\n2 - Vrijednost neprodanih artikala\n" +
                "3 - Vrijednosti prodanih artikala\n4 - Stanje\n0 - Povratak na glavni izbornik");

            articleAction = int.Parse(Console.ReadLine());

            switch (articleAction)
            {
                case 1:
                    Console.WriteLine("Ukupan broj artikala u trgovini");
                    CountArticles();
                    break;
                case 2:
                    Console.WriteLine("Vrijednost neprodanih artikala");
                    ValueOfTheArticlesInTheShop();
                    break;
                case 3:
                    Console.WriteLine("Vrijednosti prodanih artikala");
                    ValueOfTheSoldArticles();
                    break;
                case 4:
                    Console.WriteLine("Stanje");
                    State();
                    break;
                case 0:
                    Console.WriteLine("Povratak na glavni izbornik");
                    break;
                default:
                    Console.WriteLine("Pogrešan unos. Pokušajte ponovno.");
                    break;
            }
        } while (articleAction != 0);
    }

    static void CountArticles()
    {
        var totalNumberOfArticlesInTheStore = 0;

        foreach (var article in articlesDictionary)
        {
            var numberOfItemsPerArticle = article.Value.Item1;
            totalNumberOfArticlesInTheStore += numberOfItemsPerArticle;

            Console.WriteLine($"\t {article.Key} - broj artikala: {article.Value.Item1}");
        }

        Console.WriteLine($"Ukupan ima: {totalNumberOfArticlesInTheStore} artikala\n");
    }

    static void ValueOfTheArticlesInTheShop()
    {
        var totalValueOfArticlesInTheStore = 0.0m;

        foreach (var article in articlesDictionary)
        {
            var valueOfTesingleItem = article.Value.Item1 * article.Value.Item2;
            totalValueOfArticlesInTheStore += valueOfTesingleItem;

            Console.WriteLine($"\t{article.Key} - neprodana vrijednost: {valueOfTesingleItem}");
        }

        Console.WriteLine($"Ukupna vrijednost svih neprodanih artikala: {totalValueOfArticlesInTheStore}\n");
    }

    static void ValueOfTheSoldArticles()
    {
        var totalValueOTheSoldfArticles = 0.0m;

        foreach (var bill in billDictionary)
        {
            var items = bill.Value.Item2;

            foreach (var item in items)
            {
                totalValueOTheSoldfArticles += item.Item4;
            }
        }
        Console.WriteLine($"Ukupna vrijednost svih prodanih artikala: {totalValueOTheSoldfArticles}\n");
    }

    static bool TryParseInput(string prompt, out int result)
    {
        Console.Write(prompt);
        return int.TryParse(Console.ReadLine(), out result);
    }

    static void State()
    {
        if (TryParseInput("Unesite mjesec (broj): ", out var month) &&
              TryParseInput("Unesite godinu: ", out var year) &&
              TryParseInput("Unesite iznos place radnika za taj mjesec: ", out var salary) &&
              TryParseInput("Unesite iznos najma: ", out var rent) &&
              TryParseInput("Unesite iznos ostalih troskova: ", out var otherExpenses))
        {
            Dictionary<Tuple<int, int>, double> monthlyEarnings = new Dictionary<Tuple<int, int>, double>()
            {
                {Tuple.Create(7, 2023), 4333.78},
                {Tuple.Create(8, 2023), 4567.78},
                {Tuple.Create(9, 2023), 4798.08},
                {Tuple.Create(10, 2023), 5122.08},
            };

            if (monthlyEarnings.TryGetValue(Tuple.Create(month, year), out var monthlyEarning))
            {
                var netProfit = monthlyEarning * (1.0 / 3) - salary - rent - otherExpenses;

                Console.WriteLine($"Neto profit: {netProfit:C}");
            }
            else
            {
                Console.WriteLine("Nema podataka za odabrani mjesec.");
                return;
            }
        }

    }

}
