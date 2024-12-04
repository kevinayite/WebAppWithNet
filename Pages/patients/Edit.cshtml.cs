using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace HospitalWebApp.Pages.patients
{
    public class EditModel : PageModel
    {
        public PatientInfo patientInfo = new PatientInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
            String id = Request.Query["id"];  
            try
            {
                String conString = "Data Source=PIERRE-KASANANI\\SQLEXPRESS;Initial Catalog=HealthCareDB;Integrated Security=True";
                using (SqlConnection con = new SqlConnection(conString))
                {
                    con.Open();
                    string sqlQuery = "SELECT * FROM Patients WHERE id=@id";

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                               // PatientInfo patientInfo = new PatientInfo();
                                patientInfo.id = "" + reader.GetInt32(0);
                                patientInfo.name = reader.GetString(1);
                                patientInfo.email = reader.GetString(2);
                                patientInfo.phone = reader.GetString(3);
                                patientInfo.address = reader.GetString(4);
                                patientInfo.createAt = reader.GetDateTime(5).ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
        }

        public void OnPost()
        {
            patientInfo.id = Request.Query["id"];
            patientInfo.name = Request.Form["name"];
            patientInfo.email = Request.Form["email"];
            patientInfo.phone = Request.Form["phone"];
            patientInfo.address = Request.Form["address"];

            if (patientInfo.name.Length == 0 || patientInfo.email.Length == 0
                || patientInfo.phone.Length == 0 || patientInfo.address.Length == 0)
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
                    string sqlQuery = "UPDATE Patients SET name=@name, email= @email,phone= @phone,address = @address WHERE id=@id";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("id", patientInfo.id);
                        cmd.Parameters.AddWithValue("@name", patientInfo.name);
                        cmd.Parameters.AddWithValue("email", patientInfo.email);
                        cmd.Parameters.AddWithValue("@phone", patientInfo.phone);
                        cmd.Parameters.AddWithValue("@address", patientInfo.address);

                        cmd.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage= ex.Message;
                return;
            }

            patientInfo.name = "";
            patientInfo.email = "";
            patientInfo.phone = "";
            patientInfo.address = "";

            successMessage = "Patient updated with success";
            Response.Redirect("/patients/Index");

        }
    }
}
