using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace HospitalWebApp.Pages.patients
{
    public class CreateModel : PageModel
    {
        public PatientInfo patientInfo = new PatientInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }
        public void OnPost() 
        {
            patientInfo.name = Request.Form["name"];
            patientInfo.email = Request.Form["email"];
            patientInfo.phone = Request.Form["phone"];
            patientInfo.address = Request.Form["address"];

            if (patientInfo.name.Length == 0|| patientInfo.email.Length==0
                || patientInfo.phone.Length == 0|| patientInfo.address.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
                String conString = "Data Source=PIERRE-KASANANI\\SQLEXPRESS;Initial Catalog=HealthCareDB;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    string sqlQuery = "INSERT INTO Patients (name,email,phone,address) VALUES (@name,@email,@phone,@address)";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        
                            cmd.Parameters.AddWithValue("@name",patientInfo.name);
                            cmd.Parameters.AddWithValue ("email", patientInfo.email);
                            cmd.Parameters.AddWithValue("@phone",patientInfo.phone);
                            cmd.Parameters.AddWithValue("@address", patientInfo.address);

                            cmd.ExecuteNonQuery();
                        
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            patientInfo.name = "";
            patientInfo.email = "";
            patientInfo.phone = "";
            patientInfo.address = "";

            successMessage = "New patient added with success";
        }    
    }
}
