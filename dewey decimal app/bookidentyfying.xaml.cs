using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Text;
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
{
    /// <summary>
    /// THIS DEALS WITH PART 2 OF THE POE
    /// </summary>



    public partial class bookidentyfying : Window
    {// this is the declaration for the mouses location
        private Point Location;
        public bookidentyfying()
        {// creates the layout 
            InitializeComponent();
            // hides and disables the achivmetns till the conditions are hit 
          bronze_badge.Visibility = Visibility.Collapsed;
            silver_badge.Visibility = Visibility.Collapsed;
            gold_badge.Visibility = Visibility.Collapsed;
            //loads the data into the dictionary as per the rubric then into the list boxes
            ChoiceLoading();
            MatchMaker();
            Closing += OnExit;
        }

        private void OnExit(object sender, CancelEventArgs e)
        {
            var newForm = new HomePage();
            Dispatcher.BeginInvoke(new Action(() => newForm.Show()));
        }
        //this stores the different state of list control as i offer drag and drop and click to slide 
        private static branchoftree NewTree<branchoftree>(DependencyObject books)
    where branchoftree : DependencyObject
        {
            do
            {
                if (books is branchoftree)
                {
                    return (branchoftree)books;
                }
                books = VisualTreeHelper.GetParent(books);
            }
            while (books != null);
            return null;
        }

        // storing the data in the dictionary as per the rubric requirements
        //(GeeksforGeeks, 2019)
        private static Dictionary<string, string> LibraryBooks = new Dictionary<string, string>();
 
        //LOADS THE DATA FROM THE TEXT FILE INTO THE DICTIONARY AS PER THE RUBRIC 
        private static void ChoiceLoading()
        {
            LibraryBooks.Clear();
            // Read the data from the text file
            string[] datastream  = File.ReadAllLines("LibaryBooks.txt");

            // Parse the data and store it in the dictionary
            foreach (string match in datastream)
            {
                string[] seperate = match.Split(',');
                LibraryBooks.Add(seperate[0], seperate[1]);
            }
        }



        /// <summary>
        /// RUBRIC REQUIREMENT[
        ///App Functionality: User is presented with a randomly generated match-the-columns question, with more answers than questions.
        /// RUBRIC REQUIREMENT]
        /// </summary>
        private string BookCategoryNumber;
        private string BookCategoryName;
        public void MatchMaker()
        {
            Booknumbers.Items.Clear();
            booknamelist.Items.Clear();
            // this will generate the random values as per the rubric
            Random generateatrandom = new Random();
            List<string> Booknos = new List<string>(LibraryBooks.Keys);
            List<string> Bookname = new List<string>(LibraryBooks.Values);

            for (int position = 0; position < 4; position++)
            {// numbers to be chnanged and added to the list
                int listposition = generateatrandom.Next(Booknos.Count);
                Booknumbers.Items.Add(Booknos[listposition]);
                Booknos.RemoveAt(listposition);
            }

            for (int position = 0; position < 7; position++)
            {//  names to be added to the list
                int listposition = generateatrandom.Next(Bookname.Count);
                booknamelist.Items.Add(Bookname[listposition]);
                Bookname.RemoveAt(listposition);
            }
        }

        /// <summary>
        /// RUBRIC REQUIREMENT[
        /// App Functionality: The questions should alternate between descriptions to call numbers and call numbers to descriptions.
        /// RUBRIC REQUIREMENT]
        /// </summary>
        /// <param name="correctAnswers"></param>
        /// MAIN METHOD FOR THE RANDOMISER THAT RANDOMISES ANSWER WHILE SWITCHING THE LISTS
        bool everycorrectanswer = true;

        public void AlternateQuestions(bool correctAnswers)
        {
            if (correctAnswers)
            {
                if (everycorrectanswer)
                {
                    SwitchQuestionsup();
                }
                else
                {
                    MatchMaker();
                }

                // Flip the switchFlag after every correct answer
                everycorrectanswer = !everycorrectanswer;
            }
        }


        // THIS CHANGES THE QUESTIONS TO THE OPPOSING AS PER THE RUBRIC 
        public void SwitchQuestionsup()
        {//LETS THE USER KNOW THE ORDER IS CHANGING 
           
            List<string> Booknos = new List<string>(LibraryBooks.Keys);
            List<string> Bookname = new List<string>(LibraryBooks.Values);

            Booknumbers.Items.Clear();
            booknamelist.Items.Clear();
            //GENERATES AT RANDOM THE VALUES 
            Random generateatrandom = new Random();


            for (int i = 0; i < 4; i++)
            {// NEW VALUES TO BE ADDED
                int index = generateatrandom.Next(Bookname.Count);
                Booknumbers.Items.Add(Bookname[index]);
                Bookname.RemoveAt(index);
            }

            for (int i = 0; i < 7; i++)
            {// NEW VALUES TO BE ADDED 
                int index = generateatrandom.Next(Booknos.Count);
                booknamelist.Items.Add(Booknos[index]);
                Booknos.RemoveAt(index);
            }

        }


        /// <summary >BOOKNOS_SELECTIONCHANGED,bookname_leftbuttonclick,booknumbers_leftbuttonclickx,
        /// RUBRIC REQUIREMENT[
        /// App Functionality: User can complete the match the columns question.
        /// App Functionality: App allows the user to keep practising.
        /// App Functionality: App checks whether the selected answers are correct.
        /// (harinikmsft, 2023)
        /// (Kumar, n.d.)
        /// RUBRIC REQUIREMENT]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Booknumber_listboxswitch_action(object sender, SelectionChangedEventArgs e)
        { 
            bool isBooknumbersForBookNos = Booknumbers.Items.Count > 0 && Booknumbers.Items[0] is string key && LibraryBooks.ContainsKey(key);

            BookCategoryNumber = isBooknumbersForBookNos ? (string)Booknumbers.SelectedItem : (string)booknamelist.SelectedItem;
            BookCategoryName = isBooknumbersForBookNos ? (string)booknamelist.SelectedItem : (string)Booknumbers.SelectedItem;
      
        // this will check if all fields are filled
            if (BookCategoryNumber != null && BookCategoryName != null)
            {
                //this will then update the display table with new data
                display_output.Items.Add(new { BookNum = BookCategoryNumber, BookNm = BookCategoryName, Score = "" });

                // this will then check if the answers are correct if so plus 1 to score
                bool isCorrect = LibraryBooks[BookCategoryNumber] == BookCategoryName;
                GamificationScore(isCorrect, display_output.Items.Count - 1);

                if (isCorrect)
                {
                    MessageBox.Show("Correct Match :)");
                    // if the answers are correct it will change the field to green if not red and thus allowing for how well the user is doing visually
                    DataGridRow booknorow = (DataGridRow)display_output.ItemContainerGenerator.ContainerFromIndex(display_output.Items.Count - 1);
                    booknorow.Background = Brushes.Green;
                    // this removes the correct answers from the listr of choosable answers in order to prevent duplicate scoring 
                    Booknumbers.Items.Remove(BookCategoryNumber);
                    booknamelist.Items.Remove(BookCategoryName);
                }
                else
                {
                    MessageBox.Show("Incorrect Match :(");
                    // if the answer is inccorect it will change it to red
                    DataGridRow answerstore = (DataGridRow)display_output.ItemContainerGenerator.ContainerFromIndex(display_output.Items.Count - 1);
                    answerstore.Background = Brushes.Red;
                }

               // CLEAR THINGS
                Booknumbers.SelectedItem = null;
                booknamelist.SelectedItem = null;
                BookCategoryNumber = null;
                BookCategoryName = null;
            }
        }

        private void Bookname_listboxswitch_action(object sender, SelectionChangedEventArgs e)
        {
            bool isBooknumbersForBookNos = Booknumbers.Items.Count > 0 && Booknumbers.Items[0] is string key && LibraryBooks.ContainsKey(key);

            BookCategoryNumber = isBooknumbersForBookNos ? (string)Booknumbers.SelectedItem : (string)booknamelist.SelectedItem;
            BookCategoryName = isBooknumbersForBookNos ? (string)booknamelist.SelectedItem : (string)Booknumbers.SelectedItem;


            // and again this checks if the grid is filled
            if (BookCategoryNumber != null && BookCategoryName != null)
            {
                // this update the table with new data 
               display_output.Items.Add(new { BookNum = BookCategoryNumber, BookNm = BookCategoryName, Score = "" });

                // this will check the users answers 
                bool isCorrect = LibraryBooks[BookCategoryNumber] == BookCategoryName;
                GamificationScore(isCorrect, display_output.Items.Count - 1);

                if (isCorrect)
                {
                    MessageBox.Show("Correct Match :)");

                    //when the answer is correct change the tab to green if not red
                    DataGridRow row = (DataGridRow)display_output.ItemContainerGenerator.ContainerFromIndex(display_output.Items.Count - 1);
                    row.Background = Brushes.Green;

                    // this removes correct amswers from the choosable list to prevent duplicates 
                    Booknumbers.Items.Remove(BookCategoryNumber);
                    booknamelist.Items.Remove(BookCategoryName);
                }
                else
                {
                    MessageBox.Show("Incorrect Match :(");

                    // if the answer is red change the row to red 
                    DataGridRow row = (DataGridRow)display_output.ItemContainerGenerator.ContainerFromIndex(display_output.Items.Count - 1);
                    row.Background = Brushes.Red;
                }

              
                Booknumbers.SelectedItem = null;
                booknamelist.SelectedItem = null;
                BookCategoryNumber = null;
                BookCategoryName = null;
            }
        }



        private void Booknumbers_leftbuttonclickx(object sender, MouseButtonEventArgs Position)
        {
            // this gets the current drag position to keep track of the item
            Point itemGPS = Position.GetPosition(null);
            Vector GPSdiff = Location - itemGPS;

            //this checks from the point of click to drag 
            if (Position.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(GPSdiff.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(GPSdiff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // this wil check and get which item is being dragged
                ListBoxItem numberitem = NewTree<ListBoxItem>((DependencyObject)Position.OriginalSource);

                if (numberitem != null)
                {
                 
                    string columnValue = (string)numberitem.DataContext;

                    // this will execute the drag and drop system
                    DataObject dragData = new DataObject("myFormat", columnValue);
                    DragDrop.DoDragDrop(numberitem, dragData, DragDropEffects.Move);
                }
            }
        }

        private void Booknames_leftbuttonclick(object sender, MouseButtonEventArgs Position)
        {
            // this gets the current drag position to keep track of the item
            Point itemGPS = Position.GetPosition(null);
            Vector GPSdiff = Location - itemGPS;

            if (Position.LeftButton == MouseButtonState.Pressed &&(Math.Abs(GPSdiff.X) > SystemParameters.MinimumHorizontalDragDistance ||Math.Abs(GPSdiff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // this wil check and get which item is being dragged
                ListBoxItem NameItem =
                    NewTree<ListBoxItem>((DependencyObject)Position.OriginalSource);

                if (NameItem != null)
                {
                    string columnValue = (string)NameItem.DataContext;


                    // this will execute the drag and drop system
                    DataObject dragData = new DataObject("myFormat", columnValue);
                    DragDrop.DoDragDrop(NameItem, dragData, DragDropEffects.Move);
                }
            }
        }


        private int score = 0;
        /// <summary>
        /// RUBRIC REQUIREMENT[
        /// App Functionality: Gamification feature implemented.
        /// 
        /// RUBRIC REQUIREMENT]
        /// </summary>
        /// <param name="isCorrect"></param>
        /// <param name="rowIndex"></param>
        private void GamificationScore(bool isCorrect, int rowIndex)
        {
            // this will keep track of the score
            if (isCorrect)
            {
                score++;
                AlternateQuestions(true);
            }
            else
            {
                score--;
               
            }
            

            // this will update the answer score as of that answer
            dynamic scorerow = display_output.Items[rowIndex];
            display_output.Items[rowIndex] = new { BookNum = scorerow.BookNum, BookNm = scorerow.BookNm, Score = score };

            // when a score is hit reward the user with an award which can be seen under the button 
            if (score == 6)
            {
               
                bronze_badge.Visibility = Visibility.Visible;
            }
            else if (score == 12) 
            {
                silver_badge.Visibility = Visibility.Visible;
            }
            else if (score == 18)
            {
                gold_badge.Visibility = Visibility.Visible;
            }

        }


        /// <summary>
        /// RUBRIC REQUIREMENT[
        /// App Functionality: App checks whether the selected answers are correct.
        /// RUBRIC REQUIREMENT]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Display_table(object sender, DragEventArgs Query)
        {
            if (Query.Data.GetDataPresent("myFormat"))
            {
                string DisplayColumn = Query.Data.GetData("myFormat") as string;

           //this checks the position of an item 
                if (LibraryBooks.ContainsKey(DisplayColumn))
                {
                 // this item belongs to the book numbers 
                    BookCategoryNumber = DisplayColumn;

                    // this checks for empty field to allow for the user to start in anyway they deem needed 
                    int emptyRowIndex = -1;
                    for (int i = 0; i < display_output.Items.Count; i++)
                    {
                        dynamic row = display_output.Items[i];
                        if (row.BookNum == "")
                        {
                            emptyRowIndex = i;
                            break;
                        }
                    }

                    if (emptyRowIndex >= 0)
                    {
                        // this adds the new item 
                        dynamic row = display_output.Items[emptyRowIndex];
                        display_output.Items[emptyRowIndex] = new { BookNum = BookCategoryNumber, BookNm = row.BookNm, Score = "" };
                        // this checks the answers 
                        bool isCorrect = LibraryBooks[BookCategoryNumber] == row.BookNm;
                        GamificationScore(isCorrect, emptyRowIndex);
                       
                        
                        if (isCorrect)
                        {
                            MessageBox.Show("Correct Match :)");

                         // if the answer is correct it turns green if not red
                            DataGridRow dataGridRow = (DataGridRow)display_output.ItemContainerGenerator.ContainerFromIndex(emptyRowIndex);
                            dataGridRow.Background = Brushes.Green;

                         // this prevents duplicate answers by removign corredt answers 
                            Booknumbers.Items.Remove(BookCategoryNumber);
                            booknamelist.Items.Remove(row.BookNm);
                        }
                        else if (row.BookNm != "")
                        {
                            MessageBox.Show("Incorrect Match :(");
                            // if user gets it wrong highliths in red 
                            DataGridRow dataGridRow = (DataGridRow)display_output.ItemContainerGenerator.ContainerFromIndex(emptyRowIndex);
                            dataGridRow.Background = Brushes.Red;
                        }
                    }
                    else
                    {
                        // updates the table with a new row of items 
                        display_output.Items.Add(new { BookNum = BookCategoryNumber, BookNm = "", Score = "" });
                    }
                                          BookCategoryNumber = null;              }
                else
                {
                    // this item belongs to the next item the book names 
                    BookCategoryName = DisplayColumn;

                    // this will check for an empty row to fill 
                    int empty = -1;
                    for (int i = 0; i < display_output.Items.Count; i++)
                    {
                        dynamic row = display_output.Items[i];
                        if (row.BookNm == "")
                        {
                            empty = i;
                            break;
                        }
                    }

                    if (empty >= 0)
                    {
                        //this will fill the row
                        dynamic row = display_output.Items[empty];
                        display_output.Items[empty] = new { BookNum = row.BookNum, BookNm = BookCategoryName, Score = "" };

                   // this will match the items 
                        bool isCorrect = LibraryBooks[row.BookNum] == BookCategoryName;
                        GamificationScore(isCorrect, empty);

                        if (isCorrect)
                        {
                            MessageBox.Show("Correct Match :)");

                            // if correcy this wil allow for the row to turn green if not red
                            DataGridRow dataGridRow = (DataGridRow)display_output.ItemContainerGenerator.ContainerFromIndex(empty);
                            dataGridRow.Background = Brushes.Green;
                            // prevents duplicates 
                            Booknumbers.Items.Remove(row.BookNum);
                            booknamelist.Items.Remove(BookCategoryName);
                        }
                        else if (row.BookNum != "")
                        {
                            MessageBox.Show("Incorrect Match :(");

                   //changes wrong answers to red
                            DataGridRow dataGridRow = (DataGridRow)display_output.ItemContainerGenerator.ContainerFromIndex(empty);
                            dataGridRow.Background = Brushes.Red;
                        }
                    }
                    else
                    {// if both condtions are met add the data to the display
                        
                        display_output.Items.Add(new { BookNum = "", BookNm = BookCategoryName, Score = "" });
                    }

                 
                    BookCategoryName = null;
                }
            }
        }




        // THIS METHOD CALLS THE RANDOMIZER

        private void Display_List_Buttton(object sender, RoutedEventArgs e)
        {// this generates the random lists 
            MatchMaker();
        }

        //this method return to the mainmenu
        private void Return_toMenu(object sender, RoutedEventArgs e)
        {// go back to main menu 
            
            this.Close();

        }

    
        // this shows the user what level they reached to earn the badge 

        private void Bronze_badge_Click(object sender, RoutedEventArgs e)
        {// badge 1
            MessageBox.Show("user has scored 3 and earned a bronze badge");
        }

        private void Silver_badge_Click(object sender, RoutedEventArgs e)
        {
            // badge 2
            MessageBox.Show("user has scored 6 and earned a silver Badge");
        }

        private void Gold_badge_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("user has scored 9 and earned a gold badge Badge");
        }
    }

    }







    


