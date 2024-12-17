using System;
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
            bieuthuc += $" {operation}"; // Thêm phép toán vào biểu thức
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
        // Xử lý các nút số
        private void btn0_Click(object sender, EventArgs e)
        {
            if (isErrorState) HandleErrorState();
            txtDISPLAY.Text += btn0.Text;
            bieuthuc += btn0.Text;
            UpdateCurrentHistory(); // Cập nhật txtCURRENTHISTORY
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            if (isErrorState) HandleErrorState();
            txtDISPLAY.Text += btn1.Text;
            bieuthuc += btn1.Text;
            UpdateCurrentHistory();
        }
        private void btn2_Click(object sender, EventArgs e)
        {
            if (isErrorState) HandleErrorState();
            txtDISPLAY.Text += btn2.Text;
            bieuthuc += btn2.Text;
            UpdateCurrentHistory();
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            if (isErrorState) HandleErrorState();
            txtDISPLAY.Text += btn3.Text;
            bieuthuc += btn3.Text;
            UpdateCurrentHistory();
        }
        private void btn4_Click(object sender, EventArgs e)
        {
            if (isErrorState) HandleErrorState();
            txtDISPLAY.Text += btn4.Text;
            bieuthuc += btn4.Text;
            UpdateCurrentHistory();
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            if (isErrorState) HandleErrorState();
            txtDISPLAY.Text += btn5.Text;
            bieuthuc += btn5.Text;
            UpdateCurrentHistory();
        }
        private void btn6_Click(object sender, EventArgs e)
        {
            if (isErrorState) HandleErrorState();
            txtDISPLAY.Text += btn6.Text;
            bieuthuc += btn6.Text;
            UpdateCurrentHistory();
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            if (isErrorState) HandleErrorState();
            txtDISPLAY.Text += btn7.Text;
            bieuthuc += btn7.Text;
            UpdateCurrentHistory();
        }
        private void btn8_Click(object sender, EventArgs e)
        {
            if (isErrorState) HandleErrorState();
            txtDISPLAY.Text += btn8.Text;
            bieuthuc += btn8.Text;
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            if (isErrorState) HandleErrorState(); ;
            txtDISPLAY.Text += btn9.Text;
            bieuthuc += btn9.Text;
            UpdateCurrentHistory();
        }
        private void btnCHAM_Click(object sender, EventArgs e)
        {
            txtDISPLAY.Text += btnCHAM.Text;
            bieuthuc += btnCHAM.Text;
            UpdateCurrentHistory();
        }

        // Xử lý dấu %
        private void btnPHANTRAM_Click(object sender, EventArgs e)
        {
            try
            {
                if (double.TryParse(txtDISPLAY.Text, out double s2))
                {
                    txtDISPLAY.Text = (s2 / 100).ToString();
                }
                else
                {
                    txtDISPLAY.Text = "Invalid input";
                    isErrorState = true; // Kích hoạt trạng thái lỗi
                    
                }
            }
            catch
            {
                txtDISPLAY.Text = "Error";
                isErrorState = true; // Kích hoạt trạng thái lỗi
                
            }
        }

        // Các nút phép toán
        private void btnCONG_Click(object sender, EventArgs e)
        {
            SetOperation("➕");
        }

        private void btnTRU_Click(object sender, EventArgs e)
        {
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
            if (!string.IsNullOrEmpty(txtDISPLAY.Text))
            {
                txtDISPLAY.Text = txtDISPLAY.Text.Substring(0, txtDISPLAY.Text.Length - 1);
            }
        }

        // Xử lý kết quả
        private void btnKETQUA_Click(object sender, EventArgs e)
        {
            try
            {
                double finalResult = 0;

                // Lấy giá trị nhập vào cho s2
                string inputS2 = txtDISPLAY.Text;
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
                    }
                    else
                    {
                        txtDISPLAY.Text = "Invalid input";
                        isErrorState = true;
                        ;
                        return;
                    }
                }
                else if (!double.TryParse(inputS2, out s2))
                {
                    txtDISPLAY.Text = "Invalid input";
                    isErrorState = true;
                    
                    return;
                }

                // Thực hiện phép toán
                switch (pheptinh)
                {
                    case "➕":
                        finalResult = s1 + s2;
                        break;
                    case "➖":
                        finalResult = s1 - s2;
                        break;
                    case "✖️":
                        finalResult = s1 * s2;
                        break;
                    case "➗":
                        if (s2 == 0)
                        {
                            txtDISPLAY.Text = "Cannot divide by 0";
                            isErrorState = true;
                            
                            return;
                        }
                        finalResult = s1 / s2;
                        break;
                    case "^":
                        finalResult = Math.Pow(s1, s2);
                        break;
                    default:
                        txtDISPLAY.Text = "Invalid operation";
                        isErrorState = true;
                        
                        return;
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
                ;
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
            txtCURRENTHISTORY.Text = bieuthuc ; // Cập nhật biểu thức vào txtCURRENTHISTORY
        }

        private void txtCURRENTHISTORY_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
