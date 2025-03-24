using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DrivingSkillCert.DAO;
using Model.Models;

namespace DrivingSkillCert
{
    public partial class ManageQuestionsAndAnswers : Page
    {
        private readonly QuestionDAO _questionDAO;
        private readonly AnswerDAO _answerDAO;
        private readonly BankQuestion _bankQuestion;
        private List<Question> _questions;
        private Question _currentQuestion;
        private List<Answer> _currentAnswers;

        public ManageQuestionsAndAnswers(BankQuestion bankQuestion)
        {
            InitializeComponent();
            _questionDAO = new QuestionDAO();
            _answerDAO = new AnswerDAO();
            _bankQuestion = bankQuestion;
            _currentAnswers = new List<Answer>();
            LoadData();
        }

        private void LoadData()
        {
            _questions = _questionDAO.GetQuestionsByBankId(_bankQuestion.BankId);
            dgQuestions.ItemsSource = _questions;
            txtBankName.Text = $"Managing Questions for: {_bankQuestion.BankName}";
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

        private void btnAddQuestion_Click(object sender, RoutedEventArgs e)
        {
            _currentQuestion = new Question { BankId = _bankQuestion.BankId };
            _currentAnswers = new List<Answer>();
            txtQuestion.Text = string.Empty;
            icAnswers.ItemsSource = _currentAnswers;
            detailPanel.Visibility = Visibility.Visible;
        }

        private void btnEditQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Question question)
            {
                _currentQuestion = question;
                _currentAnswers = _answerDAO.GetAnswersByQuestionId(question.QuestionId);
                txtQuestion.Text = question.Question1;
                icAnswers.ItemsSource = _currentAnswers;
                detailPanel.Visibility = Visibility.Visible;
            }
        }

        private void btnDeleteQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Question question)
            {
                if (MessageBox.Show($"Are you sure you want to delete question '{question.Question1}'?",
                    "Confirm Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    try
                    {
                        _questionDAO.DeleteQuestion(question.QuestionId);
                        _questions.Remove(question);
                        dgQuestions.ItemsSource = null;
                        dgQuestions.ItemsSource = _questions;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error deleting question: {ex.Message}");
                    }
                }
            }
        }


        private void btnAddAnswer_Click(object sender, RoutedEventArgs e)
        {
            _currentAnswers.Add(new Answer { Answer1 = "", IsTrue = false });
            icAnswers.ItemsSource = null;
            icAnswers.ItemsSource = _currentAnswers;
        }

        private void btnRemoveAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Answer answer)
            {
                _currentAnswers.Remove(answer);
                icAnswers.ItemsSource = null;
                icAnswers.ItemsSource = _currentAnswers;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtQuestion.Text) || _currentAnswers.Count == 0)
            {
                MessageBox.Show("Please enter a question and at least one answer.");
                return;
            }

            try
            {
                _currentQuestion.Question1 = txtQuestion.Text;
                if (_currentQuestion.QuestionId > 0)
                {
                    _questionDAO.UpdateQuestion(_currentQuestion);
                    _answerDAO.DeleteAnswersByQuestionId(_currentQuestion.QuestionId); // Xóa đáp án cũ
                }
                else
                {
                    _questionDAO.CreateQuestion(_currentQuestion);
                }

                foreach (var answer in _currentAnswers)
                {
                    answer.QuestionId = _currentQuestion.QuestionId;
                    answer.AnswerId = 0;
                    _answerDAO.CreateAnswer(answer);
                }

                detailPanel.Visibility = Visibility.Collapsed;
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving question: {ex.Message}");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            detailPanel.Visibility = Visibility.Collapsed;
            _currentQuestion = null;
            _currentAnswers.Clear();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
    }
}