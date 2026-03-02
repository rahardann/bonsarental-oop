using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tugas_Akhir_PBO.App.Context.Admin;
using Tugas_Akhir_PBO.App.Models.Admin;

namespace Tugas_Akhir_PBO.View.Admin
{
    public partial class UCAddRiwayat : UserControl
    {
        UserControlDashboard UCDashboard;
        UserControlStok UCStok;

        public UCAddRiwayat(UserControlDashboard UCDashboard, UserControlStok UCStok)
        {
            InitializeComponent();
            this.UCDashboard = UCDashboard;
            this.UCStok = UCStok;
            LoadRiwayatTransaksi();
        }

        public void LoadRiwayatTransaksi()
        {
            try
            {
                dataGridViewRiwayat.AllowUserToAddRows = false;

                TransaksiContext transaksiContext = new TransaksiContext();
                List<Transaksi> transaksiList = transaksiContext.GetAllTransaksi();

                dataGridViewRiwayat.Columns.Clear();


                dataGridViewRiwayat.DataSource = transaksiList;

                dataGridViewRiwayat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                dataGridViewRiwayat.Columns["id_transaksi"].HeaderText = "ID Transaksi";
                dataGridViewRiwayat.Columns["Nama"].HeaderText = "Nama Pelanggan";
                dataGridViewRiwayat.Columns["NIK"].HeaderText = "NIK";
                dataGridViewRiwayat.Columns["TotalHarga"].HeaderText = "Total Harga";
                dataGridViewRiwayat.Columns["TanggalKembali"].HeaderText = "Tanggal Kembali";
                dataGridViewRiwayat.Columns["TanggalTransaksi"].HeaderText = "Tanggal Transaksi";
                dataGridViewRiwayat.Columns["Status"].HeaderText = "Status";

                if (!dataGridViewRiwayat.Columns.Contains("UpdateStatus"))
                {
                    DataGridViewButtonColumn updateButtonColumn = new DataGridViewButtonColumn
                    {
                        Name = "UpdateStatus",
                        HeaderText = "Update Status",
                        Text = "Selesai",
                        UseColumnTextForButtonValue = true
                    };
                    dataGridViewRiwayat.Columns.Add(updateButtonColumn);
                }

                dataGridViewRiwayat.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Terjadi kesalahan saat memuat data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridViewRiwayat_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridViewRiwayat.Columns["UpdateStatus"].Index && e.RowIndex >= 0)
            {
                try
                {
                    TransaksiContext transaksiContext = new TransaksiContext();

                    int idTransaksi = Convert.ToInt32(dataGridViewRiwayat.Rows[e.RowIndex].Cells["id_transaksi"].Value);

                    string currentStatus = dataGridViewRiwayat.Rows[e.RowIndex].Cells["Status"].Value.ToString();

                    if (currentStatus == "Disewa")
                    {
                        dataGridViewRiwayat.Rows[e.RowIndex].Cells["Status"].Value = "Selesai";

                        transaksiContext.UpdateStatusToSelesai(idTransaksi);

                        transaksiContext.UpdateStokProduk(idTransaksi);

                        UCDashboard.LoadTotalProdukDisewa();

                        UCDashboard.LoadRiwayat();

                        UCStok.LoadKatalog();

                        MessageBox.Show($"Status transaksi dengan ID {idTransaksi} telah Selesai.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Status sudah selesai.", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Terjadi kesalahan saat memperbarui status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CloseBox_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void UCAddRiwayat_Load(object sender, EventArgs e)
        {

        }
    }
}
