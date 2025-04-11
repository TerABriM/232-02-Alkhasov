using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace StudentsApp
{
    public partial class Form1 : Form
    {                   
        private BindingList<Student> students = new BindingList<Student>();
        private string currentFilePath = string.Empty;
        private bool isModified = false;

        public Form1()
        {
            InitializeComponent();
            SetupDataGridView();
            SetupEventHandlers();
            InitializeDateTimePicker();
        }

        private void SetupDataGridView()
        {
            dataGridView.DataSource = students;
            dataGridView.AutoGenerateColumns = true;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void SetupEventHandlers()
        {
            btnAdd.Click += (s, e) => AddStudent();
            btnEdit.Click += (s, e) => EditStudent();
            btnDelete.Click += (s, e) => DeleteStudent();
            btnSave.Click += (s, e) => SaveData();
            btnLoad.Click += (s, e) => LoadData();
            btnExport.Click += (s, e) => ExportToCsv();
            btnImport.Click += (s, e) => ImportFromCsv();
            btnStats.Click += (s, e) => ShowStatistics();

            dataGridView.SelectionChanged += (s, e) => UpdateFormWithSelectedStudent();
            dataGridView.SortCompare += DataGridView_SortCompare;

            foreach (Control control in Controls)
            {
                if (control is TextBox)
                {
                    control.TextChanged += (s, e) => isModified = true;
                }
            }
        }

        private void InitializeDateTimePicker()
        {
            dateTimePickerBirthDate.Format = DateTimePickerFormat.Custom;
            dateTimePickerBirthDate.CustomFormat = "dd.MM.yyyy";
            dateTimePickerBirthDate.MinDate = new DateTime(1991, 12, 25);
            dateTimePickerBirthDate.MaxDate = DateTime.Today;
        }

        private void AddStudent()
        {
            if (!ValidateInputs()) return;

            var student = new Student
            {
                LastName = txtLastName.Text,
                FirstName = txtFirstName.Text,
                MiddleName = txtMiddleName.Text,
                Course = int.Parse(txtCourse.Text),
                Group = txtGroup.Text,
                BirthDate = dateTimePickerBirthDate.Value,
                Email = txtEmail.Text,
                Phone = txtPhone.Text
            };

            students.Add(student);
            isModified = true;
            ClearForm();
        }

        private void EditStudent()
        {
            if (dataGridView.SelectedRows.Count == 0) return;
            if (!ValidateInputs()) return;

            var selectedStudent = (Student)dataGridView.SelectedRows[0].DataBoundItem;

            selectedStudent.LastName = txtLastName.Text;
            selectedStudent.FirstName = txtFirstName.Text;
            selectedStudent.MiddleName = txtMiddleName.Text;
            selectedStudent.Course = int.Parse(txtCourse.Text);
            selectedStudent.Group = txtGroup.Text;
            selectedStudent.BirthDate = dateTimePickerBirthDate.Value;
            selectedStudent.Email = txtEmail.Text;
            selectedStudent.Phone = txtPhone.Text;

            students.ResetBindings();
            isModified = true;
        }

        private void DeleteStudent()
        {
            if (dataGridView.SelectedRows.Count == 0) return;

            if (MessageBox.Show("Вы уверены, что хотите удалить выбранного студента?",
                "Подтверждение удаления", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                var selectedStudent = (Student)dataGridView.SelectedRows[0].DataBoundItem;
                students.Remove(selectedStudent);
                isModified = true;
                ClearForm();
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtLastName.Text))
                return ShowError("Фамилия обязательна для заполнения", txtLastName);
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
                return ShowError("Имя обязательно для заполнения", txtFirstName);
            if (string.IsNullOrWhiteSpace(txtCourse.Text) || !int.TryParse(txtCourse.Text, out _))
                return ShowError("Курс должен быть числом", txtCourse);
            if (string.IsNullOrWhiteSpace(txtGroup.Text))
                return ShowError("Группа обязательна для заполнения", txtGroup);
            if (!ValidateEmail(txtEmail.Text))
                return ShowError("Некорректный email. Допустимые домены: yandex.ru, gmail.com, icloud.com", txtEmail);
            if (!ValidatePhone(txtPhone.Text))
                return ShowError("Телефон должен быть в формате +7-XXX-XXX-XX-XX", txtPhone);

            return true;
        }

        private bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            var pattern = @"^[^@]{3,}@(yandex\.ru|gmail\.com|icloud\.com)$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        private bool ValidatePhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;

            var pattern = @"^\+7-\d{3}-\d{3}-\d{2}-\d{2}$";
            return Regex.IsMatch(phone, pattern);
        }

        private bool ShowError(string message, Control control)
        {
            MessageBox.Show(message, "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
            control.Focus();
            return false;
        }

        private void UpdateFormWithSelectedStudent()
        {
            if (dataGridView.SelectedRows.Count == 0) return;

            var student = (Student)dataGridView.SelectedRows[0].DataBoundItem;
            txtLastName.Text = student.LastName;
            txtFirstName.Text = student.FirstName;
            txtMiddleName.Text = student.MiddleName;
            txtCourse.Text = student.Course.ToString();
            txtGroup.Text = student.Group;
            dateTimePickerBirthDate.Value = student.BirthDate;
            txtEmail.Text = student.Email;
            txtPhone.Text = student.Phone;
        }

        private void ClearForm()
        {
            foreach (Control control in Controls)
            {
                if (control is TextBox)
                {
                    ((TextBox)control).Clear();
                }
            }
            dateTimePickerBirthDate.Value = DateTime.Today;
        }

        private void SaveData()
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                currentFilePath = saveFileDialog.FileName;
                SaveDataToFile(currentFilePath);
            }
        }

        private void SaveDataToFile(string filePath)
        {
            try
            {
                var json = JsonConvert.SerializeObject(students.ToList(), Formatting.Indented);
                File.WriteAllText(filePath, json);
                isModified = false;
                MessageBox.Show("Данные успешно сохранены", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData()
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                currentFilePath = openFileDialog.FileName;
                LoadDataFromFile(currentFilePath);
            }
        }

        private void LoadDataFromFile(string filePath)
        {
            try
            {
                var json = File.ReadAllText(filePath);
                var loadedStudents = JsonConvert.DeserializeObject<List<Student>>(json);

                students.Clear();
                foreach (var student in loadedStudents)
                {
                    students.Add(student);
                }

                isModified = false;
                MessageBox.Show("Данные успешно загружены", "Загрузка", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToCsv()
        {
            if (saveCsvDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var csv = new StringBuilder();
                    csv.AppendLine("LastName,FirstName,MiddleName,Course,Group,BirthDate,Email,Phone");

                    foreach (var student in students)
                    {
                        csv.AppendLine($"{student.LastName},{student.FirstName},{student.MiddleName}," +
                                       $"{student.Course},{student.Group},{student.BirthDate:dd.MM.yyyy}," +
                                       $"{student.Email},{student.Phone}");
                    }

                    File.WriteAllText(saveCsvDialog.FileName, csv.ToString());
                    MessageBox.Show("Данные успешно экспортированы в CSV", "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при экспорте: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ImportFromCsv()
        {
            if (openCsvDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    var lines = File.ReadAllLines(openCsvDialog.FileName);
                    var newStudents = new List<Student>();

                    for (int i = 1; i < lines.Length; i++) // Пропускаем заголовок
                    {
                        var values = lines[i].Split(',');
                        if (values.Length >= 8)
                        {
                            var student = new Student
                            {
                                LastName = values[0],
                                FirstName = values[1],
                                MiddleName = values[2],
                                Course = int.Parse(values[3]),
                                Group = values[4],
                                BirthDate = DateTime.ParseExact(values[5], "dd.MM.yyyy", null),
                                Email = values[6],
                                Phone = values[7]
                            };
                            newStudents.Add(student);
                        }
                    }

                    students.Clear();
                    foreach (var student in newStudents)
                    {
                        students.Add(student);
                    }

                    isModified = true;
                    MessageBox.Show("Данные успешно импортированы из CSV", "Импорт", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при импорте: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ShowStatistics()
        {
            var stats = students
                .GroupBy(s => s.Course)
                .Select(g => new { Course = g.Key, Count = g.Count() })
                .OrderBy(x => x.Course)
                .ToList();

            var message = new StringBuilder("Статистика по курсам:\n");
            foreach (var stat in stats)
            {
                message.AppendLine($"Курс {stat.Course}: {stat.Count} студентов");
            }

            var groupStats = students
                .GroupBy(s => s.Group)
                .Select(g => new { Group = g.Key, Count = g.Count() })
                .OrderBy(x => x.Group)
                .ToList();

            message.AppendLine("\nСтатистика по группам:");
            foreach (var stat in groupStats)
            {
                message.AppendLine($"Группа {stat.Group}: {stat.Count} студентов");
            }

            MessageBox.Show(message.ToString(), "Статистика", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void DataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Name == "BirthDateColumn")
            {
                e.SortResult = DateTime.Parse(e.CellValue1.ToString()).CompareTo(DateTime.Parse(e.CellValue2.ToString()));
                e.Handled = true;
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isModified)
            {
                var result = MessageBox.Show("Есть несохраненные изменения. Сохранить перед выходом?",
                    "Подтверждение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (string.IsNullOrEmpty(currentFilePath))
                    {
                        if (saveFileDialog.ShowDialog() != DialogResult.OK)
                        {
                            e.Cancel = true;
                            return;
                        }
                        currentFilePath = saveFileDialog.FileName;
                    }
                    SaveDataToFile(currentFilePath);
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private void txtLastName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtFirstName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnImport_Click(object sender, EventArgs e)
        {

        }
    }

    public class Student
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public int Course { get; set; }
        public string Group { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}