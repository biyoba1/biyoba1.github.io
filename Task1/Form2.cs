using System;
using System.Drawing;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using Task1.Models;

namespace Task1
{
    public partial class Form2 : Form
    {
        private readonly string _url = "https://ydstvssozszyririptsi.supabase.co";
        private readonly string _key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Inlkc3R2c3NvenN6eXJpcmlwdHNpIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NzMzNjUxOTQsImV4cCI6MjA4ODk0MTE5NH0.w8ZEsp8CPvvZPowUO92cFDtsPIPhnk3AVwi0ya2lAF4";
        private HttpClient _httpClient;
        private bool _isRegisterMode = false; // false = вход, true = регистрация

        public Form2()
        {
            InitializeComponent();

            // Настраиваем кнопки
            btnLogin.Text = "Войти";
            btnLogin.Click += BtnLogin_Click;
            btnRegister.Click += BtnRegister_Click;
            btnClear.Click += BtnClear_Click;

            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("apikey", _key);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_key}");

            // По умолчанию режим входа
            SetLoginMode();
        }

        //  ВХОД 
        private async void BtnLogin_Click(object sender, EventArgs e)
        {
            if (_isRegisterMode)
            {
                SetLoginMode();
                return;
            }

            label3.Text = "";
            label4.Text = "";

            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text))
            {
                label3.Text = "Введите логин и пароль";
                label3.ForeColor = Color.DarkRed;
                return;
            }

            try
            {
                label3.Text = "Проверка...";
                label3.ForeColor = Color.Blue;

                string username = textBox1.Text.Trim();
                string password = textBox2.Text;
                var url = $"{_url}/rest/v1/users?username=eq.{username}&password=eq.{password}";
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    label3.Text = $"✗ Ошибка сервера: {response.StatusCode}";
                    label3.ForeColor = Color.DarkRed;
                    return;
                }

                var users = JsonSerializer.Deserialize<JsonElement>(content);

                if (users.GetArrayLength() > 0)
                {
                    label3.Text = "Вход выполнен!";
                    label3.ForeColor = Color.DarkGreen;
                    label4.Text = $"Добро пожаловать, {username}!";
                    label4.ForeColor = Color.DarkGreen;

                    await Task.Delay(1000);
                    this.Hide();
                    new Form1().Show();
                }
                else
                {
                    label3.Text = "✗ Неверный логин или пароль";
                    label3.ForeColor = Color.DarkRed;
                    label4.Text = "Или такого пользователя не существует";
                    label4.ForeColor = Color.DarkRed;
                }
            }
            catch (Exception ex)
            {
                label3.Text = $"✗ Ошибка: {ex.Message}";
                label4.Text = ex.InnerException?.Message ?? "Нет деталей";
                label3.ForeColor = Color.DarkRed;
                label4.ForeColor = Color.DarkRed;
            }
        }

        // РЕГИСТРАЦИЯ 
        private async void BtnRegister_Click(object sender, EventArgs e)
        {
            if (!_isRegisterMode)
            {
                SetRegisterMode();
                return;
            }

            label3.Text = "";
            label4.Text = "";

            // Валидация
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text))
            {
                label3.Text = "Заполните все поля";
                label3.ForeColor = Color.DarkRed;
                return;
            }

            if (textBox2.Text != textBox3.Text)
            {
                label3.Text = "Пароли не совпадают";
                label3.ForeColor = Color.DarkRed;
                return;
            }

            if (textBox2.Text.Length < 6)
            {
                label3.Text = "Пароль минимум 6 символов";
                label3.ForeColor = Color.DarkRed;
                return;
            }

            try
            {
                label3.Text = "Проверка пользователя...";
                label3.ForeColor = Color.Blue;

                string username = textBox1.Text.Trim();
                string password = textBox2.Text;

                // Проверяем существование пользователя
                var checkUrl = $"{_url}/rest/v1/users?username=eq.{username}";
                var checkResponse = await _httpClient.GetAsync(checkUrl);
                var checkContent = await checkResponse.Content.ReadAsStringAsync();

                var existingUsers = JsonSerializer.Deserialize<JsonElement>(checkContent);
                if (existingUsers.GetArrayLength() > 0)
                {
                    label3.Text = "Пользователь уже существует";
                    label3.ForeColor = Color.DarkRed;
                    return;
                }

                label3.Text = "Создание пользователя...";

                // Создаем пользователя
                var userData = new
                {
                    username = username,
                    password = password
                };

                var json = JsonSerializer.Serialize(userData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var insertUrl = $"{_url}/rest/v1/users";
                var response = await _httpClient.PostAsync(insertUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    label3.Text = "✓ Регистрация успешна!";
                    label3.ForeColor = Color.DarkGreen;
                    label4.Text = $"Теперь войдите, {username}!";
                    label4.ForeColor = Color.DarkGreen;

                    textBox1.Clear();
                    textBox2.Clear();
                    textBox3.Clear();
                    await Task.Delay(1500);
                    SetLoginMode();
                }
                else
                {
                    label3.Text = $"✗ Ошибка: {response.StatusCode}";
                    label4.Text = responseContent;
                    label3.ForeColor = Color.DarkRed;
                    label4.ForeColor = Color.DarkRed;
                }
            }
            catch (Exception ex)
            {
                label3.Text = $"✗ Ошибка: {ex.Message}";
                label4.Text = ex.InnerException?.Message ?? "Нет деталей";
                label3.ForeColor = Color.DarkRed;
                label4.ForeColor = Color.DarkRed;
            }
        }

        //ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ
        private void SetLoginMode()
        {
            _isRegisterMode = false;
            btnLogin.Text = "Войти";
            btnRegister.Text = "Регистрация";
            this.Text = "Вход в систему";

            // Скрываем третье поле 
            textBox3.Visible = false;
            label2.Text = "Пароль:";

            label3.Text = "";
            label4.Text = "";
        }

        private void SetRegisterMode()
        {
            _isRegisterMode = true;
            btnLogin.Text = "← Вход";
            btnRegister.Text = "Зарегистрироваться";
            this.Text = "Регистрация";

            // Показываем третье поле 
            textBox3.Visible = true;
            label2.Text = "Пароль:";

            label3.Text = "";
            label4.Text = "";
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            label3.Text = "";
            label4.Text = "";
            textBox1.Focus();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _httpClient?.Dispose();
            base.OnFormClosing(e);
        }
    }
}