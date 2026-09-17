using System;
using System.Collections.Generic;
using System.Text;

public class Answer
{
    public int AnswerId { get; set; }
    public string AnswerText { get; set; }

    public Answer(int answerId, string answerText)
    {
        AnswerId = answerId;
        AnswerText = answerText;
    }

    public override string ToString()
    {
        return $"[{AnswerId}] - {AnswerText}";
    }
}

public abstract class Question : ICloneable, IComparable<Question>
{
    public int RightAnswerId { get; set; }
    public int Mark { get; set; }
    public string Header { get; set; }
    public string Body { get; set; }
    public Answer[] AnswerList { get; set; }
    public int SelectedAnswerId { get; set; }
    public Question(int rightAnswerId, int mark, string header, string body, Answer[] answerList)
    {
        RightAnswerId = rightAnswerId;
        Mark = mark;
        Header = header;
        Body = body;
        AnswerList = answerList;
        SelectedAnswerId = 0;
    }

    public virtual object Clone()
    {
        return this.MemberwiseClone();
    }

    public int CompareTo(Question? other)
    {
        if (other == null) return 1;
        return this.Mark.CompareTo(other.Mark);
    }

    public abstract void ShowQuestion();

    public override string ToString()
    {
        return $"{Header} - {Body} | Mark: {Mark}";
    }
}

public class TrueFalseQuestion : Question
{
    public TrueFalseQuestion(int rightAnswerId, int mark, string header, string body, Answer[] answerList) : base(rightAnswerId, mark, header, body, answerList)
    {
        if (answerList.Length != 2)
            throw new ArgumentException("True/False question must have exactly 2 answers.");
    }

    public override void ShowQuestion()
    {
        Console.WriteLine($"Q: {Header}");
        Console.WriteLine($"[True/False - mark {Mark}]");
        Console.WriteLine(Body);
        foreach (var answer in AnswerList)
        {
            Console.WriteLine(answer);
        }
    }
}

public class MCQ_Question : Question
{
    public MCQ_Question(int rightAnswerId, int mark, string header, string body, Answer[] answerList) : base(rightAnswerId, mark, header, body, answerList)
    {
    }

    public override void ShowQuestion()
    {
        Console.WriteLine($"Q: {Header}");
        Console.WriteLine($"[MCQ - mark {Mark}]");
        Console.WriteLine(Body);

        foreach (var answer in AnswerList)
        {
            Console.WriteLine(answer);
        }
    }
}

public abstract class Exam
{
    public int Time_Exam { get; set; }
    public int Num_Questions { get; set; }
    public Question[] Questions { get; set; }

    public Exam(int time_Exam, int num_Questions, Question[] questions)
    {
        Time_Exam = time_Exam;
        Num_Questions = num_Questions;
        Questions = questions;
    }

    public abstract void ShowExam();

    public override string ToString()
    {
        return $"Time of Exam : {Time_Exam} Minutes, {Num_Questions} question";
    }
}

public class FinalExam : Exam
{
    public FinalExam(int time_Exam, int num_Questions, Question[] questions) : base(time_Exam, num_Questions, questions)
    {
    }
    public override void ShowExam()
    {
        Console.WriteLine($"-----Final Exam----- \nTime: {Time_Exam} Hour\nNumber of Questions: {Num_Questions}");
        int totalGrade = 0;
        int maxGrade = 0;
        foreach (var question in Questions)
        {
            question.ShowQuestion();

            string selectedText = default;
            string correctText = default;

            foreach (var answer in question.AnswerList)
            {
                if (answer.AnswerId == question.RightAnswerId)
                {
                    correctText = answer.AnswerText;
                }

                if (answer.AnswerId == question.SelectedAnswerId)
                {
                    selectedText = answer.AnswerText;
                }
            }

            if (question.SelectedAnswerId == question.RightAnswerId)
            {
                totalGrade += question.Mark;
            }

            maxGrade += question.Mark;
            Console.WriteLine();
            Console.WriteLine($"Your Answer: {selectedText}");
            Console.WriteLine($"Correct Answer: {correctText}");
            Console.WriteLine();
        }
        Console.WriteLine($"Grade: {totalGrade} / {maxGrade}");
    }

}

public class PracticalExam : Exam
{
    public PracticalExam(int time_Exam, int num_Questions, Question[] questions) : base(time_Exam, num_Questions, questions)
    {
        foreach (var question in questions)
        {
            if (!(question is MCQ_Question))
            {
                throw new ArgumentException("Practical Exam can only contain MCQ questions.");
            }
        }
    }

    public override void ShowExam()
    {
        Console.WriteLine($"-----Practical Exam----- \nTime: {Time_Exam} Hour\nNumber of Questions: {Num_Questions}");
        foreach (var question in Questions)
        {
            question.ShowQuestion();

            string selectedText = default;
            string correctText = default;

            foreach (var answer in question.AnswerList)
            {
                if (answer.AnswerId == question.RightAnswerId)
                {
                    correctText = answer.AnswerText;
                }

                if (answer.AnswerId == question.SelectedAnswerId)
                {
                    selectedText = answer.AnswerText;
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Your Answer: {selectedText}");
            Console.WriteLine($"Correct Answer: {correctText}");
            Console.WriteLine();
        }
    }
}

public class Subject
{
    public int ID { get; set; }
    public string Name { get; set; }
    public Exam Sup_Exam { get; set; }

    public Subject(int id, string name)
    {
        ID = id;
        Name = name;
        Sup_Exam = default;
    }

    public void CreateFinalExam(int examDurationInHours, int numberOfQuestions, Question[] questions)
    {
        Sup_Exam = new FinalExam(examDurationInHours, numberOfQuestions, questions);
    }

    public void CreatePracticalExam(int examDurationInHours, int numberOfQuestions, Question[] questions)
    {
        Sup_Exam = new PracticalExam(examDurationInHours, numberOfQuestions, questions);
    }

    public override string ToString()
    {
        return $"Subject ID: {ID}, Name: {Name}";
    }
}