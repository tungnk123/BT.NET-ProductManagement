using ProductManagement.DataConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductManagement
{
    public partial class ProductManager : Form
    {
        private readonly ProductManagenentEntities _context;
        public ProductManager()
        {
            InitializeComponent();
            _context = new ProductManagenentEntities();

            loadSanPham();

        }

        private void loadSanPham()
        {
            var listSP = _context.Products.ToList();
            dgDanhSachSanPham.DataSource = listSP;
            dgTimKiemSanPham.DataSource = listSP;
        }

        private void btnLuuSanPham_Click(object sender, EventArgs e)
        {
            if (!KiemTraDieuKien())
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin sản phẩm");
                return;
            }
            try
            {
                Product product = new Product();
                product.MaSp = tbMaSP.Text;
                product.TenSp = tbTenSP.Text;
                product.SoLuong = int.Parse(tbSoLuong.Text);
                product.DonGia = int.Parse(tbDonGia.Text);
                product.XuatXu = tbXuatXu.Text;
                product.NgayHetHan = dpNgayHetHan.Value;

                var kiemTraMaSp = _context.Products.Find(product.MaSp);
                if (kiemTraMaSp != null)
                {
                    MessageBox.Show("Mã sản phẩm đã tồn tại trong chương trình");
                    return;
                }

                _context.Products.Add(product);
                _context.SaveChanges();
                ResetList();
                MessageBox.Show("Thêm sản phẩm thành công");
            }
            catch
            {
                MessageBox.Show("Vui lòng kiểm tra lại định dạng thông tin");   
            }
        }

        private void ResetList()
        {
            var newList = _context.Products.ToList();       
            dgDanhSachSanPham.DataSource = newList;
            dgTimKiemSanPham.DataSource = newList;
        }

        private bool KiemTraDieuKien()
        {
            if(tbMaSP.Text.Length > 0
                && tbTenSP.Text.Length > 0
                && tbSoLuong.Text.Length > 0 
                && tbDonGia.Text.Length > 0 
                && tbXuatXu.Text.Length > 0)
            {
                return true;
            }
            return false;
        }

        private void btnXoaSanPham_Click(object sender, EventArgs e)
        {
            if(tbMaSP.Text.Length == 0)
            {
                MessageBox.Show("Vui lòng điền mã sản phẩm để xóa");
                return;
            }
            var sanPham = _context.Products.Find(tbMaSP.Text);
            if(sanPham == null)
            {
                MessageBox.Show("Không tìm thấy sản phẩm cần xóa");
                return;
            }
            _context.Products.Remove(sanPham);
            _context.SaveChanges();
            ResetList();
            MessageBox.Show("Xóa sản phẩm thành công");
        }

        private void btnDonGiaCaoNhat_Click(object sender, EventArgs e)
        {
            var sanPham = _context.Products.OrderByDescending(p => p.DonGia).FirstOrDefault();
            var newList = new List<Product>();      
            newList.Add(sanPham);   
            dgTimKiemSanPham.DataSource = newList;
        }

        private void btnSanPhamTuNhatBan_Click(object sender, EventArgs e)
        {
            var sanPham = _context.Products.Where(p => p.XuatXu == "Nhật Bản").ToList().FirstOrDefault();
            var newList = new List<Product>();
            newList.Add(sanPham);
            dgTimKiemSanPham.DataSource = newList;
        }

        private void btnXuatSPHetHan_Click(object sender, EventArgs e)
        {
            var listSanPhamHetHan = _context.Products.Where(p => p.NgayHetHan.Value < DateTime.Now).ToList();
            var newList = new List<Product>();
            foreach (var item in listSanPhamHetHan)
            {
                newList.Add(item);  
            }
            dgTimKiemSanPham.DataSource= newList;   
        }

        private void btnXuatTuADenB_Click(object sender, EventArgs e)
        {
            if(tbA.Text.Length == 0 || tbB.Text.Length == 0)
            {
                MessageBox.Show("Vui lòng điền đầy đủ giá trị a và b");
                return;
            }
            try
            {
                int a = int.Parse(tbA.Text);        
                int b = int.Parse(tbB.Text);    
                var listSanPhamPhuHop = _context.Products.Where(p => p.DonGia >= a && p.DonGia <= b).ToList();
                var newList = new List<Product>();
                foreach (var item in listSanPhamPhuHop)
                {
                    newList.Add(item);
                }
                dgTimKiemSanPham.DataSource = newList;
            }
            catch
            {
                MessageBox.Show("Vui lòng điền a và b là 2 số nguyên");
            }
        }

        private void btnXoaXuatXu_Click(object sender, EventArgs e)
        {
            var listSpXoa = _context.Products.Where(p => p.XuatXu == tbXoaXuatXu.Text).ToList();
            foreach (var item in listSpXoa)
            {
                _context.Products.Remove(item);     
            }
            _context.SaveChanges();
            ResetList();
            MessageBox.Show("Đã xóa thành công các sản phâm có xuất xứ " + tbXoaXuatXu.Text);
        }

        private void btnKiemTraKhoQuaHan_Click(object sender, EventArgs e)
        {
            var listSanPhamHetHan = _context.Products.Where(p => p.NgayHetHan.Value < DateTime.Now).ToList();
            MessageBox.Show($"Có {listSanPhamHetHan.Count} sản phẩm hết hạn trong kho");
        }


        private void btnXoaSPHetHan_Click_1(object sender, EventArgs e)
        {
            var listSanPhamHetHan = _context.Products.Where(p => p.NgayHetHan.Value < DateTime.Now).ToList();
            foreach (var item in listSanPhamHetHan)
            {
                _context.Products.Remove(item);
            }
            _context.SaveChanges();
            ResetList();
            MessageBox.Show("Đã xóa toàn bộ sản phẩm hết hạn");
        }

        private void btnXoaToanBoSP_Click(object sender, EventArgs e)
        {
            var listSp = _context.Products.ToList();
            _context.Products.RemoveRange(listSp);
            _context.SaveChanges();
            ResetList();
            MessageBox.Show("Xóa toàn bộ sản phẩm thành công");
        }
    }
}
