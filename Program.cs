using System.Diagnostics;

public class Program
{
    public static void Main(string[] args)
    {

        #region Subject
        int subjectId = HelperClass.ReadInt("Enter subject ID: ");
        string subjectName = HelperClass.ReadNonEmptyString("Enter subject name: ");

        Subject subject = new Subject(subjectId, subjectName);
        #endregion

        #region EX_type
        Console.WriteLine("\nChoose Exam Type:");
        Console.WriteLine("1. Final Exam");
        Console.WriteLine("2. Practical Exam");

        int Exam_choice;
        while (true)
        {
            Exam_choice = HelperClass.ReadInt("(1 or 2):");
            if (Exam_choice == 1 || Exam_choice == 2)
            {
                break;
            }
            Console.WriteLine("Invalid choice, please enter 1 or 2.");
        }
        #endregion

        #region Duration and Num_Questions
        int duration = HelperClass.ReadInt("\nEnter Exam Duration 30 to 180 minutes: ");
        while (duration < 30 || duration > 180)
        {
            Console.WriteLine("Duration must be between 30 and 180 minutes.");
            duration = HelperClass.ReadInt("\nEnter Exam Duration 30 to 180 minutes: ");
        }

        int numQuestions = HelperClass.ReadInt("Enter Number of Questions: ");
        while (numQuestions < 0)
        {
            Console.WriteLine("Number of questions must be greater than 0.");
            numQuestions = HelperClass.ReadInt("Enter Number of Questions: ");
        }

        Console.WriteLine("--------------------------------------");

        Question[] questions = new Question[numQuestions];
        #endregion

        #region Questions
        for (int i = 0; i < numQuestions; i++)
        {
            Console.WriteLine($"Question {i + 1}:");

            string QuestionType;
            if (Exam_choice == 2)
            {
                QuestionType = "mcq";
            }
            else
            {
                QuestionType = HelperClass.ReadNonEmptyString("Enter Question Type (mcq or tf): ").ToLower();
                while (true)
                {
                    if (QuestionType == "mcq" || QuestionType == "tf")
                    {
                        break;
                    }
                    Console.WriteLine("Invalid question type. Please enter 'mcq' or 'tf'.");
                    QuestionType = HelperClass.ReadNonEmptyString("Enter Question Type (mcq or tf): ").ToLower();
                }
            }

            string header = HelperClass.ReadNonEmptyString("Enter Question Header: ");
            string body = HelperClass.ReadNonEmptyString("Enter Question Body: ");

            int mark = HelperClass.ReadInt("Enter Question Mark: ");
            while (mark <= 0)
            {
                Console.WriteLine("Mark must be greater than 0.");
                mark = HelperClass.ReadInt("Enter Mark: ");
            }
            Console.WriteLine("--------------------------------------");

            Answer[] answers;
            int rightAnswerId;

            if (QuestionType == "tf")
            {
                answers = new Answer[]
                {
                    new Answer (1,"True"),
                    new Answer (2,"False"),
                };

                rightAnswerId = HelperClass.ReadInt("Enter Right Answer ID (1 for True, 2 for False): ");
                while (rightAnswerId != 1 && rightAnswerId != 2)
                {
                    Console.WriteLine("Right Answer must be 1 or 2.");
                    rightAnswerId = HelperClass.ReadInt("Enter Right Answer (1 = True, 2 = False): ");
                }

                questions[i] = new TrueFalseQuestion(rightAnswerId,mark,header,body,answers);
            }
            else
            {
                answers = new Answer[4];

                for (int j = 0; j < 4; j++)
                {
                    string answerText = HelperClass.ReadNonEmptyString($"Enter Answer {j + 1} Text: ");
                    answers[j] = new Answer(j + 1, answerText);
                }

                rightAnswerId = HelperClass.ReadInt("Enter Right Answer Id (1-4): ");
                while (rightAnswerId < 1 || rightAnswerId > 4)
                {
                    Console.WriteLine($"Right Answer Id must be between 1 and 4.");
                    rightAnswerId = HelperClass.ReadInt($"Enter Right Answer Id (1-4): ");
                }

                questions[i] = new MCQ_Question(rightAnswerId, mark, header, body, answers);

            }
            Console.WriteLine("--------------------------------------");
        }
        #endregion

        #region Exam Creation

        string startExam = HelperClass.ReadNonEmptyString("Starting Exam Now? (y/n): ").ToLower();

        if (startExam == "y")
        {
            Console.WriteLine("--- Now Answer Each Question ---");

            Stopwatch examTimer = Stopwatch.StartNew();

            foreach (var question in questions)
            {
                question.ShowQuestion();

                int maxAnswerId = question.AnswerList.Length;
                int selected = HelperClass.ReadInt($"Your Answer Id (1-{maxAnswerId}): ");
                while (selected < 1 || selected > maxAnswerId)
                {
                    Console.WriteLine($"Please enter a valid Answer Id between 1 and {maxAnswerId}.");
                    selected = HelperClass.ReadInt($"Your Answer Id (1-{maxAnswerId}): ");
                }

                question.SelectedAnswerId = selected;
                Console.WriteLine();
            }

            examTimer.Stop();

            if (Exam_choice == 1)
            {
                subject.CreateFinalExam(duration, numQuestions, questions);
            }
            else
            {
                subject.CreatePracticalExam(duration, numQuestions, questions);
            }

            TimeSpan elapsed = examTimer.Elapsed;
            Console.WriteLine($"\nTime Taken: {elapsed.Minutes} min {elapsed.Seconds} sec");
            Console.WriteLine();
            Console.WriteLine(subject);
            subject.Sup_Exam.ShowExam();
            Console.WriteLine("Thank You");
        }
        else
            Console.WriteLine("OK byee");

        #endregion
    }
}