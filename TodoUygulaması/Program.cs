using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TodoUygulaması
{
    class Program
    {
       static void Main(string[] args)
       {
            TaskManager taskManager = new TaskManager();

            while (true)
                {
                    Console.WriteLine("Lütfen yapmak istediğiniz işlemi seçiniz:");
                    Console.WriteLine("*******************************************");
                    Console.WriteLine("(1) Board Listelemek");
                    Console.WriteLine("(2) Board'a Kart Eklemek");
                    Console.WriteLine("(3) Board'dan Kart Silmek");
                    Console.WriteLine("(4) Kart Taşımak");

                    int secim;
                    if (int.TryParse(Console.ReadLine(), out secim))
                    {
                        switch (secim)
                        {
                            case 1:
                                taskManager.BoarduListele();
                                break;
                            case 2:
                                taskManager.KartEkle();
                                break;
                            case 3:
                                taskManager.KartSil();
                                break;
                            case 4:
                                taskManager.KartTasi();
                                break;
                            default:
                                Console.WriteLine("Geçersiz seçim. Lütfen tekrar deneyin.");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Geçersiz seçim. Lütfen tekrar deneyin.");
                    }

                    Console.WriteLine();
                }
            }
        }

        enum KartBuyukluk
        {
            XS = 1,
            S,
            M,
            L,
            XL
        }

        class Task
        {
            public string Baslik { get; set; }
            public string Icerik { get; set; }
            public int AtananKisi { get; set; }
            public KartBuyukluk Buyukluk { get; set; }

            public Task(string baslik, string icerik, int atananKisi, KartBuyukluk buyukluk)
            {
                Baslik = baslik;
                Icerik = icerik;
                AtananKisi = atananKisi;
                Buyukluk = buyukluk;
            }
        }

        class TaskManager
        {
            private List<Task> todoLine;
            private List<Task> inProgressLine;
            private List<Task> doneLine;
            private Dictionary<int, string> takimUyeleri;

            public TaskManager()
            {
                todoLine = new List<Task>();
                inProgressLine = new List<Task>();
                doneLine = new List<Task>();

                takimUyeleri = new Dictionary<int, string>
        {
            { 1, "Ahmet" },
            { 2, "Mehmet" },
            { 3, "Ayşe" },
            { 4, "Fatma" }
        };

                // Varsayılan olarak 3 kart ekleniyor
                todoLine.Add(new Task("Kart 1", "Kart 1 İçerik", 1, KartBuyukluk.M));
                inProgressLine.Add(new Task("Kart 2", "Kart 2 İçerik", 2, KartBuyukluk.S));
                doneLine.Add(new Task("Kart 3", "Kart 3 İçerik", 3, KartBuyukluk.XL));
            }

            public void BoarduListele()
            {
                Console.WriteLine("TODO Line");
                Console.WriteLine("************************");
                Listele(todoLine);

                Console.WriteLine("IN PROGRESS Line");
                Console.WriteLine("************************");
                Listele(inProgressLine);

                Console.WriteLine("DONE Line");
                Console.WriteLine("************************");
                Listele(doneLine);
            }

            private void Listele(List<Task> tasks)
            {
                if (tasks.Count == 0)
                {
                    Console.WriteLine("~ BOŞ ~");
                    return;
                }

                foreach (var task in tasks)
                {
                    Console.WriteLine($"Başlık\t: {task.Baslik}");
                    Console.WriteLine($"İçerik\t: {task.Icerik}");
                    Console.WriteLine($"Atanan Kişi\t: {takimUyeleri[task.AtananKisi]}");
                    Console.WriteLine($"Büyüklük\t: {task.Buyukluk}");
                    Console.WriteLine("-");
                }
            }

            public void KartEkle()
            {
                Console.WriteLine("Başlık Giriniz\t\t\t: ");
                string baslik = Console.ReadLine();

                Console.WriteLine("İçerik Giriniz\t\t\t: ");
                string icerik = Console.ReadLine();

                Console.WriteLine("Büyüklük Seçiniz -> XS(1),S(2),M(3),L(4),XL(5)\t: ");
                int buyukluk;
                if (!int.TryParse(Console.ReadLine(), out buyukluk) || !Enum.IsDefined(typeof(KartBuyukluk), buyukluk))
                {
                    Console.WriteLine("Hatalı girişler yaptınız!");
                    return;
                }

                Console.WriteLine("Kişi Seçiniz\t\t\t: ");
                int atananKisi;
                if (!int.TryParse(Console.ReadLine(), out atananKisi) || !takimUyeleri.ContainsKey(atananKisi))
                {
                    Console.WriteLine("Hatalı girişler yaptınız!");
                    return;
                }

                Task task = new Task(baslik, icerik, atananKisi, (KartBuyukluk)buyukluk);
                todoLine.Add(task);

                Console.WriteLine("Kart başarıyla eklendi.");
            }

            public void KartSil()
            {
                Console.WriteLine("Öncelikle silmek istediğiniz kartı seçmeniz gerekiyor.");
                Console.WriteLine("Lütfen kart başlığını yazınız:");

                string baslik = Console.ReadLine();
                bool silindi = false;

                silindi = KartSil(todoLine, baslik);
                if (!silindi)
                {
                    silindi = KartSil(inProgressLine, baslik);
                    if (!silindi)
                    {
                        silindi = KartSil(doneLine, baslik);
                    }
                }

                if (!silindi)
                {
                    Console.WriteLine("Aradığınız kriterlere uygun kart board'da bulunamadı.");
                    Console.WriteLine("Lütfen bir seçim yapınız:");
                    Console.WriteLine("* Silmeyi sonlandırmak için\t: (1)");
                    Console.WriteLine("* Yeniden denemek için\t\t: (2)");

                    int secim;
                    if (int.TryParse(Console.ReadLine(), out secim))
                    {
                        if (secim == 1)
                        {
                            return;
                        }
                        else if (secim == 2)
                        {
                            KartSil();
                        }
                        else
                        {
                            Console.WriteLine("Hatalı seçim yaptınız!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Hatalı seçim yaptınız!");
                    }
                }
                else
                {
                    Console.WriteLine("Kart başarıyla silindi.");
                }
            }

            private bool KartSil(List<Task> tasks, string baslik)
            {
                bool silindi = false;

                for (int i = 0; i < tasks.Count; i++)
                {
                    if (tasks[i].Baslik == baslik)
                    {
                        tasks.RemoveAt(i);
                        silindi = true;
                        i--; // Bir önceki indeksi kontrol etmek için
                    }
                }

                return silindi;
            }

            public void KartTasi()
            {
                Console.WriteLine("Öncelikle taşımak istediğiniz kartı seçmeniz gerekiyor.");
                Console.WriteLine("Lütfen kart başlığını yazınız:");

                string baslik = Console.ReadLine();
                Task task = null;

                task = KartBul(todoLine, baslik);
                if (task == null)
                {
                    task = KartBul(inProgressLine, baslik);
                    if (task == null)
                    {
                        task = KartBul(doneLine, baslik);
                    }
                }

                if (task == null)
                {
                    Console.WriteLine("Aradığınız kriterlere uygun kart board'da bulunamadı.");
                    Console.WriteLine("Lütfen bir seçim yapınız:");
                    Console.WriteLine("* İşlemi sonlandırmak için\t: (1)");
                    Console.WriteLine("* Yeniden denemek için\t\t: (2)");

                    int secim;
                    if (int.TryParse(Console.ReadLine(), out secim))
                    {
                        if (secim == 1)
                        {
                            return;
                        }
                        else if (secim == 2)
                        {
                            KartTasi();
                        }
                        else
                        {
                            Console.WriteLine("Hatalı seçim yaptınız!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Hatalı seçim yaptınız!");
                    }
                }
                else
                {
                    Console.WriteLine("Bulunan Kart Bilgileri:");
                    Console.WriteLine("**************************************");
                    Console.WriteLine($"Başlık\t: {task.Baslik}");
                    Console.WriteLine($"İçerik\t: {task.Icerik}");
                    Console.WriteLine($"Atanan Kişi\t: {takimUyeleri[task.AtananKisi]}");
                    Console.WriteLine($"Büyüklük\t: {task.Buyukluk}");

                    Console.WriteLine("Lütfen taşımak istediğiniz Line'ı seçiniz:");
                    Console.WriteLine("(1) TODO");
                    Console.WriteLine("(2) IN PROGRESS");
                    Console.WriteLine("(3) DONE");

                    int secim;
                    if (int.TryParse(Console.ReadLine(), out secim))
                    {
                    TaskManager taskManager = new TaskManager();
                    switch (secim)
                        {
                            case 1:
                                taskManager.Tasi(task, todoLine);
                                break;
                            case 2:
                                taskManager.Tasi(task, inProgressLine);
                                break;
                            case 3:
                                taskManager.Tasi(task, doneLine);
                                break;
                            default:
                                Console.WriteLine("Hatalı bir seçim yaptınız!");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Hatalı bir seçim yaptınız!");
                    }
                }
            }

            private Task KartBul(List<Task> tasks, string baslik)
            {
                foreach (var task in tasks)
                {
                    if (task.Baslik == baslik)
                    {
                        return task;
                    }
                }

                return null;
            }

            private void Tasi(Task task, List<Task> hedefLine)
            {
                List<Task> kaynakLine;

                if (todoLine.Contains(task))
                {
                    kaynakLine = todoLine;
                }
                else if (inProgressLine.Contains(task))
                {
                    kaynakLine = inProgressLine;
                }
                else if (doneLine.Contains(task))
                {
                    kaynakLine = doneLine;
                }
                else
                {
                    Console.WriteLine("Geçersiz bir kart taşıma işlemi gerçekleştirilmek istendi.");
                    return;
                }

                kaynakLine.Remove(task);
                hedefLine.Add(task);

                Console.WriteLine("Kart başarıyla taşındı.");
            }
        }

    }
