using System.Text.Json;
using Microsoft.JSInterop;

namespace CricketVerse.Services;

public class LanguageService
{
    private readonly IJSRuntime _jsRuntime;
    private const string StorageKey = "language";
    private Dictionary<string, Dictionary<string, string>> _translations;
    public string CurrentLanguage { get; private set; } = "en";
    public event Func<Task>? OnLanguageChanged;
    private bool _isInitialized;
    public bool IsRTL => CurrentLanguage == "ar";

    public LanguageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
        _translations = new Dictionary<string, Dictionary<string, string>>
        {
            ["en"] = new Dictionary<string, string>
            {
                ["Home"] = "Home",
                ["LiveScores"] = "Live Scores",
                ["TeamBuilder"] = "Team Builder",
                ["MatchCenter"] = "Match Center",
                ["Fantasy"] = "Fantasy League",
                ["Rewards"] = "Rewards",
                ["Wallet"] = "Wallet",
                ["Leaderboard"] = "Leaderboard",
                ["CreateTeam"] = "Create Team",
                ["Loading"] = "Loading...",
                ["NotFound"] = "Not Found",
                ["PageNotFound"] = "Sorry, the page you requested could not be found.",
                ["ReturnHome"] = "Return Home",
                ["RewardsDescription"] = "Earn points and redeem exclusive PSL merchandise",
                ["ExpiresOn"] = "Expires on",
                ["Redeem"] = "Redeem",
                ["Unavailable"] = "Unavailable",
                ["PremiumMember"] = "Premium Member",
                ["Settings"] = "Settings",
                ["Profile"] = "Profile",
                ["DarkMode"] = "Dark Mode",
                ["Language"] = "Language",
                ["TeamName"] = "Team Name",
                ["SelectPlayers"] = "Select Players",
                ["SaveTeam"] = "Save Team",
                ["DeleteTeam"] = "Delete Team",
                ["TeamCreated"] = "Team Created Successfully",
                ["TeamDeleted"] = "Team Deleted Successfully",
                ["PSLDreamXI"] = "PSL Dream XI",
                ["Budget"] = "Budget",
                ["Players"] = "Players",
                ["Points"] = "Points",
                ["SelectionRules"] = "Team Selection Rules",
                ["Rule1"] = "Maximum 4 players from one PSL team",
                ["Rule2"] = "1-4 Batsmen, 1-4 Bowlers, 1-2 All-rounders, 1 Wicket-keeper",
                ["Rule3"] = "Total budget of 100M",
                ["Rule4"] = "Must select 11 players",
                ["YourTeams"] = "Your Teams",
                ["All"] = "All Players",
                ["Batsman"] = "Batsmen",
                ["Bowler"] = "Bowlers",
                ["All-rounder"] = "All-rounders",
                ["Wicket-keeper"] = "Wicket-keepers",
                ["UpcomingMatches"] = "Upcoming Matches",
                ["LiveMatches"] = "Live Matches",
                ["CompletedMatches"] = "Completed Matches",
                ["PSLSchedule"] = "PSL Schedule",
                ["FantasyPoints"] = "Fantasy Points",
                ["TotalPoints"] = "Total Points",
                ["TeamValue"] = "Team Value",
                ["LeaderboardRank"] = "Rank",
                ["GuestUser"] = "Guest User",
                ["TotalParticipants"] = "Total Participants",
                ["TopScore"] = "Top Score",
                ["YourRank"] = "Your Rank",
                ["Player"] = "Player",
                ["PrizePool"] = "Prize Pool",
                ["FirstPrize"] = "First Prize",
                ["SecondPrize"] = "Second Prize",
                ["ThirdPrize"] = "Third Prize",
                ["TeamComplete"] = "Team Complete!",
                ["TeamCompleteMessage"] = "Great job! You've selected all 11 players. Save your team to continue.",
                ["EnterTeamName"] = "Enter your team name",
                ["SelectedPlayers"] = "Selected Players",
                ["ISL"] = "Islamabad United",
                ["KAR"] = "Karachi Kings",
                ["LAH"] = "Lahore Qalandars",
                ["MUL"] = "Multan Sultans",
                ["PES"] = "Peshawar Zalmi",
                ["QUE"] = "Quetta Gladiators",
                ["AvailableBalance"] = "Available Balance",
                ["AddMoney"] = "Add Money",
                ["RecentTransactions"] = "Recent Transactions",
                ["Amount"] = "Amount",
                ["SelectPaymentMethod"] = "Select Payment Method",
                ["Cancel"] = "Cancel",
                ["Proceed"] = "Proceed",
                ["NoTransactions"] = "No transactions yet",
                ["LastUpdated"] = "Last updated",
                ["MinimumAmount"] = "Minimum amount: PKR 100",
                ["Cost"] = "Cost",
                ["Rule5"] = "Select a captain and vice-captain for your team",
                ["SelectCaptains"] = "Select Team Leaders",
                ["SelectCaptain"] = "Select Captain",
                ["SelectViceCaptain"] = "Select Vice Captain",
                ["ChooseCaptain"] = "Choose your captain",
                ["ChooseViceCaptain"] = "Choose your vice captain",
                ["Captain"] = "Captain",
                ["ViceCaptain"] = "Vice Captain",
                ["InsufficientFunds"] = "Insufficient Funds",
                ["InsufficientFundsMessage"] = "You don't have enough funds to create this team. Please add money to your wallet.",
                ["Required"] = "Required",
                ["AddFunds"] = "Add Funds",
                ["TeamCreatedSuccess"] = "Team created successfully! You can now use this team in contests.",
                ["SearchPlayers"] = "Search players by name, team or role",
                ["AllTeams"] = "All Teams",
                ["SortByPoints"] = "Sort by Points",
                ["SortByPrice"] = "Sort by Price",
                ["SortByName"] = "Sort by Name",
                ["NoPlayersFound"] = "No players found matching your criteria",
                ["FilterResults"] = "Filter Results",
                ["ClearFilters"] = "Clear Filters",
                ["PlayerStats"] = "Player Stats",
                ["LastMatches"] = "Last 5 Matches",
                ["AvgPoints"] = "Avg. Points",
                ["SelectionRate"] = "Selection %",
                ["PlayerDetails"] = "Player Details",
                ["ViewStats"] = "View Stats",
                ["CloseStats"] = "Close Stats",
                ["WelcomeBack"] = "Welcome Back!",
                ["SignInToContinue"] = "Sign in to continue to CricketVerse",
                ["Email"] = "Email",
                ["Password"] = "Password",
                ["RememberMe"] = "Remember me",
                ["ForgotPassword"] = "Forgot Password?",
                ["SignIn"] = "Sign In",
                ["SigningIn"] = "Signing in...",
                ["DontHaveAccount"] = "Don't have an account?",
                ["CreateAccount"] = "Create Account",
                ["OrSignInWith"] = "Or sign in with",
                ["SignInSuccess"] = "Signed in successfully!",
                ["SignInError"] = "Invalid email or password",
                ["JoinCricketVerse"] = "Join CricketVerse and start your fantasy cricket journey",
                ["FirstName"] = "First Name",
                ["LastName"] = "Last Name",
                ["Username"] = "Username",
                ["ConfirmPassword"] = "Confirm Password",
                ["AcceptTerms"] = "I accept the",
                ["TermsAndConditions"] = "Terms and Conditions",
                ["CreatingAccount"] = "Creating account...",
                ["AlreadyHaveAccount"] = "Already have an account?",
                ["OrSignUpWith"] = "Or sign up with",
                ["WeakPassword"] = "Weak password",
                ["MediumPassword"] = "Medium password",
                ["StrongPassword"] = "Strong password",
                ["RegistrationSuccess"] = "Account created successfully!",
                ["RegistrationError"] = "Error creating account",
                ["ComingSoon"] = "Coming soon!",
                ["InvalidEmail"] = "Please enter a valid email address",
                ["PasswordRequirements"] = "Password must be at least 8 characters and contain uppercase, lowercase, number and special character",
                ["PasswordsDoNotMatch"] = "Passwords do not match",
                ["MustAcceptTerms"] = "You must accept the terms and conditions",
                ["WalletBudget"] = "Wallet Budget",
                ["FromWallet"] = "From wallet",
                ["InsufficientBudget"] = "Not enough budget in your wallet",
                ["TeamCreationCost"] = "Team creation fee"
            },
            ["ur"] = new Dictionary<string, string>
            {
                ["Home"] = "ہوم",
                ["LiveScores"] = "لائیو سکور",
                ["TeamBuilder"] = "ٹیم بلڈر",
                ["MatchCenter"] = "میچ سنٹر",
                ["Fantasy"] = "فینٹسی لیگ",
                ["Rewards"] = "انعامات",
                ["Wallet"] = "والٹ",
                ["Leaderboard"] = "لیڈر بورڈ",
                ["CreateTeam"] = "ٹیم بنائیں",
                ["Loading"] = "لوڈ ہو رہا ہے...",
                ["NotFound"] = "نہیں ملا",
                ["PageNotFound"] = "معذرت، آپ کا مطلوبہ صفحہ نہیں مل سکا۔",
                ["ReturnHome"] = "ہوم پر واپس جائیں",
                ["RewardsDescription"] = "پی ایس ایل مرچنڈائز حاصل کریں",
                ["ExpiresOn"] = "میعاد ختم",
                ["Redeem"] = "حاصل کریں",
                ["Unavailable"] = "دستیاب نہیں",
                ["PremiumMember"] = "پریمیم ممبر",
                ["Settings"] = "ترتیبات",
                ["Profile"] = "پروفائل",
                ["DarkMode"] = "ڈارک موڈ",
                ["Language"] = "زبان",
                ["TeamName"] = "ٹیم کا نام",
                ["SelectPlayers"] = "کھلاڑی منتخب کریں",
                ["SaveTeam"] = "ٹیم محفوظ کریں",
                ["DeleteTeam"] = "ٹیم حذف کریں",
                ["TeamCreated"] = "ٹیم کامیابی سے بنائی گئی",
                ["TeamDeleted"] = "ٹیم کامیابی سے حذف کر دی گئی",
                ["PSLDreamXI"] = "پی ایس ایل ڈریم الیون",
                ["Budget"] = "بجٹ",
                ["Players"] = "کھلاڑی",
                ["Points"] = "پوائنٹس",
                ["SelectionRules"] = "ٹیم کے انتخاب کے قوانین",
                ["Rule1"] = "ایک پی ایس ایل ٹیم سے زیادہ سے زیادہ 4 کھلاڑی",
                ["Rule2"] = "1-4 بلے باز، 1-4 گیند باز، 1-2 آل راؤنڈر، 1 وکٹ کیپر",
                ["Rule3"] = "کل بجٹ 100M",
                ["Rule4"] = "11 کھلاڑی منتخب کرنا ضروری ہے",
                ["YourTeams"] = "آپ کی ٹیمیں",
                ["All"] = "تمام کھلاڑی",
                ["Batsman"] = "بلے باز",
                ["Bowler"] = "گیند باز",
                ["All-rounder"] = "آل راؤنڈر",
                ["Wicket-keeper"] = "وکٹ کیپر",
                ["UpcomingMatches"] = "آنے والے میچز",
                ["LiveMatches"] = "لائیو میچز",
                ["CompletedMatches"] = "مکمل شدہ میچز",
                ["PSLSchedule"] = "پی ایس ایل شیڈول",
                ["FantasyPoints"] = "فینٹسی پوائنٹس",
                ["TotalPoints"] = "کل پوائنٹس",
                ["TeamValue"] = "ٹیم کی قیمت",
                ["LeaderboardRank"] = "درجہ",
                ["GuestUser"] = "مہمان صارف",
                ["TotalParticipants"] = "کل شرکاء",
                ["TopScore"] = "ٹاپ سکور",
                ["YourRank"] = "آپ کا درجہ",
                ["Player"] = "کھلاڑی",
                ["PrizePool"] = "انعامی پول",
                ["FirstPrize"] = "پہلا انعام",
                ["SecondPrize"] = "دوسرا انعام",
                ["ThirdPrize"] = "تیسرا انعام",
                ["TeamComplete"] = "ٹیم مکمل!",
                ["TeamCompleteMessage"] = "شاندار! آپ نے تمام 11 کھلاڑی منتخب کر لیے ہیں۔ جاری رکھنے کے لیے اپنی ٹیم محفوظ کریں۔",
                ["EnterTeamName"] = "اپنی ٹیم کا نام درج کریں",
                ["SelectedPlayers"] = "منتخب کھلاڑی",
                ["ISL"] = "اسلام آباد یونائیٹڈ",
                ["KAR"] = "کراچی کنگز",
                ["LAH"] = "لاہور قلندرز",
                ["MUL"] = "ملتان سلطانز",
                ["PES"] = "پشاور زلمی",
                ["QUE"] = "کوئٹہ گلیڈی ایٹرز",
                ["AvailableBalance"] = "دستیاب بیلنس",
                ["AddMoney"] = "رقم شامل کریں",
                ["RecentTransactions"] = "حالیہ لین دین",
                ["Amount"] = "رقم",
                ["SelectPaymentMethod"] = "ادائیگی کا طریقہ منتخب کریں",
                ["Cancel"] = "منسوخ کریں",
                ["Proceed"] = "آگے بڑھیں",
                ["NoTransactions"] = "ابھی تک کوئی لین دین نہیں",
                ["LastUpdated"] = "آخری تجدید",
                ["MinimumAmount"] = "کم از کم رقم: PKR 100",
                ["Cost"] = "قیمت",
                ["Rule5"] = "اپنی ٹیم کے لیے کپتان اور نائب کپتان منتخب کریں",
                ["SelectCaptains"] = "ٹیم لیڈرز منتخب کریں",
                ["SelectCaptain"] = "کپتان منتخب کریں",
                ["SelectViceCaptain"] = "نائب کپتان منتخب کریں",
                ["ChooseCaptain"] = "اپنا کپتان چنیں",
                ["ChooseViceCaptain"] = "اپنا نائب کپتان چنیں",
                ["Captain"] = "کپتان",
                ["ViceCaptain"] = "نائب کپتان",
                ["InsufficientFunds"] = "ناکافی فنڈز",
                ["InsufficientFundsMessage"] = "آپ کے پاس ٹیم بنانے کے لیے کافی فنڈز نہیں ہیں۔ براہ کرم اپنے والٹ میں رقم شامل کریں۔",
                ["Required"] = "درکار",
                ["AddFunds"] = "فنڈز شامل کریں",
                ["TeamCreatedSuccess"] = "ٹیم کامیابی سے بنائی گئی! اب آپ اس ٹیم کو مقابلوں میں استعمال کر سکتے ہیں۔",
                ["SearchPlayers"] = "نام، ٹیم یا کردار سے کھلاڑیوں کو تلاش کریں",
                ["AllTeams"] = "تمام ٹیمیں",
                ["SortByPoints"] = "پوائنٹس کے لحاظ سے ترتیب دیں",
                ["SortByPrice"] = "قیمت کے لحاظ سے ترتیب دیں",
                ["SortByName"] = "نام کے لحاظ سے ترتیب دیں",
                ["NoPlayersFound"] = "آپ کے معیار سے مماثل کوئی کھلاڑی نہیں ملا",
                ["FilterResults"] = "نتائج کو فلٹر کریں",
                ["ClearFilters"] = "فلٹرز صاف کریں",
                ["PlayerStats"] = "کھلاڑی کے اعدادوشمار",
                ["LastMatches"] = "آخری 5 میچز",
                ["AvgPoints"] = "اوسط پوائنٹس",
                ["SelectionRate"] = "انتخاب کی شرح",
                ["PlayerDetails"] = "کھلاڑی کی تفصیلات",
                ["ViewStats"] = "اعدادوشمار دیکھیں",
                ["CloseStats"] = "اعدادوشمار بند کریں",
                ["WelcomeBack"] = "خوش آمدید!",
                ["SignInToContinue"] = "کرکٹ ورس جاری رکھنے کے لیے سائن ان کریں",
                ["Email"] = "ای میل",
                ["Password"] = "پاس ورڈ",
                ["RememberMe"] = "مجھے یاد رکھیں",
                ["ForgotPassword"] = "پاس ورڈ بھول گئے؟",
                ["SignIn"] = "سائن ان",
                ["SigningIn"] = "سائن ان ہو رہا ہے...",
                ["DontHaveAccount"] = "اکاؤنٹ نہیں ہے؟",
                ["CreateAccount"] = "اکاؤنٹ بنائیں",
                ["OrSignInWith"] = "یا سائن ان کریں",
                ["SignInSuccess"] = "کامیابی سے سائن ان ہو گیا!",
                ["SignInError"] = "غلط ای میل یا پاس ورڈ",
                ["JoinCricketVerse"] = "کرکٹ ورس میں شامل ہوں اور اپنا فینٹسی کرکٹ سفر شروع کریں",
                ["FirstName"] = "پہلا نام",
                ["LastName"] = "آخری نام",
                ["Username"] = "صارف نام",
                ["ConfirmPassword"] = "پاس ورڈ کی تصدیق کریں",
                ["AcceptTerms"] = "میں قبول کرتا/کرتی ہوں",
                ["TermsAndConditions"] = "شرائط و ضوابط",
                ["CreatingAccount"] = "اکاؤنٹ بن رہا ہے...",
                ["AlreadyHaveAccount"] = "پہلے سے اکاؤنٹ ہے؟",
                ["OrSignUpWith"] = "یا سائن اپ کریں",
                ["WeakPassword"] = "کمزور پاس ورڈ",
                ["MediumPassword"] = "درمیانہ پاس ورڈ",
                ["StrongPassword"] = "مضبوط پاس ورڈ",
                ["RegistrationSuccess"] = "اکاؤنٹ کامیابی سے بن گیا!",
                ["RegistrationError"] = "اکاؤنٹ بنانے میں خرابی",
                ["ComingSoon"] = "جلد آ رہا ہے!",
                ["InvalidEmail"] = "برائے مہربانی درست ای میل ایڈریس درج کریں",
                ["PasswordRequirements"] = "پاس ورڈ کم از کم 8 حروف کا ہونا چاہیے اور اس میں بڑے حروف، چھوٹے حروف، نمبر اور خاص حروف شامل ہونے چاہئیں",
                ["PasswordsDoNotMatch"] = "پاس ورڈز مماثل نہیں ہیں",
                ["MustAcceptTerms"] = "آپ کو شرائط و ضوابط قبول کرنا ہوں گے",
                ["WalletBudget"] = "والیٹ بجٹ",
                ["FromWallet"] = "والیٹ سے",
                ["InsufficientBudget"] = "آپ کے والیٹ میں کافی بجٹ نہیں ہے",
                ["TeamCreationCost"] = "ٹیم بنانے کی فیس"
            }
        };
    }

    public async Task InitializeLanguage()
    {
        if (_isInitialized) return;

        try
        {
            var language = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", StorageKey);
            if (!string.IsNullOrEmpty(language))
            {
                CurrentLanguage = language;
                await ApplyLanguage(language);
            }
            _isInitialized = true;
        }
        catch (InvalidOperationException)
        {
            // Ignore JavaScript interop errors during prerendering
        }
    }

    public async Task ToggleLanguage()
    {
        CurrentLanguage = CurrentLanguage == "en" ? "ar" : "en";
        await ApplyLanguage(CurrentLanguage);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, CurrentLanguage);
        if (OnLanguageChanged != null)
        {
            await OnLanguageChanged.Invoke();
        }
    }

    private async Task ApplyLanguage(string language)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("document.documentElement.setAttribute", "dir", language == "ar" ? "rtl" : "ltr");
            await _jsRuntime.InvokeVoidAsync("document.documentElement.setAttribute", "lang", language);
        }
        catch (InvalidOperationException)
        {
            // Ignore JavaScript interop errors during prerendering
        }
    }

    public string GetTranslation(string key)
    {
        if (_translations.TryGetValue(CurrentLanguage, out var translations) &&
            translations.TryGetValue(key, out var translation))
        {
            return translation;
        }
        return key;
    }
} 