using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using DrivingSkillCert.DAO;
using Model.Models;

namespace DrivingSkillCert
{
    public partial class DoTheoryExam : Page
    {
        private readonly ExamDAO _examDAO;
        private readonly QuestionDAO _questionDAO;
        private readonly AnswerDAO _answerDAO;
        private readonly ResultDAO _resultDAO;
        private readonly User _currentUser;
        private readonly Exam _currentExam;
        private List<QuestionViewModel> _questions;
        private DispatcherTimer _timer;
        private TimeSpan _timeLeft;

        public DoTheoryExam(Exam exam)
        {
            InitializeComponent();
            _examDAO = new ExamDAO();
            _questionDAO = new QuestionDAO();
            _answerDAO = new AnswerDAO();
            _resultDAO = new ResultDAO();
            _currentUser = (User)Application.Current.Properties["LoggedInUser"];
            _currentExam = exam;
            _timeLeft = TimeSpan.FromMinutes(25);
            LoadQuestions();
            StartTimer();
        }

        private void LoadQuestions()
        {
            try
            {
                // Lấy tất cả câu hỏi từ BankQuestion của Course
                var allQuestions = _questionDAO.GetQuestionsByCourseId(_currentExam.CourseId);
                if (allQuestions.Count < 25)
                {
                    MessageBox.Show("Not enough questions available for this exam.");
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        NavigationService?.GoBack();
                    }), DispatcherPriority.Background);
                    return;
                }

                // Chọn ngẫu nhiên 25 câu
                _questions = allQuestions.OrderBy(q => Guid.NewGuid()).Take(25).Select(q => new QuestionViewModel
                {
                    QuestionId = q.QuestionId,
                    QuestionText = q.Question1,
                    Answers = _answerDAO.GetAnswersByQuestionId(q.QuestionId).Select(a => new AnswerViewModel
                    {
                        AnswerId = a.AnswerId,
                        AnswerText = a.Answer1,
                        IsCorrect = a.IsTrue.Value,
                        IsSelected = false
                    }).ToList()
                }).ToList();

                icQuestions.ItemsSource = _questions;
                txtExamInfo.Text = $"Exam: {_currentExam.Course.CourseName} - {_currentExam.Date:yyyy-MM-dd HH:mm}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading questions: {ex.Message}");
            }
        }

        private void StartTimer()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _timeLeft = _timeLeft.Subtract(TimeSpan.FromSeconds(1));
            txtTimer.Text = $"Time Left: {_timeLeft:mm\\:ss}";

            if (_timeLeft <= TimeSpan.Zero)
            {
                _timer.Stop();
                SubmitExam();
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            SubmitExam();
        }

        private void SubmitExam()
        {
            try
            {
                // Tính điểm
                int correctAnswers = _questions.Count(q => q.Answers.Any(a => a.IsSelected && a.IsCorrect));
                decimal score = (decimal)correctAnswers / 25 * 100; // Tính điểm trên thang 100
                bool passStatus = score >= 50; // 50% là đậu

                // Lưu kết quả vào Results
                var result = new Result
                {
                    ExamId = _currentExam.ExamId,
                    UserId = _currentUser.UserId,
                    Score = score,
                    PassStatus = passStatus,
                    IsDelete = false
                };
                _resultDAO.AddResult(result);

                MessageBox.Show($"Exam completed!\nScore: {score:F2}/100\nStatus: {(passStatus ? "Pass" : "Fail")}");
                NavigationService?.Navigate(new ExamStudent { CameFromTheoryExam = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting exam: {ex.Message}");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel the exam? Your progress will not be saved.",
                "Confirm Cancel", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _timer?.Stop();
                NavigationService?.GoBack();
            }
        }
    }

    public class QuestionViewModel
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public List<AnswerViewModel> Answers { get; set; }
    }

    public class AnswerViewModel
    {
        public int AnswerId { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
        public bool IsSelected { get; set; }
    }
}