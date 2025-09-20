using AspNetCore8Test.Models;

namespace AspNetCore8Test.Services
{
    public class LibraryService
    {
        // 模擬資料，實際應用中應連接資料庫
        private readonly List<Book> _books;
        private readonly List<LibraryNews> _news;
        private readonly List<LibraryEvent> _events;

        public LibraryService()
        {
            _books = GetSampleBooks();
            _news = GetSampleNews();
            _events = GetSampleEvents();
        }

        public BorrowingStats GetBorrowingStats()
        {
            return new BorrowingStats
            {
                TodayHours = "08:30 - 17:00",
                JournalRoomHours = "17樓期刊室 10:24",
                SpecialCollections = 17,
                ThirdFloorReading = 20,
                TodayBorrowingNote = "3樓閱覽/284"
            };
        }

        public LibraryHours GetTodayHours()
        {
            return new LibraryHours
            {
                Day = DateTime.Now.ToString("yyyy/MM/dd"),
                OpenTime = new TimeSpan(8, 30, 0),
                CloseTime = new TimeSpan(17, 0, 0),
                IsOpen = true,
                SpecialNote = "週六 20 (週六)"
            };
        }

        public List<LibraryNews> GetLatestNews()
        {
            return _news.OrderByDescending(n => n.PublishDate).Take(5).ToList();
        }

        public List<Book> GetNewArrivals()
        {
            return _books.Where(b => b.IsNewArrival).Take(4).ToList();
        }

        public List<Book> GetFeaturedBooks()
        {
            return _books.Where(b => b.IsFeatured).Take(6).ToList();
        }

        public List<Book> GetPopularBooks()
        {
            return _books.Where(b => b.IsPopular).Take(4).ToList();
        }

        public LibraryEvent? GetCurrentEvent()
        {
            return _events.FirstOrDefault(e => e.IsActive);
        }

        public List<Book> SearchBooks(string query, string category)
        {
            var results = _books.AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                results = results.Where(b => 
                    b.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    b.Author.Contains(query, StringComparison.OrdinalIgnoreCase));
            }

            if (category != "all" && !string.IsNullOrEmpty(category))
            {
                results = results.Where(b => b.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
            }

            return results.ToList();
        }

        public Book? GetBookById(int id)
        {
            return _books.FirstOrDefault(b => b.Id == id);
        }

        public UserAccount GetUserAccount(string userId)
        {
            // 模擬用戶資料
            return new UserAccount
            {
                UserId = userId,
                Name = "張同學",
                Email = "student@university.edu.tw",
                BorrowedBooks = new List<BorrowedBook>
                {
                    new BorrowedBook
                    {
                        BookId = 1,
                        BookTitle = "人工智慧大革命",
                        BorrowDate = DateTime.Now.AddDays(-5),
                        DueDate = DateTime.Now.AddDays(9)
                    }
                },
                ReservedBooks = new List<ReservedBook>(),
                Fines = 0
            };
        }

        public List<Models.LibraryServiceInfo> GetLibraryServices()
        {
            return new List<Models.LibraryServiceInfo>
            {
                new Models.LibraryServiceInfo { Name = "圖書查詢", Description = "線上圖書目錄檢索", Icon = "fas fa-search", Link = "/Library/Search" },
                new Models.LibraryServiceInfo { Name = "個人帳戶", Description = "查看借閱記錄與預約", Icon = "fas fa-user", Link = "/Library/MyAccount" },
                new Models.LibraryServiceInfo { Name = "數位資源", Description = "電子書籍與資料庫", Icon = "fas fa-laptop", Link = "/Library/Resources" },
                new Models.LibraryServiceInfo { Name = "活動資訊", Description = "圖書館舉辦的各項活動", Icon = "fas fa-calendar", Link = "/Library/Events" }
            };
        }

        public List<LibraryEvent> GetUpcomingEvents()
        {
            return _events.Where(e => e.StartDate >= DateTime.Now).OrderBy(e => e.StartDate).ToList();
        }

        public List<DigitalResource> GetDigitalResources()
        {
            return new List<DigitalResource>
            {
                new DigitalResource { Name = "電子書平台", Type = "電子書", Description = "提供各類電子書籍", Url = "#", RequiresAuthentication = true },
                new DigitalResource { Name = "學術資料庫", Type = "資料庫", Description = "學術論文與期刊", Url = "#", RequiresAuthentication = true },
                new DigitalResource { Name = "語言學習", Type = "多媒體", Description = "線上語言學習資源", Url = "#", RequiresAuthentication = false },
                new DigitalResource { Name = "數位典藏", Type = "典藏", Description = "校史與珍貴文獻", Url = "#", RequiresAuthentication = false }
            };
        }

        private List<Book> GetSampleBooks()
        {
            return new List<Book>
            {
                new Book
                {
                    Id = 1,
                    Title = "人工智慧大革命",
                    Author = "李開復",
                    Publisher = "天下文化",
                    PublicationYear = 2023,
                    Category = "科技",
                    Location = "3F-科技類",
                    TotalCopies = 5,
                    AvailableCopies = 3,
                    IsNewArrival = true,
                    IsFeatured = true,
                    CoverImageUrl = "/images/books/ai-revolution.jpg"
                },
                new Book
                {
                    Id = 2,
                    Title = "金錢心理學",
                    Author = "摩根‧豪瑟",
                    Publisher = "天下雜誌",
                    PublicationYear = 2023,
                    Category = "財經",
                    Location = "2F-財經類",
                    TotalCopies = 3,
                    AvailableCopies = 1,
                    IsPopular = true,
                    IsFeatured = true,
                    CoverImageUrl = "/images/books/psychology-of-money.jpg"
                },
                new Book
                {
                    Id = 3,
                    Title = "原子習慣",
                    Author = "詹姆斯‧克利爾",
                    Publisher = "方智",
                    PublicationYear = 2022,
                    Category = "自我成長",
                    Location = "2F-心理類",
                    TotalCopies = 8,
                    AvailableCopies = 5,
                    IsPopular = true,
                    IsFeatured = true,
                    CoverImageUrl = "/images/books/atomic-habits.jpg"
                },
                new Book
                {
                    Id = 4,
                    Title = "台灣百年追求",
                    Author = "薛化元",
                    Publisher = "玉山社",
                    PublicationYear = 2023,
                    Category = "歷史",
                    Location = "1F-歷史類",
                    TotalCopies = 4,
                    AvailableCopies = 4,
                    IsNewArrival = true,
                    CoverImageUrl = "/images/books/taiwan-history.jpg"
                },
                new Book
                {
                    Id = 5,
                    Title = "夜的盡頭",
                    Author = "東野圭吾",
                    Publisher = "皇冠",
                    PublicationYear = 2023,
                    Category = "文學",
                    Location = "2F-文學類",
                    TotalCopies = 6,
                    AvailableCopies = 2,
                    IsNewArrival = true,
                    IsPopular = true,
                    CoverImageUrl = "/images/books/end-of-night.jpg"
                },
                new Book
                {
                    Id = 6,
                    Title = "府城的故事",
                    Author = "王瑞德",
                    Publisher = "遠流",
                    PublicationYear = 2023,
                    Category = "歷史",
                    Location = "1F-歷史類",
                    TotalCopies = 3,
                    AvailableCopies = 3,
                    IsFeatured = true,
                    CoverImageUrl = "/images/books/tainan-story.jpg"
                }
            };
        }

        private List<LibraryNews> GetSampleNews()
        {
            return new List<LibraryNews>
            {
                new LibraryNews
                {
                    Id = 1,
                    Title = "【公告】圖書館於本學期期中考期間延長開館時間使用規定",
                    PublishDate = new DateTime(2024, 9, 14, 0, 0, 0, DateTimeKind.Local),
                    IsPinned = true,
                    Link = "#"
                },
                new LibraryNews
                {
                    Id = 2,
                    Title = "【活動】歡迎參與本校「智慧學習：上圖書館，向AI學習，用AI協助學習」",
                    PublishDate = new DateTime(2024, 9, 16, 0, 0, 0, DateTimeKind.Local),
                    Link = "#"
                },
                new LibraryNews
                {
                    Id = 3,
                    Title = "【公告】三樓 - 10R09 (時) 圖書定名研討會議會議報告",
                    PublishDate = new DateTime(2024, 9, 19, 0, 0, 0, DateTimeKind.Local),
                    Link = "#"
                },
                new LibraryNews
                {
                    Id = 4,
                    Title = "【公告】整修 - 11R05－17時30分期刊室地上暫停業服務修繕計畫",
                    PublishDate = new DateTime(2024, 9, 17, 0, 0, 0, DateTimeKind.Local),
                    Link = "#"
                },
                new LibraryNews
                {
                    Id = 5,
                    Title = "【活動】100文（時）逸大系列市立師研習計畫二文師學於公外",
                    PublishDate = new DateTime(2024, 9, 17, 0, 0, 0, DateTimeKind.Local),
                    Link = "#"
                }
            };
        }

        private List<LibraryEvent> GetSampleEvents()
        {
            return new List<LibraryEvent>
            {
                new LibraryEvent
                {
                    Id = 1,
                    Title = "閱讀桃花季",
                    Description = "Minecraft 活動進行中，歡迎同學參與體驗。",
                    ImageUrl = "/images/events/minecraft-reading.jpg",
                    StartDate = new DateTime(2024, 9, 1, 0, 0, 0, DateTimeKind.Local),
                    EndDate = new DateTime(2024, 9, 30, 0, 0, 0, DateTimeKind.Local),
                    IsActive = true,
                    Location = "圖書館2樓活動區",
                    Link = "#"
                }
            };
        }
    }
}