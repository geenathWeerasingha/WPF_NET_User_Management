﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MvvmDemo.Models
{
    public class EmployeeService
    {


        SqlConnection ObjSqlConnection;
        SqlCommand ObjSqlCommand;

        
        public EmployeeService()
        {
            ObjSqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["EMSConnection"].ConnectionString);
            ObjSqlCommand = new SqlCommand();
            ObjSqlCommand.Connection = ObjSqlConnection;
            ObjSqlCommand.CommandType = CommandType.StoredProcedure;

        }

        public List<Employee> GetAll()
        {
            List<Employee> ObjEmployeesList = new List<Employee>();
            try
            {
                ObjSqlCommand.Parameters.Clear();
                ObjSqlCommand.CommandText = "udp_SelectAllEmployees";

                ObjSqlConnection.Open();
                var ObjSqlDataReader=ObjSqlCommand.ExecuteReader();

                if(ObjSqlDataReader.HasRows)
                {
                    Employee ObjEmployee = null;
                    while(ObjSqlDataReader.Read())
                    {
                        ObjEmployee = new Employee();
                        ObjEmployee.Id=ObjSqlDataReader.GetInt32(0);
                        ObjEmployee.Name=ObjSqlDataReader.GetString(1);
                        ObjEmployee.Age=ObjSqlDataReader.GetInt32(2);

                        ObjEmployeesList.Add(ObjEmployee);


                    }  

                }
                ObjSqlDataReader.Close();

            }catch(SqlException ex)
            {
                throw ex;
            }
            finally
            {
                ObjSqlConnection.Close();
            }

            return ObjEmployeesList;

        }


        public bool Add(Employee objNewEmployee)
        {
            bool IsAdded = false;
            if (objNewEmployee.Age<21 || objNewEmployee.Age>58)
            {
                throw new ArgumentException("Invalid age limit foe employee");
            }
            try
            {
                ObjSqlCommand.Parameters.Clear();
                ObjSqlCommand.CommandText = "udp_InsertEmployee";
                ObjSqlCommand.Parameters.AddWithValue("@Id",objNewEmployee.Id);
                ObjSqlCommand.Parameters.AddWithValue("@Name", objNewEmployee.Name);
                ObjSqlCommand.Parameters.AddWithValue("@Age", objNewEmployee.Age);
                ObjSqlConnection.Open();

                int NoOfRowsAffected=ObjSqlCommand.ExecuteNonQuery();
                IsAdded = NoOfRowsAffected > 0;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                ObjSqlConnection.Close();
            }

             
            return IsAdded;

        }

        public bool Update(Employee objEmployeeToUpdate)
        {
            bool IsUpdated = false;

            try
            {
                ObjSqlCommand.Parameters.Clear();
                ObjSqlCommand.CommandText = "udp_UpdateEmployee";
                ObjSqlCommand.Parameters.AddWithValue("@Id", objEmployeeToUpdate.Id);
                ObjSqlCommand.Parameters.AddWithValue("@Name", objEmployeeToUpdate.Name);
                ObjSqlCommand.Parameters.AddWithValue("@Age", objEmployeeToUpdate.Age);
                ObjSqlConnection.Open();

                int NoOfRowsAffected = ObjSqlCommand.ExecuteNonQuery();
                IsUpdated = NoOfRowsAffected > 0;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                ObjSqlConnection.Close();
            }


            return IsUpdated;
        }


        public bool Delete(int id)
        {
            bool IsDeleted = false;

            try
            {
                ObjSqlCommand.Parameters.Clear();
                ObjSqlCommand.CommandText = "udp_DeleteEmployee";
                ObjSqlCommand.Parameters.AddWithValue("@Id", id);
                ObjSqlConnection.Open();

                int NoOfRowsAffected = ObjSqlCommand.ExecuteNonQuery();
                IsDeleted = NoOfRowsAffected > 0;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                ObjSqlConnection.Close();
            }

            return IsDeleted;
        }


        public Employee Search(int id)
        {
            Employee ObjEmployee = null;

            try
            {
                ObjSqlCommand.Parameters.Clear();
                ObjSqlCommand.CommandText = "udp_SelectEmployeeById";
                ObjSqlCommand.Parameters.AddWithValue("@Id",id);

                ObjSqlConnection.Open();
                var ObjSqlDataReader = ObjSqlCommand.ExecuteReader();

                if(ObjSqlDataReader.HasRows)
                {
                    ObjSqlDataReader.Read();
                    ObjEmployee=new Employee();
                    ObjEmployee.Id= ObjSqlDataReader.GetInt32(0);
                    ObjEmployee.Name= ObjSqlDataReader.GetString(1);
                    ObjEmployee.Age= ObjSqlDataReader.GetInt32(2);
                }

                ObjSqlDataReader.Close();

            }
            catch(SqlException ex)
            {
                throw ex;
            }finally
            {
                ObjSqlConnection.Close();
            }

            return ObjEmployee;






        }




    }
}









