using Microsoft.AspNetCore.Http.HttpResults;
using MvcCoreCrudDoctores.Models;
using System.Data;
using System.Data.SqlClient;
using System.Numerics;

#region Procedures

//create procedure SP_INSERTDOCTOR
//(@ID_HOSPITAL INT, @APELLIDO NVARCHAR(50) , @ESPECIALIDAD NVARCHAR(50) , @SALARIO INT)
//AS
//    declare @nextId int
//	select @nextId = max(DOCTOR_NO) +1 from doctor
//    insert into doctor values(@nextId, @ID_HOSPITAL, @APELLIDO, @ESPECIALIDAD, @SALARIO)
//GO

#endregion

namespace MvcCoreCrudDoctores.Repositories
{
    public class RepositoryDoctores
    {
        SqlConnection cn;
        SqlCommand com;
        SqlDataReader reader;

        public RepositoryDoctores()
        {
            string connectionString = "Data Source=LOCALHOST\\SQLEXPRESS;Initial Catalog=Hospital;Persist Security Info=True;User ID=SA;Password=MCSD2023";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public async Task<List<Doctores>> GetDoctoresAsync()
        {
            string sql = "select * from doctor";
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            this.reader = await this.com.ExecuteReaderAsync();
            List<Doctores> doctores = new List<Doctores>();
            while (this.reader.Read())
            {
                Doctores doct = new Doctores();
                doct.IdHospital = int.Parse(this.reader["HOSPITAL_COD"].ToString());
                doct.IdDoctor = int.Parse(this.reader["DOCTOR_NO"].ToString());
                doct.Apellido = this.reader["APELLIDO"].ToString();
                doct.Especialidad = this.reader["ESPECIALIDAD"].ToString();
                doct.Salario = int.Parse(this.reader["SALARIO"].ToString());
                doctores.Add(doct);
            }
            await this.cn.CloseAsync();
            await this.reader.CloseAsync();
            return doctores;
        }

        public async Task<Doctores> FindDoctoresAsync(int id)
        {
            string sql = "select * from doctor where doctor_no=@id";
            this.com.Parameters.AddWithValue("@id",id);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            this.reader = await this.com.ExecuteReaderAsync();
            Doctores doctor = null;
            if (await this.reader.ReadAsync())
            {
                //Tenemos datos
                doctor = new Doctores();
                doctor.IdHospital = int.Parse(this.reader["HOSPITAL_COD"].ToString());
                doctor.IdDoctor = int.Parse(this.reader["DOCTOR_NO"].ToString());
                doctor.Apellido = this.reader["APELLIDO"].ToString();
                doctor.Especialidad = this.reader["ESPECIALIDAD"].ToString();
                doctor.Salario = int.Parse(this.reader["SALARIO"].ToString());
            }
            else
            {
                //No tenemos datos
            }
            await this.cn.CloseAsync();
            await this.reader.CloseAsync();
            return doctor;
        }

        public async Task InsertDoctor(int id_hospital , string apellido, string especialidad, int salario)
        {
            this.com.Parameters.AddWithValue("@ID_HOSPITAL",id_hospital);
            this.com.Parameters.AddWithValue("@APELLIDO",apellido);
            this.com.Parameters.AddWithValue("@ESPECIALIDAD",especialidad);
            this.com.Parameters.AddWithValue("@SALARIO",salario);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_INSERTDOCTOR";
            await this.cn.OpenAsync();
            int af = await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();

        }
    }
}
