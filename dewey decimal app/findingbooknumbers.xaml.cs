using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace dewey_decimal_app
{/// <summary>
/// THIS DEALS WITH PART 3 OF THE POE
/// </summary>
    
    public partial class findingbooknumbers : Window
    { //GENERATES RANDOM CHOIVES FROM THE TEXT FILE
        private static readonly Random options = new Random();
        //CALLING THE TREE SYSTEM CLASS 
        LibrarybookTree LibaryOrganisationTree = new LibrarybookTree();
        // THIS IS A SUB SYSTEM OF THE TREE OR A NODE IF YOU WISH 
        TreeBranch SubBranch;
      
        public findingbooknumbers()
        {
            
            InitializeComponent();
           
            //sets the colour of the competitive progress bars 
            progress.Foreground = Brushes.Green;
            // red for incorrect answers and greed for correct
                negprogress.Foreground = Brushes.Red;

            //sets the visibility of the achievments 
          bronze.Visibility = Visibility.Collapsed;
            silver.Visibility = Visibility.Collapsed;
            gold.Visibility = Visibility.Collapsed;

            progress.Visibility = Visibility.Visible;
            //calls the score tracker and data loader 
            newlevel();
            booktextfileLoad();
            Closing += OnExit;
        }
        private bool isResetAndRestart = false;
        private void OnExit(object sender, CancelEventArgs e)
        {
            if (!isResetAndRestart)
            {
                var Window = new HomePage();
                Dispatcher.BeginInvoke(new Action(() => Window.Show()));
            }
        }
        //adds the data to the dictionary before adding it to the tree
        Dictionary<string, string> BookCategorisation = new Dictionary<string, string>();

        // this method loads the data from the txt file as per the requirements 
        public void booktextfileLoad()
        {
            using (StreamReader TextFileData = new StreamReader("CallNumbers.txt"))
            {
                string streamingdata;
                while ((streamingdata = TextFileData.ReadLine()) != null)
                {
                    int HasPosition = streamingdata.IndexOf(" ");
                    if (HasPosition >= 0)
                    {
                        string booknum = streamingdata.Substring(0, HasPosition);
                        string bookclassifcations = streamingdata.Substring(HasPosition + 1);

                        
                       BookCategorisation[booknum] = bookclassifcations;
                       LibaryOrganisationTree.NewBookBranch(booknum, bookclassifcations);
                    }
                    else
                    {
                      //stuff 
                    }
                }

                TextFileData.Close();
                
                TreeBookDataLoad();
            }
        }
        // this keeps score for the gamifcation 
        private int rowPosition = 0;

        private int incorrects = 0;
        private int score = 0;
        private int corrects = 0;

        // this is the gamification tracker 
        public void newlevel()
        {// SCORES ARE STORED IN THE TXT FILE CALL SCORS
            string scoresPath = "scores.txt";

            // WE USE A STREAM READER TO GET THE DATA 
            using (StreamReader fileparse = new StreamReader(scoresPath))
            {
                // THEN LINE BY LINE WE READ THE DATA INTO VARIABLES 
                string newrow;
                while ((newrow = fileparse.ReadLine()) != null)
                {
                    try
                    {
                        string[] DATA = newrow.Split(',');
                        // IN THIS CASE WE ARE READING THE LAST SAVED PROGRESS OF THE USER
                       
                        int level = int.Parse(DATA[0].Trim());
                        int incorrect = int.Parse(DATA[1].Trim());
                        int correct = int.Parse(DATA[2].Trim());
                        int scores = int.Parse(DATA[3].Trim());
                        // HERE WE CHECK WHAT LEVEL THEY ARE ON
                        if (level == 0)
                        {
                            MessageBox.Show("level 1");
                        }
                        else if (level == 1)
                        {
                            negprogress.Value = incorrect;
                            progress.Value = correct;
                            incorrects = incorrect;
                            corrects = correct;
                            score = scores;
             
                            correctA.Text = "WINS:" + "\n" + corrects.ToString();
                            incorrectA.Text = "Losses" + "\n" + incorrects.ToString();
                            totalscore.Text = "SCORE:" + "\n" + score.ToString();
              
                        }
                    }
                    catch (Exception e) { 
                    MessageBox.Show(e.Message);
                    
                    }

                }
                
            }
        }
        //ensures questions are done correctly 
        bool checkforthirdoptiononly(int questions)
        {
            string question = questions.ToString();
            return !Regex.IsMatch(question, @"\d+0$");
        }



        // THIS LOADS THE DATA INTO THE TREE AFTER THE DATA HAS BEEN LOADED FROM THE TXT FILE AS PER THE REQUIREMENTS
        public void TreeBookDataLoad()
        {
           // LISTS FOR MANAGING THE TREE DATA 
            List<int> disp1 = new List<int>();
            List<int> disp2 = new List<int>();

        // WE THEN LOAD THE DATA FROM EACH NODE INTO THE LIST WITH THE RUBRIC REQUIREMENT CONDTIONS 
            foreach (TreeBranch genre in LibaryOrganisationTree.genre)
            {// HERE WE CHECK FOR ALL NUMBERS WITH 00 THUS LOADING ONLY TOP LEVEL FIRST
                if (genre.BookNumberID.EndsWith("00"))
                {// WE THEN RECONSTRUCT THE STRINGS ACCORDINGLY AND ADD IT TO THE LIST 
                    booknamelist2.Items.Add(genre.BookNumberID + " " + genre.BookValues);
                }
            }
            // HERE WE KEEP THE COUNT TO 4
            while (disp1.Count < 4)
            {// THIS CHECKS THE COUNT 
                int data = options.Next(LibaryOrganisationTree.genre.Count - 1);
                //HERE WE CHECK FOR DATA 
                 if (!disp1.Contains(data) && !checkforthirdoptiononly(data))
                    {// ONCE DATA IS FOUND WE ADD THE CONDITION OF ONLY 1 THIRD LEVEL ITEM AS PER THE RUBRIC 
                    disp1.Add(data);
                    if (disp2.Count < 1)
                    {// HERE WE ADDING SAID ITEM TO THE LIST 
                        disp2.Add(data);
                       
                        Booknumbers1.Items.Add(LibaryOrganisationTree.genre.ElementAt(data).BookValues);
                        SubBranch = LibaryOrganisationTree.genre.ElementAt(data);
                        Booknumbers1.SelectedIndex = 0;
                    }
                }
            }
            // THIS GETS THE QUESTION FIRST DIGIT 
            string substring = SubBranch.BookNumberID.Substring(0,1);

            // CHECKS THE INDEX 
            int Position = -1;
            booknamelist2.Items.Insert(0, "=== Top Level Options ===");
            for (int i = 0; i < booknamelist2.Items.Count; i++)
            {//MATCHES IT AGAINST THE BOOKNAMELIST 
                if (booknamelist2.Items[i].ToString().StartsWith(substring + "00"))
                {
                    Position = i;
                    break;
                }
            }
           
              

            // HERE WE REMOVE RANDOM OPTIONS THAT DO NOT MATCH THE ID 
            if (Position != -1)
            {
                int optionchoice = 0;
               Random shuffle = new Random();
                for (int i = 0; i < optionchoice; i++)
                {
                    int DeleteOption = shuffle.Next(booknamelist2.Items.Count);
                    while (DeleteOption == Position)
                    {
                        DeleteOption = shuffle.Next(booknamelist2.Items.Count);
                    }
                    booknamelist2.Items.RemoveAt(DeleteOption);
                }
            }
        }




        // THIS IS THE TREE CLASS WHERE I STORE,QUERY AND SORT DATA AS PER THE REQUIREMENTS 
        public class LibrarybookTree
        {// THE TREE CLASS VARIABLES AND STORAGE USING A STACK 
            public TreeBranch pages { get; set; }
            public List<TreeBranch> genre { get; } = new List<TreeBranch>();
            private Stack<TreeBranch> sub = new Stack<TreeBranch>();
            private Stack<TreeBranch> mains = new Stack<TreeBranch>();

            // THIS IS A BRANCH TO ADD NEW STUFF TO THE LISTS
            public LibrarybookTree NewBookBranch(string ID, string val)
            {// THIS ALSO ALLOWS US TO QUERY IT 
                TreeBranch leaf;
                if (sub.Count == 0)
                {
                    leaf = new TreeBranch(ID, val, null);
                    pages = leaf;
                }
                else
                {
                    leaf = sub.Peek().addtoinsert(ID, val);
                }
                genre.Add(leaf);
                sub.Push(leaf);

                return this;
            }


        }


        public class TreeBranch
        { // THESE ARE THE VARIABLS FOR THE TREE BRANCH 
            public string BookNumberID { get; set; }
            public string BookValues { get; set; }
            public TreeBranch Book { get; set; }
            public List<TreeBranch> SubFolder { get; set; }
            // THIS IS A CONTRUCTOR FOR THE TREBRANCH CLASS THAT GETS DATA 
            public TreeBranch(string BranchID, string val, TreeBranch MainTree)
            {// ASSINGIGN THE VARIABLES 
                BookNumberID = BranchID;
                BookValues = val;
                Book = MainTree;
                SubFolder = new List<TreeBranch>();
            }
            // THIS ADDS IT TO THE SB FOLDER LIST WHICH THEN CAN BE QUERIED IF NEEDED
            public TreeBranch addtoinsert(string key, string val)
            {
                var newTreeBranch = new TreeBranch( key, val, this);
                SubFolder.Add(newTreeBranch);
                return newTreeBranch;
            }
        }
      
      
     //THIS KEEPS TRACK OF THE LEVEL OF ANSWERS AKA THE DEWEY DECIMAL LEVEL GAME LEVEL 1 = DEWEY LEVEL 3 , 2=2 , 1=3
        private int Gamelevel = 1;

        // THIS THE GETTER AND SETTER FOR IT
        public int mainlevels
        {
            get { return Gamelevel; }
            set { Gamelevel = value; }
        }
      // THIS MANAGES MY LIST AS IT LETS THE USER SELECT ANSWERS ,CHECK ANSWERS AND RESET THE SYSTEM AS PER THE REQUIREMENTS
        private void bookname_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool checkend = false;
            int lev = 0;
            // CHECKING THE ANSWER COUNT 
            if (booknamelist2.Items.Count > 0)
            {// ADDDING THE ANSWER TO A VARIABLE TO BE COMPARED 
                if (booknamelist2.SelectedItem != null)
                {
                    var booknums = booknamelist2.SelectedItem.ToString();
                    var nameBooks = booknums;
                    // THIS CHECKS THE ANSWERS
                    bool Answers = false;
                    if (mainlevels == 1)
                    {// HERE WE SPLIT TO SUB STRINGS THIS ALLOWS US TO CHECK EACH LEVEL 1 ITEM  WHEN THEY ARE SELECTED 
                        Answers = nameBooks.Substring(0, 1) == SubBranch.BookNumberID.ToString().Substring(0, 1);

                    }
                    else if (mainlevels == 2)
                    {
                        //HERE WE CHECK IF THE LEVEL 2 ITEMS MATCH BY SUB STRINGS 
                        Answers = nameBooks.Substring(0, 2) == SubBranch.BookNumberID.ToString().Substring(0, 2);
                    }
                    else if (mainlevels == 3)
                    {
                        // HERE WE CHECK LEVEL 3 ITEMS BY SUB STRINGS 
                        Answers = nameBooks.Substring(0, 3) == SubBranch.BookNumberID.Substring(0, 3);

                    }

                    if (Answers)
                    {// IF THE ANSWER IS CORRECT PROCEED WITH THIS CODE 
                        MessageBox.Show("Correct");
                        // CALL THE GAMIFICATION SYSTEM 
                        GamificationScore(Answers, rowPosition);

                        // WE ADD IT TO THE LIST SO I CAN QUERY IT 
                        List<string> BookNumbers = new List<string>();
                        List<int> bookverify = new List<int>();

                        // IF LEVEL = 1 THEN EXECUTE THE BELOW CODE
                        if (mainlevels == 1)
                        { 
                            try
                            {
                                // THIS CHECKS THE FIRST DIGIT PLUS 0 AKA LEVEL 2 AGAISNT THE SYSTEM IDS TO FIND THE MATCHING ITEM 
                                TreeBranch answer = LibaryOrganisationTree.genre.First(g => g.BookValues == Booknumbers1.SelectedItem.ToString());

                                // THIS HOLDS THE TWO DIGITS OF THE QUESTION 
                                string answerlevel1 = answer.BookNumberID.Substring(0, 2) + "0";
                                BookNumbers.Add(answerlevel1);


                                // WHILE THE COUNT ON BOOK NAME LIST IS LESS THAN 4 ADD NEW ANSWERS FOR THE NEXT QUESTION TO THE BOOKNAMESLIST AS PER THE RUBRIC
                                while (BookNumbers.Count < 4)
                                {
                                    int x = options.Next(LibaryOrganisationTree.genre.Count - 1);
                                    string bookNumberID = LibaryOrganisationTree.genre.ElementAt(x).BookNumberID;
                                    if (bookNumberID.Length == 3 && bookNumberID.EndsWith("0") && !bookNumberID.EndsWith("00"))
                                    {
                                        string topLevel = bookNumberID;
                                        if (!BookNumbers.Contains(topLevel) && topLevel != answerlevel1)
                                        {
                                            BookNumbers.Add(topLevel);
                                        }
                                    }

                                }

                                // SORT IT NUMERICALLY AS PER THE RUBRIC
                                BookNumbers.Sort();


                                //THIS ADDS IT 
                                booknamelist2.Items.Insert(11, "=== Second Level Options ===");
                                foreach (string num in BookNumbers)
                                {
                                    TreeBranch option = LibaryOrganisationTree.genre.First(g => g.BookNumberID == num);
                                    booknamelist2.Items.Add(option.BookNumberID + " " + option.BookValues);
                                }
                            }
                            catch
                            {
                                //stuff
                            }

                        }// IF ANSWER = LEVEL 2 CORRECTLY 
                        if (mainlevels == 2)
                        {
                            //HERE WE CHECK FOR A LEVEL 3 ITEM AND WHILE LESS THAN 4 WE ADD IT TO THE LIST 
                            TreeBranch correctOption = LibaryOrganisationTree.genre.First(g => g.BookValues == Booknumbers1.SelectedItem.ToString());
                            string correctBookNumber = correctOption.BookNumberID;
                            BookNumbers.Add(correctBookNumber);

                            // CHECKING FOR LESS THAN 4
                            while (BookNumbers.Count < 4)
                            {
                                int x = options.Next(LibaryOrganisationTree.genre.Count - 1);
                                string topLevel = LibaryOrganisationTree.genre.ElementAt(x).BookNumberID;

                                if (!BookNumbers.Contains(topLevel) && topLevel != correctBookNumber)
                                {
                                    BookNumbers.Add(topLevel);
                                }
                            }
                            // SORTING NUMERICALLY 
                            BookNumbers.Sort();

                            //ADDING TO THE LIST 
                            booknamelist2.Items.Insert(16, "=== Third Level Options ===");
                            foreach (string num in BookNumbers)
                            {
                                TreeBranch option = LibaryOrganisationTree.genre.First(g => g.BookNumberID == num);
                                booknamelist2.Items.Add(option.BookNumberID + " " + option.BookValues);
                            }


                        }
                        // IF CORRECT ANSWER = LEVEL 3 ITEM
                        if (mainlevels == 3)
                        {//THIS WILL ALLOW THE USE TO CHOOSE IF THEY WANT TO GO ANOTHER ROUND OR RETURN TO MAIN MENU 
                         // THIS KEEPS TRACK OF THE NEW LEVEL 
                            int newlevel = 0;
                            //SHOW THE QUESTION
                            // IF NO THEY RETURN TO MENU IF YES THEY KEEP PLAYING 
                            MessageBoxResult Confirm = MessageBox.Show("Do you want to Play again or return?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (Confirm == MessageBoxResult.Yes)
                            {// IF YES THE SCORES ARE WRITTEN TO A TEXT FILE CALLED SCORS TO KEEP TRACK 
                                string scoresPath = "Scores.txt";
                                // WE USE A STREAM WRITER
                                using (StreamWriter outputFile = new StreamWriter(scoresPath))
                                {// if the user chooses yes it will then save the scores to the files
                                    newlevel = 1;
                                    outputFile.WriteLine(newlevel + "," + incorrects + "," + corrects + "," + score);
                                }
                                // this tills the main window not to go to home
                                isResetAndRestart = true;
                                // restart the game window
                                var newlevelpage = new findingbooknumbers();
                                newlevelpage.Show();
                                this.Close();
                            }// if the message box button is no 
                            else if (Confirm == MessageBoxResult.No)
                            {
                                string scorestxt = "Scores.txt";

                                // reset the scores  then quit which opens the homepage 
                                using (StreamWriter ToFile = new StreamWriter(scorestxt))
                                {//set all scores to zero
                                    newlevel = 0;
                                    incorrects = 0;
                                    corrects = 0;
                                    score = 0;
                                    ToFile.WriteLine(newlevel + "," + incorrects + "," + corrects + "," + score);

                                }
                                //closes the window
                                isResetAndRestart = false;
                                this.Close();
                            }

                        }


                        //increment the levels
                        mainlevels++;
                        lev++;
                    }

                    else if (booknamelist2.SelectedIndex == 0)
                    {
                        // If the heading is selected, deselect it
                        booknamelist2.SelectedIndex = -1;
                    }
                    else if (booknamelist2.SelectedIndex == 11)
                    {
                        // If the heading is selected, deselect it
                        booknamelist2.SelectedIndex = -1;
                    }
                    else if (booknamelist2.SelectedIndex == 16)
                    {
                        // If the heading is selected, deselect it
                        booknamelist2.SelectedIndex = -1;
                    }
                    else if (incorrects != 109 && mainlevels == 2)
                    {
                        int Newlevel = 1;
                        MessageBox.Show("Incorrect!,next Level starting");
                        // call th scoring method
                        GamificationScore(Answers, rowPosition);
                        string scoresPath = "Scores.txt";
                        // WE USE A STREAM WRITER
                        using (StreamWriter outputFile = new StreamWriter(scoresPath))
                        {
                            Newlevel = 1;
                            outputFile.WriteLine(Newlevel + "," + incorrects + "," + corrects + "," + score);
                        }
                        // as per part 3 requirement if the user gets the answer wrong a new question must show

                        isResetAndRestart = true;
                        // restart the game window
                        var newlevelpage = new findingbooknumbers();
                        newlevelpage.Show();
                        this.Close();

                    }
                    else if (incorrects != 109 && mainlevels == 3)
                    {
                        int newlevel = 1;
                        MessageBox.Show("Incorrect!,next Level starting");
                        // call th scoring method
                        GamificationScore(Answers, rowPosition);
                        string scoresPath = "Scores.txt";
                        // WE USE A STREAM WRITER
                        using (StreamWriter outputFile = new StreamWriter(scoresPath))
                        {
                            newlevel = 1;
                            outputFile.WriteLine(newlevel + "," + incorrects + "," + corrects + "," + score);
                        }
                        // as per part 3 requirement if the user gets the answer wrong a new question must show

                        isResetAndRestart = true;
                        // restart the game window
                        var newlevelpage = new findingbooknumbers();
                        newlevelpage.Show();
                        this.Close();

                    }
                    else if (incorrects != 109)
                    {// reset the level and save the scores 
                        int Newlevel = 1;
                        MessageBox.Show("Incorrect!,next Level starting");
                        // call th scoring method
                        GamificationScore(Answers, rowPosition);
                        string scoresPath = "Scores.txt";
                        // WE USE A STREAM WRITER
                        using (StreamWriter outputFile = new StreamWriter(scoresPath))
                        {
                            Newlevel = 1;
                            outputFile.WriteLine(Newlevel + "," + incorrects + "," + corrects + "," + score);
                        }
                        // as per part 3 requirement if the user gets the answer wrong a new question must show
                        isResetAndRestart = true;
                        Booknumbers1.Items.Clear();
                        booknamelist2.Items.Clear();
                        LibaryOrganisationTree = new LibrarybookTree();
                        BookCategorisation.Clear();

                        newlevel();
                        booktextfileLoad();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect!,end game");
                        GamificationScore(Answers, rowPosition);
                        int newlevel;


                        // IF NO THEY RETURN TO MENU IF YES THEY KEEP PLAYING 
                        MessageBoxResult Confirm = MessageBox.Show("Do you want to Play again or return?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (Confirm == MessageBoxResult.Yes)
                        {// IF YES THE SCORES ARE WRITTEN TO A TEXT FILE CALLED SCORS TO KEEP TRACK 
                            string scoresPath = "Scores.txt";
                            // WE USE A STREAM WRITER
                            using (StreamWriter outputFile = new StreamWriter(scoresPath))
                            {// if the user chooses yes it will then save the scores to the files
                                newlevel = 1;
                                outputFile.WriteLine(newlevel + "," + incorrects + "," + corrects + "," + score);
                            }
                            // this tills the main window not to go to home
                            isResetAndRestart = true;
                            // restart the game window
                            var newlevelpage = new findingbooknumbers();
                            newlevelpage.Show();
                            this.Close();
                        }// if the message box button is no 
                        else if (Confirm == MessageBoxResult.No)
                        {
                            string scorestxt = "Scores.txt";

                            // reset the scores  then quit which opens the homepage 
                            using (StreamWriter ToFile = new StreamWriter(scorestxt))
                            {//set all scores to zero
                                newlevel = 0;
                                incorrects = 0;
                                corrects = 0;
                                score = 0;
                                ToFile.WriteLine(newlevel + "," + incorrects + "," + corrects + "," + score);

                            }
                            //closes the window

                            this.Close();
                        }

                    }
                }
            }

            else
            {// clear the lists 
                Booknumbers1.Items.Clear();
                booknamelist2.Items.Clear();
            }

          
        }
        // achiviments for the system
        private void silver_Click(object sender, RoutedEventArgs e)
        {
            // badge 2
            MessageBox.Show("user has scored 4 and earned a silver Badge");
        }

        private void bronze_Click(object sender, RoutedEventArgs e)
        {
            // badge 1
            MessageBox.Show("user has scored 2 and earned a bronze badge");
        }

        private void gold_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("user has scored 6 and earned a gold badge Badge");
        }
        //if something isnt working right this resets the page aka bug fix 
        private void Display_Click(object sender, RoutedEventArgs e)
        {
            isResetAndRestart = true;
            // restart the game window
            var newlevelpage = new findingbooknumbers();
            newlevelpage.Show();
            this.Close();
        }


        // go back to main menu
        private void Backbutton_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close();
        }
        // this is the method for the gamification as per the rubric 
        /// <summary>
        /// rubric requirement
        /// App Functionality: Gamification feature implemented.
        /// rubric requirement
        /// </summary>
        /// <param name="isCorrect"></param>
        /// <param name="rowIndex"></param>
        private void GamificationScore(bool isCorrect, int rowIndex)
        {
            // this will keep track of the score
            if (isCorrect)
            {// if correct add to the score
                score++;
            }
            else
            {// if incorrect minus from the score
                score--;
            }
            if (isCorrect)
            {// if correct add to the scoreboarcd
                corrects++;
            }
            else
            {// if incorrect add to the incorrect score board
                incorrects++;
            }
            //adding the scores to the scoreboard
            correctA.Text = "WINS:" + "\n" + corrects.ToString();
            incorrectA.Text = "Losses" + "\n" + incorrects.ToString();
            totalscore.Text = "SCORE:" + "\n" + score.ToString();
            //upadting the progress bars
            progress.Value = score;
            negprogress.Value = incorrects;

            //show each propt at each trigger
            if (incorrects == 9)
            {


                MessageBox.Show("are you ok?? :(");
            }
           
            else if (incorrects == 18)
            {
                MessageBox.Show("someone didnt study... :(");
            }
            // if 14 answers were incorrect they go back to the menu
            else if(incorrects == 55)
            {
                MessageBox.Show("MISSION FAILED! back to menu with you!");
              
                int newlevel;


                // IF NO THEY RETURN TO MENU IF YES THEY KEEP PLAYING 
                MessageBoxResult Confirm = MessageBox.Show("Do you want to Play again or return?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Confirm == MessageBoxResult.Yes)
                {// IF YES THE SCORES ARE WRITTEN TO A TEXT FILE CALLED SCORS TO KEEP TRACK 
                    string scoresPath = "Scores.txt";
                    // WE USE A STREAM WRITER
                    using (StreamWriter outputFile = new StreamWriter(scoresPath))
                    {// if the user chooses yes it will then save the scores to the files
                        newlevel = 1;
                        outputFile.WriteLine(newlevel + "," + incorrects + "," + corrects + "," + score);
                    }
                    // this tills the main window not to go to home
                    isResetAndRestart = true;
                    // restart the game window
                    var newlevelpage = new findingbooknumbers();
                    newlevelpage.Show();
                    this.Close();
                }// if the message box button is no 
                else if (Confirm == MessageBoxResult.No)
                {
                    string scorestxt = "Scores.txt";

                    // reset the scores  then quit which opens the homepage 
                    using (StreamWriter ToFile = new StreamWriter(scorestxt))
                    {//set all scores to zero
                        newlevel = 0;
                        incorrects = 0;
                        corrects = 0;
                        score = 0;
                        ToFile.WriteLine(newlevel + "," + incorrects + "," + corrects + "," + score);

                    }
                    //closes the window

                    this.Close();
                }

            }


            // achievement triggers for the scoring 
            if (score == 15)
            {
                bronze.Visibility = Visibility.Visible;
            }
            else if (score == 20)
            {
                silver.Visibility = Visibility.Visible;
            }
            else if (score == 25)
            {
                MessageBox.Show("Half way to Perfection!");
            }
            else if (score == 30)
            {
                gold.Visibility = Visibility.Visible;
            }
            else if (score == 45)
            {
                MessageBox.Show("Damn,still going are we ;)");
            }
            //if correct score equals 14 then the program sends them back to menu 
            else if (score == 115)
            {

             
                MessageBox.Show("well done, Game completed ;)");
               
               

            }
            

        }
        //changes the question if you know the answer
        private void Reload_Click(object sender, RoutedEventArgs e)
        {
               if (incorrects != 109 && mainlevels == 2)
            {
                int Newlevel = 1;
        
            
                string scoresPath = "Scores.txt";
                // WE USE A STREAM WRITER
                using (StreamWriter outputFile = new StreamWriter(scoresPath))
                {
                    Newlevel = 1;
                    outputFile.WriteLine(Newlevel + "," + incorrects + "," + corrects + "," + score);
                }
                // as per part 3 requirement if the user gets the answer wrong a new question must show

                isResetAndRestart = true;
                // restart the game window
                var newlevelpage = new findingbooknumbers();
                newlevelpage.Show();
                this.Close();

            }
            else if (incorrects != 109 && mainlevels == 3)
            {
                int newlevel = 1;
         
               
             string scoresPath = "Scores.txt";
                // WE USE A STREAM WRITER
                using (StreamWriter outputFile = new StreamWriter(scoresPath))
                {
                    newlevel = 1;
                    outputFile.WriteLine(newlevel + "," + incorrects + "," + corrects + "," + score);
                }
                // as per part 3 requirement if the user gets the answer wrong a new question must show

                isResetAndRestart = true;
                // restart the game window
                var newlevelpage = new findingbooknumbers();
                newlevelpage.Show();
                this.Close();

            }
            else if (incorrects != 109)
            {// reset the level and save the scores 
     
                isResetAndRestart = true;
                Booknumbers1.Items.Clear();
                booknamelist2.Items.Clear();
                LibaryOrganisationTree = new LibrarybookTree();
                BookCategorisation.Clear();

                newlevel();
                booktextfileLoad();
            }
        }
    }
}

