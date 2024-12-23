﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace CalculatorCUSTOM
{
    public partial class Form1 : Form
    {
        private string pheptinh = "";
        private double s1 = 0;
        private double s2 = 0;
        private string bieuthuc = ""; // Biến lưu giữ biểu thức
        private bool isErrorState = false; // Theo dõi trạng thái lỗi hoặc hoàn thành phép tính
        public Form1()
        {
            InitializeComponent();
            // Đăng ký sự kiện MouseDown để xử lý di chuyển Form
            this.MouseDown += new MouseEventHandler(Form_MouseDown);
        }
        // Import các hàm từ thư viện user32.dll
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        // Hằng số Windows API
        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        // Xử lý sự kiện khi nhấn chuột
        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture(); // Giải phóng chuột để cho phép di chuyển
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0); // Gửi lệnh di chuyển Form
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void txtDISPLAY_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnTHOAT_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Thiết lập phép toán
        private void SetOperation(string operation)
        {
            // Kiểm tra nếu đầu vào là căn bậc hai
            if (txtDISPLAY.Text.StartsWith("√"))
            {
                string input = txtDISPLAY.Text.Replace("√", "");
                if (double.TryParse(input, out double sqrtValue))
                {
                    if (sqrtValue < 0)
                    {
                        txtDISPLAY.Text = "Invalid input";
                        isErrorState = true;

                        return;
                    }
                    s1 = Math.Sqrt(sqrtValue); // Lưu giá trị thập phân
                    bieuthuc = $"√{input}"; // Lưu biểu thức dưới dạng √
                }
                else
                {
                    txtDISPLAY.Text = "Invalid input";
                    isErrorState = true;

                    return;
                }
            }
            else if (!double.TryParse(txtDISPLAY.Text, out s1))
            {
                txtDISPLAY.Text = "Invalid input";
                isErrorState = true;

                return;
            }
            else
            {
                bieuthuc = txtDISPLAY.Text; // Lưu giá trị đầu tiên
            }

            pheptinh = operation;
            bieuthuc += $" {operation}{" "}"; // Thêm phép toán vào biểu thức
            txtDISPLAY.Text = "";
            UpdateCurrentHistory();
        }


        private void HandleErrorState()
        {
            if (isErrorState)
            {
                txtDISPLAY.Text = "";   // Xóa màn hình nếu đang ở trạng thái lỗi
                txtCURRENTHISTORY.Text = "";  // Reset lịch sử biểu thức hiện tại
                isErrorState = false;   // Đặt lại trạng thái
                bieuthuc = "";        // Làm sạch biểu thức (nếu cần)
            }
        }

       
        private void btnCHAM_Click(object sender, EventArgs e)
        {
            // Nếu màn hình đã trống, thêm "0." thay vì chỉ dấu "."
            if (string.IsNullOrEmpty(txtDISPLAY.Text))
            {
                txtDISPLAY.Text = "0.";
                bieuthuc += "0.";
            }
            // Nếu đã có dấu chấm trong số hiện tại, không cho phép thêm
            else if (!txtDISPLAY.Text.Contains("."))
            {
                txtDISPLAY.Text += ".";
                bieuthuc += ".";
            }

            UpdateCurrentHistory(); // Cập nhật lịch sử
        }

        // Xử lý dấu %
        private void btnPHANTRAM_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDISPLAY.Text))
                return;

            // Kiểm tra nếu giá trị đã chứa %
            if (txtDISPLAY.Text.Contains("%"))
            {
                txtDISPLAY.Text = "Invalid input";
                isErrorState = true;
                return;
            }

            // Thêm ký hiệu % vào màn hình và biểu thức
            txtDISPLAY.Text += "%";
            bieuthuc += "%";
            UpdateCurrentHistory();
        }

        // Các nút phép toán
        private void btnCONG_Click(object sender, EventArgs e)
        {
            SetOperation("➕");
        }

        private void btnTRU_Click(object sender, EventArgs e)
        {
            // Nếu txtDISPLAY đang rỗng, dấu trừ được coi là nhập số âm
            if (string.IsNullOrEmpty(txtDISPLAY.Text))
            {
                txtDISPLAY.Text = "-";
                bieuthuc += "-";
                UpdateCurrentHistory();
                return;
            }

            // Nếu đã có giá trị, thực hiện phép toán trừ
            SetOperation("➖");
        }


        private void btnNHAN_Click(object sender, EventArgs e)
        {
            SetOperation("✖️");
        }

        private void btnCHIA_Click(object sender, EventArgs e)
        {
            SetOperation("➗");
        }

        private void btnLUYTHUA_Click(object sender, EventArgs e)
        {
            SetOperation("^");
        }

        // Xử lý căn bậc hai
        private void btnCAN_Click(object sender, EventArgs e)
        {
            if (isErrorState) // Nếu đang trong trạng thái lỗi, làm sạch màn hình
            {
                txtDISPLAY.Text = "";
                isErrorState = false;

            }

            // Kiểm tra nếu căn bậc hai có sẵn
            if (!txtDISPLAY.Text.StartsWith("√"))
            {
                txtDISPLAY.Text = "√"; // Thêm ký tự √ vào màn hình hiển thị
            }

            // Không thêm "√" nhiều lần vào biểu thức
            if (!bieuthuc.EndsWith("√")) // Chỉ thêm nếu "√" chưa ở cuối
            {
                bieuthuc += " √";
            }

        }

        // Xóa một ký tự
        private void btnXOA_Click(object sender, EventArgs e)
        {
            // Nếu đang ở trạng thái lỗi hoặc đã hiện kết quả, reset hoàn toàn
            if (isErrorState || pheptinh == "" && s2 == 0 && txtDISPLAY.Text != "")
            {
                ResetState();
                txtDISPLAY.Text = "";
                txtCURRENTHISTORY.Text = "";
                return;
            }

            if (!string.IsNullOrEmpty(txtDISPLAY.Text))
            {
                // Xóa từng ký tự trong txtDISPLAY và bieuthuc
                txtDISPLAY.Text = txtDISPLAY.Text.Substring(0, txtDISPLAY.Text.Length - 1);
                if (bieuthuc.Length > 0) // Đảm bảo không xóa vượt giới hạn
                {
                    bieuthuc = bieuthuc.Substring(0, bieuthuc.Length - 1);
                }
            }
            else if (!string.IsNullOrEmpty(pheptinh))
            {
                // Xóa phép toán nếu txtDISPLAY đã trống
                pheptinh = "";
                bieuthuc = bieuthuc.Substring(0, bieuthuc.LastIndexOf(' ')).Trim();
            }
            else if (s1 != 0)
            {
                // Xóa từng ký tự trong s1
                string s1Str = s1.ToString();
                if (s1Str.Length > 1)
                {
                    s1Str = s1Str.Substring(0, s1Str.Length - 1);
                    s1 = double.Parse(s1Str);
                }
                else
                {
                    s1 = 0; // Nếu chỉ còn một chữ số, reset s1
                }
                bieuthuc = s1.ToString();
            }

            UpdateCurrentHistory(); // Cập nhật lại txtCURRENTHISTORY
        }


        // Xử lý kết quả
        private void btnKETQUA_Click(object sender, EventArgs e)
        {
            try
            {
                double finalResult = 0;

                // Lấy giá trị nhập vào cho s2
                string inputS2 = txtDISPLAY.Text;

                // Kiểm tra nếu s2 có dấu %
                bool isS2Percent = inputS2.EndsWith("%");

                if (inputS2.StartsWith("√"))
                {
                    string input = inputS2.Replace("√", "");
                    if (double.TryParse(input, out double sqrtValue))
                    {
                        if (sqrtValue < 0)
                        {
                            txtDISPLAY.Text = "Invalid input";
                            isErrorState = true;
                            return;
                        }
                        s2 = Math.Sqrt(sqrtValue);
                        if (string.IsNullOrEmpty(pheptinh)) // Nếu không có phép toán, hiển thị kết quả trực tiếp
                        {
                            finalResult = s2;
                            txtDISPLAY.Text = finalResult.ToString();
                            AddToHistory($"{inputS2} = {finalResult}"); // Lưu vào lịch sử
                            ResetState(); // Reset trạng thái
                            return; // Kết thúc hàm
                        }
                    }
                    else
                    {
                        txtDISPLAY.Text = "Invalid input";
                        isErrorState = true;
                        return;
                    }
                }
                else if (isS2Percent)
                {
                    // Dữ nguyên giá trị với % trên màn hình, chỉ xử lý nếu có phép toán liên quan
                    string percentPart = inputS2.Replace("%", "");
                    if (!double.TryParse(percentPart, out double percentValue) || percentValue < 0)
                    {
                        txtDISPLAY.Text = "Invalid input";
                        isErrorState = true;
                        return;
                    }
                    s2 = percentValue; // Giá trị thập phân được tính chỉ khi cần
                }
                else if (!double.TryParse(inputS2, out s2))
                {
                    // Kiểm tra nếu input kết thúc bằng dấu chấm (vd: "2.")
                    if (inputS2.EndsWith("."))
                    {
                        inputS2 = inputS2.TrimEnd('.'); // Xóa dấu chấm ở cuối
                        if (!double.TryParse(inputS2, out s2)) // Thử lại sau khi loại bỏ dấu chấm
                        {
                            txtDISPLAY.Text = "Invalid input";
                            isErrorState = true;
                            return;
                        }
                    }
                    else
                    {
                        txtDISPLAY.Text = "Invalid input";
                        isErrorState = true;
                        return;
                    }
                }

                // Thực hiện phép toán
                switch (pheptinh)
                {
                    case "➕":
                        finalResult = s1 + (isS2Percent ? (s1 * s2) / 100 : s2);
                        break;
                    case "➖":
                        finalResult = s1 - (isS2Percent ? (s1 * s2) / 100 : s2);
                        break;
                    case "✖️":
                        finalResult = s1 * (isS2Percent ? s2 / 100 : s2);
                        break;
                    case "➗":
                        if (s2 == 0)
                        {
                            txtDISPLAY.Text = "Cannot divide by 0";
                            isErrorState = true;
                            return;
                        }
                        finalResult = s1 / (isS2Percent ? s2 / 100 : s2);
                        break;
                    case "^":
                        // Kiểm tra nếu inputS2 có dấu âm
                        bool isNegativeBase = inputS2.StartsWith("-") && !inputS2.StartsWith("-√");

                        double baseValue = s1; // Cơ số
                        double exponentValue = isS2Percent ? (s2 / 100) : s2; // Số mũ

                        if (isNegativeBase)
                        {
                            baseValue = -s1; // Áp dụng dấu âm vào cơ số
                        }

                        finalResult = Math.Pow(baseValue, exponentValue); // Thực hiện phép toán lũy thừa

                        if (double.IsNaN(finalResult) || double.IsInfinity(finalResult))
                        {
                            txtDISPLAY.Text = "Invalid input";
                            isErrorState = true;
                            return;
                        }
                        break;
                    default:
                        // Nếu không có phép toán, kiểm tra s2 có ký tự % hay không
                        if (string.IsNullOrEmpty(pheptinh))
                        {
                            if (inputS2.StartsWith("√"))
                            {
                                string sqrtPart = inputS2.Replace("√", "");
                                if (double.TryParse(sqrtPart, out double sqrtValue) && sqrtValue >= 0)
                                {
                                    finalResult = Math.Sqrt(sqrtValue);
                                    txtDISPLAY.Text = finalResult.ToString();
                                    AddToHistory($"{inputS2} = {finalResult}");
                                    ResetState();
                                    return;
                                }
                                else
                                {
                                    txtDISPLAY.Text = "Invalid input";
                                    isErrorState = true;
                                    return;
                                }
                            }
                            else if (isS2Percent)
                            {
                                finalResult = s2 / 100; // Chuyển % thành giá trị thập phân
                                txtDISPLAY.Text = finalResult.ToString();
                                AddToHistory($"{inputS2} = {finalResult}");
                                ResetState();
                                return;
                            }
                            else
                            {
                                txtDISPLAY.Text = "Invalid operation";
                                isErrorState = true;
                                return;
                            }
                        }
                        else
                        {
                            txtDISPLAY.Text = "Invalid operation";
                            isErrorState = true;
                            return;
                        }
                }


                // Cập nhật biểu thức và kết quả
                bieuthuc += $" = {finalResult}";
                txtDISPLAY.Text = finalResult.ToString(); // Hiển thị kết quả
                AddToHistory(bieuthuc); // Lưu vào lịch sử

                // Reset trạng thái
                ResetState();

                // Lưu kết quả làm s1 cho bài toán tiếp theo
                s1 = finalResult;
            }
            catch (Exception ex)
            {
                txtDISPLAY.Text = $"Error: {ex.Message}";
                isErrorState = true;
            }
        }

        // Reset lại trạng thái sau khi tính toán
        private void ResetState()
        {
            pheptinh = "";      // Làm sạch phép toán
            s2 = 0;             // Đặt lại giá trị s2
            bieuthuc = "";      // Làm sạch biểu thức
            isErrorState = false; // Đặt lại trạng thái lỗi (nếu có)
        }

        // Thêm biểu thức vào lịch sử
        private void AddToHistory(string newEntry)
        {
            if (string.IsNullOrEmpty(txtHISTORY.Text))
            {
                txtHISTORY.Text = newEntry;
            }
            else
            {
                txtHISTORY.Text += Environment.NewLine + newEntry;
            }

            // Giới hạn tối đa 10 dòng lịch sử
            var historyLines = txtHISTORY.Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            if (historyLines.Length > 10)
            {
                txtHISTORY.Text = string.Join(Environment.NewLine, historyLines.Skip(1));
            }
        }

        // Xóa toàn bộ màn hình và lịch sử
        private void btnCLEAR_Click(object sender, EventArgs e)
        {
            bieuthuc = "";
            txtDISPLAY.Text = "";
            txtCURRENTHISTORY.Text = "";
            isErrorState = false; //reset trang thai loi
            ResetState();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtHISTORY.Text = "";  // Xóa lịch sử
        }
        private void UpdateCurrentHistory()
        {
            txtCURRENTHISTORY.Text = bieuthuc; // Cập nhật biểu thức vào txtCURRENTHISTORY
        }

        private void txtCURRENTHISTORY_TextChanged(object sender, EventArgs e)
        {

        }

        //Gộp các nút số
        private void btnNum_Click(object sender, EventArgs e)
        {
            if (isErrorState) HandleErrorState();
            Button btn = sender as Button;
            if (btn != null)
            {
                txtDISPLAY.Text += btn.Text;
                bieuthuc += btn.Text;
                UpdateCurrentHistory();
            }
        }
    }
}
