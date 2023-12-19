using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;

namespace dewey_decimal_app
{
    /// <summary>
    /// THIS DEALS WITH PART 1 OF THE POE
    /// </summary>
    public partial class BookOrdering : Window
    {
        public BookOrdering()
        {
            InitializeComponent();
            //this allows me to set objects for the lists 
            QuestionsDisplay.SelectionChanged += QuestionsList;
            AnswersDisplay.SelectionChanged += AnswersList;
            AnswersDisplay.ItemContainerGenerator.ItemsChanged += Answers_Check;
            //hiding achievments so they cant be seen or hovered over
            gold.Visibility = Visibility.Collapsed;
            silver.Visibility = Visibility.Collapsed;
            bronze.Visibility = Visibility.Collapsed;

            // the submit button is only enabled if the user has submitted at least 1 option
            if (AnswersDisplay.Items.Count > 0)
            {
                submit.IsEnabled = AnswersDisplay.Items.Count > 0;
            }
            else { submit.IsEnabled = false; }
            //an action method on the window close to back to home screen 
            Closing += OnExit;
            newlevel();
        }
        //debug window 
        testwindow tests = new testwindow();

        // storing the data in a list as per the rubric requirements
        public List<string> DeweyDecimalQuestions = new List<string>();
        public List<string> DeweyDecimalAnswer = new List<string>();


       
        
        //this deals wih the exit so when the user exits it auto goes back to home page 
    private void OnExit(object sender, CancelEventArgs e)
    {
            scorings();
       
    }


        // this will generate the random books from the text file 
    private void randombooks(object sender, RoutedEventArgs e)
        {
            // clearing the form on new generation 
            tests.myListBox.Items.Clear();
            tests.myListBox2.Items.Clear();
            DeweyDecimalQuestions.Clear();
            DeweyDecimalAnswer.Clear();
            AnswersDisplay.Items.Clear();
            QuestionsDisplay.Items.Clear();

         // this will import the data from the text file 
            string[] importdata = File.ReadAllLines("Choices.txt");

            // randomiser method from c# 
            Random books = new Random();
            int lenthg = importdata.Length;
            while (lenthg > 1)
            {// checking the lenth 
                lenthg--;
                int nextposition = books.Next(lenthg + 1);
                (importdata[lenthg], importdata[nextposition]) = (importdata[nextposition], importdata[lenthg]);
            }


            for (int position = 0; position < 10; position++)
            {
               //adding to the lists till the count of ten
                DeweyDecimalQuestions.Add(importdata[position]);
                QuestionsDisplay.Items.Add(importdata[position]);
            }
        }
        private void Forcorrectattempts()
        {//if correct it should autogenerate the next batch or when clicked 
            tests.myListBox.Items.Clear();
            tests.myListBox2.Items.Clear();
            DeweyDecimalQuestions.Clear();
            DeweyDecimalAnswer.Clear();
            AnswersDisplay.Items.Clear();
            QuestionsDisplay.Items.Clear();
            string[] DataStream = File.ReadAllLines("Choices.txt");

            Random production = new Random();
            int lenthc = DataStream.Length;
            while (lenthc > 1)
            {
                lenthc--;
                int nextbook = production.Next(lenthc + 1);
                (DataStream[lenthc], DataStream[nextbook]) = (DataStream[nextbook], DataStream[lenthc]);
            }


            for (int position = 0; position < 10; position++)
            {

                DeweyDecimalQuestions.Add(DataStream[position]);
                QuestionsDisplay.Items.Add(DataStream[position]);
            }
        }

        //this is for the question listbox which prints all the questions 
        private void QuestionsList(object sender, SelectionChangedEventArgs e)
        {
            if (QuestionsDisplay.SelectedItem != null)
            {//adding to the questions list 
                string chosenbooks = QuestionsDisplay.SelectedItem.ToString();
                QuestionsDisplay.Items.Remove(chosenbooks);
                AnswersDisplay.Items.Add(chosenbooks);
                //checking the count of the list
                if (AnswersDisplay.Items.Count > 0)
                {//while empty adding to the list
                    submit.IsEnabled = AnswersDisplay.Items.Count > 0;
                }
                else { submit.IsEnabled = false; }

            }
        }
        // this prints all the answers
        private void AnswersList(object sender, SelectionChangedEventArgs e)
        {//adding to the ansers list 
            if (AnswersDisplay.SelectedItem != null)
            {
       //ths adds the text file data to the asnwers list 
                string originalbooks = AnswersDisplay.SelectedItem.ToString();
                AnswersDisplay.Items.Remove(originalbooks);
                QuestionsDisplay.Items.Add(originalbooks);
                //checking if its 0
                if (AnswersDisplay.Items.Count > 0)
                {
                    submit.IsEnabled = AnswersDisplay.Items.Count > 0;
                }
                else { submit.IsEnabled=false; }

            }
        }



        // if by bug or odd chnace the auto answer checking doesnt work the user can click this to check thier answers
        private void Manual_Submission(object sender, RoutedEventArgs e)
        {
           
 //clearing answers 
            DeweyDecimalAnswer.Clear();
            //checking count 
            for (int position = 0; position < AnswersDisplay.Items.Count; position++)
            {
                string answers = AnswersDisplay.Items[position].ToString();
                if (answers != null)
                    DeweyDecimalAnswer.Add(answers);
            }


        
            
            // sorting and checking the answers by merge sort 
            Algorithimsortbymerge(DeweyDecimalQuestions, 0, DeweyDecimalQuestions.Count - 1);
          
            //bool for checking 
            bool correctanswer = true;
            if (DeweyDecimalAnswer.Count == DeweyDecimalQuestions.Count && DeweyDecimalAnswer.Count>0)
            {
                for (int position = 0; position < DeweyDecimalQuestions.Count; position++)
                {
                    if (!DeweyDecimalQuestions[position].Equals(DeweyDecimalAnswer[position]))
                    {
                        correctanswer = false;
                        break;
                    }
                }
            }
            else
            {
                correctanswer = false;
            }
            // if the answer is correct show that method and say correct 
            if (correctanswer)
            {
                Forcorrectattempts();
                MessageBox.Show("CORRECT!");

                
            }
            else
            {// same way if incorrect 
                ForIncorrect_CorrectAttempts();
                MessageBox.Show("incorrect");
    
            }
        }

        // if the count hits 10 this will autocheck the answers 
        private void Answers_Check(object sender, ItemsChangedEventArgs e)
        {//checking answers and displaying the approtiate response 
            //seein if the count equals 10 
            if (AnswersDisplay.Items.Count == 10)
            {
                ForIncorrect_CorrectAttempts();
            }
        }
        // where correct and incorrect is amgious or both needed this will be called 
        private void ForIncorrect_CorrectAttempts()
        {// clearing ther list 

            DeweyDecimalAnswer.Clear();
            //checking the count 
            for (int i = 0; i < AnswersDisplay.Items.Count; i++)
            {// if below tje level add to the list 
                string item = AnswersDisplay.Items[i].ToString();
                if (item != null)
                    DeweyDecimalAnswer.Add(item);
            }

      
            // checking the order using merge sort 
            Algorithimsortbymerge(DeweyDecimalQuestions, 0, DeweyDecimalQuestions.Count - 1);
            

            bool isCorrect = true;
            if (DeweyDecimalAnswer.Count == DeweyDecimalQuestions.Count && DeweyDecimalAnswer.Count > 0)
            {
                for (int i = 0; i < DeweyDecimalQuestions.Count; i++)
                {
                    if (!DeweyDecimalQuestions[i].Equals(DeweyDecimalAnswer[i]))
                    {
                        isCorrect = false;
                        break;
                    }
                }
            }
            else
            {// thos activates the scoring 
                isCorrect = false;
            }

            if (isCorrect)
            {// activates the postive score 
                MessageBox.Show("CORRECT!");
                GamificationScore(isCorrect);
            }
            else
            {// activates the negative score  
                MessageBox.Show("INCORRECT, Please try again");
                AnswersDisplay.Items.Clear();
                GamificationScore(isCorrect);
                foreach (string incorrect in DeweyDecimalAnswer)
                {
                    QuestionsDisplay.Items.Add(incorrect);
                }

            }
        }
        // this calls the merge sort 
            public static void Algorithimsortbymerge(List<string> list, int callnum, int letters)
        {
            if (callnum < letters)
            {
                int decimalnum = (callnum + letters) / 2;
                Algorithimsortbymerge(list, callnum, decimalnum);
                Algorithimsortbymerge(list, decimalnum + 1, letters);
                MergeSortConditions(list, callnum, decimalnum, letters);
            }
        }
        //A merge sort uses a technique called divide and conquer.The list is repeatedly divided into two until all the elements are separated individually.Pairs of elements are then compared, placed into order and combined.The process is then repeated until the list is recompiled as a whole.

        //merge sort algorithim
        private static void MergeSortConditions(List<string> books, int callnumstart, int decimalnum, int letters)
        {
            int n1 = decimalnum - callnumstart + 1;
            int n2 = letters - decimalnum;
            string[] strings = new string[n1];
            string[] strings2 = new string[n2];
            for (int positions = 0; positions < n1; positions++)
                strings[positions] = books[callnumstart + positions];
            for (int position = 0; position < n2; position++)
                strings2[position] = books[decimalnum + 1 + position];
            int callnum = callnumstart;
            int incremnet1 = 0;
            int increment2 = 0;
            while (incremnet1 < n1 && increment2 < n2)
            {
                if (strings[incremnet1].CompareTo(strings2[increment2]) <= 0)
                {
                    books[callnum] = strings[incremnet1];
                    incremnet1++;
                }
                else
                {
                    books[callnum] = strings2[increment2];
                    increment2++;
                }
                callnum++;
            }
            while (incremnet1 < n1)
            {
                books[callnum] = strings[incremnet1];
                incremnet1++;
                callnum++;
            }
            while (increment2 < n2)
            {
                books[callnum] = strings2[increment2];
                increment2++;
                callnum++;
            }
        }

      // this closesthe program 

        private void OnExit(object sender, RoutedEventArgs e)
        {
            
            this.Close();

        }
        //this is the gamification workings 
        private int score = 0;
        private void GamificationScore(bool isCorrect)
        {
            // this will keep track of the score
            if (isCorrect)
            {
                score++;
            }
            else
            {
                score--;
            }
           scoring.Text = score.ToString();

            // this will update the answer score as of that answer
            

            // when a score is hit reward the user with an award which can be seen under the button 
            if (score == 2)
            {

                bronze.Visibility = Visibility.Visible;
            }
            else if (score == 4)
            {
                silver.Visibility = Visibility.Visible;
            }
            else if (score == 6)
            {
                gold.Visibility = Visibility.Visible;
            }

        }
 

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

        public void scorings()
        {

            int newlevel;


            // IF NO THEY RETURN TO MENU IF YES THEY KEEP PLAYING 
            MessageBoxResult Confirm = MessageBox.Show("Do you want to save your score:", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (Confirm == MessageBoxResult.Yes)
            {// IF YES THE SCORES ARE WRITTEN TO A TEXT FILE CALLED SCORS TO KEEP TRACK 
                string scoresPath = "ScoresBookorder.txt";
                // WE USE A STREAM WRITER
                using (StreamWriter outputFile = new StreamWriter(scoresPath))
                {// if the user chooses yes it will then save the scores to the files
                    newlevel = 1;
                    outputFile.WriteLine(newlevel + "," + score );
                }
                // this tills the main window not to go to home

                var newForm = new HomePage();
                Dispatcher.BeginInvoke(new Action(() => newForm.Show()));
            }// if the message box button is no 
            else if (Confirm == MessageBoxResult.No)
            {
                string scorestxt = "ScoresBookorder.txt";

                // reset the scores  then quit which opens the homepage 
                using (StreamWriter ToFile = new StreamWriter(scorestxt))
                {//set all scores to zero
                    newlevel = 0;
           
                    score = 0;
                    ToFile.WriteLine(newlevel + "," + score);

                }
                //closes the window
                var newForm = new HomePage();
                Dispatcher.BeginInvoke(new Action(() => newForm.Show()));
            }
        }
        public void newlevel()
        {// SCORES ARE STORED IN THE TXT FILE CALL SCORS
            string scoresPath = "ScoresBookorder.txt";

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
                    
                        int scores = int.Parse(DATA[1].Trim());
                        // HERE WE CHECK WHAT LEVEL THEY ARE ON
                        if (level == 0)
                        {
                            MessageBox.Show("level 1");
                        }
                        else if (level == 1)
                        {
                            
                            score = scores;
                     scoring.Text = "SCORE:" + "\n" + score.ToString();

                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);

                    }

                }

            }
        }
    }
}

